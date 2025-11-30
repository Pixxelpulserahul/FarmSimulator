using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator
{
    public class Orange
    {
        public Texture2D texture;
        public int height;
        public int width;
        public int numberOfItems;

        public Orange(Texture2D text)
        {
            this.texture = text;
            this.height = texture.Height;
            this.width = texture.Width;
            this.numberOfItems = 10;
        }

        public void decreaseItem(int num = 1)
        {
            numberOfItems -= num;
        }

        public void increaseItem(int num = 1)
        {
            numberOfItems += num;
        }

        public string item()
        {
            return numberOfItems.ToString();
        }
    }
}
