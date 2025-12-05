using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarmSimulator
{
    public class InventorySystem
    {

        private Texture2D texture;
        private SpriteFont _font;
        private Vector2 inventorySysPos = new Vector2(100, 400);
        public string currentItem;
        public Corn _corn;
        public Orange _orange;
        public Potato _potato;
        public Tomato _tomato;
        public Coin _coin;
        int currentIndex;

        public InventorySystem(Texture2D text, SpriteFont font, Orange orange, Potato potato, Tomato tomato, Corn corn, Coin coin)
        {
            this.texture = text;
            this._orange = orange;
            this._potato = potato;
            this._tomato = tomato;
            this._corn = corn;
            this._font = font;
            this._coin = coin;
         }

        public void update(KeyboardState KS)
        {

            if (KS.IsKeyDown(Keys.D1))
            {
                currentIndex = 1;
                currentItem = "Corn";
            }
            if (KS.IsKeyDown(Keys.D2))
            {
                currentIndex = 2;
                currentItem = "Orange";
            }
            if (KS.IsKeyDown(Keys.D3))
            {
                currentIndex = 3;
                currentItem = "Potato";
            }
            if (KS.IsKeyDown(Keys.D4))
            {
                currentIndex = 4;
                currentItem = "Tomato";
            }
        }


        public void draw(SpriteBatch _spritebatch)
        {
            Rectangle seedRectangle = new Rectangle(80, 0, 16, 16);

            //Drawing Inventory with seeds
            _spritebatch.Draw(texture, inventorySysPos, null, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

            //Drawing Inventory with crops
            Rectangle rect = new Rectangle(28,0, 80, 32);
            _spritebatch.Draw(texture, new Vector2(inventorySysPos.X + 300, inventorySysPos.Y), rect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

            if (currentItem == "Corn")
            {
                _spritebatch.Draw(_corn.texture, new Vector2(inventorySysPos.X + 78, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_corn.texture, new Vector2(inventorySysPos.X + 78, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

            if (currentItem == "Orange")
            {
                _spritebatch.Draw(_orange.texture, new Vector2(inventorySysPos.X + 121, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 2.8f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_orange.texture, new Vector2(inventorySysPos.X + 121, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 2.3f, SpriteEffects.None, 0f);
            }

            if (currentItem == "Potato")
            {
                _spritebatch.Draw(_potato.texture, new Vector2(inventorySysPos.X + 164, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_potato.texture, new Vector2(inventorySysPos.X + 164, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

            if (currentItem == "Tomato")
            {
                _spritebatch.Draw(_tomato.texture, new Vector2(inventorySysPos.X + 207, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_tomato.texture, new Vector2(inventorySysPos.X + 207, inventorySysPos.Y + 18), seedRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

            _spritebatch.Draw(_coin.texture, new Vector2(inventorySysPos.X + 10, inventorySysPos.Y + 16), null, Color.White, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);


            try
            {
                Rectangle cropRectangle = new Rectangle(0, 0, 16, 16);

                //Drawing Seeds.
                _spritebatch.DrawString(spriteFont: _font, _corn.seeds(), new Vector2(inventorySysPos.X + 90, inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _orange.seeds(), new Vector2(inventorySysPos.X + 130,inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _potato.seeds(), new Vector2(inventorySysPos.X + 175, inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _tomato.seeds(), new Vector2(inventorySysPos.X + 220, inventorySysPos.Y + 40), Color.White);

                //Drawing Coin
                _spritebatch.DrawString(spriteFont: _font, _coin.item(), new Vector2(inventorySysPos.X + 20, inventorySysPos.Y + 40), Color.White);

                //Drawing Vegetables
                _spritebatch.Draw(_tomato.texture, new Vector2(inventorySysPos.X + 300 + 10, inventorySysPos.Y + 15), cropRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                _spritebatch.Draw(_corn.texture, new Vector2(inventorySysPos.X + 300 + 55, inventorySysPos.Y + 15), cropRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                _spritebatch.Draw(_potato.texture, new Vector2(inventorySysPos.X + 300 + 95, inventorySysPos.Y + 15), cropRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                _spritebatch.Draw(_orange.texture, new Vector2(inventorySysPos.X + 300 + 140, inventorySysPos.Y + 15), cropRectangle, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

                _spritebatch.DrawString(spriteFont: _font, _tomato.item(), new Vector2(inventorySysPos.X + 300 + 10, inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _corn.items(), new Vector2(inventorySysPos.X + 300 + 55, inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _potato.item(), new Vector2(inventorySysPos.X + 300 + 95, inventorySysPos.Y + 40), Color.White);
                _spritebatch.DrawString(spriteFont: _font, _orange.item(), new Vector2(inventorySysPos.X + 300 + 140, inventorySysPos.Y + 40), Color.White);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}