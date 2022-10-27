using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindApi.Services;
using NorthwindApi.Models;

namespace NorthwindAPI.Tests
{
    public class ServiceTests
    {
        private NorthwindContext _context;
        private IService _sut;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "NorthwindDB").Options;
            _context = new NorthwindContext(options);
            _sut = new SupplierServiceLayer(_context);
            _sut.CreateSupplierAsync(new Supplier { SupplierId = 1, CompanyName = "Sparta Global", City = "Birmingham", Country = "UK", ContactName = "Nish Mandal", ContactTitle = "Manager" }).Wait();
            _sut.CreateSupplierAsync(new Supplier { SupplierId = 2, CompanyName = "Nintendo", City = "Tokyo", Country = "Japan", ContactName = "Shigeru Miyamoto", ContactTitle = "CEO" }).Wait();
        }

        [Test]
        public void GivenValidID_GetSupplierById_ReturnsCorrectSupplier()
        {
            var result = _sut.GetSupplierByIdAsync(1).Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        }

        [Test]
        public async Task CreateSupplier_Creates()
        {
            int countPrior = _context.Suppliers.Count();
            await _sut.CreateSupplierAsync(new Supplier { SupplierId = 3, CompanyName = "Nintendo", City = "Tokyo", Country = "Japan", ContactName = "Shigeru Miyamoto", ContactTitle = "CEO" });
            int countPost= _context.Suppliers.Count();
            Assert.That(countPost, Is.EqualTo(countPrior + 1));
            await _sut.RemoveSupplierAsync(_context.Suppliers.Find(3));
        }

        [Test]
        public async Task DeleteSupplier_Deletes()
        {
            int countPrior = _context.Suppliers.Count();
            await _sut.RemoveSupplierAsync(_context.Suppliers.Find(2));
            int countPost = _context.Suppliers.Count();
            Assert.That(countPost, Is.EqualTo(countPrior - 1));
            await _sut.CreateSupplierAsync(new Supplier { SupplierId = 2, CompanyName = "Nintendo", City = "Tokyo", Country = "Japan", ContactName = "Shigeru Miyamoto", ContactTitle = "CEO" });
        }
        [Test]
        public void GetSuppliers_ReturnsListOfSuppliers()
        {
            var result = _sut.GetSuppliers();
            Assert.That(result, Is.TypeOf<List<Supplier>>());
        }
        [Test]
        public async Task GetSupplierById_ReturnsSupplier()
        {
            var result = await _sut.GetSupplierByIdAsync(1);
            Assert.That(result, Is.TypeOf<Supplier>());
            Assert.That(result.Country, Is.EqualTo("UK"));
        }
    }
}