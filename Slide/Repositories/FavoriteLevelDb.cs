using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Windows.UI.WebUI;

namespace Slide.Repositories
{
    public static class FavoriteLevelDb
    {
        private const string dbFileName = "SliderFavoriteLevels.json";

        private static readonly JsonSerializerOptions jsonSerializerOptions = new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        private static Dictionary<string, int> dic = new();

        public static void Load()
        {
            if (File.Exists(dbFileName))
            {
                using StreamReader reader = File.OpenText(dbFileName);
                string content = reader.ReadToEnd();
                try
                {
                    dic = JsonSerializer.Deserialize<Dictionary<string, int>>(content, jsonSerializerOptions) ?? new();
                }
                catch
                {
                }
            }
        }

        public static void Save()
        {
            using StreamWriter writer = new(dbFileName);
            string content = JsonSerializer.Serialize(dic, jsonSerializerOptions);
            writer.Write(content);
        }

        public static void AddOrUpdate(string fileFullName, int level)
        {
            if (level == 0)
            {
                dic.Remove(fileFullName);
            }
            else
            {
                dic[fileFullName] = level;
            }
        }

        public static int GetLevel(string fileFullName)
        {
            return dic.TryGetValue(fileFullName, out int level) ? level : 0;
        }
    }
}
