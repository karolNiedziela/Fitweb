using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Unit.Fixtures
{
    public class RandomStringGenerator
    {
        public string RandomString { get; set; }

        public RandomStringGenerator(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double number = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * number));
                letter = Convert.ToChar(shift * 65);
                stringBuilder.Append(letter);
            }

            RandomString = stringBuilder.ToString();
        }
    }
}
