using Newtonsoft.Json;


namespace Selenium_ProjectMars.Utilities
{
    public class JsonDataReader
    {
        public static T LoadJson<T>(string filePath)
        {
            if (!File.Exists(filePath)) 
                throw new FileNotFoundException($"JSON file not found: {filePath}");
            var json = File.ReadAllText(filePath); 
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
