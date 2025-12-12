using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GpuTest
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.ComponentModel;
    using System.Xml.Linq;

    [TestClass]
    public sealed class GpuTest
    {
        private Gpu gpu;

        //Сетап
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

        //Властивості
        [TestMethod]
        [DataRow("")]
        [DataRow("asd")]
        [DataRow("asdasdasdaasdasdasdaasdasdasdaasdasdasdaasd")]
        [DataRow("фівфів")]
        [DataRow("/asdasd")]
        public void ModelName_incorrect_name(string name)
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
        [DataRow (100)]
        [DataRow (10000)]
        public void GpuClock_incorrect_value(int clock)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => gpu.GpuClock = clock);
        }

        [TestMethod]
        public void GpuClock_correct()
        {
            //Arrange
            gpu.GpuClock = 2367;
            int expected = 2367;

            //Act
            int actual = gpu.GpuClock;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(-2)]
        [DataRow(50)]
        public void MemorySize_incorrect_value(int size)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => gpu.MemorySize = size);
        }

        [TestMethod]
        public void MemorySize_correct()
        {
            //Arrange
            gpu.MemorySize = 16;
            int expected = 16;

            //Act
            int actual = gpu.MemorySize;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(10000)]
        public void MemoryBusWidth_incorrect_value(int width_int)
        {
            //Arrange
            short width = (short)width_int;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => gpu.MemoryBusWidth = width);
        }

        [TestMethod]
        public void MemoryBusWidth_correct()
        {
            //Arrange
            gpu.MemoryBusWidth = 128;
            int expected = 128;

            //Act
            int actual = gpu.MemoryBusWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(-20)]
        [DataRow(0)]
        public void LaunchPrice_less_then_0(double price_double)
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

        //Метод
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

        [TestMethod]
        public void Parse_empty_string()
        {
            //Arrange
            string s = null;

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => Gpu.Parse(s));
        }

        [TestMethod]
        [DataRow ("asdasd")]
        [DataRow ("asdasd;asdasd")]
        [DataRow ("asdasd;asdasd;asdasd;asdasd")]
        public void Parse_incorrect_format(string s)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws< FormatException>(() => Gpu.Parse(s));
        }

        [TestMethod]
        [DataRow("asdasd;;100")]
        [DataRow("asdasd;asdasd;100")]
        [DataRow("asdasd;20;100")]
        public void Parse_incorrect_architecture(string s)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<InvalidEnumArgumentException>(() => Gpu.Parse(s));
        }

        [TestMethod]
        [DataRow("asdasd;Turing;")]
        [DataRow("asdasd;Turing;asd")]
        public void Parse_incorrect_price(string s)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => Gpu.Parse(s));
        }

        [TestMethod]
        [DataRow(";Turing;100")]
        [DataRow("asd;Turing;100")]
        [DataRow("asdasdasdaasdasdasdaasdasdasdaasdasdasdaasd;Turing;100")]
        [DataRow("фівфів;Turing;100")]
        [DataRow("/asdasd;Turing;100")]
        public void Parse_incorrect_name(string s)
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => Gpu.Parse(s));
        }

        [TestMethod]
        public void Parse_correct()
        {
            //Arrange
            string s = "Gigabyte GeForce RTX 5060 Ti;Blackwell;470";
            Gpu expected = new Gpu();
            expected.ModelName = "Gigabyte GeForce RTX 5060 Ti";
            expected.Architecture = GPUArchitecture.Blackwell;
            expected.LaunchPrice = 470;

            //Act
            Gpu actual = Gpu.Parse(s);

            //Assert
            Assert.AreEqual(expected.ModelName, actual.ModelName);
            Assert.AreEqual(expected.Architecture, actual.Architecture);
            Assert.AreEqual(expected.LaunchPrice, actual.LaunchPrice);
        }

        [TestMethod]
        [DataRow ("")]

        //size
        [DataRow("asdasd")]
        [DataRow("asdasd;asdasd")]
        [DataRow("asdasd;asdasd;asdasd;asdasd")]

        //architecture
        [DataRow("asdasd;;100")]
        [DataRow("asdasd;asdasd;100")]
        [DataRow("asdasd;20;100")]

        //price
        [DataRow("asdasd;Turing;")]
        [DataRow("asdasd;Turing;asd")]

        //name
        [DataRow(";Turing;100")]
        [DataRow("asd;Turing;100")]
        [DataRow("asdasdasdaasdasdasdaasdasdasdaasdasdasdaasd;Turing;100")]
        [DataRow("фівфів;Turing;100")]
        [DataRow("/asdasd;Turing;100")]
        public void TryParse_incorrect(string s)
        {
            //Arrange
            Gpu parsedGpu = null;

            //Act
            bool actual = Gpu.TryParse(s, out parsedGpu);

            //Assert
            Assert.IsFalse(actual);
            Assert.IsNull(parsedGpu);
        }

        [TestMethod]
        public void TryParse_correct()
        {
            //Arrange
            Gpu parsedGpu = null;
            Gpu expected = new Gpu();
            string s = "Gigabyte GeForce RTX 5060 Ti;Blackwell;470";

            expected.ModelName = "Gigabyte GeForce RTX 5060 Ti";
            expected.Architecture = GPUArchitecture.Blackwell;
            expected.LaunchPrice = 470;

            //Act
            bool actual = Gpu.TryParse(s, out parsedGpu);

            //Assert
            Assert.IsTrue(actual);
            Assert.IsNotNull(parsedGpu);

            Assert.AreEqual(expected.ModelName, parsedGpu.ModelName);
            Assert.AreEqual(expected.Architecture, parsedGpu.Architecture);
            Assert.AreEqual(expected.LaunchPrice, parsedGpu.LaunchPrice);
        }
    }
}
