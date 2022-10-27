using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Linq;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheNumberOfCustemersIncreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                int originalCount = db.Customers.Count();
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                int afterCount = db.Customers.Count();
                Assert.AreEqual(originalCount + 1, afterCount);
            }


        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var testCustomer = db.Customers.Find("MANDA");
                Assert.That(testCustomer.ContactName == "Nish Mandal");
            }
        }

        [Test]
        public void WhenACustomersDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global", "Paris");
                _customerManager.Update("MANDA", "Nish Mandal", "Sparta Global", "London", "Lon Don");
                var testCustomer = db.Customers.Find("MANDA");
                Assert.That(testCustomer.City == "London");
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Update("MANDA", "Nish Mandal", "Sparta Global", "London", "Lon Don");
                var upd = db.Customers.Find("MAND");
                Assert.That(upd.ContactName, Is.EqualTo(_customerManager.SelectedCustomer.ContactName));
            }
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            var test = _customerManager.Update("MAN", "Nish Man", "Sporta Global", "Landon", "LON DON");
            Assert.That(test, Is.False); ;
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                int originalCount = db.Customers.Count();
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                int afterCount = db.Customers.Count();
                Assert.AreEqual(originalCount + 1, afterCount);
                _customerManager.Delete("MANDA");
                int lastCount = db.Customers.Count();
                Assert.AreEqual(lastCount, originalCount);
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                _customerManager.Delete("MANDA");
                Assert.That(db.Customers.Find("MANDA"), Is.Null);
            }

        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}