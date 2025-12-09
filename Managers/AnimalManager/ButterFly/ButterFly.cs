using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FarmSimulator
{
    public class Butterfly
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;

        // Animation variables
        private int currentFrame;
        private double animationTimer;
        private double animationInterval = 0.15; // Butterflies flutter quickly
        private const int frameWidth = 16;
        private const int frameHeight = 16;
        private const int framesPerRow = 3; // 3 columns
        private const int totalRows = 4; // 4 rows for 4 directions

        // Movement variables
        private Random random;
        private double movementTimer;
        private double currentMovementDuration;
        private float flySpeed = 25f; // Butterflies fly at moderate speed

        // Map boundaries
        private int mapWidth;
        private int mapHeight;
        private int tileSize;

        // Direction enum matching sprite sheet rows
        private enum FlyDirection
        {
            Up = 0,      // First row
            Right = 1,   // Second row
            Down = 2,    // Third row
            Left = 3     // Fourth row
        }

        private FlyDirection currentDirection;

        public Butterfly(Texture2D butterflyTexture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels, Random sharedRandom)
        {
            texture = butterflyTexture;
            mapWidth = mapWidthInTiles;
            mapHeight = mapHeightInTiles;
            tileSize = tileSizeInPixels;
            random = sharedRandom;

            // Spawn at random position on the map
            SpawnAtRandomPosition();

            // Initialize movement state
            currentFrame = 0;
            SetNewDirection();
            SetNewMovementDuration();
        }

        private void SpawnAtRandomPosition()
        {
            // Spawn butterfly randomly on the map with small margin
            int margin = tileSize * 2;

            position.X = random.Next(0, 800);
            position.Y = random.Next(0, 800);
        }

        public void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update movement
            UpdateMovement(deltaTime);

            // Update animation
            UpdateAnimation(deltaTime);
        }

        private void UpdateMovement(double deltaTime)
        {
            movementTimer += deltaTime;

            // Move the butterfly
            position += velocity * (float)deltaTime;

            // Keep butterfly within map bounds and bounce off edges
            bool changedDirection = false;

            if (position.X <= 0)
            {
                position.X = 0;
                currentDirection = FlyDirection.Right;
                changedDirection = true;
            }
            else if (position.X >= (mapWidth * tileSize) - frameWidth)
            {
                position.X = (mapWidth * tileSize) - frameWidth;
                currentDirection = FlyDirection.Left;
                changedDirection = true;
            }

            if (position.Y <= 0)
            {
                position.Y = 0;
                currentDirection = FlyDirection.Down;
                changedDirection = true;
            }
            else if (position.Y >= (mapHeight * tileSize) - frameHeight)
            {
                position.Y = (mapHeight * tileSize) - frameHeight;
                currentDirection = FlyDirection.Up;
                changedDirection = true;
            }

            if (changedDirection)
            {
                UpdateVelocity();
            }

            // Check if movement duration is over - change direction randomly
            if (movementTimer >= currentMovementDuration)
            {
                movementTimer = 0;
                SetNewDirection();
                SetNewMovementDuration();
            }
        }

        private void SetNewDirection()
        {
            // Randomly choose a new direction
            currentDirection = (FlyDirection)random.Next(0, 4);
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            // Set velocity based on current direction
            switch (currentDirection)
            {
                case FlyDirection.Up:
                    velocity = new Vector2(0, -flySpeed);
                    break;
                case FlyDirection.Right:
                    velocity = new Vector2(flySpeed, 0);
                    break;
                case FlyDirection.Down:
                    velocity = new Vector2(0, flySpeed);
                    break;
                case FlyDirection.Left:
                    velocity = new Vector2(-flySpeed, 0);
                    break;
            }
        }

        private void SetNewMovementDuration()
        {
            // Butterflies fly in one direction for 1-4 seconds before changing
            currentMovementDuration = 1.0 + (random.NextDouble() * 3.0);
        }

        private void UpdateAnimation(double deltaTime)
        {
            animationTimer += deltaTime;

            if (animationTimer >= animationInterval)
            {
                animationTimer = 0;

                // Advance frame through the 3 columns
                currentFrame++;
                if (currentFrame >= framesPerRow)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Get the row based on current direction
            int row = (int)currentDirection;

            // Calculate source rectangle from sprite sheet
            Rectangle sourceRect = new Rectangle(
                currentFrame * frameWidth,
                row * frameHeight,
                frameWidth,
                frameHeight
            );

            // Draw the butterfly
            spriteBatch.Draw(
                texture,
                position,
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }

    // Manager class to handle multiple butterflies
    public class ButterflyManager
    {
        private List<Butterfly> butterflies;
        private Texture2D butterflyTexture;
        private Random random;

        public ButterflyManager(Texture2D texture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels, int butterflyCount = 10)
        {
            butterflyTexture = texture;
            random = new Random();
            butterflies = new List<Butterfly>();

            // Spawn multiple butterflies based on butterflyCount parameter
            for (int i = 0; i < butterflyCount; i++)
            {
                Butterfly butterfly = new Butterfly(butterflyTexture, mapWidthInTiles, mapHeightInTiles, tileSizeInPixels, random);
                butterflies.Add(butterfly);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var butterfly in butterflies)
            {
                butterfly.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var butterfly in butterflies)
            {
                butterfly.Draw(spriteBatch);
            }
        }

        // Methods to add or remove butterflies dynamically
        public void AddButterfly(int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels)
        {
            Butterfly butterfly = new Butterfly(butterflyTexture, mapWidthInTiles, mapHeightInTiles, tileSizeInPixels, random);
            butterflies.Add(butterfly);
        }

        public void RemoveButterfly(int index)
        {
            if (index >= 0 && index < butterflies.Count)
            {
                butterflies.RemoveAt(index);
            }
        }

        public void ClearAllButterflies()
        {
            butterflies.Clear();
        }

        public int ButterflyCount => butterflies.Count;
        public List<Butterfly> GetButterflies() => butterflies;
    }
}