# Лабораторна робота №5

## Тема: МОДУЛЬНЕ ТЕСТУВАННЯ (UNIT TESTING)

**Виконав:**

* Студент: Вартанов Олексій Олександрович
* Група: 622п
* Освітня програма: 121 Інженерія програмного забезпечення
* Викладач: доц. Лучшев П.О.

---

## Мета роботи

* Навчитися застосовувати фреймворк MSTest для створення unit-тестів.

---

## Завдання

На основі отриманого на лекції 5 теоретичного матеріалу виконати модульне тестування (unit testing) розробленого класу, який був вдосконалений у практичній роботі Lab-4:

1.  У Solution, в якому реалізовано основний проект (реалізація практичної роботи Lab-4), додати проєкт для побудови модульних тестів MSTest.
2.  Для тест-класу MSTest-проєкта змінити назву за замовчуванням на відповідну назву (назва тест-класу має відповідати назві класу, який тестується).
3.  Розробити тест-методи (test methods) для:
    *	повного тестування логіки роботи всіх нестатичних і статичних методів класу;
    *	тестування роботи секції set декількох властивостей (секція set повинна мати реалізовані перевірки).
4. Запустити виконання розроблених тестів. Якщо деякі тести будуть непройдені (fail), то необхідно підкорегувати програмний код, який тестується, і досягти повного проходження всіх розроблених тестів.
   
### Має меню з пунктами:

* `1` – Додати об'єкт
* `2` – Переглянути всі об'єкти
* `3` – Знайти об'єкт
* `4` – Продемонструвати поведінку
* `5` – Видалити об'єкт
* `6` – Продемонструвати static-методи
* `0` – Вийти з програми

---

## Опис класу `Gpu`

### Характеристики:

* Назва відеокарти (`string`)
* Частота GPU (`int`)
* Архітектура (`enum GPUArchitecture`)
* Об'єм пам'яті (`int`)
* Розрядність шини (`short`)
* Дата випуску (`DateTime`)
* Ціна на релізі (`decimal`)
* У кошику (`bool`)

### Валідація:

* Назва повинна бути від 5 до 40 символів, складається з латинських літер, цифр та пробілу;
*	Частота GPU повинна бути в межах від 1000 до 4000 МГц;
*	Приймає тільки коректні архітектури з переліку enum;
*	Об'єм пам'яті повинен бути в межах від 1 до 32 Гб;
*	Дата випуску не може бути в майбутньому;
*	Розрядність шини має бути в діапазоні 128-2048 біт.
*	Ціна на релізі має бути більше 0.

### Поведінка:

* Розрахунок, скільки часу пройшло з виходу відеокарти;
* Розрахунок, скільки часу пройшло з виходу відеокарти до заданої дати;
* Розрахунок ціни зі знижкою;
* Додавання відеокарти до кошику;
* Видалення відеокарти з кошика.


### Тестування:

Створені testcase для тестування класу, а саме:
* Публічних методів классу;
* Публічних властивостей классу;
* Методів`Parse` і `TryPrase`.

###  Висновок:

Під час виконання роботи я навчився:
* Роботі з MSTest;
* Використовувати TestMethod та DataRow для тестування класу;
* Реалізовувати автоматичне тестування для свого класу.

---

## Програма реалізація класу

```csharp
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml;

public class Gpu
{
    //Приватні змінні
    private string _modelName;
    private int _gpuClock;
    private GPUArchitecture _architecture;
    private int _memorySize;
    private DateTime _releaseDate;
    private short _memoryBusWidth;
    private decimal _launchPrice;

    //Приватні статичні значення
    private static int _counter;
    private static decimal _discount;

    //Публічні дефолтні значення
    public const string DefName = "DefaultName";
    public const int DefClock = 1000;
    public const GPUArchitecture DefArchitecture = GPUArchitecture.Turing;
    public const int DefMemory = 1;
    public const short DefBus = 128;
    public const decimal DefPrice = 0.01m;


    //Публічні властивості
    public bool InBasket { get; private set; } = true;

    public string ModelName
    {
        get => _modelName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва моделі не може бути порожньою.");

            if (value.Length < 5 || value.Length > 40)
                throw new ArgumentException("Довжина назви моделі має бути від 5 до 40 символів.");

            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9\s]+$"))
                throw new ArgumentException("Назва моделі може містити лише латинські літери та цифри.");

            _modelName = value;
        }
    }

    public int GpuClock
    {
        get => _gpuClock;
        set
        {
            if (value < 1000 || value > 4000)
                throw new ArgumentException("Частота GPU має бути в діапазоні 1000-4000 МГц.");
            _gpuClock = value;
        }
    }

    public GPUArchitecture Architecture
    {
        get => _architecture;
        set
        {
            if (!Enum.IsDefined(typeof(GPUArchitecture), value))
            {
                throw new InvalidEnumArgumentException("Архітектура не коректна.");
            }
            _architecture = value;
        }
    }

    public int MemorySize
    {
        get => _memorySize;
        set
        {
            if (value < 1 || value > 32)
                throw new ArgumentException("Об'єм пам'яті має бути в діапазоні 1–32 ГБ.");
            _memorySize = value;
        }
    }

    public DateTime ReleaseDate
    {
        get => _releaseDate;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Дата випуску не може бути у майбутньому.");
            _releaseDate = value;
        }
    }

    public short MemoryBusWidth
    {
        get => _memoryBusWidth;
        set
        {
            if (value < 128 || value > 2048)
                throw new ArgumentException("Розрядність шини має бути в діапазоні 128-2048 біт.");
            _memoryBusWidth = value;
        }
    }

    public decimal LaunchPrice
    {
        get => _launchPrice;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Ціна на релізі має бути більше 0.");
            _launchPrice = value;
        }
    }


    //Публічні статичні властивості
    public static int Counter => _counter;

    public static decimal Discount
    {
        get { return _discount; }
        set
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException("Знижка повинна бути в діапазоні від 0 до 100%.");
            }
            _discount = value;
        }
    }


    //Публічні методи
    public string PrintInfo()
    {
        string result = string.Empty;
        result += ($"Модель: {ModelName}\n");
        result += ($"GPU Clock: {GpuClock} МГц\n");
        result += ($"Архітектура: {Architecture}\n");
        result += ($"Пам'ять: {MemorySize} ГБ\n");
        result += ($"Розрядність шини: {MemoryBusWidth} біт\n");
        result += ($"Дата випуску: {ReleaseDate.ToShortDateString()}\n");
        result += ($"Ціна на релізі: {LaunchPrice} $\n");

        if (InBasket)
        {
            result += ("Відеокарта знаходиться в кошику\n");
        }
        else
        {
            result += ("Відеокарта не знаходиться в кошику\n");
        }

        return result;
    }

    public int YearsSinceRelease()
    {
        return DateTime.Now.Year - ReleaseDate.Year;
    }

    public int YearsSinceRelease(DateTime selectedDate)
    {
        if (selectedDate.Year - ReleaseDate.Year < 0)
            return -1;
        return selectedDate.Year - ReleaseDate.Year;
    }

    public string AddToBasket()
    {
        if (!InBasket)
        {
            InBasket = true;
            return "Відеокарта додана в кошик.";
        }
        else
        {
            return "Відеокарта вже знаходиться в кошику.";
        }
    }

    public string DeleteFromBasket()
    {
        if (InBasket)
        {
            InBasket = false;
            return "Відеокарта видалена з кошика.";
        }
        else
        {
            return "Відеокарти не було в кошику.";
        }
    }


    //Публічні статичні методи
    public static decimal PriceWithDiscount(decimal price)
    {
        return price * (1 - Discount);
    }


    //Статичний конструктор
    static Gpu()
    {
        Discount = 0.15m;
    }


    //Конструктори
    public Gpu() : this(DefName, DefClock, DefArchitecture, DefMemory, DateTime.MinValue, DefBus, DefPrice)
    {
        
    }

    public Gpu(string modelName, GPUArchitecture architecture, decimal launchPrice) : this(modelName, DefClock, architecture, DefMemory, DateTime.MinValue, DefBus, launchPrice)
    {
        
    }

    public Gpu(string modelName, int gpuClock, GPUArchitecture architecture, int memorySize, DateTime releaseDate, short memoryBusWidth, decimal launchPrice)
    {
        ModelName = modelName;
        GpuClock = gpuClock;
        Architecture = architecture;
        MemorySize = memorySize;
        ReleaseDate = releaseDate;
        MemoryBusWidth = memoryBusWidth;
        LaunchPrice = launchPrice;

        _counter++;
    }


    //Parse та TryParce
    public override string ToString()
    {
        return $"{ModelName};{Architecture};{LaunchPrice}";
    }

    public static Gpu Parse(string s)
    {
        if (string.IsNullOrEmpty(s))
            throw new ArgumentNullException(null, "Строка не може бути нулем або пустою.");

        string[] part = s.Split(';');

        if (part.Length != 3)
            throw new FormatException("Строка неправильного формату.");

        GPUArchitecture arch;
        decimal price;

        if (!Enum.TryParse<GPUArchitecture>(part[1], true, out arch))
            throw new InvalidEnumArgumentException("Архітектура не коректна.");

        if (!decimal.TryParse(part[2], out price))
            throw new ArgumentException("Значення ціни не коректне.");

        return new Gpu(part[0], arch, price);
    }

    public static bool TryParse(string s, out Gpu gpu)
    {
        gpu = null;
        bool valid = false;

        try
        {
            gpu = Parse(s);
            valid = true;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TryParse: {ex.Message}");
        }

        return valid;
    }
}
```

---

## ПРОГРАМНА РЕАЛІЗАЦІЯ ТЕСТ-КЛАСУ

```csharp
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
```
