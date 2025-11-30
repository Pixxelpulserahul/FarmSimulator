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
        private Vector2 inventorySysPos = new Vector2(300, 400);
        public string currentItem;
        public Corn _corn;
        public Orange _orange;
        public Potato _potato;
        public Tomato _tomato;
        int currentIndex;

        public InventorySystem(Texture2D text, Orange orange, Potato potato, Tomato tomato, Corn corn)
        {
            this.texture = text;
            this._orange = orange;
            this._potato = potato;
            this._tomato = tomato;
            this._corn = corn;

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
            Rectangle rect = new Rectangle(0, 0, 16, 16);
            _spritebatch.Draw(texture, inventorySysPos, null, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

            if(currentItem == "Corn")
            {
                _spritebatch.Draw(_corn.texture, new Vector2(inventorySysPos.X + 78, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_corn.texture, new Vector2(inventorySysPos.X + 78, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);

            }

            if(currentItem == "Orange")
            {
                _spritebatch.Draw(_orange.texture, new Vector2(inventorySysPos.X + 121, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 2.8f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_orange.texture, new Vector2(inventorySysPos.X + 121, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 2.3f, SpriteEffects.None, 0f);
            }

            if(currentItem == "Potato")
            {
                _spritebatch.Draw(_potato.texture, new Vector2(inventorySysPos.X + 164, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_potato.texture, new Vector2(inventorySysPos.X + 164, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

            if (currentItem == "Tomato")
            {
                _spritebatch.Draw(_tomato.texture, new Vector2(inventorySysPos.X + 207, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {
                _spritebatch.Draw(_tomato.texture, new Vector2(inventorySysPos.X + 207, inventorySysPos.Y + 18), rect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

        }
    }
}
