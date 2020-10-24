using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tcp_server
{
    class Configuration
    {
        private static IConfigurationRoot config;

        static Configuration()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName);
            builder.AddJsonFile("appsettings.json");
            config = builder.Build();
        }

        public static object GetValue(Type type, string key)
        {
            return config.GetValue(type, key);
        }

        public static string GetConnectionString(string name)
        {
            return config.GetConnectionString(name);
        }
    }
}
