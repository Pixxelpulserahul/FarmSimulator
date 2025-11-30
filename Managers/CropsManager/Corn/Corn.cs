using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator
{
    public class Corn
    {
        public Texture2D texture;
        public int height;
        public int width;
        public int numberOfItems;

        public Corn(Texture2D text)
        {
            this.texture = text;
            this.height = texture.Height;
            this.width = texture.Width;
            numberOfItems = 10;
        }

        public void increaseItem(int num = 1)
        {
            numberOfItems += num;
        }

        public void decreaseitem(int num = 1)
        {
            numberOfItems -= num;
        }

        public string items()
        {
            return numberOfItems.ToString();
        }

    }
}
