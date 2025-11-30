using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator
{
    public class Coin
    {
        public Texture2D texture;
        public int height;
        public int width;
        public int numberOfItems;

        public Coin(Texture2D text)
        {
            this.texture = text;
            this.height = texture.Height;
            this.width = texture.Width;
            numberOfItems = 50;
        }

        public void increaseItem(int num = 1)
        {
            numberOfItems += num;
        }

        public void decreaseItem(int num = 1)
        {
            numberOfItems -= num;
        }

        public string item()
        {
            return numberOfItems.ToString();
        }

    }
}
