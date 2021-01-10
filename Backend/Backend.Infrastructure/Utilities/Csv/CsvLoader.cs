using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.Utilities.Csv
{
    public class CsvLoader<T, TMap> : ICsvLoader<T, TMap> 
        where T : class
        where TMap : ClassMap
    {
        public List<T> LoadCsvAsync(string filepath)
        {
            try
            {
                using var reader = new StreamReader(filepath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<TMap>();
                var records = csv.GetRecords<T>().ToList();

                return records;
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
