using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Labster
{
    [TestFixture]
    class RestaurantTests
    {
        [Test]
        public void SimpleTest()
        {
            var restaurant = new Restaurant(
                new OpeningHour(8, 16), // Sunday
                new OpeningHour(8, 17), // Monday
                new OpeningHour(8, 18), // Tuesday
                new OpeningHour(8, 19), // Wednesday
                new OpeningHour(8, 20), // Thursday
                new OpeningHour(8, 21), // Friday
                new OpeningHour(8, 22) // Saturday
            );

            Assert.AreEqual("Sun: 8-16, Mon: 8-17, Tue: 8-18, Wed: 8-19, Thu: 8-20, Fri: 8-21, Sat: 8-22",
                restaurant.GetOpeningHours());
        }

        [Test]
        public void SingleGroupTest()
        {
            var restaurant = new Restaurant(
                new OpeningHour(8, 16), // Sunday
                new OpeningHour(8, 16), // Monday
                new OpeningHour(8, 16), // Tuesday
                new OpeningHour(8, 16), // Wednesday
                new OpeningHour(8, 20), // Thursday
                new OpeningHour(8, 21), // Friday
                new OpeningHour(8, 22) // Saturday
            );

            Assert.AreEqual("Sun - Wed: 8-16, Thu: 8-20, Fri: 8-21, Sat: 8-22", restaurant.GetOpeningHours());
        }

        [Test]
        public void MultipleGroupsTest()
        {

            var restaurant = new Restaurant(
                new OpeningHour(8, 16), // Sunday
                new OpeningHour(8, 16), // Monday
                new OpeningHour(8, 16), // Tuesday
                new OpeningHour(8, 17), // Wednesday
                new OpeningHour(8, 18), // Thursday
                new OpeningHour(8, 20), // Friday
                new OpeningHour(8, 20) // Saturday
            );

            Assert.AreEqual("Sun - Tue: 8-16, Wed: 8-17, Thu: 8-18, Fri - Sat: 8-20", restaurant.GetOpeningHours());
        }

        [Test]
        public void EdgeCaseTest()
        {

            var restaurant = new Restaurant(
                new OpeningHour(8, 16), // Sunday
                new OpeningHour(8, 17), // Monday
                new OpeningHour(8, 17), // Tuesday
                new OpeningHour(8, 17), // Wednesday
                new OpeningHour(8, 16), // Thursday
                new OpeningHour(8, 16), // Friday
                new OpeningHour(8, 16) // Saturday
            );

            Assert.AreEqual("Sun, Thu - Sat: 8-16, Mon - Wed: 8-17", restaurant.GetOpeningHours());
        }
    }
}
