using NorthwindData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NorthwindData;
using Microsoft.EntityFrameworkCore;

namespace NorthwindTests;

public class CustomerServiceTests
{
    private CustomerService _sut;
    private NorthwindContext _context;
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName: "Example_DB")
            .Options;
        _context = new NorthwindContext(options);
        _sut = new CustomerService(_context);

        _sut.CreateCustomer(new Customer { CustomerId = "PHILL", ContactName = "Phillip Windridge", CompanyName = "Sparta Global", City = "London"});
        _sut.CreateCustomer(new Customer { CustomerId = "MANDA", ContactName = "Nishant Mandal", CompanyName = "Sparta Global", City = "London"});
    }
    [Test]
    public void GivenAValidID_CorrectCustomerIsReturned()
    {
        var result = _sut.GetCustomerById("PHILL");
        Assert.That(result, Is.TypeOf<Customer>());
        Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
    }
    [Test]
    public void WhenACustomerIsAdded_CreateCustomerAddsCustomer()
    {
        int count1 = _context.Customers.Count();
        var newCust = new Customer { CustomerId = "ODELL", ContactName = "Max Odell", CompanyName = "Sparta Global", City = "Surrey" };
        _sut.CreateCustomer(newCust);
        int count2 = _context.Customers.Count();

        Assert.That(count1, Is.EqualTo(count2 - 1));
    }
    [Test]
    public void GetCustomerListReturnsCustomerCount()
    {
        var GCLReturn = _sut.GetCustomerList();
        var GCLReturnCount = GCLReturn.Count();
        var DBCount = _context.Customers.Count();
        Assert.That(GCLReturn, Is.TypeOf<List<Customer>>());
        Assert.That(GCLReturnCount, Is.EqualTo(DBCount));
    }
    [Test]
    public void WhenCustomerRemoved_RemoveCustomerDo()
    {
        int count1 = _context.Customers.Count();
        var newCust = new Customer { CustomerId = "ODELL", ContactName = "Max Odell", CompanyName = "Sparta Global", City = "Surrey" };
        _sut.CreateCustomer(newCust);
        int count2 = _context.Customers.Count();
        var customerToDelete = _sut.GetCustomerById("ODELL");
        _sut.RemoveCustomer(customerToDelete);
        int count3 = _context.Customers.Count();
        var isDeleted = _sut.GetCustomerById("ODELL");

        Assert.That(count2, Is.EqualTo(count1 + 1));
        Assert.That(count3, Is.EqualTo(count1));
        Assert.That(isDeleted, Is.Null);
    }
}
