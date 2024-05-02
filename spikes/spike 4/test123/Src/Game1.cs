using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Configuration;

namespace test123.Src
{
    public class Game1 : Game
    {
        //ball set up for basic apperance and movement
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;
        Texture2D pauseTexture;
        Vector2 pausePosition;

        private Hitbox ballHitbox;

        bool pause = false;

        //protected virtual Vector2 destination => Mouse.GetState().Position.ToVector2();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //ball being dragged or not and how much so how fast should it be
        public static bool IsDragged = false;
        public static Vector2 LastPressedAt;
        public static Vector2 LastDragReleased;
        public Vector2 ballSpeed2;
        public static Vector2 GetDragVector()
        {
            Vector2 dist = LastDragReleased - LastPressedAt;
            if (dist != Vector2.Zero)
                IsDragged = true;
            else
                IsDragged = false;
            return dist;
        }


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ballHitbox = new Hitbox(_graphics);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            pausePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;
            ballSpeed2 = Vector2.Zero;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            pauseTexture = Content.Load<Texture2D>("pause-button");
            ballHitbox.Load(ballTexture.Width, ballTexture.Height);
        }

        protected override void UnloadContent()
        {   
            ballHitbox.Unload();
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //time, keyboard state, and mouse state variables
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();




            //Mouse Controls??
            if (mstate.RightButton == ButtonState.Pressed && withInBall())
            {
                LastPressedAt = mstate.Position.ToVector2();
            }
            if (mstate.RightButton == ButtonState.Released)
            {
                LastDragReleased = mstate.Position.ToVector2();
                ballSpeed2 = GetDragVector();
            }
            if (IsDragged && withInMapp())
            {
                ballPosition.Y -= ballSpeed2.Y * time;
                ballPosition.X -= ballSpeed2.X * time;
            }

            //Follow Mouse
            /*var destination = mstate.Position;
            var dir = destination - ballPosition;
            if (dir != Vector2.Zero && Mouse.GetState().RightButton==ButtonState.Pressed)
            {
                dir.Normalize();
                ballPosition += dir * ballSpeed2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }*/

            //Keyboard Controls
            if (kstate.IsKeyDown(Keys.Space))
                pause = !pause;
            if (kstate.IsKeyDown(Keys.W) && !pause)
                ballPosition.Y -= ballSpeed * time;
            if (kstate.IsKeyDown(Keys.D) && !pause)
                ballPosition.X += ballSpeed * time;
            if (kstate.IsKeyDown(Keys.A) && !pause)
                ballPosition.X -= ballSpeed * time;
            if (kstate.IsKeyDown(Keys.S) && !pause)
                ballPosition.Y += ballSpeed * time;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            ballHitbox.Draw(_spriteBatch,ballPosition);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            //_spriteBatch.Draw(pauseTexture, pausePosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        //Check if the mouse is in the ball
        public bool withInBall()
        {
            int h = ballTexture.Height;
            int w = ballTexture.Width;
            if (ballPosition.Y - h <= Mouse.GetState().Y && ballPosition.Y >= Mouse.GetState().Y &&
                ballPosition.X + w >= Mouse.GetState().X && ballPosition.X <= Mouse.GetState().X)
                return true;
            return false;
        }
        //Check if the ball is in the map
        public bool withInMapp()
        {
            int h = ballTexture.Height;
            int w = ballTexture.Width;
            if (ballPosition.Y > _graphics.PreferredBackBufferHeight || ballPosition.X + w > _graphics.PreferredBackBufferWidth ||
                ballPosition.Y - h < _graphics.PreferredBackBufferHeight || ballPosition.X < _graphics.PreferredBackBufferWidth)
                return false;
            return true;
        }
    }
}