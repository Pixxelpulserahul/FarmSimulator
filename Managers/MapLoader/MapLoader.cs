using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FarmSimulator.Managers.MapLoader
{
    public class MapLoader
    {

        private Dictionary<string, object> mapData;
        private JObject rawJson;

        public MapLoader()
        {
            mapData = new Dictionary<string, object>();
        }

        private bool LoadMap(string MapFilePath)
        {
            // This Function return true if there is no error while loading the json file of map..
            try
            {
                if (!File.Exists(MapFilePath))
                {
                    Console.WriteLine($"Error: File {MapFilePath} does Not exist");
                    return false;
                }
                string jsonContent = File.ReadAllText(MapFilePath);
                rawJson = JObject.Parse(jsonContent);

                // Here processing the raw Json files to usable dictionary using Process Data function
                mapData = ProcessMapData(rawJson);
                //mapData = rawJson.ToObject<Dictionary<string, object>>();
                if (mapData.Count == 0)
                {
                    Console.WriteLine(mapData);
                }

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error Parsing the file : {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading map: {ex.Message}");
                return false;
            }
            return true;
        }

        //Processing the json data into usable dictionary
        private Dictionary<string, object> ProcessMapData(JObject json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            var processData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var property in json.Properties())
            {
                string key = property.Name;
                object value = ConvertJtokenToObject(property.Value);

                processData[key] = value;    // or processData.Add(key, value);
            }

            return processData;
        }



        private object ConvertJtokenToObject(JToken Token)
        {
            switch (Token.Type)
            {
                case JTokenType.Object:
                    //Console.WriteLine(Token);
                    var dict = new Dictionary<string, object>();
                    foreach (var property in ((JObject)Token).Properties())
                    {
                        dict[property.Name] = ConvertJtokenToObject(property.Value);
                    }
                    return dict;
                case JTokenType.Array:
                    var list = new List<object>();
                    foreach (var item in (JArray)Token)
                    {
                        list.Add(ConvertJtokenToObject(item));
                    }
                    return list;
                case JTokenType.Integer:
                    return Token.Value<int>();
                case JTokenType.String:
                    return Token.Value<string>();
                case JTokenType.Boolean:
                    return Token.Value<bool>();
                case JTokenType.Null:
                    return null;
                default:
                    return Token.ToString();
            }
        }

        public Dictionary<string, object> GetMapData()
        {
            return mapData;
        }

        public bool Load(string path)
        {
            return LoadMap(path);
        }
    }
}
