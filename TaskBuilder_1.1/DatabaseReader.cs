using CsvHelper;
using System.Globalization;

namespace TaskBuilder
{
    public class DatabaseReader
    {

        public Dictionary<string, Dictionary<string, string>> ReadData(string path)
        {
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> subDict = new Dictionary<string, string>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<DataInfo>();
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = new DataInfo
                    {
                        Category = csv.GetField<String>("Category"),
                        SubCategoryNumber = csv.GetField<string>("SubCategoryNumber"),
                        SubCategoryContent = csv.GetField<String>("SubCategoryContent")
                    };

                    records.Add(record);
                }

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
                return dict;
            }
        }
    }
}