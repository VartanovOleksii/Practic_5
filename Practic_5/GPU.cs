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
                throw new ArgumentException("Архітектура не коректна.");
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
                throw new ArgumentException("Знижка повинна бути в діапазоні від 0 до 100%.");
            }
            _discount = value;
        }
    }


    //Публічні методи
    public void PrintInfo()
    {
        Console.WriteLine($"Модель: {ModelName}");
        Console.WriteLine($"GPU Clock: {GpuClock} МГц");
        Console.WriteLine($"Архітектура: {Architecture}");
        Console.WriteLine($"Пам'ять: {MemorySize} ГБ");
        Console.WriteLine($"Розрядність шини: {MemoryBusWidth} біт");
        Console.WriteLine($"Дата випуску: {ReleaseDate.ToShortDateString()}");
        Console.WriteLine($"Ціна на релізі: {LaunchPrice} $");
        if (InBasket) { Console.WriteLine("Відеокарта знаходиться в кошику"); }
        else { Console.WriteLine("Відеокарта не знаходиться в кошику"); }
    }

    public int YearsSinceRelease()
    {
        return DateTime.Now.Year - ReleaseDate.Year;
    }

    public int YearsSinceRelease(DateTime selectedDate)
    {
        return selectedDate.Year - ReleaseDate.Year;
    }

    public void AddToBasket()
    {
        if (!InBasket)
        {
            InBasket = true;
            Console.WriteLine("Відеокарта додана в кошик.");
        }
        else
        {
            Console.WriteLine("Відеокарта вже знаходиться в кошику.");
        }
    }

    public void DeleteFromBasket()
    {
        if (InBasket)
        {
            InBasket = false;
            Console.WriteLine("Відеокарта видалена з кошика.");
        }
        else
        {
            Console.WriteLine("Відеокарти не було в кошику.");
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
        Console.WriteLine("Використовується статичний конструктор.");
        Discount = 0.15m;
    }


    //Конструктори
    public Gpu() : this(DefName, DefClock, DefArchitecture, DefMemory, DateTime.MinValue, DefBus, DefPrice)
    {
        Console.WriteLine("Використовується конструктор без параметрів.");
    }

    public Gpu(string modelName, GPUArchitecture architecture, decimal launchPrice) : this(modelName, DefClock, architecture, DefMemory, DateTime.MinValue, DefBus, launchPrice)
    {
        Console.WriteLine("Використовується конструктор з параметрами: назва, архітектура, ціна на релізі.");
    }

    public Gpu(string modelName, int gpuClock, GPUArchitecture architecture, int memorySize, DateTime releaseDate, short memoryBusWidth, decimal launchPrice)
    {
        Console.WriteLine("Використовується конструктор зі всіма параметрами.");

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
            throw new FormatException("Архітектура не коректна.");

        if (!decimal.TryParse(part[2], out price))
            throw new FormatException("Значення ціни не коректне.");

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