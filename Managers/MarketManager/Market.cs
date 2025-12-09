using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace FarmSimulator
{
    public enum ItemType
    {
        None,
        TomatoVeg,
        CornVeg,
        OrangeVeg,
        PotatoVeg,
        TomatoSeed,
        CornSeed,
        OrangeSeed,
        PotatoSeed
    }

    public class SelectedItem
    {
        public ItemType Type { get; set; }
        public int Price { get; set; }
        public bool IsSeed { get; set; }

        public SelectedItem()
        {
            Type = ItemType.None;
            Price = 0;
            IsSeed = false;
        }
    }

    public class Market
    {
        Texture2D texture;
        private Corn _corn;
        private Tomato _tomato;
        private Orange _orange;
        private Potato _potato;
        private Coin _coin;

        bool keypressed = false;

        KeyboardState currentKeyboard;
        KeyboardState previousKeyboard;

        MouseState currentMouse;
        MouseState previousMouse;

        // Selected item tracking
        public SelectedItem SelectedForSale { get; private set; }
        public SelectedItem SelectedForPurchase { get; private set; }

        private ItemType selectedSaleItem = ItemType.None;
        private ItemType selectedPurchaseItem = ItemType.None;

        // Define clickable areas for vegetables (left column)
        private Rectangle tomatoVegRect;
        private Rectangle cornVegRect;
        private Rectangle orangeVegRect;
        private Rectangle potatoVegRect;

        // Define clickable areas for seeds (right column)
        private Rectangle tomatoSeedRect;
        private Rectangle cornSeedRect;
        private Rectangle orangeSeedRect;
        private Rectangle potatoSeedRect;

        public Market(Texture2D text, Corn corn, Tomato tomato, Orange orange, Potato potato, Coin coin)
        {
            this.texture = text;
            _corn = corn;
            _tomato = tomato;
            _orange = orange;
            _potato = potato;
            _coin = coin;

            SelectedForSale = new SelectedItem();
            SelectedForPurchase = new SelectedItem();

            // Initialize rectangles for vegetables (selling)
            tomatoVegRect = new Rectangle(265, 115, 40, 40);
            cornVegRect = new Rectangle(265, 165, 40, 40);
            orangeVegRect = new Rectangle(265, 215, 40, 40);
            potatoVegRect = new Rectangle(265, 265, 40, 40);

            // Initialize rectangles for seeds (purchasing)
            tomatoSeedRect = new Rectangle(420, 115, 40, 40);
            cornSeedRect = new Rectangle(420, 165, 40, 40);
            orangeSeedRect = new Rectangle(420, 215, 40, 40);
            potatoSeedRect = new Rectangle(420, 265, 40, 40);
        }

        public void update()
        {
            previousKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();

            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            if (isKeyPressed(Keys.E))
            {
                keypressed = !keypressed;
                Console.WriteLine(keypressed);
            }

            if (isKeyPressed(Keys.F))
            {
               
                // Purcahsing seeds 
                if (_coin.numberOfItems >= SelectedForPurchase.Price && SelectedForPurchase.Type.ToString() == "TomatoSeed")
                {
                    _coin.numberOfItems -= SelectedForPurchase.Price;
                    _tomato.numberOfSeeds += 13;
                }

                if (_coin.numberOfItems >= SelectedForPurchase.Price && SelectedForPurchase.Type.ToString() == "OrangeSeed")
                {
                    _coin.numberOfItems -= SelectedForPurchase.Price;
                    _orange.numberOfSeeds += 13;
                }

                if (_coin.numberOfItems >= SelectedForPurchase.Price && SelectedForPurchase.Type.ToString() == "PotatoSeed")
                {
                    _coin.numberOfItems -= SelectedForPurchase.Price;
                    _orange.numberOfSeeds += 13;
                }

                if(_coin.numberOfItems >= SelectedForPurchase.Price && SelectedForPurchase.Type.ToString() == "CornSeed")
                {
                    _coin.numberOfItems -= SelectedForPurchase.Price;
                    _corn.numberOfSeeds += 13;
                }


                // Selling Vegetables
                if (_tomato.numberOfItems >= 10 && SelectedForSale.Type.ToString() == "TomatoVeg")
                {
                    _tomato.numberOfItems -= 10;
                    _coin.numberOfItems += 13;
                }
                
                if (_corn.numberOfItems >= 10 && SelectedForSale.Type.ToString() == "CornVeg")
                {
                    _corn.numberOfItems -= 10;
                    _corn.numberOfItems += 13;
                }

                if (_orange.numberOfItems >= 10 && SelectedForSale.Type.ToString() == "OrangeVeg")
                {
                    _orange.numberOfItems -= 10;
                    _corn.numberOfItems += 13;
                }

                if (_potato.numberOfItems >= 10 && SelectedForSale.Type.ToString() == "PotatoVeg")
                {
                    _potato.numberOfItems -= 10;
                    _coin.numberOfItems += 13;
                }
            }

            if (keypressed)
            {
                HandleMouseClicks();
            }
        }

        private void HandleMouseClicks()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                Point mousePoint = new Point(currentMouse.X, currentMouse.Y);

                // Check vegetables (selling)
                if (tomatoVegRect.Contains(mousePoint))
                {
                    selectedSaleItem = ItemType.TomatoVeg;
                    SelectedForSale.Type = ItemType.TomatoVeg;
                    SelectedForSale.Price = 13;
                    SelectedForSale.IsSeed = false;
                    Console.WriteLine("Selected Tomato for sale - Price: 13");
                }
                else if (cornVegRect.Contains(mousePoint))
                {
                    selectedSaleItem = ItemType.CornVeg;
                    SelectedForSale.Type = ItemType.CornVeg;
                    SelectedForSale.Price = 13;
                    SelectedForSale.IsSeed = false;
                    Console.WriteLine("Selected Corn for sale - Price: 13");
                }
                else if (orangeVegRect.Contains(mousePoint))
                {
                    selectedSaleItem = ItemType.OrangeVeg;
                    SelectedForSale.Type = ItemType.OrangeVeg;
                    SelectedForSale.Price = 13;
                    SelectedForSale.IsSeed = false;
                    Console.WriteLine("Selected Orange for sale - Price: 13");
                }
                else if (potatoVegRect.Contains(mousePoint))
                {
                    selectedSaleItem = ItemType.PotatoVeg;
                    SelectedForSale.Type = ItemType.PotatoVeg;
                    SelectedForSale.Price = 13;
                    SelectedForSale.IsSeed = false;
                    Console.WriteLine("Selected Potato for sale - Price: 13");
                }

                // Check seeds (purchasing)
                if (tomatoSeedRect.Contains(mousePoint))
                {
                    selectedPurchaseItem = ItemType.TomatoSeed;
                    SelectedForPurchase.Type = ItemType.TomatoSeed;
                    SelectedForPurchase.Price = 10;
                    SelectedForPurchase.IsSeed = true;
                    Console.WriteLine("Selected Tomato Seed for purchase - Price: 10");
                }
                else if (cornSeedRect.Contains(mousePoint))
                {
                    selectedPurchaseItem = ItemType.CornSeed;
                    SelectedForPurchase.Type = ItemType.CornSeed;
                    SelectedForPurchase.Price = 10;
                    SelectedForPurchase.IsSeed = true;
                    Console.WriteLine("Selected Corn Seed for purchase - Price: 10");
                }
                else if (orangeSeedRect.Contains(mousePoint))
                {
                    selectedPurchaseItem = ItemType.OrangeSeed;
                    SelectedForPurchase.Type = ItemType.OrangeSeed;
                    SelectedForPurchase.Price = 10;
                    SelectedForPurchase.IsSeed = true;
                    Console.WriteLine("Selected Orange Seed for purchase - Price: 10");
                }
                else if (potatoSeedRect.Contains(mousePoint))
                {
                    selectedPurchaseItem = ItemType.PotatoSeed;
                    SelectedForPurchase.Type = ItemType.PotatoSeed;
                    SelectedForPurchase.Price = 10;
                    SelectedForPurchase.IsSeed = true;
                    Console.WriteLine("Selected Potato Seed for purchase - Price: 10");
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            if (keypressed)
            {
                Rectangle rectForVeg = new Rectangle(0, 0, 16, 16);
                Rectangle rectForSeed = new Rectangle(80, 0, 16, 16);

                //Drawing Market
                _spriteBatch.Draw(texture, new Vector2(250, 100), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

                //Drawing Vegetables with selection effect
                float tomatoVegScale = selectedSaleItem == ItemType.TomatoVeg ? 3f : 2.5f;
                float cornVegScale = selectedSaleItem == ItemType.CornVeg ? 3f : 2.5f;
                float orangeVegScale = selectedSaleItem == ItemType.OrangeVeg ? 3f : 2.5f;
                float potatoVegScale = selectedSaleItem == ItemType.PotatoVeg ? 3f : 2.5f;

                _spriteBatch.Draw(_tomato.texture, new Vector2(265, 115), rectForVeg, Color.White, 0f, Vector2.Zero, tomatoVegScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_corn.texture, new Vector2(265, 165), rectForVeg, Color.White, 0f, Vector2.Zero, cornVegScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_orange.texture, new Vector2(265, 215), rectForVeg, Color.White, 0f, Vector2.Zero, orangeVegScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_potato.texture, new Vector2(265, 265), rectForVeg, Color.White, 0f, Vector2.Zero, potatoVegScale, SpriteEffects.None, 0f);

                //Drawing Coin for vegetables
                _spriteBatch.Draw(_coin.texture, new Vector2(320, 115), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(320, 165), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(320, 215), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(320, 265), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

                //Drawing coin for seeds
                _spriteBatch.Draw(_coin.texture, new Vector2(370, 115), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(370, 165), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(370, 215), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_coin.texture, new Vector2(370, 265), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

                //Drawing Seeds with selection effect
                float tomatoSeedScale = selectedPurchaseItem == ItemType.TomatoSeed ? 3f : 2.5f;
                float cornSeedScale = selectedPurchaseItem == ItemType.CornSeed ? 3f : 2.5f;
                float orangeSeedScale = selectedPurchaseItem == ItemType.OrangeSeed ? 3f : 2.5f;
                float potatoSeedScale = selectedPurchaseItem == ItemType.PotatoSeed ? 3f : 2.5f;

                _spriteBatch.Draw(_tomato.texture, new Vector2(420, 115), rectForSeed, Color.White, 0f, Vector2.Zero, tomatoSeedScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_corn.texture, new Vector2(420, 165), rectForSeed, Color.White, 0f, Vector2.Zero, cornSeedScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_orange.texture, new Vector2(420, 215), rectForSeed, Color.White, 0f, Vector2.Zero, orangeSeedScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_potato.texture, new Vector2(420, 265), rectForSeed, Color.White, 0f, Vector2.Zero, potatoSeedScale, SpriteEffects.None, 0f);

                //Drawing Arrow
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(300, 125), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(300, 175), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(300, 225), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(300, 275), Color.White);

                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(405, 125), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(405, 175), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(405, 225), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "->", new Vector2(405, 275), Color.White);

                //Drawing selling Price
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(265, 135), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(265, 190), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(265, 245), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(265, 295), Color.White);

                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(335, 135), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(335, 190), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(335, 245), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(335, 295), Color.White);

                //Draw Purchasing Price
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(365, 135), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(365, 190), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(365, 245), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "10", new Vector2(365, 295), Color.White);

                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(440, 135), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(440, 190), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(440, 245), Color.White);
                _spriteBatch.DrawString(spriteFont: font, "13", new Vector2(440, 295), Color.White);
            }
        }

        private bool isKeyPressed(Keys key)
        {
            return currentKeyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key);
        }
    }
}