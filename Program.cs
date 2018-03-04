
namespace Stock_Market
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    /*
    Структура коллекции
    Клиенты
    * Имя клиента
    * Баланс клиента по долларам 
    * Баланс клиента по ценной бумаге "A" в штуках
    * Баланс по ценной бумаге "B"
    * Баланс по ценной бумаге "C"
    * Баланс по ценной бумаге "D"
    Заявки на продажу и покупку имеют древевидную структуру 
     * A,B,C,D
          => *Цена, Количество, Клиент*          

       * если учитывать невозможность ухода в минус то данная конструкция не пременима, т.к. возможно нарушение хронологии исполнения заявок.    
   */
    //Класс контроллер
    public class CController
    {
        private List<string> flogs = new List<string>();
        private List<CCustomer_account> faccounts = new List<CCustomer_account>();
        private CStock_Collection fordersSellList = new CStock_Collection();
        private CStock_Collection fordersBuyList = new CStock_Collection();

        public List<string> Logs { get => flogs; set => flogs = value; }
        public List<CCustomer_account> Accounts { get => faccounts; set => faccounts = value; }
        public CStock_Collection OrdersSellList { get => fordersSellList; set => fordersSellList = value; }
        public CStock_Collection OrdersBuyList { get => fordersBuyList; set => fordersBuyList = value; }
        //Класс счет клиента  
        public class CCustomer_account //Есть все тесты
        {
            private string fname;
            private int fmoney;
            private int[] fstock = new int[4];
            public string Name { get => fname; set => fname = value; }
            public int Money { get => fmoney; set => fmoney = value; }
            public int[] Stock { get => fstock; set => fstock = value; }
            public CCustomer_account(string pAccount) //Есть тест
            {
                List<string> SplitAccount = pAccount.Split('\t').ToList();
                Name = SplitAccount[0];
                SplitAccount.RemoveRange(0, 1);
                int[] num = SplitAccount.Select(s => Convert.ToInt32(Regex.Replace(s, "[^0-9]", "0"))).ToArray();
                Money = num[0];
                for (int i = 1; i <= 4; i++)
                    Stock[i - 1] = num[i];
            }
            //Изменение ценных бумаг и денег на счете
            public void Deal(int pAtype, int pCount, int pPrice) //Есть тест
            {
                Stock[pAtype] += pCount;
                Money += pPrice * Math.Abs(pCount);
            }
            //Преобразование счета в строчную переменную
            public string FieldsToString() //Есть тест
            {
                return Name + '\t' + Money + '\t' + Stock[0] + '\t' + Stock[1] + '\t' + Stock[2] + '\t' + Stock[3];
            }
        }
        //Класс заявка 
        public class COrder //Есть все тесты
        {
            private string fname;
            private int fcount;
            private int fprice;

            public string Name { get => fname; set => fname = value; }
            public int Price { get => fprice; set => fprice = value; }
            public int Count { get => fcount; set => fcount = value; }
            //Create
            public COrder(string pName, int pPrice, int pCount)//Есть тест
            {
                Name = pName;
                Price = pPrice;
                Count = pCount;
            }
        }
        //Класс  коллекция заявок определенной бумаги (Сток)
        public class CStock //Есть все тесты
        {
            private int fstock;
            private List<COrder> forders = new List<COrder>();

            public int Stock { get => fstock; set => fstock = value; }
            public List<COrder> Orders { get => forders; set => forders = value; }
            //Create
            public CStock(int Pstock) //Есть тест
            {
                Stock = Pstock;
            }
            //Добовление заявки
            public COrder Add(string pName, int pPrice, int pCount)//Есть тест
            {
                Orders.Add(new COrder(pName, pPrice, pCount));
                return Orders.Last();
            }
        }
        //Класс коллекция Стоков
        public class CStock_Collection //Есть все тесты
        {
            private List<CStock> fstocks = new List<CStock>();

            public List<CStock> Stocks { get => fstocks; set => fstocks = value; }
            //Добовление стока
            public COrder Add(string pName, int pStock, int pPrice, int pCount) //Есть тест
            {
                return Find_By_Stock(pStock).Add(pName, pPrice, pCount);
            }
            //Нахождение стока по имени
            CStock Find_By_Stock(int pStock) //Есть тест
            {
                for (int i = 0; i < Stocks.Count; i++)
                    if (Stocks[i].Stock == pStock)
                        return Stocks[i];
                Stocks.Add(new CStock(pStock));
                return Stocks.Last();
            }
        }
        //Функция сохранения строк в файл        
        public void StringsListToFile(string pFileName, List<string> pStringsList) //Есть тест
        {
            try
            {
                File.WriteAllLines(pFileName, pStringsList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Функция чтения строк из файла
        public List<string> FileToStringsList(string pFileName) //Есть тест
        {
            try
            {
                List<string> TempStrings = (File.ReadAllText(pFileName).Split('\n').ToList());
                for (int i = 0; i < TempStrings.Count; i++)
                    TempStrings[i] = TempStrings[i].Replace("\r", "");
                return TempStrings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        //Функция сохранения единой строки в файл
        public void StringToFile(string pFileName, string pString) //Есть тест
        {
            try
            {
                File.WriteAllText(pFileName, pString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Сохранение в файл клиентов
        public void AccountSeveToFile(string pFileName)//Есть тест
        {
            string allstr = "";
            string[] str = Accounts.Select(s => s.Name + '\t' + s.Money + '\t' + s.Stock[0] + '\t' + s.Stock[1] + '\t' + s.Stock[2] + '\t' + s.Stock[3]).ToArray();
            for (int i = 0; i < str.Length; i++)
            {
                allstr += str[i];
                if (i > str[i].Length)
                    allstr += "\n";
            }
            StringToFile(pFileName, allstr);
        }

        //Загрузка из файла клиентов 
        public void AccountLoadFromFile(string pFileName)//Есть тест
        {
            Accounts.Clear();
            List<string> SplitCustomer = FileToStringsList(pFileName);
            for (int i = 0; i < SplitCustomer.Count; i++)
                Accounts.Add(new CCustomer_account(SplitCustomer[i]));
        }

        //Загрузка из файла заявок
        public void OrdersLoadFromFile(string pFileName)//Есть совокупный тест 
        {
            OrdersBuyList.Stocks.Clear();
            OrdersSellList.Stocks.Clear();
            List<string> Splitorder = FileToStringsList(pFileName);
            for (int i = 0; i < Splitorder.Count; i++)
                AddOrder(Splitorder[i]);
        }
        //Добавление заявки согласно иерархии
        public COrder AddOrder(string pOrderString) //Есть тест
        {
            List<string> SplitAccount = pOrderString.Split('\t').ToList();
            if (SplitAccount.Count == 5)
                return SellOrBuy(SplitAccount[1]).Add(SplitAccount[0], Convert.ToInt32((StringToInt(SplitAccount[2]))), Convert.ToInt32(SplitAccount[3]), Convert.ToInt32(SplitAccount[4]));
            return null;
        }

        //Поиск клиента по имени
        public  CCustomer_account FindCustomerByName(string pName)//Есть тест
        {
            for (int i = 0; i < Accounts.Count; i++)
                if (Accounts[i].Name == pName)
                    return Accounts[i];
            return null;
        }
        //Возврат коллекции в зависимости от mode
        public  CStock_Collection SellOrBuy(string pMode) //Есть тест
        {
            if (pMode == "b")
                return OrdersBuyList;
            if (pMode == "s")
                return OrdersSellList;
            return null;
        }
        //Функция перевода Цифровых индификаторв акций в Буквенные
        public string IntToString(int pIndex) //Есть тест
        {
            return Convert.ToString(pIndex).Replace("0", "A").Replace("1", "B").Replace("2", "C").Replace("3", "D");
        }
        //Обратная функция
        public  int StringToInt(string pIndex) //Есть тест
        {
            return Convert.ToInt32((pIndex).Replace("A", "0").Replace("B", "1").Replace("C", "2").Replace("D", "3"));
        }
        //Сопостовление данных
        public void Countin(Boolean pSaveLogs, Boolean pPartialMatching)
        {
            foreach (CStock BuyList in OrdersBuyList.Stocks)
                foreach (COrder BuyOrder in BuyList.Orders)
                {
                    int Stock = BuyList.Stock;
                    foreach (CStock SellList in OrdersSellList.Stocks)
                        if (SellList.Stock == Stock)
                            for (int s = 0; s < SellList.Orders.Count; s++)
                            {
                                COrder SellOrder = SellList.Orders[s];
                                if (SellOrder.Name != BuyOrder.Name)
                                {
                                    //Проверка существование счета для исполняемой заявки
                                    if ((FindCustomerByName(BuyOrder.Name) != null) && (FindCustomerByName(SellOrder.Name) != null))
                                        if (((!pPartialMatching) && (SellOrder.Price == BuyOrder.Price) && (SellOrder.Count == BuyOrder.Count)) || ((pPartialMatching) && ((SellOrder.Price <= BuyOrder.Price))))
                                        {
                                            int Count;
                                            if (!pPartialMatching)
                                                Count = SellOrder.Count;
                                            else
                                                Count = Math.Min(BuyOrder.Count, SellOrder.Count);
                                            FindCustomerByName(BuyOrder.Name).Deal(Stock, Count, -SellOrder.Price);
                                            FindCustomerByName(SellOrder.Name).Deal(Stock, -Count, SellOrder.Price);
                                            SellOrder.Count -= Count;
                                            BuyOrder.Count -= Count;
                                            if (pSaveLogs)
                                                Logs.Add(BuyOrder.Name + "\tSell of\t" + BuyOrder.Name + "\t" + IntToString(Stock) + "\t" + Convert.ToString(SellOrder.Price) + "$\tx" + Convert.ToString(Count) + "\t|\t" + BuyOrder.Name + "\tPaid\t" + SellOrder.Name + "\t" + Convert.ToString(SellOrder.Price * Count) + "$");
                                            SellList.Orders.Remove(SellOrder);
                                            if (SellOrder.Count == 0)
                                            {
                                                SellList.Orders.Remove(SellOrder);
                                                s -= 1;
                                            }
                                            if (BuyOrder.Count == 0)
                                                break;
                                        }
                                }
                            }
                }
        }
        //Запрос консольной команды Да\Нет
        public bool ShowQuestion(String pQuestion) //Дополнительный функционал
        {
            Console.WriteLine(pQuestion + ": y/n");
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.N:
                        Console.WriteLine("No");
                        return false;
                    case ConsoleKey.Y:
                        Console.WriteLine("Yes");
                        return true;
                }
            }
        }
        //Сохранение логов
        public void LogsSaveToFile(string pFileName) //Есть смежные тесты
        {
            StringsListToFile(pFileName, Logs);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Настройки работы программы
            Boolean Partial_Matching = false;
            Boolean Save_Logs = false;
            Boolean Show_Console_Commands = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-l")
                    Save_Logs = true;
                if (args[i] == "-pm")
                    Partial_Matching = true;
                if (args[i] == "-cc")
                    Show_Console_Commands = true;
            }
            CController Controller = new CController();
            Controller.AccountLoadFromFile("clients.txt");
            Controller.OrdersLoadFromFile("orders.txt");
            if (Show_Console_Commands)
            {
                Partial_Matching = Controller.ShowQuestion("Допускать частичное сопостовление заявок?");
                Save_Logs = Controller.ShowQuestion("Сохронить лог операций в файл?");
            }
            Controller.Countin(Save_Logs, Partial_Matching);
            Controller.AccountSeveToFile("result.txt");
            if (Save_Logs)
                Controller.LogsSaveToFile("Logs.txt");
        }
    }
}