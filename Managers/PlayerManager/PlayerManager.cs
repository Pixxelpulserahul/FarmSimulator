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
        private int frameWidth;
        private int frameHeight;
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

        public void Update(GameTime gameTime, Rectangle mapBounds)
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
                move.X -= 1; // FIXED: Was += 1
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
                position += move * speed * dt;

                // Clamp player position to map bounds
                position.X = MathHelper.Clamp(position.X, 0, mapBounds.Width - frameWidth);
                position.Y = MathHelper.Clamp(position.Y, 0, mapBounds.Height - frameHeight);

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

        public void Draw(SpriteBatch sb)
        {
            Rectangle source = new Rectangle(
                currentFrame * frameWidth,
                currentRow * frameHeight,
                frameWidth,
                frameHeight
            );
            sb.Draw(spritesheet, position, source, Color.White);
        }
    }
}