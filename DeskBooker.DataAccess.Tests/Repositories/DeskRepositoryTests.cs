using DeskBooker.Core.Domain;
using DeskBooker.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DeskBooker.DataAccess.Tests.Repositories
{
    [TestFixture]
    public class DeskRepositoryTests
    {
        [Test]
        public void should_return_the_available_desks()
        {
            // Arrange
            var date = new DateTime(2020, 1, 25);

            var options = new DbContextOptionsBuilder<DeskBookerContext>()
              .UseInMemoryDatabase(databaseName: "should_return_the_available_desks")
              .Options;

            using (var context = new DeskBookerContext(options))
            {
                context.Desk.Add(new Desk { Id = 3 });

                context.DeskBooking.Add(new DeskBooking { DeskId = 1, Date = date, FirstName = "Matt", LastName = "Brown", Email = "Matt@gmail.com" });
                context.DeskBooking.Add(new DeskBooking { DeskId = 2, Date = date.AddDays(1), FirstName = "John", LastName = "Nash", Email = "John@gmail.com" });

                context.SaveChanges();
            }

            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskRepository(context);

                // Act
                var desks = repository.GetAvailableDesks(date);

                // Assert
                Assert.AreEqual(2, desks.Count());
                Assert.IsTrue(desks.Any(d => d.Id == 2));
                Assert.IsFalse(desks.Any(d => d.Id == 1));
            }
        }

        [Test]
        public void should_get_all()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DeskBookerContext>()
              .UseInMemoryDatabase(databaseName: "should_get_all")
              .Options;

            // Act
            var desks=new List<Desk>();
            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskRepository(context);
                desks = repository.GetAll().ToList();
            }

            // Assert
            Assert.AreEqual(2, desks.Count());
        }
    }
}
