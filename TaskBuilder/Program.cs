using System.Diagnostics;
using System.Text;
using TaskBuilder.Properties;

namespace TaskBuilder
{
    internal class Program
    {
        static void Main()
        {


            var extConsole = new ExtConsoleTextColor();

            //Пользователь выбирает путь до файла
            var filepath = ChooseFileName();

            //Проверка, что файл существует
            while (!(CheckFileExist(filepath)))
            {
                extConsole.WriteLine(ConsoleColor.Red, $"Файла {filepath} не существует!");
                filepath = ChooseFileName();
            }

            //Проверка, что файл не занят1
            var fi1 = new FileInfo(filepath);

            while (IsFileLocked(fi1) == true)
            {
                extConsole.WriteLine(ConsoleColor.Red, $"Файл {filepath} запущен в другой программе. Чтобы продолжить требуется закрыть файл.");
                extConsole.WriteLine(ConsoleColor.Red, "После закрытия файла нажмите на любую клавишу для продолжения");
                Console.ReadKey();
            }

            extConsole.WriteLine(ConsoleColor.Red, filepath);

            //Считывание файла
            extConsole.WriteLine(ConsoleColor.Green, "Считывание файла...");

            var dataReader = new DatabaseReader(ChooseDelimeter(extConsole));
            var results = dataReader.ReadData(filepath);

            //генерируем preview
            var sb = UserPreviewGeneration(extConsole, results);

            extConsole.WriteLine(ConsoleColor.Green, "\n\nВывожу полученный результат для проверки:\n");
            extConsole.WriteLine(ConsoleColor.Yellow, sb);

            var acceptVariant = false;

            while (!acceptVariant)          //Пользователь выбирает понравился ли ему забитый вариант.
            {
                acceptVariant = ChooseYesOrNo(extConsole, "Устраивает ли вас результат?");
                if (!acceptVariant)   //Если не понравился, то опрос начнется заново
                {
                    sb = UserPreviewGeneration(extConsole, results);
                    extConsole.WriteLine(ConsoleColor.Green, "\n\nВывожу полученный результат для проверки:\n");
                    extConsole.WriteLine(ConsoleColor.Yellow, sb);
                }
            }

            //Пользователь согласился выше. Записываем в файл
            extConsole.WriteLine(ConsoleColor.Green, "Запись в файл");
            CreateWriteTxt(extConsole, sb);


            extConsole.WriteLine(ConsoleColor.Green, "Программа закончила работу.");
            Console.ReadKey();
        }

        static bool CheckFileExist(string filepath)
        {
            if (@File.Exists(filepath) && filepath != null)
            {
                return true;
            }
            return false;
        }

        static string GetExeLocation()
        {
            var path = Environment.ProcessPath;
            path = Environment.ProcessPath.Substring(0, path.LastIndexOf('\\'));
            return path;
        }

        static string ChooseFileName()
        {
            Console.WriteLine("Добрый день! Прошу указать имя файла для работы:");
            Console.WriteLine(" 1 - Формальные \t\n 2 - Внутренние \t\n 3 - Team3 \t\n 4 - Team4 \t\n 5 - Team5 \t\n 6 - Team6 \t\n 7 - Team7 \t\n 8 - Team8 \t\n\n 0 - Указать путь до файла\n\n \t\n\n Help - открыть инструкцию по пользованию ПО \n\n\t\n\n Converter - создать в каталоге с программой конвертер кодировки для фалов Microsoft Office\n\n");
            string filepath = Console.ReadLine();
            Console.WriteLine(filepath);

            switch (filepath)
            {
                case "1":
                    filepath = $"{GetExeLocation()}\\Формальные.csv";
                    return filepath;
                case "2":
                    filepath = $"{GetExeLocation()}\\Внутренние.csv";
                    return filepath;
                case "3":
                    filepath = $"{GetExeLocation()}\\Team3.csv";
                    return filepath;
                case "4":
                    filepath = $"{GetExeLocation()}\\Team4.csv";
                    return filepath;
                case "5":
                    filepath = $"{GetExeLocation()}\\Team5.csv";
                    return filepath;
                case "6":
                    filepath = $"{GetExeLocation()}\\Team6.csv";
                    return filepath;
                case "7":
                    filepath = $"{GetExeLocation()}\\Team7.csv";
                    return filepath;
                case "8":
                    filepath = $"{GetExeLocation()}\\Team8.csv";
                    return filepath;
                case "0":
                    Console.WriteLine(@"Прошу указать путь до файла. Например: C:\temp\MyCsvFile.csv");
                    filepath = Console.ReadLine();
                    return filepath;
                case "Converter":
                    var scriptPath = $"{GetExeLocation()}\\FileConverter.ps1";
                    var script = Resources.FileConverter;
                    File.WriteAllBytes(scriptPath, script);
                    Console.WriteLine("Конвертер создан");
                    Environment.Exit(0);
                    return null;
                case "Help":
                    filepath = $"{GetExeLocation()}\\Manual_dox.docx";
                    var Manual = Resources.Manual_dox;
                    File.WriteAllBytes(filepath, Manual);
                    Process.Start(new ProcessStartInfo(filepath) { UseShellExecute = true });
                    Environment.Exit(0);
                    return null;
                default:
                    Console.WriteLine("Что-то пошло не так, прошу выбрать заново выбрать вариантов");
                    Console.WriteLine("1 - Формальные \t 2 - Внутренние \t 3 - Team3 \t 4 - Team4 \t 5 - Team5 \t 6 - Team6 \t 7 - Team7 \t 8 - Team8 \t  0 - укзать путь до файла");
                    return ChooseFileName();
            }
        }

        static bool ChooseYesOrNo(ExtConsoleTextColor extConsole, string question)
        {
            extConsole.WriteLine(ConsoleColor.Red, question);
            extConsole.WriteLine(ConsoleColor.Red, $"y - Да; n - Нет");
            var temp = Console.ReadLine();

            switch (temp)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                case "Y":
                    return true;
                case "N":
                    return false;
                default:
                    Console.WriteLine("Что-то пошло не так, требуется заново выбрать один из вариантов");
                    return ChooseYesOrNo(extConsole, question);
            }
        }

        static StringBuilder UserPreviewGeneration(ExtConsoleTextColor extConsole, Dictionary<string, Dictionary<string, string>> results)
        {
            //Выборы подкатегорий
            extConsole.WriteLine(ConsoleColor.Green, "Запуск форматора");

            var sb = new StringBuilder();

            foreach (var item in results)
            {
                extConsole.WriteLine(ConsoleColor.Green, $"Выберите вариант для {item.Key}");
                foreach (var item2 in item.Value)
                {
                    extConsole.WriteLine($"\t {item2}");
                }

                while (true)
                {
                    try
                    {
                        var input = Console.ReadLine();
                        if (input == "")  //Если Enter, то переходим к следующей категории
                        {
                            break;
                        }

                        foreach (var elem in (CheckInputOnChoose(input))) //Мы работаем с массивом, так через запятую он может выбрать несколько вариантов
                        {
                            sb.Append($"{item.Key} \n \t {(results[item.Key][elem])} \n");
                        }
                        sb.Append('\n');
                        break;
                    }
                    catch
                    {
                        extConsole.WriteLine(ConsoleColor.Red, $"Ошибка в выборе категории. Попробуйте еще раз");
                    };
                }
            }
            return sb;
        }

        static string[] CheckInputOnChoose(string inputText)
        {
            string[] words;
            if (inputText.Length > 1)
            {
                words = inputText.Split(new char[] { ' ' });

                foreach (string s in words)
                {
                    Console.WriteLine(s);
                }
                return words;
            }
            else
            {
                words = new string[1];
                words[0] = inputText;

                return words;
            }
        }

        static void CreateWriteTxt(ExtConsoleTextColor extConsole, StringBuilder sb)
        {
            var date = ((DateTime.Now).ToString()).Replace(":", ".");
            var path = ($"{GetExeLocation()}\\Result {date}.doc");
            File.WriteAllText(path, sb.ToString());

            extConsole.WriteLine(ConsoleColor.Green, $"Файл записан по пути: {path}");

            extConsole.WriteLine(ConsoleColor.Green, "Открываю файл");

            //open file
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            //System.Diagnostics.Process.Start(path);
        }


        static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        static string ChooseDelimeter(ExtConsoleTextColor extConsole) 
        {
            extConsole.WriteLine(ConsoleColor.Red, "Укажите разделитель");
            extConsole.WriteLine(ConsoleColor.Red, $"; для документов созданных в Microsoft Office , для нормальных документов");
            var temp = Console.ReadLine();

            switch (temp)
            {
                case ";":
                    return temp;
                case ",":
                    return temp;
                default:
                    Console.WriteLine("Что-то пошло не так, требуется заново выбрать один из вариантов");
                    return ChooseDelimeter(extConsole);
            }
        }
    }
}