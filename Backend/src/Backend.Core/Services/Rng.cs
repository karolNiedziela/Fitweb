﻿using Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Core.Services
{
    public class Rng : IRng
    {
        // Chars which are replaced to string empty
        private static readonly string[] SpecialChars = { "/", "\\", "=", "+", "?", ":", "&" };

        public string Generate(int length = 30)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = Convert.ToBase64String(bytes);

            // Replacing SpecialChars with string empty
            return SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, String.Empty));
        }
    }
}
