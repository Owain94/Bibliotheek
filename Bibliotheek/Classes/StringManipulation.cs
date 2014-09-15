using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotheek.Classes
{
    public static class StringManipulation
    {
        /// <summary>
        /// Lowercase string. 
        /// </summary>
        public static string ToLowerFast(string value)
        {
            char[] output = value.ToCharArray();
            for (int i = 0; i < output.Length; i++)
            {
                if (output[i] >= 'A' &&
                output[i] <= 'Z')
                {
                    output[i] = (char)(output[i] + 32);
                }
            }
            return new string(output);
        }
    }
}