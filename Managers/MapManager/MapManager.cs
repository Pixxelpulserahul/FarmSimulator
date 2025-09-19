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



namespace FarmSimulator.Managers.MapManager
{
    public class MapManager
    {
        private MapLoader.MapLoader _mapLoader;

        private Dictionary<string, object> _map;

        private void FetchData(string Path)
        {
            bool dict = _mapLoader.Load(Path);
        }

        public void GettingStarted(String Path)
        {
            this.FetchData(Path);
        }

    }
}
