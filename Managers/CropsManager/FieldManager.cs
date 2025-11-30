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
        
        private List<Microsoft.Xna.Framework.Point> plantedCrops = new List<Microsoft.Xna.Framework.Point>();


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
                foreach (var e in plantedCrops)
                {
                    int xCor = e.X;
                    int yCor = e.Y;

                    if (tomatoData[xCor, yCor] > 1) tomatoData[xCor, yCor] -= 1;
                    if (cornData[xCor, yCor] > 1) cornData[yCor, xCor] -= 1;
                    if (potatoData[xCor, yCor] > 1) potatoData[xCor, yCor] -= 1;
                    if (orangeData[xCor, yCor] > 1) orangeData[xCor, yCor] -= 1;
                    
                }
                timer = 0f;
                Console.WriteLine(tomatoData[14, 19]);
                Console.WriteLine("Timer Reset!!!!");
            }


            if (KS.IsKeyDown(Keys.L))
            {
                bool canSowTomato = (cornData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Tomato") && (tomatoData[locY, locX] < 1);
                bool canSowCorn = (tomatoData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Corn") && (cornData[locY, locX] < 1);
                bool canSowOrange = (tomatoData[locY, locX] == 0) && (potatoData[locY, locX] == 0) && (cornData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Orange") && (orangeData[locY, locX] < 1);
                bool canSowPotato = (tomatoData[locY, locX] == 0) && (cornData[locY, locX] == 0) && (orangeData[locY, locX] == 0) && (fieldData[locY, locX] != 0) && (currentItem == "Potato") && (potatoData[locY, locX] < 1);

                if (canSowTomato)
                {
                    Console.WriteLine($"x:{locX} y:{locY}");
                    tomatoData[locY, locX] = 4;
                    plantedCrops.Add(new Microsoft.Xna.Framework.Point(locY, locX));
                }

                if (canSowCorn)
                {
                    Console.WriteLine($"x:{locX} y:{locY}");
                    cornData[locY, locX] = 4;
                    plantedCrops.Add(new Microsoft.Xna.Framework.Point(locY, locX));

                }

                if (canSowPotato)
                {
                    Console.WriteLine($"x:{locX} y:{locY}");
                    potatoData[locY, locX] = 4;
                    plantedCrops.Add(new Microsoft.Xna.Framework.Point(locY, locX));

                }

                if (canSowOrange)
                {
                    Console.WriteLine($"x:{locX} y:{locY}");
                    orangeData[locY, locX] = 4;
                    plantedCrops.Add(new Microsoft.Xna.Framework.Point(locY, locX));
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
