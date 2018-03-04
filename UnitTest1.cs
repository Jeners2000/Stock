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
        //���� ��� �����
        CController.CCustomer_account TestCustomer = new CController.CCustomer_account("TestClient	1000	130	240	760	320");
        public void Equal_Account(CController.CCustomer_account pAccount, string Pname, int pMoney, int pA, int pB, int pC, int pD)
        {
            //��� �������
            Assert.AreEqual(pAccount.Name, Pname);
            //���� � $
            Assert.AreEqual(pAccount.Money, pMoney);
            //���������� ������ ����� A (0)
            Assert.AreEqual(pAccount.Stock[0], pA);
            //���������� ������ ����� B (1)
            Assert.AreEqual(pAccount.Stock[1], pB);
            //���������� ������ ����� C (2)
            Assert.AreEqual(pAccount.Stock[2], pC);
            //���������� ������ ����� D (3)
            Assert.AreEqual(pAccount.Stock[3], pD);
        }
        [TestMethod]
        public void CCustomer_account_FieldsToString_TEST()
        {
            Assert.AreEqual(TestCustomer.FieldsToString(), "TestClient	1000	130	240	760	320");
        }
        [TestMethod]
        //�������� ����� ������� �� ������ �������� ������
        public void CCustomer_account_Create_TEST()
        {
            Equal_Account(TestCustomer, "TestClient", 1000, 130, 240, 760, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Sell_TEST()
        {
            //������� ����� C � ����������� 60 �� ����� ������� ����� 10$ �� �����
            // -60 C + 600(60*10)$
            TestCustomer.Deal(2, -60, 10);
            Equal_Account(TestCustomer, "TestClient", 1600, 130, 240, 700, 320);
        }
        [TestMethod]
        public void CCustomer_account_Deal_Buy_TEST()
        {
            //������� ����� A � ����������� 70 ���� ����� 10$ �� �����
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
            //��� �������, ��������� �� �����, �����������
            CController.COrder TestOrder = new CController.COrder("TestClient", 10, 20);
            //��� �������
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //��������� �� �����
            Assert.AreEqual(TestOrder.Price, 10);
            //�����������
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
            //����
            Assert.AreEqual(TestStock.Stock, 1);
        }
        [TestMethod]
        public void CStock_Add_TEST()
        {
            //��� �������, ��������� �� �����, �����������
            CController.COrder TestOrder = TestStock.Add("TestClient", 10, 20);
            //����
            Assert.AreEqual(TestStock.Stock, 1);
            //��� �������
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //��������� �� �����
            Assert.AreEqual(TestOrder.Price, 10);
            //�����������
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
            //��� �������,����, ��������� �� �����, �����������
            TestStockCollection.Add("TestClient", 1, 10, 20);
            //����
            Assert.AreEqual(TestStockCollection.Stocks.Last().Stock, 1);
            //��� �������
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Name, "TestClient");
            //��������� �� �����
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Price, 10);
            //�����������
            Assert.AreEqual(TestStockCollection.Stocks.Last().Orders.Last().Count, 20);
        }
        [TestMethod]
        public void CStock_Collection_FindByStock_TEST()
        {
            //��� �������,����, ��������� �� �����, �����������
            TestStockCollection.Add("", 1, 0, 0);
            //����
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
            //���� ������������ ����������\�������� �����
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
            //��� �������
            Assert.AreEqual(pAccount.Name, "TestClient");
            //���� � $
            Assert.AreEqual(pAccount.Money, 1000);
            //���������� ������ ����� A (0)
            Assert.AreEqual(pAccount.Stock[0], 130);
            //���������� ������ ����� B (1)
            Assert.AreEqual(pAccount.Stock[1], 240);
            //���������� ������ ����� C (2)
            Assert.AreEqual(pAccount.Stock[2], 760);
            //���������� ������ ����� D (3)
            Assert.AreEqual(pAccount.Stock[3], 320);
        }
        [TestMethod]
        public void CController_AddOrder_TEST()
        {
            CController.COrder TestOrder = TestController.AddOrder("TestClient	b	C	15	4");
            Assert.AreEqual(TestOrder.Name, "TestClient");
            //��������� �� �����
            Assert.AreEqual(TestOrder.Price, 15);
            //�����������
            Assert.AreEqual(TestOrder.Count, 4);
        }
        [TestMethod]
        public void CController_FindCustomerByName_TEST()
        {
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	50	320	220	90	60"));
            //������� ����
            TestController.Accounts.Add(new CController.CCustomer_account("FindClient	1	2	3	4	5"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	43	30	520	10	540"));
            TestController.Accounts.Add(new CController.CCustomer_account("Other1	412	31	11	923	420"));
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("FindClient");
            //��� �������
            Assert.AreEqual(pAccount.Name, "FindClient");
            //���� � $
            Assert.AreEqual(pAccount.Money, 1);
            //���������� ������ ����� A (0)
            Assert.AreEqual(pAccount.Stock[0], 2);
            //���������� ������ ����� B (1)
            Assert.AreEqual(pAccount.Stock[1], 3);
            //���������� ������ ����� C (2)
            Assert.AreEqual(pAccount.Stock[2], 4);
            //���������� ������ ����� D (3)
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
            //���� Jake ������� 50 ����� ����� � 10$ �� �����, Mike �� ��������
            TestController.Accounts.Add(new CController.CCustomer_account("Jake	0	100	2	3	4"));
            TestController.Accounts.Add(new CController.CCustomer_account("Mike	501	1	2	3	4"));
            TestController.AddOrder("Jake	s	A	10	50");
            TestController.AddOrder("Mike	b	A	10	50");
            CController.CCustomer_account pAccount = TestController.FindCustomerByName("Jake");
            TestController.Countin(false, false);
            //��� �������
            Assert.AreEqual(pAccount.Name, "Jake");
            //���� � $
            Assert.AreEqual(pAccount.Money, 500);
            //���������� ������ ����� A (0)
            Assert.AreEqual(pAccount.Stock[0], 50);
            //���������� ������ ����� B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //���������� ������ ����� C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //���������� ������ ����� D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
            pAccount = TestController.FindCustomerByName("Mike");
            //��� �������
            Assert.AreEqual(pAccount.Name, "Mike");
            //���� � $
            Assert.AreEqual(pAccount.Money, 1);
            //���������� ������ ����� A (0)
            Assert.AreEqual(pAccount.Stock[0], 51);
            //���������� ������ ����� B (1)
            Assert.AreEqual(pAccount.Stock[1], 2);
            //���������� ������ ����� C (2)
            Assert.AreEqual(pAccount.Stock[2], 3);
            //���������� ������ ����� D (3)
            Assert.AreEqual(pAccount.Stock[3], 4);
        }
    }
}
