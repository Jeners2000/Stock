using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock_Market;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//Тесты
namespace Stock_Market_UnitTests
{
    [TestClass]
    public class TCCustomer_account
    {
        //Áàçà äëÿ òåñòà
        CController.CCustomer_account TestCustomer = new CController.CCustomer_account("TestClient	1000	130	240	760	320");
        public void Equal_Account(CController.CCustomer_account pAccount, string Pname, int pMoney, int pA, int pB, int pC, int pD)
        {
            //Èìÿ êëèåíòà
            Assert.AreEqual(pAccount.Name, Pname);
            //Ñ÷åò â $
            Assert.AreEqual(pAccount.Money, pMoney);
            //Êîëè÷åñòâî öåííûõ áóìàã A (0)
            Assert.AreEqual(pAccount.Stock[0], pA);
            //Êîëè÷åñòâî öåííûõ áóìàã B (1)
            Assert.AreEqual(pAccount.Stock[1], pB);
            //Êîëè÷åñòâî öåííûõ áóìàã C (2)
            Assert.AreEqual(pAccount.Stock[2], pC);
            //Êîëè÷åñòâî öåííûõ áóìàã D (3)
            Assert.AreEqual(pAccount.Stock[3], pD);
        }
        [TestMethod]
        public void CCustomer_account_FieldsToString_TEST()
        {
            Assert.AreEqual(TestCustomer.FieldsToString(), "TestClient	1000	130	240	760	320");
        }
        [TestMethod]
        //Ñîçäàíèå ñ÷åòà êëèåíòà íà îñíîâå ôàéëîâîé çàïèñè
        public void CCustomer_account_Create_TEST()
        {
            Equal_Account(TestCustomer, "TestClient", 1000, 130, 240, 760, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Sell_TEST()
        {
            //Ïðîäàæà áóìàã C â êîëëè÷åñòâå 60 ñî ñ÷åòà êëèåíòà öåíîé 10$ çà øòóêó
            // -60 C + 600(60*10)$
            TestCustomer.Deal(2, -60, 10);
            Equal_Account(TestCustomer, "TestClient", 1600, 130, 240, 700, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Buy_TEST()
        {
            //Ïîêóïêà áóìàã A â êîëëè÷åñòâå 70 øòóê öåíîé 10$ çà øòóêó
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
            //Èìÿ êëèåíòà, Ñòîèìîñòü çà øòóêó, Êîëëè÷åñòâî
            CController.COrder TestOrder = new CController.COrder("TestClient", 10, 20);
            //Èìÿ êëèåíòà
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Ñòîèìîñòü çà øòóêó
            Assert.AreEqual(TestOrder.Price, 10);
            //Êîëëè÷åñòâî
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
            //Ñòîê
            Assert.AreEqual(TestStock.Stock, 1);
        }
        [TestMethod]
        public void CStock_Add_TEST()
        {
            //Èìÿ êëèåíòà, Ñòîèìîñòü çà øòóêó, Êîëëè÷åñòâî
            CController.COrder TestOrder = TestStock.Add("TestClient", 10, 20);
            //Ñòîê
            Assert.AreEqual(TestStock.Stock, 1);
            //Èìÿ êëèåíòà
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Ñòîèìîñòü çà øòóêó
            Assert.AreEqual(TestOrder.Price, 10);
            //Êîëëè÷åñòâî
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
            //Èìÿ êëèåíòà,Ñòîê, Ñòîèìîñòü çà øòóêó, Êîëëè÷åñòâî
            TestStockCollection.Add("TestClient", 1, 10, 20);
            //Ñòîê
            Assert.AreEqual(TestStockCollection.Stocks.Last().Stock, 1);
            //Èìÿ êëèåíòà
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Name, "TestClient");
            //Ñòîèìîñòü çà øòóêó
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Price, 10);
            //Êîëëè÷åñòâî
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Count, 20);
        }
        [TestMethod]
        public void CStock_Collection_FindByStock_TEST()
        {
            //Èìÿ êëèåíòà,Ñòîê, Ñòîèìîñòü çà øòóêó, Êîëëè÷åñòâî
            TestStockCollection.Add("", 1, 0, 0);
            //Ñòîê
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
            //Òåñò ïðàâèëüíîñòè ñîõðàíåíèÿ\çàãðóçêè ôàéëà
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
            //Èìÿ êëèåíòà
            Assert.AreEqual(pAccount.Name, "TestClient");
            //Ñ÷åò â $
            Assert.AreEqual(pAccount.Money, 1000);
            //Êîëè÷åñòâî öåííûõ áóìàã A (0)
            Assert.AreEqual(pAccount.Stock[0], 130);
            //Êîëè÷åñòâî öåííûõ áóìàã B (1)
            Assert.AreEqual(pAccount.Stock[1], 240);
            //Êîëè÷åñòâî öåííûõ áóìàã C (2)
            Assert.AreEqual(pAccount.Stock[2], 760);
            //Êîëè÷åñòâî öåííûõ áóìàã D (3)
            Assert.AreEqual(pAccount.Stock[3], 320);
        }
        [TestMethod]
        public void CController_AddOrder_TEST()
        {
            CController.COrder TestOrder = TestController.AddOrder("TestClient	b	C	15	4");
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //Ñòîèìîñòü çà øòóêó
            Assert.AreEqual(TestOrder.Price, 15);
            //Êîëëè÷åñòâî
            Assert.AreEqual(TestOrder.Count, 4);
        }
        [TestMethod]
        public void CController_FindCustomerByName_TEST()
        {
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	50	320	220	90	60"));
            //Èñêîìûé ñ÷åò
            TestController.Accounts.Add(new CController.CCustomer_account("FindClient	1	2	3	4	5"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	43	30	520	10	540"));
            TestController.Accounts.Add(new CController.CCustomer_account("Other1	412	31	11	923	420"));
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("FindClient");
            //Èìÿ êëèåíòà
            Assert.AreEqual(pAccount.Name, "FindClient");
            //Ñ÷åò â $
            Assert.AreEqual(pAccount.Money, 1);
            //Êîëè÷åñòâî öåííûõ áóìàã A (0)
            Assert.AreEqual(pAccount.Stock[0], 2);
            //Êîëè÷åñòâî öåííûõ áóìàã B (1)
            Assert.AreEqual(pAccount.Stock[1], 3);
            //Êîëè÷åñòâî öåííûõ áóìàã C (2)
            Assert.AreEqual(pAccount.Stock[2], 4);
            //Êîëè÷åñòâî öåííûõ áóìàã D (3)
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
            //Òåñò Jake Ïðîäàåò 50 ñâîèõ àêöèé À 10$ çà øòóêó, Mike èõ ïîêóïàåò
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	0	100	2	3	4"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	501	1	2	3	4"));
            TestController.AddOrder("Jake	s	A	10	50");
            TestController.AddOrder("Mike	b	A	10	50");
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("Jake");
            TestController.Countin(false, false);
            //Èìÿ êëèåíòà
            Assert.AreEqual(pAccount.Name, "Jake");
            //Ñ÷åò â $
            Assert.AreEqual(pAccount.Money, 500);
            //Êîëè÷åñòâî öåííûõ áóìàã A (0)
            Assert.AreEqual(pAccount.Stock[0], 50);
            //Êîëè÷åñòâî öåííûõ áóìàã B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //Êîëè÷åñòâî öåííûõ áóìàã C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //Êîëè÷åñòâî öåííûõ áóìàã D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
            pAccount = TestController.FindCustomerByName("Mike");
            //Èìÿ êëèåíòà
            Assert.AreEqual(pAccount.Name, "Mike");
            //Ñ÷åò â $
            Assert.AreEqual(pAccount.Money, 1);
            //Êîëè÷åñòâî öåííûõ áóìàã A (0)
            Assert.AreEqual(pAccount.Stock[0], 51);
            //Êîëè÷åñòâî öåííûõ áóìàã B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //Êîëè÷åñòâî öåííûõ áóìàã C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //Êîëè÷åñòâî öåííûõ áóìàã D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
        }
    }
}
