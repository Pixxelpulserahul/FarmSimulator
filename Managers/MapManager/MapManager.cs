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
using System.Data;



namespace FarmSimulator.Managers.MapManager
{
    public class MapManager
    {
        private MapLoader.MapLoader _mapLoader;

        //This has to be fixed according to map layers and size of the map if anything changes in the map data you have to change it here too..
        private int[,,] tiles = new int[8, 200, 300];


        private Dictionary<string, object> _map;
        
        public MapManager()
        {
            _mapLoader = new MapLoader.MapLoader();
            _map = new Dictionary<string, object>();
        }


        private void FetchData(string Path)
        {
           
            // This Block is just calling a function that is loading the map from the system.
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

        public void GettingStarted(String Path)
        {
            this.FetchData(Path);
        }        
        


        // The work of this function is to extract the Tiles data and convert it into usable list.
        public void ExtractTilesData()
        {

            foreach(var item in _map)
            {
                Console.WriteLine($"{(string)item.Key}");
            }


        }


    }
}
