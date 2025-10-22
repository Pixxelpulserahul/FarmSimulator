using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FarmSimulator.Managers.TileHandler;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator.Managers.TextureLoader
{
    public class TileLoader
    {

        private List<Texture2D> sprites;


        public TileLoader()
        {
            sprites = new List<Texture2D>();
        }

        /// <summary>
        /// this function takes dictionary and return the array or list of the sprites loaded.
        /// </summary>
        public void loadTilesToArray(Dictionary<string, TileInfo> tilesData)
        {
            foreach (var pair in tilesData)
            {
                Console.WriteLine(pair.Key);
            }
        }


    }
}
