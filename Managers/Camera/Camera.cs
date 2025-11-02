using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FarmSimulator.Managers.TileHandler;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace FarmSimulator.Managers.Camera
{

    public class Camera
    {
        public Vector2 position;
        public int viewPortHeight;
        public int viewPortWidth;

        public Camera(int viewPortWidth, int viewPortHeight)
        {
            this.viewPortHeight = viewPortHeight;
            this.viewPortWidth = viewPortWidth;
            this.position = new Vector2();
        }

    }
}
