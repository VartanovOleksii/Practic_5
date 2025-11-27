System.Console.OutputEncoding = System.Text.Encoding.Unicode;

int maxCount = 0;
while (true)
{
    Console.Write("Введіть максимальну кількість відеокарт (N > 0): ");

    if (int.TryParse(Console.ReadLine(), out maxCount) && maxCount > 0)
        break;

    Console.WriteLine("Помилка: введіть додатне число!");
}

List<Gpu> gpus = new List<Gpu>(maxCount);

start_of_loop:
while (true)
{
    Console.WriteLine("\n==== МЕНЮ ====");
    Console.WriteLine("1 - Додати об'єкт");
    Console.WriteLine("2 - Переглянути додані об'єкти");
    Console.WriteLine("3 - Знайти об'єкт");
    Console.WriteLine("4 - Демонстрація поведінки");
    Console.WriteLine("5 - Видалити об'єкт");
    Console.WriteLine("6 - Демонстрація static-методів");
    Console.WriteLine("0 - Вихід");
    Console.Write("Ваш вибір -> ");

    string choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        //Додати об'єкт
        case "1":
            while (true)
            {
                if (Gpu.Counter >= maxCount)
                {
                    Console.WriteLine("Досягнута максимальна кількість об'єктів!");
                    break;
                }

                Console.WriteLine("\n==== МЕНЮ ====");
                Console.WriteLine("1 - Задано користувачем");
                Console.WriteLine("2 - Тест конструкторів");
                Console.WriteLine("3 - Створення через рядок");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш вибір -> ");

                string selectchoice = Console.ReadLine();
                Console.WriteLine();

                switch (selectchoice)
                {
                    //Задано користувачем
                    case "1":
                        try
                        {
                            Gpu vcard = AddGPU();
                            gpus.Add(vcard);
                            Console.WriteLine("Відеокарта успішно додана!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                        }
                        break;

                    //Тест конструкторів
                    case "2":
                        Random seed = new Random();
                        int constructor = seed.Next(3);

                        Gpu card = null;

                        switch (constructor)
                        {
                            case 0:
                                try
                                {
                                    card = new Gpu();
                                    gpus.Add(card);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }

                                if (card == null)
                                {
                                    Console.WriteLine("Об'єкт не було створено");
                                }

                                break;

                            case 1:
                                try
                                {
                                    card = new Gpu("GeForce RTX 5060 Ti", GPUArchitecture.Blackwell, 429.99m);
                                    gpus.Add(card);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }

                                if (card == null)
                                {
                                    Console.WriteLine("Об'єкт не було створено");
                                }

                                break;

                            case 2:
                                try
                                {
                                    var rDate = new DateTime(2025, 04, 16);
                                    card = new Gpu("GeForce RTX 5060 Ti", 2602, GPUArchitecture.Blackwell, 16, rDate, 128, 429.99m);
                                    gpus.Add(card);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }

                                if (card == null)
                                {
                                    Console.WriteLine("Об'єкт не було створено");
                                }

                                break;
                        }

                        break;

                    //Створення через рядок
                    case "3":
                        Console.WriteLine("Введіть інформацію про відеокарту в форматі \"Назва;Архітектура;Ціна\":");
                        string gpu = Console.ReadLine();
                        card = null;

                        if (Gpu.TryParse(gpu, out card))
                        {
                            gpus.Add(card);
                        }
                        else
                        {
                            Console.WriteLine("Об'єкт не було додано.");
                        }
                        break;

                    //Назад
                    case "0":
                        goto start_of_loop;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте знову.");
                        break;
                }
            }

            break;

        //Переглянути додані об'єкти
        case "2":
            if (Gpu.Counter == 0)
            {
                Console.WriteLine($"Лічильник {Gpu.Counter}\n");
                Console.WriteLine("Список порожній.");
            }
            else
            {
                Console.WriteLine($"Лічильник {Gpu.Counter}\n");
                Console.WriteLine("Список відеокарт:");
                foreach (var card in gpus)
                {
                    card.PrintInfo();
                    Console.WriteLine("\n");
                }
            }
            break;

        //Знайти об'єкт
        case "3":
            while (true)
            {
                Console.WriteLine("Виберіть параметр для пошуку:");
                Console.WriteLine("1 - Назва");
                Console.WriteLine("2 - Архітектура");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш вибір -> ");

                List<Gpu> findresults = new List<Gpu>();

                string findchoice = Console.ReadLine();
                Console.WriteLine();

                switch (findchoice)
                {
                    //Пошук за назвою
                    case "1":
                        Console.Write("Введіть назву моделі: ");
                        string searchName = Console.ReadLine();
                        findresults = gpus.FindAll(vc => vc.ModelName.Contains(searchName, StringComparison.OrdinalIgnoreCase));
                        break;


                    //Пошук за архітектурою
                    case "2":
                        Console.WriteLine("Доступні архітектури:");
                        foreach (var arch in Enum.GetValues(typeof(GPUArchitecture)))
                            Console.WriteLine($"- {arch}");

                        Console.Write("Введіть архітектуру: ");
                        string searchArch = Console.ReadLine();

                        if (Enum.TryParse(searchArch, true, out GPUArchitecture archValue))
                        {
                            findresults = gpus.FindAll(vc => vc.Architecture == archValue);
                        }
                        else
                        {
                            Console.WriteLine("Помилка: некоректна архітектура!");
                        }
                        break;

                    //Назад
                    case "0":
                        goto start_of_loop;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте знову.");
                        break;
                }

                Console.WriteLine("\n");

                if (findresults.Count > 0)
                {
                    foreach (Gpu res in findresults)
                    {
                        res.PrintInfo();
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Об'єкт не знайдено");
                }
            }

        //Демонстрація поведінки
        case "4":
            if (Gpu.Counter == 0)
            {
                Console.WriteLine("Додайте об'єкти для демонстрації поведінки");
            }
            while (true)
            {
                Console.WriteLine("\n==== МЕНЮ ====");
                Console.WriteLine("1 - Переглянути характеристики");
                Console.WriteLine("2 - Кількість років з релізу");
                Console.WriteLine("3 - Кількість років від релізу до заданої дати");
                Console.WriteLine("4 - Додати до кошику");
                Console.WriteLine("5 - Видалити з кошику");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш вибір -> ");

                string subchoice = Console.ReadLine();
                Console.WriteLine();

                switch (subchoice)
                {
                    //Переглянути характеристики
                    case "1":
                        gpus[0].PrintInfo();
                        break;

                    //Кількість років з релізу
                    case "2":
                        Console.WriteLine($"Пройшло {gpus[0].YearsSinceRelease()} років");
                        break;

                    //Кількість років від релізу до заданої дати
                    case "3":
                        Console.Write("Введіть дату: ");
                        var selectedDate = new DateTime();

                        try
                        {
                            if (DateTime.TryParse(Console.ReadLine(), out selectedDate))
                            {
                                Console.WriteLine($"Пройшло {gpus[0].YearsSinceRelease(selectedDate)} років");
                            }
                            else
                            {
                                throw new ArgumentException("Дата не коректна!");
                            }
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Дата не коректна!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    //Додати до кошику
                    case "4":
                        gpus[0].AddToBasket();
                        break;

                    //Видалити з кошику
                    case "5":
                        gpus[0].DeleteFromBasket();
                        break;

                    //Назад
                    case "0":
                        goto start_of_loop;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте знову.");
                        break;
                }
            }

        //Видалити об'єкт
        case "5":
            if (Gpu.Counter == 0)
            {
                Console.WriteLine("Список порожній.");
                break;
            }
            while (true)
            {
                Console.WriteLine("Виберіть параметр видалення:");
                Console.WriteLine("1 - Номер");
                Console.WriteLine("2 - Назва");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш вибір -> ");

                string deletechoice = Console.ReadLine();
                Console.WriteLine();

                switch (deletechoice)
                {
                    //Видалення за номером
                    case "1":
                        int index;

                        while (true)
                        {
                            Console.Write("Введіть номер об'єкту для видалення: ");

                            if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index < gpus.Count())
                                break;

                            Console.WriteLine("Помилка: введіть корректний індекс!");
                        }

                        gpus.RemoveAt(index);
                        Console.WriteLine("Об'єкт видалено.");
                        break;

                    //Видалення за назвою
                    case "2":
                        Console.Write("Введіть назву моделі для видалення:");
                        string deleteName = Console.ReadLine();

                        var removed = gpus.RemoveAll(vc => vc.ModelName.Equals(deleteName, StringComparison.OrdinalIgnoreCase));
                        Console.WriteLine(removed > 0 ? "Об'єкт видалено." : "Об'єкт не знайдено.");
                        break;

                    //Назад
                    case "0":
                        goto start_of_loop;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте знову.");
                        break;
                }
            }

        //Демонстрація static-методів
        case "6":
            if (Gpu.Counter == 0)
            {
                Console.WriteLine("Додайте об'єкти для демонстрації поведінки");
                break;
            }
            while (true)
            {
                Console.WriteLine("\n==== МЕНЮ ====");
                Console.WriteLine("1 - Ціна після уцінки");
                Console.WriteLine("2 - Метод ToString");
                Console.WriteLine("3 - Метод TryParse");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш вибір -> ");

                string staticchoice = Console.ReadLine();
                Console.WriteLine();

                switch (staticchoice)
                {
                    //Ціна після уцінки
                    case "1":
                        Console.Write("Введіть знижку уцінки (у відсотках від 0 до 100): ");
                        decimal discount;

                        try
                        {
                            if (decimal.TryParse(Console.ReadLine(), out discount))
                                Gpu.Discount = discount * 0.01m;


                            decimal result = Gpu.PriceWithDiscount(gpus[0].LaunchPrice);

                            Console.WriteLine($"Ціна відеокарти з уцінкою: {result}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                        }
                        break;

                    //Метод ToString
                    case "2":
                        Console.WriteLine("Результат роботи gpus[0].ToString()");
                        Console.WriteLine(gpus[0].ToString());
                        break;

                    //Метод TryParse
                    case "3":
                        Console.WriteLine("Введіть інформацію про відеокарту в форматі \"Назва;Архітектура;Ціна\":");
                        string gpu = Console.ReadLine();
                        Gpu testcase = null;

                        if (Gpu.TryParse(gpu, out testcase))
                        {
                            Console.WriteLine("Результат:");
                            testcase.PrintInfo();
                            maxCount++;
                        }
                        break;


                    //Назад
                    case "0":
                        goto start_of_loop;

                    default:
                        Console.WriteLine("Неправильний вибір, спробуйте знову.");
                        break;
                }
            }

        //Вихід
        case "0":
            Console.WriteLine("Вихід із програми...");
            return;

        default:
            Console.WriteLine("Неправильний вибір, спробуйте знову.");
            break;
    }
}

static Gpu AddGPU()
{
    Gpu vc = new Gpu();


    Console.Write("Введіть назву моделі: ");
    vc.ModelName = Console.ReadLine();


    Console.Write("Введіть частоту GPU (1000–4000): ");
    vc.GpuClock = int.Parse(Console.ReadLine());


    GPUArchitecture architecture;
    Console.WriteLine("Виберіть архітектуру: ");
    foreach (var arch in Enum.GetValues(typeof(GPUArchitecture)))
        Console.WriteLine($"- {arch}");
    Console.Write("Ваш вибір: ");
    if (Enum.TryParse<GPUArchitecture>(Console.ReadLine(), true, out architecture))
    {
        vc.Architecture = architecture;
    }
    else
    {
        throw new ArgumentException("Архітектура не коректна!");
    }


    Console.Write("Введіть обсяг пам'яті (1–32 ГБ): ");
    vc.MemorySize = int.Parse(Console.ReadLine());


    DateTime releaseDate;
    Console.Write("Введіть дату випуску: ");
    if (DateTime.TryParse(Console.ReadLine(), out releaseDate))
    {
        vc.ReleaseDate = releaseDate;
    }
    else
    {
        throw new ArgumentException("Дата не коректна!");
    }


    Console.Write("Введіть розрядність шини (128–2048 біт): ");
    vc.MemoryBusWidth = short.Parse(Console.ReadLine());


    Console.Write("Введіть ціну на релізі (>0$): ");
    vc.LaunchPrice = decimal.Parse(Console.ReadLine());


    return vc;
}