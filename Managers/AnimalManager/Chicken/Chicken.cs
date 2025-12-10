using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FarmSimulator.Managers.Audio;

namespace FarmSimulator
{
    public class Chicken
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;

        // Animation variables
        private int currentFrame;
        private double animationTimer;
        private double animationInterval = 0.15; // Time between frames in seconds
        private const int frameWidth = 16;
        private const int frameHeight = 16;
        private const int framesPerRow = 4; 

        // Movement variables
        private Random random;
        private double movementTimer;
        private double currentMovementDuration;
        private double idleTimer;
        private double currentIdleDuration;
        private bool isWalking;
        private float walkSpeed = 20f;

        private int mapWidth;
        private int mapHeight;
        private int tileSize;

        private bool facingRight = true;


        public Chicken(Texture2D chickenTexture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels, Random sharedRandom)
        {
            texture = chickenTexture;
            mapWidth = mapWidthInTiles;
            mapHeight = mapHeightInTiles;
            tileSize = tileSizeInPixels;
            random = sharedRandom;

            // Spawn at random position on the map
            SpawnAtRandomPosition();

            // Initialize movement state
            currentFrame = 0;
            isWalking = false;
            SetNewIdleDuration();
        }

        private void SpawnAtRandomPosition()
        {
            // Spawn chicken randomly on the map, avoiding edges
            int margin = tileSize * 2; // Keep 2 tiles away from edges

            position.X = random.Next(1, 300);
            position.Y = random.Next(1, 300);
        }

        public void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (isWalking)
            {
                UpdateWalking(deltaTime);
            }
            else
            {
                UpdateIdle(deltaTime);
            }

            UpdateAnimation(deltaTime);

        }

        private void UpdateWalking(double deltaTime)
        {
            movementTimer += deltaTime;

            position += velocity * (float)deltaTime;

            position.X = MathHelper.Clamp(position.X, 0, (mapWidth * tileSize) - frameWidth);
            position.Y = MathHelper.Clamp(position.Y, 0, (mapHeight * tileSize) - frameHeight);

            if (movementTimer >= currentMovementDuration)
            {
                isWalking = false;
                velocity = Vector2.Zero;
                movementTimer = 0;
                SetNewIdleDuration();
            }
        }

        private void UpdateIdle(double deltaTime)
        {
            idleTimer += deltaTime;

            if (idleTimer >= currentIdleDuration)
            {
                isWalking = true;
                idleTimer = 0;
                SetNewMovementDirection();
                SetNewMovementDuration();
            }
        }

        private void SetNewMovementDirection()
        {

            float angle = (float)(random.NextDouble() * Math.PI * 2);

            velocity.X = (float)Math.Cos(angle) * walkSpeed;
            velocity.Y = (float)Math.Sin(angle) * walkSpeed;

            if (velocity.X > 0)
                facingRight = true;
            else if (velocity.X < 0)
                facingRight = false;
        }

        private void SetNewMovementDuration()
        {

            currentMovementDuration = 1.0 + (random.NextDouble() * 3.0);
        }

        private void SetNewIdleDuration()
        {
            currentIdleDuration = 2.0 + (random.NextDouble() * 4.0);
        }

        private void UpdateAnimation(double deltaTime)
        {
            animationTimer += deltaTime;

            if (animationTimer >= animationInterval)
            {
                animationTimer = 0;

                currentFrame++;
                if (currentFrame >= framesPerRow)
                {
                    currentFrame = 0;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            int row = isWalking ? 1 : 0;

            Rectangle sourceRect = new Rectangle(
                currentFrame * frameWidth,
                row * frameHeight,
                frameWidth,
                frameHeight
            );

            SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(
                texture,
                position,
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                effect,
                0f
            );
        }
    }

    // Manager class to handle multiple chickens
    public class ChickenManager
    {
        private List<Chicken> chickens;
        private Texture2D chickenTexture;
        private Random random;
        float timer;
        private SoundTrackManager _soundTrackManager;


        public ChickenManager(Texture2D texture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels,SoundTrackManager _soundTrack, int chickenCount = 6)
        {
            chickenTexture = texture;
            random = new Random();
            chickens = new List<Chicken>();
            timer = 0f;
            _soundTrackManager = _soundTrack;

            // Spawn multiple chickens
            for (int i = 0; i < chickenCount; i++)
            {
                Chicken chicken = new Chicken(chickenTexture, mapWidthInTiles, mapHeightInTiles, tileSizeInPixels, random);
                chickens.Add(chicken);
            }

            Console.WriteLine($"ChickenManager: Spawned {chickenCount} chickens");
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var chicken in chickens)
            {
                chicken.Update(gameTime);
            }

            if (timer > 14f)
            {
                Console.WriteLine(timer);
                playChickenSound(gameTime);
                timer = 0f;
            }

        }

        private void playChickenSound(GameTime gameTime)
        {
            _soundTrackManager.Update(gameTime);
            _soundTrackManager.PlaySound("chicken");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var chicken in chickens)
            {
                chicken.Draw(spriteBatch);
            }
        }

       
    }
}