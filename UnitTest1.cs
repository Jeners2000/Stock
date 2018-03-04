using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock_Market;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Stock_Market_UnitTests
{
    [TestClass]
    public class TCCustomer_account
    {
        //База для теста
        CController.CCustomer_account TestCustomer = new CController.CCustomer_account("TestClient	1000	130	240	760	320");
        public void Equal_Account(CController.CCustomer_account pAccount, string Pname, int pMoney, int pA, int pB, int pC, int pD)
        {
            //Имя клиента
            Assert.AreEqual(pAccount.Name, Pname);
            //Счет в $
            Assert.AreEqual(pAccount.Money, pMoney);
            //Количество ценных бумаг A (0)
            Assert.AreEqual(pAccount.Stock[0], pA);
            //Количество ценных бумаг B (1)
            Assert.AreEqual(pAccount.Stock[1], pB);
            //Количество ценных бумаг C (2)
            Assert.AreEqual(pAccount.Stock[2], pC);
            //Количество ценных бумаг D (3)
            Assert.AreEqual(pAccount.Stock[3], pD);
        }
        [TestMethod]
        public void CCustomer_account_FieldsToString_TEST()
        {
            Assert.AreEqual(TestCustomer.FieldsToString(), "TestClient	1000	130	240	760	320");
        }
        [TestMethod]
        //Создание счета клиента на основе файловой записи
        public void CCustomer_account_Create_TEST()
        {
            Equal_Account(TestCustomer, "TestClient", 1000, 130, 240, 760, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Sell_TEST()
        {
            //Продажа бумаг C в колличестве 60 со счета клиента ценой 10$ за штуку
            // -60 C + 600(60*10)$
            TestCustomer.Deal(2, -60, 10);
            Equal_Account(TestCustomer, "TestClient", 1600, 130, 240, 700, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Buy_TEST()
        {
            //Покупка бумаг A в колличестве 70 штук ценой 10$ за штуку
            // +70 A - 700(70*10)$
            TestCustomer.Deal(0, 70, -10);
            Equal_Account(TestCustomer, "TestClient", 300, 200, 240, 760, 320);
        }
    }
    [TestClass]
    public class TCOrder
    {
        [TestMethod]
        public void COrder_Create_TEST()
        {
            //Имя клиента, Стоимость за штуку, Колличество
            CController.COrder TestOrder = new CController.COrder("TestClient", 10, 20);
            //Имя клиента
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Стоимость за штуку
            Assert.AreEqual(TestOrder.Price, 10);
            //Колличество
            Assert.AreEqual(TestOrder.Count, 20);
        }
    }
    [TestClass]
    public class TCStock
    {
        CController.CStock TestStock = new CController.CStock(1);
        [TestMethod]
        public void CStock_Create_TEST()
        {
            //Сток
            Assert.AreEqual(TestStock.Stock, 1);
        }
        [TestMethod]
        public void CStock_Add_TEST()
        {
            //Имя клиента, Стоимость за штуку, Колличество
            CController.COrder TestOrder = TestStock.Add("TestClient", 10, 20);
            //Сток
            Assert.AreEqual(TestStock.Stock, 1);
            //Имя клиента
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Стоимость за штуку
            Assert.AreEqual(TestOrder.Price, 10);
            //Колличество
            Assert.AreEqual(TestOrder.Count, 20);
        }
    }
    [TestClass]
    public class TCStock_Collection
    {
        CController.CStock_Collection TestStockCollection = new CController.CStock_Collection();
        [TestMethod]
        public void CStock_Collection_Add_TEST()
        {
            //Имя клиента,Сток, Стоимость за штуку, Колличество
            TestStockCollection.Add("TestClient", 1, 10, 20);
            //Сток
            Assert.AreEqual(TestStockCollection.Stocks.Last().Stock, 1);
            //Имя клиента
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Name, "TestClient");
            //Стоимость за штуку
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Price, 10);
            //Колличество
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Count, 20);
        }
        [TestMethod]
        public void CStock_Collection_FindByStock_TEST()
        {
            //Имя клиента,Сток, Стоимость за штуку, Колличество
            TestStockCollection.Add("", 1, 0, 0);
            //Сток
            Assert.AreEqual(TestStockCollection.Stocks.Last().Stock, 1);
        }
    }
    [TestClass]
    public class TCController
    {
        CController TestController = new CController();
        [TestMethod]
        public void CController_SaveLoadStrings_TEST()
        {
            //Тест правильности сохранения\загрузки файла
            List<string> TestStrings = new List<string>();
            TestStrings.Add("C1	2760	0	0	0	0");
            TestStrings.Add("C2	4350	370	120	950	560");
            TestController.StringsListToFile("Test.txt", TestStrings);
            Assert.AreEqual(File.Exists("Test.txt"), true);
            TestStrings.Clear();
            TestStrings = TestController.FileToStringsList("Test.txt");
            Assert.AreEqual(TestStrings[0], "C1	2760	0	0	0	0");
            Assert.AreEqual(TestStrings[1], "C2	4350	370	120	950	560");
            File.Delete("Test.txt");
            Assert.AreEqual(File.Exists("Test.txt"), false);
            string TestString = "C1	2760	0	0	0	0\nC2	4350	370	120	950	560";
            TestController.StringToFile("Test.txt", TestString);
            TestStrings.Clear();
            TestStrings = TestController.FileToStringsList("Test.txt");
            Assert.AreEqual(TestStrings[0], "C1	2760	0	0	0	0");
            Assert.AreEqual(TestStrings[1], "C2	4350	370	120	950	560");
            Assert.AreEqual(File.Exists("Test.txt"), true);
            File.Delete("Test.txt");
            Assert.AreEqual(File.Exists("Test.txt"), false);
        }
        [TestMethod]
        public void CController_AccountSeveLoadFile_TEST()
        {
            TestController.Accounts.Add(new CController.CCustomer_account("TestClient	1000	130	240	760	320"));
            TestController.AccountSeveToFile("Test.txt");
            Assert.AreEqual(File.Exists("Test.txt"), true);
            TestController.AccountLoadFromFile("Test.txt");
            File.Delete("Test.txt");
            Assert.AreEqual(File.Exists("Test.txt"), false);
            CController.CCustomer_account pAccount = TestController.Accounts[0];
            //Имя клиента
            Assert.AreEqual(pAccount.Name, "TestClient");
            //Счет в $
            Assert.AreEqual(pAccount.Money, 1000);
            //Количество ценных бумаг A (0)
            Assert.AreEqual(pAccount.Stock[0], 130);
            //Количество ценных бумаг B (1)
            Assert.AreEqual(pAccount.Stock[1], 240);
            //Количество ценных бумаг C (2)
            Assert.AreEqual(pAccount.Stock[2], 760);
            //Количество ценных бумаг D (3)
            Assert.AreEqual(pAccount.Stock[3], 320);
        }
        [TestMethod]
        public void CController_AddOrder_TEST()
        {
            CController.COrder TestOrder = TestController.AddOrder("TestClient	b	C	15	4");
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Стоимость за штуку
            Assert.AreEqual(TestOrder.Price, 15);
            //Колличество
            Assert.AreEqual(TestOrder.Count, 4);
        }
        [TestMethod]
        public void CController_FindCustomerByName_TEST()
        {
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	50	320	220	90	60"));
            //Искомый счет
            TestController.Accounts.Add(new CController.CCustomer_account("FindClient	1	2	3	4	5"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	43	30	520	10	540"));
            TestController.Accounts.Add(new CController.CCustomer_account("Other1	412	31	11	923	420"));
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("FindClient");
            //Имя клиента
            Assert.AreEqual(pAccount.Name, "FindClient");
            //Счет в $
            Assert.AreEqual(pAccount.Money, 1);
            //Количество ценных бумаг A (0)
            Assert.AreEqual(pAccount.Stock[0], 2);
            //Количество ценных бумаг B (1)
            Assert.AreEqual(pAccount.Stock[1], 3);
            //Количество ценных бумаг C (2)
            Assert.AreEqual(pAccount.Stock[2], 4);
            //Количество ценных бумаг D (3)
            Assert.AreEqual(pAccount.Stock[3], 5);
        }
        [TestMethod]
        public void CController_SellOrBuy_TEST()
        {
            Assert.AreEqual(TestController.SellOrBuy("b"), TestController.OrdersBuyList);
            Assert.AreEqual(TestController.SellOrBuy("s"), TestController.OrdersSellList);
        }
        [TestMethod]
        public void CController_IntToString_TEST()
        {
            Assert.AreEqual(TestController.IntToString(0), "A");
            Assert.AreEqual(TestController.IntToString(1), "B");
            Assert.AreEqual(TestController.IntToString(2), "C");
            Assert.AreEqual(TestController.IntToString(3), "D");
        }
        [TestMethod]
        public void CController_StringToInt_TEST()
        {
            Assert.AreEqual(0, TestController.StringToInt("A"));
            Assert.AreEqual(1, TestController.StringToInt("B"));
            Assert.AreEqual(2, TestController.StringToInt("C"));
            Assert.AreEqual(3, TestController.StringToInt("D"));
        }
        [TestMethod]
        public void CController_Countin_TEST()
        {
            //Тест Jake Продает 50 своих акций А 10$ за штуку, Mike их покупает
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	0	100	2	3	4"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	501	1	2	3	4"));
            TestController.AddOrder("Jake	s	A	10	50");
            TestController.AddOrder("Mike	b	A	10	50");
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("Jake");
            TestController.Countin(false, false);
            //Имя клиента
            Assert.AreEqual(pAccount.Name, "Jake");
            //Счет в $
            Assert.AreEqual(pAccount.Money, 500);
            //Количество ценных бумаг A (0)
            Assert.AreEqual(pAccount.Stock[0], 50);
            //Количество ценных бумаг B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //Количество ценных бумаг C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //Количество ценных бумаг D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
            pAccount = TestController.FindCustomerByName("Mike");
            //Имя клиента
            Assert.AreEqual(pAccount.Name, "Mike");
            //Счет в $
            Assert.AreEqual(pAccount.Money, 1);
            //Количество ценных бумаг A (0)
            Assert.AreEqual(pAccount.Stock[0], 51);
            //Количество ценных бумаг B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //Количество ценных бумаг C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //Количество ценных бумаг D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
        }
    }
}
