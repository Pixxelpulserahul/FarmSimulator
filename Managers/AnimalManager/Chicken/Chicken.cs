using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        private const int framesPerRow = 4; // Assuming 4 frames per animation row

        // Movement variables
        private Random random;
        private double movementTimer;
        private double currentMovementDuration;
        private double idleTimer;
        private double currentIdleDuration;
        private bool isWalking;
        private float walkSpeed = 20f; // Pixels per second

        // Map boundaries
        private int mapWidth;
        private int mapHeight;
        private int tileSize;

        // Direction
        private bool facingRight = true;

        // Audio (placeholder for future implementation)
        // private SoundEffect cluckSound;
        // private double cluckTimer;
        // private double cluckInterval;

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

            // Update animation
            UpdateAnimation(deltaTime);

            // TODO: Uncomment when audio is added
            // UpdateAudio(deltaTime);
        }

        private void UpdateWalking(double deltaTime)
        {
            movementTimer += deltaTime;

            // Move the chicken
            position += velocity * (float)deltaTime;

            // Keep chicken within map bounds
            position.X = MathHelper.Clamp(position.X, 0, (mapWidth * tileSize) - frameWidth);
            position.Y = MathHelper.Clamp(position.Y, 0, (mapHeight * tileSize) - frameHeight);

            // Check if movement duration is over
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

            // Check if idle duration is over
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
            // Random angle for movement
            float angle = (float)(random.NextDouble() * Math.PI * 2);

            velocity.X = (float)Math.Cos(angle) * walkSpeed;
            velocity.Y = (float)Math.Sin(angle) * walkSpeed;

            // Determine facing direction based on velocity
            if (velocity.X > 0)
                facingRight = true;
            else if (velocity.X < 0)
                facingRight = false;
        }

        private void SetNewMovementDuration()
        {
            // Walk for 1-4 seconds
            currentMovementDuration = 1.0 + (random.NextDouble() * 3.0);
        }

        private void SetNewIdleDuration()
        {
            // Idle for 2-6 seconds
            currentIdleDuration = 2.0 + (random.NextDouble() * 4.0);
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

        // TODO: Uncomment and implement when audio is added
        /*
        public void LoadAudio(SoundEffect cluck)
        {
            cluckSound = cluck;
            cluckInterval = 3.0 + (random.NextDouble() * 5.0); // Cluck every 3-8 seconds
        }
        
        private void UpdateAudio(double deltaTime)
        {
            if (cluckSound == null) return;
            
            cluckTimer += deltaTime;
            
            if (cluckTimer >= cluckInterval)
            {
                cluckSound.Play();
                cluckTimer = 0;
                cluckInterval = 3.0 + (random.NextDouble() * 5.0); // Random interval for next cluck
            }
        }
        */

        public void Draw(SpriteBatch spriteBatch)
        {
            // Determine which row to use (0 = idle/resting, 1 = walking)
            int row = isWalking ? 1 : 0;

            // Calculate source rectangle from sprite sheet
            Rectangle sourceRect = new Rectangle(
                currentFrame * frameWidth,
                row * frameHeight,
                frameWidth,
                frameHeight
            );

            // Determine sprite effects for flipping
            SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Draw the chicken
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

        public ChickenManager(Texture2D texture, int mapWidthInTiles, int mapHeightInTiles, int tileSizeInPixels, int chickenCount = 6)
        {
            chickenTexture = texture;
            random = new Random();
            chickens = new List<Chicken>();

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
            foreach (var chicken in chickens)
            {
                chicken.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var chicken in chickens)
            {
                chicken.Draw(spriteBatch);
            }
        }

        // Optional: Add audio to all chickens
        /*
        public void LoadAudio(SoundEffect cluckSound)
        {
            foreach (var chicken in chickens)
            {
                chicken.LoadAudio(cluckSound);
            }
        }
        */

        public int ChickenCount => chickens.Count;
        public List<Chicken> GetChickens() => chickens;
    }
}