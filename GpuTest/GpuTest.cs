using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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
            Assert.AreEqual(expected, actual, 0.001m);
        }

        [TestMethod]
        [DataRow ("")]
        [DataRow ("asd")]
        [DataRow ("asdasdasdaasdasdasdaasdasdasdaasdasdasdaasd")]
        [DataRow ("фівфів")]
        [DataRow ("/asdasd")]
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

        [TestMethod]
        [DataRow (-20)]
        [DataRow (0)]
        public void LaunchPrice_ArgumentException(double price_double)
        {
            //Arrange
            decimal price = (decimal)price_double;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => gpu.LaunchPrice = price);
        }

        [TestMethod]
        public void LaunchPrice_ModelName_correct()
        {
            //Arrange
            decimal price = 399.99m;
            gpu.LaunchPrice = price;
            decimal expected = 399.99m;

            //Act
            decimal actual = gpu.LaunchPrice;

            //Assert
            Assert.AreEqual(expected, actual, 0.001m);
        }

        [TestMethod]
        [DataRow (true)]
        [DataRow (false)]
        public void PrintInfo(bool in_basket)
        {
            //Arrange
            gpu.ModelName = "Gigabyte GeForce RTX 5060 Ti";
            gpu.GpuClock = 2647;
            gpu.Architecture = GPUArchitecture.Blackwell;
            gpu.MemorySize = 16;
            gpu.MemoryBusWidth = 128;
            gpu.ReleaseDate = DateTime.Parse("01.04.2025");
            gpu.LaunchPrice = 470;

            if (in_basket)
            {
                gpu.AddToBasket();
            }
            else
            {
                gpu.DeleteFromBasket();
            }

            string expected = "Модель: Gigabyte GeForce RTX 5060 Ti\n"
                              + "GPU Clock: 2647 МГц\n"
                              + "Архітектура: Blackwell\n"
                              + "Пам'ять: 16 ГБ\n"
                              + "Розрядність шини: 128 біт\n"
                              + "Дата випуску: 01.04.2025\n"
                              + "Ціна на релізі: 470 $\n";

            if (in_basket)
            {
                expected += "Відеокарта знаходиться в кошику\n";
            }
            else
            {
                expected += "Відеокарта не знаходиться в кошику\n";
            }

            //Act
            string actual = gpu.PrintInfo();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString()
        {
            //Arrange
            gpu.ModelName = "Gigabyte GeForce RTX 5060 Ti";
            gpu.Architecture = GPUArchitecture.Blackwell;
            gpu.LaunchPrice = 470;

            string expected = "Gigabyte GeForce RTX 5060 Ti;Blackwell;470";

            //Act
            string actual = gpu.ToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
