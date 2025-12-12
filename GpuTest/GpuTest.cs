using System.Diagnostics.CodeAnalysis;

namespace GpuTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class GpuTest
    {
        private Gpu gpu;

        [TestInitialize]
        public void Setup()
        {
            gpu = new Gpu();
        }

        [TestCleanup]
        public void Cleanup()
        {
            gpu = null;
        }

        [TestMethod]
        public void YearsSinceRelease_with_date_selection_date_after_release()
        {
            //Arrange
            gpu.ReleaseDate = DateTime.Parse("18.05.2020");
            DateTime selected_date = DateTime.Parse("03.05.2024");
            int expected = 4;

            //Act
            int actual = gpu.YearsSinceRelease(selected_date);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void YearsSinceRelease_with_date_selection_date_before_release()
        {
            //Arrange
            gpu.ReleaseDate = DateTime.Parse("18.05.2020");
            DateTime selected_date = DateTime.Parse("03.05.1976");
            int expected = -1;

            //Act
            int actual = gpu.YearsSinceRelease(selected_date);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void YearsSinceRelease()
        {
            //Arrange
            gpu.ReleaseDate = DateTime.Parse("18.05.2020");
            int expected = DateTime.Now.Year - gpu.ReleaseDate.Year;

            //Act
            int actual = gpu.YearsSinceRelease();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddToBasket_was_in_basket()
        {
            //Arrange
            gpu.AddToBasket();
            string expected = "Відеокарта вже знаходиться в кошику.";

            //Act
            string actual = gpu.AddToBasket();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(gpu.InBasket);
        }

        [TestMethod]
        public void AddToBasket_was_not_in_basket()
        {
            //Arrange
            gpu.DeleteFromBasket();
            string expected = "Відеокарта додана в кошик.";

            //Act
            string actual = gpu.AddToBasket();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(gpu.InBasket);
        }

        [TestMethod]
        public void DeleteFromBasket_was_in_basket()
        {
            //Arrange
            gpu.AddToBasket();
            string expected = "Відеокарта видалена з кошика.";

            //Act
            string actual = gpu.DeleteFromBasket();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(gpu.InBasket);
        }

        [TestMethod]
        public void DeleteFromBasket_was_not_in_basket()
        {
            //Arrange
            gpu.DeleteFromBasket();
            string expected = "Відеокарти не було в кошику.";

            //Act
            string actual = gpu.DeleteFromBasket();

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(gpu.InBasket);
        }

        [TestMethod]
        [DataRow (1.2)]
        [DataRow (-0.2)]
        public void Discount(double discount_double)
        {
            //Arrange
            decimal discount = (decimal) discount_double;

            //Act

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Gpu.Discount = discount);
        }

        [TestMethod]
        public void PriceWithDiscount()
        {
            //Arrange
            decimal price = 100.0m;
            decimal discount = 0.2m;
            decimal expected = 80.0m;

            Gpu.Discount = discount;
            gpu.LaunchPrice = price;

            //Act
            decimal actual = Gpu.PriceWithDiscount(price);

            //Assert
            Assert.AreEqual(expected, actual, 0.01m);
        }

        [TestMethod]
        [DataRow ("")]
        [DataRow("asd")]
        [DataRow("asdasdasdaasdasdasdaasdasdasdaasdasdasdaasd")]
        [DataRow("фівфів")]
        [DataRow("/asdasd")]
        public void ModelName_ArgumentException(string name)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => gpu.ModelName = name);
        }

        [TestMethod]
        public void ModelName_correct()
        {
            //Arrange
            gpu.ModelName = "Gigabyte GeForce RTX 5060 Ti";
            string expected = "Gigabyte GeForce RTX 5060 Ti";

            //Act
            string actual = gpu.ModelName;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
