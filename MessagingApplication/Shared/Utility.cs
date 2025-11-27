using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Utility
    {
        public static string GetConfigStringOrThrow(IConfiguration configuration, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(configuration[key]))
                    throw new ArgumentException($"Exchange Configuration {key} must be specified.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException($"Exchange Configuration {key} must be specified.");
            }
            return configuration[key] ?? "";
        }

        public static string GetConfigDictOrThrow(IConfiguration configuration, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(configuration[key]))
                    throw new ArgumentException($"Exchange Configuration {key} must be specified.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException($"Exchange Configuration {key} must be specified.");
            }
            return configuration[key] ?? "";
        }
    }
}
