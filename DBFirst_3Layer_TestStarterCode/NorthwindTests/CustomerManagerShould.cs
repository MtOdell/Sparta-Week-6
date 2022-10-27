using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData.Services;
using NorthwindData;
using System.Threading;

namespace NorthwindTests;

public class CustomerManagerShould
{
    private CustomerManager _sut;
    [Test]
    public void BeAbleToConstructCustomerManager()
    {
        _sut = new CustomerManager(null);
        Assert.That(_sut, Is.InstanceOf<CustomerManager>());
    }
    [Test]
    public void AbleToConstruct_UsingMoq()
    {
        var mockObj = new Mock<ICustomerService>();
        _sut = new CustomerManager(mockObj.Object);
        Assert.That(_sut, Is.InstanceOf<CustomerManager>());
    }
    [Category("happy path")]
    [Test]
    public void ReturnTrue_WhenUpdateCalled()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCust);
        _sut = new CustomerManager(mockObj.Object);

        var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        Assert.That(result);
    }
    [Category("happy path")]
    [Test]
    public void UpdateSelected_WhenUpdateCalled()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCust);
        _sut = new CustomerManager(mockObj.Object);

        var result = _sut.Update("MANDA", "Nishant Mandal", "Sparta Global", It.IsAny<string>(), It.IsAny<string>());
        Assert.That(result);
        Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nishant Mandal"));
    }
    [Category("sad path")]
    [Test]
    public void ReturnFalse_WhenUpdateCalledWhenInvalidId()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
        _sut = new CustomerManager(mockObj.Object);

        var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        Assert.That(result, Is.False);
    }
    [Category("sad path")]
    [Test]
    public void SelectedCustomerDoesNotChange_WhenUpdateCalled_WithValidId()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCust);
        _sut = new CustomerManager(mockObj.Object);
        _sut.SetSelectedCustomer(originalCust);
        var result = _sut.Update("MANDA", It.IsAny<string>(), "England", It.IsAny<string>(), It.IsAny<string>());
        Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("England"));
    }
    [Category("Happy path")]
    [Test]
    public void RemoveCustomer_RemovesCustomer_WhenValidId()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCust);
        _sut = new CustomerManager(mockObj.Object);

        var result = _sut.Delete("MANDA");
        Assert.That(result, Is.True);
        Assert.That(_sut.SelectedCustomer, Is.Null);
    }
    [Category("Happy path")]
    [Test]
    public void RemoveCustomer_NotRemovesCustomer_WhenNotValidId()
    {
        var mockObj = new Mock<ICustomerService>();
        var originalCust = new Customer { CustomerId = "MANDA" };
        mockObj.Setup(cs => cs.GetCustomerById("MANDA")).Returns((Customer)null);
        _sut = new CustomerManager(mockObj.Object);

        var result = _sut.Delete(It.IsAny<string>());
        Assert.That(result, Is.False);
    }
    [Test]
    public void RetrieveAllGetsAll()
    {
        var mockObj = new Mock<ICustomerService>();
        Customer testCustomer = new Customer 
        { 
            CustomerId = It.IsAny<string>(),
            ContactName = It.IsAny<string>(),
            CompanyName = It.IsAny<string>()
        };
        mockObj.Setup(cs => cs.GetCustomerList().Returns(new List<Customer> { testCustomer }));
        _sut = new CustomerManager(mockObj.Object);
        var result = _sut.RetrieveAll();
        Assert.That(result, Is.TypeOf<List<Customer>>());
    }
    [Test]
    public void SetSelectedTests()
    {
        var mockObj = new Mock<ICustomerService>();
        mockObj.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
        _sut = new CustomerManager(mockObj.Object);
        _sut.SetSelectedCustomer(It.IsAny<Object>());
        Assert.That(_sut.SelectedCustomer, Is.TypeOf<Customer>());
    }
    [Test]
    public void CustomerManagerCreate_CallsServiceMethod()
    {
        var mockObj = new Mock<ICustomerService>();
        mockObj.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
        _sut = new CustomerManager(mockObj.Object);
        var result = _sut.Delete(It.IsAny<string>());
        mockObj.Verify(cs => cs.RemoveCustomer(It.IsAny<Customer>()), Times.Once);
    }
    [Test]
    public void CustomerManagerDelete_CallsServiceMethod()
    {
        var mockObj = new Mock<ICustomerService>();
        mockObj.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
        _sut = new CustomerManager(mockObj.Object);
        var n = _sut.Create("MANDA", "Nish Mandal", "Sparta Global");
        mockObj.Verify(cs => cs.CreateCustomer(It.IsAny<Customer>()), Times.Once);
    }
}
