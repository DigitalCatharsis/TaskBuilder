using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace TaskBuilder
{
    public class DatabaseReader
    {
        public DatabaseReader(string delimeter) 
        {
            Delimeter = delimeter;
        }
        public string Delimeter { get; set; }

        public Dictionary<string, Dictionary<string, string>> ReadData(string path)
        {
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> subDict = new Dictionary<string, string>();

            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = this.Delimeter, Encoding = Encoding.UTF8 };

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {

                var records = new List<DataInfo>();
                csv.Read();
                try { csv.ReadHeader(); }
                catch { Console.WriteLine("Чтения. Убедитесь, что разделитель выбран правильно, и файл не поврежден"); Console.ReadKey(); Environment.Exit(0); }
                try
                {
                    while (csv.Read())
                    {
                        var record = new DataInfo
                        {
                            Category = csv.GetField<string>("Category"),
                            SubCategoryNumber = csv.GetField<string>("SubCategoryNumber"),
                            SubCategoryContent = csv.GetField<string>("SubCategoryContent")
                        };

                        records.Add(record);
                    }
                }
                catch { Console.WriteLine("Ошибка чтения файла. Убедитесь, что категории файла указаны верно"); Console.ReadKey(); Environment.Exit(0); }

                try
                {
                    for (int i = 0; i < (records.Count); i++)      //Формирую на return словарь 
                    {
                        if (i < records.Count - 1)
                        {
                            if (records[i].Category == records[i + 1].Category)
                            {
                                subDict.Add(records[i].SubCategoryNumber, records[i].SubCategoryContent);
                            }
                            else
                            {
                                subDict.Add(records[i].SubCategoryNumber, records[i].SubCategoryContent);
                                var copied = new Dictionary<string, string>(subDict);
                                dict.Add(records[i].Category, copied);
                                subDict.Clear();
                            }
                        }
                        else
                        {
                            subDict.Add(records[i].SubCategoryNumber, records[i].SubCategoryContent);
                            dict.Add(records[i].Category, subDict);
                        }
                    }
                }
                catch { Console.WriteLine("Ошибка добавления подкатегории. Убедитесь, но номера в подкатегории не совпадают"); Console.ReadKey(); Environment.Exit(0); }

                return dict;
            }
        }
    }
}