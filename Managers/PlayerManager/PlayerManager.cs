using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarmSimulator.Managers.PlayerManager
{
    public class PlayerManager
    {
        private Texture2D spritesheet;
        public Vector2 position;
        public float speed = 100f; // Set default speed

        // Animation Variables
        public int frameWidth;
        public int frameHeight;
        private float animationTimer;
        private float frameDuration = 0.15f;
        private int currentFrame = 0;

        // Walking cycle: use only first 4 frames (0,1,2,3)
        private int walkFrameCount = 4;

        // Direction rows: Down=0, Left=1, Right=2, Up=3
        private int currentRow = 0;

        public PlayerManager(Texture2D spriteSheet, Vector2 startPosition, int frameW, int frameH)
        {
            spritesheet = spriteSheet;
            position = startPosition;
            frameHeight = frameH;
            frameWidth = frameW;
        }

        public void Update(GameTime gameTime, Rectangle mapBounds, Dictionary<string, int[,]> tileData, int tileSize)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState k = Keyboard.GetState();
            Vector2 move = Vector2.Zero;

            // Handle input
            if (k.IsKeyDown(Keys.D) || k.IsKeyDown(Keys.Right))
            {
                move.X += 1;
                currentRow = 2;
            }
            if (k.IsKeyDown(Keys.W) || k.IsKeyDown(Keys.Up))
            {
                move.Y -= 1;
                currentRow = 3;
            }
            if (k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.Left))
            {
                move.X -= 1;
                currentRow = 1;
            }
            if (k.IsKeyDown(Keys.S) || k.IsKeyDown(Keys.Down))
            {
                move.Y += 1;
                currentRow = 0;
            }

            bool moving = move != Vector2.Zero;

            if (moving)
            {
                move.Normalize();

                // Calculate new position
                Vector2 newPosition = position + move * speed * dt;

                // Clamp to map bounds
                newPosition.X = MathHelper.Clamp(newPosition.X, 0, mapBounds.Width - frameWidth);
                newPosition.Y = MathHelper.Clamp(newPosition.Y, 0, mapBounds.Height - frameHeight);

                // Check if new position is walkable
                if (IsWalkable(newPosition, tileData, tileSize))
                {
                    position = newPosition; // Only update if walkable
                }

                // Animate
                animationTimer += dt;
                if (animationTimer >= frameDuration)
                {
                    animationTimer = 0;
                    currentFrame = (currentFrame + 1) % walkFrameCount;
                }
            }
            else
            {
                currentFrame = 0; // Idle frame
                animationTimer = 0;
            }
        }

        private bool IsWalkable(Vector2 pos, Dictionary<string, int[,]> tileData, int tileSize)
        {
            // Calculate tile coordinates for the player's center point
            int tileX = (int)((pos.X + frameWidth / 2) / tileSize);
            int tileY = (int)((pos.Y + frameHeight / 2) / tileSize);

            // Check Fences layer
            if (tileData.ContainsKey("Fences"))
            {
                int[,] fences = tileData["Fences"];

                // Check bounds
                if (tileY >= 0 && tileY < fences.GetLength(0) && tileX >= 0 && tileX < fences.GetLength(1))
                {
                    if (fences[tileY, tileX] != 0) // Non-zero means fence/blocked
                        return false;
                }
            }

            // Check Water layer
            if (tileData.ContainsKey("Water"))
            {
                int[,] water = tileData["Water"];

                // Check bounds
                if (tileY >= 0 && tileY < water.GetLength(0) && tileX >= 0 && tileX < water.GetLength(1))
                {
                    if (water[tileY, tileX] != 0) // Non-zero means water/blocked
                        return false;
                }
            }

            return true; // Tile is walkable
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle source = new Rectangle(
                currentFrame * frameWidth,
                currentRow * frameHeight,
                frameWidth,
                frameHeight
            );
            sb.Draw(spritesheet, position, source, Color.Black);
        }
    }
}