using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarmSimulator.Managers.MapManager;
using FarmSimulator.Managers.MapLoader;
using System.Runtime.CompilerServices;
using System.IO;
using System;


namespace FarmSimulator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private MapManager _mapManager;
        
        Texture2D _texture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _mapManager = new MapManager(); //Initializing the Map Manager.


        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            string _mapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Maps", "Map.json");


            _mapManager.GettingStarted(_mapPath);

            //_mapManager.Print();

            try
            {
                    _texture = Content.Load<Texture2D>("Tiles/Doors");
                


            }
            catch ( Exception e)
            {
                Console.WriteLine(e);
            }

            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            Console.WriteLine($"Width = {width} // Height = {height}");



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, new Vector2(100, 100), Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
