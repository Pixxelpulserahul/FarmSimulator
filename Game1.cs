using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml.Schema;
using FarmSimulator.Managers;
using FarmSimulator.Managers.AnimalManager;
using FarmSimulator.Managers.AnimalManager.Cow;
using FarmSimulator.Managers.Audio;
using FarmSimulator.Managers.Camera;
using FarmSimulator.Managers.MapLoader;
using FarmSimulator.Managers.MapManager;
using FarmSimulator.Managers.PlayerManager;
using FarmSimulator.Managers.TileHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarmSimulator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<string, TileInfo> spritesData;
        private Dictionary<string, Texture2D> sprites;
        private Dictionary<string, int[,]> tileArranData;
        private SpriteFont _font;

        int tileSize;

        const int mapWidth = 300;
        const int mapHeight = 200;

        private MapManager _mapManager;
        private Camera _camera;
        private PlayerManager _player;

        
        private FieldManager _fieldManager;
        private InventorySystem _inventorySystem;
        private Market _market;

        //private Chicken _chicken;
        private ChickenManager _chickenManager;
        private CowManager _cowManager;
        private ButterflyManager _butterflyManager;

        Tomato _tomato;
        Orange _orange;
        Potato _potato;
        Corn _corn;
        Coin _coin;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _mapManager = new MapManager();

            tileSize = 16;
            sprites = new Dictionary<string, Texture2D>();
            tileArranData = new Dictionary<string, int[,]>();
        }

        protected override void Initialize()
        {
            _camera = new Camera(
                _graphics.GraphicsDevice.Viewport.Width,
                _graphics.GraphicsDevice.Viewport.Height
            );
            Console.WriteLine(_graphics.GraphicsDevice.Viewport.Width);
            Console.WriteLine(_graphics.GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            string _mapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Maps", "Map.json");

            Texture2D playerSheet = Content.Load<Texture2D>("Player/Player");

            // Spritesheet frames are 32x32, but we'll scale to 16x16
            int frameW = 32;
            int frameH = 32;

            _player = new PlayerManager(
                playerSheet,
                new Vector2(80, 80),
                frameW,
                frameH
            );

            _mapManager.GettingStarted(_mapPath);
            spritesData = _mapManager.getTileData();

            tileArranData = _mapManager.getTileArranData();


            try
            {
                if (spritesData != null)
                {
                    foreach (var item in spritesData)
                    {
                        if (!item.Value.isCollection)
                        {
                            Texture2D temp = Content.Load<Texture2D>(item.Value.ImagePath.Split(".png")[0]);
                            sprites.Add(item.Value.TileSetName, temp);
                        }
                    }
                }

                Console.WriteLine("Gotcha");
                loadCropTexture();
                _font = Content.Load<SpriteFont>("Fonts/File");
                loadInventoryTexture();
                _fieldManager = new FieldManager(fieldData: tileArranData["FarmLand_Tile"], tomato: _tomato, corn: _corn, potato: _potato, orange: _orange);

                if(_fieldManager == null)
                {
                    Console.WriteLine("fieldManager is null");
                }

                loadMarketTexture();
                //LoadSoundTracks();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyState = Keyboard.GetState();

            Rectangle mapBounds = new Rectangle(0, 0, mapWidth * tileSize, mapHeight * tileSize);

            _player.Update(gameTime, mapBounds, tileArranData, tileSize);


            UpdateCameraFollowPlayer();

            try
            {
                _inventorySystem.update(keyState);


                _fieldManager.SowCrop(keyState, _player.position.X, _player.position.Y, _inventorySystem.currentItem, gameTime);
                _fieldManager.HarvestCrop(keyState, _player.position);
                _chickenManager.Update(gameTime);
                _cowManager.Update(gameTime);
                _butterflyManager.Update(gameTime);
                _market.update();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error is here {e.Message}");
            }

            base.Update(gameTime);
        }


        private void UpdateCameraFollowPlayer()
        {
            // Calculate the center of the viewport
            float viewportCenterX = _camera.viewPortWidth / 2f;
            float viewportCenterY = _camera.viewPortHeight / 2f;

            // Calculate desired camera position (centered on player)
            float desiredCameraX = _player.position.X - viewportCenterX;
            float desiredCameraY = _player.position.Y - viewportCenterY;

            // Clamp camera to map boundaries
            float maxCameraX = (mapWidth * tileSize) - _camera.viewPortWidth;
            float maxCameraY = (mapHeight * tileSize) - _camera.viewPortHeight;

            _camera.position.X = MathHelper.Clamp(desiredCameraX, 0, maxCameraX);
            _camera.position.Y = MathHelper.Clamp(desiredCameraY, 0, maxCameraY);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix transformMatrix = Matrix.CreateTranslation(-_camera.position.X, -_camera.position.Y, 0);
            
            //Draw with camera Transition
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp);

            drawMap(tileArranData);
            _player.Draw(_spriteBatch);
            _chickenManager.Draw(_spriteBatch);
            _cowManager.Draw(_spriteBatch);
            _butterflyManager.Draw(_spriteBatch);

            _spriteBatch.End();


            //Static draw
            _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp
                );
            _inventorySystem.draw(_spriteBatch);
            _market.Draw(_spriteBatch, _font);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawMap(Dictionary<string, int[,]> tileArranData)
        {
            foreach (var layer in tileArranData)
            {
                int[,] map = layer.Value;
                string layerName = layer.Key;

                TileInfo info = spritesData[layerName];

                if (info.isCollection == false)
                {
                    Texture2D tempTexture = sprites[layerName];
                    int startx = (int)(_camera.position.X / tileSize);
                    int starty = (int)(_camera.position.Y / tileSize);

                    int tilex = (_camera.viewPortWidth / tileSize) + 2;
                    int tiley = (_camera.viewPortHeight / tileSize) + 2;

                    startx = Math.Max(0, startx);
                    starty = Math.Max(0, starty);

                    int endx = Math.Min(mapWidth, startx + tilex);
                    int endy = Math.Min(mapHeight, starty + tiley);

                    int tilesPerRow = info.width / tileSize;

                    for (int y = starty; y < endy; y++)
                    {
                        for (int x = startx; x < endx; x++)
                        {
                            int gid = map[y, x];
                            if (gid == 0) continue;

                            int localId = gid - info.firstGID;

                            int srcx = (localId % tilesPerRow) * tileSize;
                            int srcy = (localId / tilesPerRow) * tileSize;

                            Vector2 worldPos = new Vector2(x * tileSize, y * tileSize);

                            Rectangle sourceRect = new Rectangle(srcx, srcy, tileSize, tileSize);
                            _spriteBatch.Draw(tempTexture, worldPos, sourceRect, Color.White);

                            //_fieldManager.cropsDraw(_spriteBatch, worldPos, x: x, y: y);


                        }
                    }
                }
                else
                {
                    Console.WriteLine(info.TileSetName);
                }
            }
        }


        private void loadCropTexture()
        {
            try
            {
                Texture2D texture = Content.Load<Texture2D>("Crops/Tomato/Tomato");
                _tomato = new Tomato(texture);

                texture = Content.Load<Texture2D>("Crops/Corn/Corn");
                _corn = new Corn(texture);

                texture = Content.Load<Texture2D>("Crops/Potato/Potato");
                _potato = new Potato(texture);

                texture = Content.Load<Texture2D>("Crops/Orange/Orange");
                _orange = new Orange(texture);

                texture = Content.Load<Texture2D>("Crops/Coin/Coin");
                _coin = new Coin(texture);

                Console.WriteLine("Crops Texture Load Successfully");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void loadInventoryTexture()
        {
            try
            {
                Texture2D text = Content.Load<Texture2D>("Inventory/Inventory");
                _inventorySystem = new InventorySystem(text, font: _font, corn: _corn, tomato: _tomato, orange: _orange, potato: _potato, coin: _coin);
            }
            catch(Exception e)
            {
                Console.WriteLine($" Inventory error: {e.Message}");
            }
        }

        private void loadMarketTexture()
        {

            Texture2D text = Content.Load<Texture2D>("Market/Market");
            _market = new Market(text, corn: _corn, potato: _potato, tomato: _tomato, orange: _orange, coin: _coin);

            
            text = Content.Load<Texture2D>("Animals/Chicken/Chicken");
            _chickenManager = new ChickenManager(text, mapWidth, mapHeight, tileSize, chickenCount: 6);

            text = Content.Load<Texture2D>("Animals/Cow/Cow");
            _cowManager = new CowManager(text, mapWidth, mapHeight, tileSize, cowCount: 5);

            text = Content.Load<Texture2D>("Animals/ButterFly/ButterFly");
            _butterflyManager = new ButterflyManager(text, mapWidth, mapHeight, butterflyCount: 16, tileSizeInPixels: 16);
        }

    }
}