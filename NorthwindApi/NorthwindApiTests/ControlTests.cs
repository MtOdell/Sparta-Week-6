using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindApi.Services;
using NorthwindApi.Models;
using NorthwindApi.Controllers;
using Moq;
using NorthwindAPI.Models.DTO;
using Microsoft.Extensions.Logging;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindApiTests
{
    public class ControlTests
    {
        private SuppliersController _sut;
        [Test]
        public async Task CreateCallsAddAndSave()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.GetSuppliers()).Returns(new List<Supplier>());
            var result = await _sut.PostSupplier(It.IsAny<SupplierDTO>());
            mockObj.Verify(cs => cs.CreateSupplierAsync(It.IsAny<Supplier>()), Times.Once);
        }
        #region Delete
        [Test]
        public async Task DeleteCallsGetSupplierAndRemoveSupplier()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()).Result).Returns(new Supplier());
            var result = await _sut.DeleteSupplier(It.IsAny<int>());
            mockObj.Verify(cs => cs.RemoveSupplierAsync(It.IsAny<Supplier>()), Times.Once);
        }
        [Test]
        public async Task Delete_WhenIdBad_ReturnsNotFound()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()).Result).Returns((Supplier)null);
            var result = await _sut.DeleteSupplier(It.IsAny<int>());
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
        #endregion

        #region Update
        [Test]
        public async Task WhenIdMismatch_UpdateBadRequest()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            var result = await _sut.PutSupplier(1, new SupplierDTO { SupplierId = 2});
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
        [Test]
        public async Task UpdateCallsGetSupplierAndRemoveSupplier_WhenIdGood()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            var result = await _sut.PutSupplier(1, new SupplierDTO { SupplierId = 1, CompanyName = "a", ContactName = "b", ContactTitle = "c", Country = "d"});
            mockObj.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion
        [Test]
        public void SupplierExistsCallsSupplierExists()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            var result = _sut.SupplierExists(It.IsAny<int>());
            mockObj.Verify(cs => cs.SupplierExists(It.IsAny<int>()), Times.Once);
        }
        [Test]
        public void GetSuppliersCallsGetSuppliers()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.GetSuppliers()).Returns(new List<Supplier>());
            var result = _sut.GetSuppliers();
            mockObj.Verify(cs => cs.GetSuppliers(), Times.Once);
            Assert.That(result, Is.TypeOf<List<SupplierDTO>>());
        }
        #region GetSupplier
        [Test]
        public async Task GetSupplier_WhenIdGood_CallsGetSupplier()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.SupplierExists(It.IsAny<int>())).Returns(true);
            mockObj.Setup(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()).Result).Returns(new Supplier());
            var result = await _sut.GetSupplier(It.IsAny<int>());
            mockObj.Verify(cs => cs.GetSupplierByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.That(result.Value, Is.TypeOf<SupplierDTO>());
        }
        [Test]
        public async Task GetSupplier_WhenIdBad_ReturnsNotFound()
        {
            var mockObj = new Mock<IService>();
            var mockLogger = new Mock<ILogger<SuppliersController>>();            
            _sut = new SuppliersController(mockLogger.Object, mockObj.Object);
            mockObj.Setup(cs => cs.SupplierExists(It.IsAny<int>())).Returns(false);
            var result = await _sut.GetSupplier(It.IsAny<int>());
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }
        #endregion
    }
}
