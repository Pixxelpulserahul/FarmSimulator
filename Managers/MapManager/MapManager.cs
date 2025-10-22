using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using FarmSimulator.Managers.MapLoader;
using FarmSimulator.Managers.TileHandler;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Data;
using FarmSimulator.Managers.TextureLoader;



namespace FarmSimulator.Managers.MapManager
{
    public class MapManager
    {
        private MapLoader.MapLoader _mapLoader;
        private TileHandler.TileHandler _tileHandler;
        private TileLoader _tileLoader;


        //This has to be fixed according to map layers and size of the map if anything changes in the map data you have to change it here too..
        private const int layerCnt = 13;
        private const int mapHeight = 200;
        private const int mapWidth = 300;


        private int[,,] tiles;


        private Dictionary<string, object> _map;
        private Dictionary<int, TileInfo> _tileDict;


        public MapManager()
        {
            this._tileHandler = new TileHandler.TileHandler();
            this._mapLoader = new MapLoader.MapLoader();
            this._map = new Dictionary<string, object>();
            this._tileDict = new Dictionary<int, TileInfo>();
            this.tiles = new int[layerCnt, mapWidth, mapHeight];
        }


        private void FetchData(string Path)
        {

            // This Block is just calling a function that is loading the map from the system.
            bool dict = _mapLoader.Load(Path);
            if (dict)
            {
                
                this._map = _mapLoader.GetMapData();
                this.ConvertToMatrix(_map);

                _tileHandler.BuildDictionary((List<object>)_map["tilesets"]);


                _tileLoader.loadTilesToArray(_tileHandler.getTileData());
            }
            else
            {
                Console.WriteLine("Map data load unSuccess");
            }
        }


        public void GettingStarted(String Path)
        {
            this.FetchData(Path);
        }


        //The work of this function is to Convert a 1D array to 2D array..
        public void ConvertToMatrix(Dictionary<string, object> map)
        {
            try
            {
                for (int i = 0; i < layerCnt; i++)
                {
                    List<object> tileArr = (List<object>)(((Dictionary<string, object>)(((List<object>)map["layers"])[i]))["data"]);
                    //Console.WriteLine($"{tileArr[2]}");

                    for (int y = 0; y < mapHeight; y++)
                    {

                        for (int x = 0; x < mapHeight; x++)
                        {
                            int index = y * mapHeight + x;
                            this.tiles[i, y, x] = (int)tileArr[index];

                        }
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

            }

        }
    }


}

