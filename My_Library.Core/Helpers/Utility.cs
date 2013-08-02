using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace My_Library.Core.Helpers
{
    public static class Utility
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static Task<byte[]> ReadFileAsync(string filePath, int bufferSize = 64 * 1024)
        {
            var fi = new FileInfo(filePath);
            var buffer = new byte[fi.Length];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize,
                                          FileOptions.Asynchronous))
            {
                return file.ReadAsync(buffer, 0, buffer.Length).ContinueWith(t => buffer);
            }

        }

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();

            //var field = fields
            //    .SelectMany(f => f.GetCustomAttributes(
            //        typeof(DescriptionAttribute), false), (
            //            f, a) => new { Field = f, Att = a })
            //    .Where(a => ((DescriptionAttribute)a.Att)
            //        .Description == description).SingleOrDefault();

            var field = fields
                .SelectMany(f => f.GetCustomAttributes(
                    typeof(DescriptionAttribute), false), (
                        f, a) => new { Field = f, Att = a }).SingleOrDefault(a => ((DescriptionAttribute)a.Att)
                            .Description == description);

            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static bool IsWithin(int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}
