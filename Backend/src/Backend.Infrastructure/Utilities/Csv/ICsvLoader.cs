using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Utilities.Csv
{
    public interface ICsvLoader<T, TMap> 
        where T : class
        where TMap : ClassMap
    {
       List<T> LoadCsvAsync(string filepath);
    }
}
