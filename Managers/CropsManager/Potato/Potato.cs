using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator
{

    public class Potato
    {
        public Texture2D texture;
        public int height;
        public int width;
        public int numberOfItems;
        public int numberOfSeeds;

        public Potato(Texture2D text)
        {
            this.texture = text;
            this.height = texture.Height;
            this.width = texture.Width;
            this.numberOfItems = 10;
            this.numberOfSeeds = 10;
        }

        public void increaseItem(int num = 1)
        {
            numberOfItems += num;
        }

        public void decreaseItem(int num = 1)
        {
            numberOfItems -= num;
        }

        public void increaseSeeds(int num = 1)
        {
            numberOfSeeds += num;
        }

        public void decreaseSeeds(int num = 1)
        {
            numberOfSeeds -= num;
        }

        public string seeds()
        {
            return numberOfSeeds.ToString();
        }

        public string item()
        {
            return numberOfItems.ToString();
        }
    }
}