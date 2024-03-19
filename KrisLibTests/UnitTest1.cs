using KrisLib;

namespace KrisLibTests;

public class Tests
{
    private Class1 _lib;
    [SetUp]
    public void Setup()
    {
        _lib = new Class1();
    }

    [Test]
    public void Login_ValidData_True()
    {
        //arrange
        string sql = "select user_id from user where login = @login and password = @password";
        string login = "test";
        string password = "test";
        bool expected = true;

        //act
        bool actual = _lib.Login(sql, login, password);
        
        //assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Login_InvalidData_False()
    {
        //arrange
        string sql = "select user_id from user where login = @login and password = @password";
        string login = "1212qwqwqw";
        string password = "1231weqq";
        bool expected = false;
        
        //act
        bool actual = _lib.Login(sql, login, password);
        
        //assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void AddOrder_ValidData_True()
    {
        //arrange
        int user = 5;
        int product = 2;
        int quantity = 1;
        decimal price = 350;
        decimal totalPrice = 350;
        int table = 1;
        bool expected = true;
        
        //act
        bool actual = _lib.AddOrder(user, product, quantity, price, totalPrice, table);
        
        //assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void AddOrder_InvalidData_False()
    {
        //arrange
        int user = -5;
        int product = -2;
        int quantity = -1;
        decimal price = -350;
        decimal totalPrice = -350;
        int table = -1;
        bool expected = false;
        
        //act
        bool actual = _lib.AddOrder(user, product, quantity, price, totalPrice, table);
        
        //assert
        Assert.AreEqual(expected, actual);
    }
}