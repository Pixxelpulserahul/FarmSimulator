using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FarmSimulator.Managers.Audio;

namespace FarmSimulator.Managers.AnimalManager.Cow
{
    public class Cow
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;

        // Animation variables
        private int currentFrame;
        private double animationTimer;
        private double animationInterval = 0.2; // Cows move slower, so slower animation
        private const int frameWidth = 32;
        private const int frameHeight = 32;
        private const int framesPerRow = 4; 

        // Movement variables
        private Random random;
        private double movementTimer;
        private double currentMovementDuration;
        private double idleTimer;
        private double currentIdleDuration;
        private bool isWalking;
        private float walkSpeed = 15f; 

        private int mapWidth;
        private int mapHeight;
        private int tileSize;

        private bool facingRight = true;

        public Cow(Texture2D cowTexture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels, Random sharedRandom)
        {
            texture = cowTexture;
            mapWidth = mapWidthInTiles;
            mapHeight = mapHeightInTiles;
            tileSize = tileSizeInPixels;
            random = sharedRandom;

            SpawnAtRandomPosition();

            currentFrame = 0;
            isWalking = false;
            SetNewIdleDuration();
        }

        private void SpawnAtRandomPosition()
        {
            int margin = tileSize * 3;

            position.X = random.Next(0, 600);
            position.Y = random.Next(0, 600);
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
            currentMovementDuration = 1.5 + (random.NextDouble() * 3.5);
        }

        private void SetNewIdleDuration()
        {
            currentIdleDuration = 3.0 + (random.NextDouble() * 5.0);
        }

        private void UpdateAnimation(double deltaTime)
        {
            animationTimer += deltaTime;

            if (animationTimer >= animationInterval)
            {
                animationTimer = 0;

                // Advance frame
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

    public class CowManager
    {
        private List<Cow> cows;
        private Texture2D cowTexture;
        private Random random;
        private SoundTrackManager _soundTrackManager;

        float timer;


        public CowManager(Texture2D texture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels,SoundTrackManager _soundTrack, int cowCount = 5)
        {
            cowTexture = texture;
            _soundTrackManager = _soundTrack;
            random = new Random();
            timer = 0f;
            cows = new List<Cow>();

            for (int i = 0; i < cowCount; i++)
            {
                Cow cow = new Cow(cowTexture, mapWidthInTiles, mapHeightInTiles, tileSizeInPixels, random);
                cows.Add(cow);
            }

        }

        public void Update(GameTime gameTime)
        {
            foreach (var cow in cows)
            {
                cow.Update(gameTime);
            }

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > 6f)
            {
                playCowSound(gameTime);
                timer = 0f;
            }

        }

        private void playCowSound(GameTime gameTime)
        {
            _soundTrackManager.Update(gameTime);
            _soundTrackManager.PlaySound("cow_moo");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cow in cows)
            {
                cow.Draw(spriteBatch);
            }
        }
    }
}