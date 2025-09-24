using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using FarmSimulator.Managers.MapLoader;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;



namespace FarmSimulator.Managers.MapManager
{
    public class MapManager
    {
        private MapLoader.MapLoader _mapLoader;

        private Dictionary<string, object> _map;
        
        public MapManager()
        {
            _mapLoader = new MapLoader.MapLoader();
            _map = new Dictionary<string, object>();
        }


        private void FetchData(string Path)
        {
            bool dict = _mapLoader.Load(Path);
            if(dict)
            {
                Console.WriteLine("Mission Successfully");
                _map = _mapLoader.GetMapData();

            }
            else
            {
                Console.WriteLine("Mission Failed Successfully");
            }


        }

        public void Print()
        {

            try
            {
                var dict = ((List<object>)_map["layers"]).ToArray();

                foreach (var item in dict)
                {
                    Array dict2 = ((List<object>)(((Dictionary<string, object>)item)["data"])).ToArray(); //accessing the data of each layer
                    Console.WriteLine(dict2.Length);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void GettingStarted(String Path)
        {
            this.FetchData(Path);
        }

    }
}
