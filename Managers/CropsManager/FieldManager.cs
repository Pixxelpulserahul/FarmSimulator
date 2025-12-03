using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarmSimulator
{
    public class FieldManager
    {
        private int[,] fieldData;
        public int[,] tomatoData;
        public int[,] cornData;
        public int[,] orangeData;
        public int[,] potatoData;

        public Tomato _tomato;
        public Corn _corn;
        public Orange _orange;
        public Potato _potato;
        private string inventory;
        
        private float timer = 0;

        Random rand = new Random();
        
        private Dictionary<string, Microsoft.Xna.Framework.Vector2> plantedCrop = new Dictionary<string, Microsoft.Xna.Framework.Vector2>();

        public FieldManager(int[,] fieldData, Tomato tomato, Corn corn, Potato potato, Orange orange)
        {
            this.fieldData = fieldData;
            this._tomato = tomato;
            this._corn = corn;
            this._orange = orange;
            this._potato = potato;

            this.tomatoData = new int[200 * 16, 300 * 16];
            this.cornData = new int[200 * 16, 300 * 16];
            this.orangeData = new int[200 * 16, 300 * 16];
            this.potatoData = new int[200 * 16, 300 * 16];
        }

        public void SowCrop(KeyboardState KS, float playerPosX, float playerPosY, string currentItem, GameTime gameTime)
        {
            int locX = (int)playerPosX / 16;
            int locY = (int)playerPosY / 16;

            inventory = currentItem;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(timer > 5f)
            {

                foreach (var (key, value) in plantedCrop)
                {
                    if (tomatoData[(int)value.X, (int)value.Y] > 1) tomatoData[(int)value.X, (int)value.Y] -= 1;
                    if (cornData[(int)value.X, (int)value.Y] > 1) cornData[(int)value.X, (int)value.Y] -= 1;
                    if (potatoData[(int)value.X, (int)value.Y] > 1) potatoData[(int)value.X, (int)value.Y] -= 1;
                    if (orangeData[(int)value.X, (int)value.Y] > 1) orangeData[(int)value.X, (int)value.Y] -= 1;
                }

                timer = 0f;
                Console.WriteLine("Timer Reset!!!!");
            }


            if (KS.IsKeyDown(Keys.L))
            {
                bool canSowTomato = (cornData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Tomato") && (tomatoData[locY, locX] < 1) && (_tomato.numberOfItems > 0);
                bool canSowCorn = (tomatoData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Corn") && (cornData[locY, locX] < 1) && (_corn.numberOfItems > 0);
                bool canSowOrange = (tomatoData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (cornData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Orange") && (orangeData[locY, locX] < 1) && (_orange.numberOfItems > 0);
                bool canSowPotato = (tomatoData[locY, locX] == 0) && (cornData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Potato") && (potatoData[locY, locX] < 1) && (_potato.numberOfItems > 0);

                if (canSowTomato)
                {
                    tomatoData[locY, locX] = 4;
                    plantedCrop.Add(new Microsoft.Xna.Framework.Vector2(locY, locX).ToString(), new Microsoft.Xna.Framework.Vector2(locY, locX));
                    _tomato.decreaseItem();
                }

                if (canSowCorn)
                {
                    cornData[locY, locX] = 4;
                    plantedCrop.Add(new Microsoft.Xna.Framework.Vector2(locY, locX).ToString(), new Microsoft.Xna.Framework.Vector2(locY, locX));
                    _corn.decreaseitem();

                }

                if (canSowPotato)
                {
                    potatoData[locY, locX] = 4;
                    plantedCrop.Add(new Microsoft.Xna.Framework.Vector2(locY, locX).ToString(), new Microsoft.Xna.Framework.Vector2(locY, locX));

                    _potato.decreaseItem();
                }

                if (canSowOrange)
                {
                    orangeData[locY, locX] = 4;
                    plantedCrop.Add(new Microsoft.Xna.Framework.Vector2(locY, locX).ToString(), new Microsoft.Xna.Framework.Vector2(locY, locX));
                    _orange.decreaseItem();
                }
            }
        }

        public void HarvestCrop(KeyboardState ks, Microsoft.Xna.Framework.Vector2 pos)
        {

            if(ks.IsKeyDown(Keys.H))
            {
                int x = (int)(pos.X / 16);
                int y = (int)(pos.Y / 16);

                Console.WriteLine($"Corn data = {cornData[y, x]}");

                if (cornData[y, x] == 1)
                {
                    int num = rand.Next(1, 2);
                    cornData[y, x] = 0;
                    plantedCrop.Remove(new Microsoft.Xna.Framework.Vector2(y, x).ToString());
                    _corn.increaseItem(num);
                }

                if (tomatoData[y, x] == 1)
                {
                    int num = rand.Next(1, 2);
                    tomatoData[y, x] = 0;
                    plantedCrop.Remove(new Microsoft.Xna.Framework.Vector2(y, x).ToString());
                    _tomato.increaseItem(num);
                }

                if (potatoData[y, x] == 1)
                {
                    int num = rand.Next(1, 2);
                    plantedCrop.Remove(new Microsoft.Xna.Framework.Vector2(y, x).ToString());
                    potatoData[y, x] = 0;
                    _potato.increaseItem(num);
                }

                if (orangeData[y, x] == 1)
                {
                    int num = rand.Next(1, 2);
                    orangeData[y, x] = 0;
                    _orange.increaseItem(num);
                    plantedCrop.Remove(new Microsoft.Xna.Framework.Vector2(y, x).ToString());
                }

            }
        }


        public void cropsDraw(SpriteBatch _spriteBatch, Microsoft.Xna.Framework.Vector2 worldPos, int x, int y)
        {

            if (tomatoData[y, x] != 0)
            {
                Microsoft.Xna.Framework.Rectangle src = new Microsoft.Xna.Framework.Rectangle(16 * tomatoData[y, x], 0, 16, 16);
                _spriteBatch.Draw(_tomato.texture, worldPos, src, Microsoft.Xna.Framework.Color.White);
            }

            if (cornData[y, x] != 0)
            {
                Microsoft.Xna.Framework.Rectangle src = new Microsoft.Xna.Framework.Rectangle(16 * cornData[y, x], 0, 16, 16);
                _spriteBatch.Draw(_corn.texture, worldPos, src, Microsoft.Xna.Framework.Color.White);
            }

            if (orangeData[y, x] != 0)
            {
                Microsoft.Xna.Framework.Rectangle src = new Microsoft.Xna.Framework.Rectangle(16 * orangeData[y, x], 0, 16, 16);
                _spriteBatch.Draw(_orange.texture, worldPos, src, Microsoft.Xna.Framework.Color.White);
            }

            if (potatoData[y, x] != 0) 
            {
                Microsoft.Xna.Framework.Rectangle src = new Microsoft.Xna.Framework.Rectangle(16 * potatoData[y, x], 0, 16, 16);
                _spriteBatch.Draw(_potato.texture, worldPos, src, Microsoft.Xna.Framework.Color.White);
            }

        }

    }
}
