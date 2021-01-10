using Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Core.Services
{
    public class RngService : IRngService
    {
        private static readonly string[] SpecialChars = { "/", "\\", "=", "+", "?", ":", "&" };

        public string Generate(int length = 30)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = Convert.ToBase64String(bytes);

            return SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, String.Empty));
        }
    }
}
