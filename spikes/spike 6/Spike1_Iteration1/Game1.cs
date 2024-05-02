using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text.Json;
using System;
using System.IO;

namespace Spike1_Iteration1
{
    public class PlayerStats
    {
        public int Strokes { get; set; }
        public int Score { get; set; }

    }

    public class SaveLoadSystem
    {
        // https://www.youtube.com/watch?v=gYksT0d_xLM

        private const string PATH = "stats.json";

        public void Save(PlayerStats stats)
        {
            try
            {
                var serializedText = JsonSerializer.Serialize<PlayerStats>(stats);
                Trace.WriteLine(serializedText);
                File.WriteAllText(PATH, serializedText);

                Console.WriteLine("File saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file: " + ex.Message);
            }
        }

        public void Load()
        {

        }
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballSprite;
        Texture2D grassSprite;
        Texture2D holeSprite;

        SpriteFont gameFont;

        Vector2 ballPosition = new Vector2(300, 300);
        Vector2 holePosition = new Vector2(400, 400);
        const int BALL_RADIUS = 15;
        const int HOLE_RADIUS = 30;

        MouseState mState;
        int score = 0;
        bool mReleased = true;

        private PlayerStats pStats;
        private SaveLoadSystem saveLoadSystem;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            pStats = new PlayerStats();
            pStats.Strokes = 7;
            pStats.Score = 3000;

            saveLoadSystem = new SaveLoadSystem();
            saveLoadSystem.Save(pStats);
            //******************************************
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            ballSprite = Content.Load<Texture2D>("golfBall_smaller");
            grassSprite = Content.Load<Texture2D>("Grass");
            holeSprite = Content.Load<Texture2D>("hole");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if(Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Up
                ballPosition.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Left
                ballPosition.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Down
                ballPosition.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Right
                ballPosition.X += 1;
            }

            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                // To get the vector position of the mouse:
                // mState.Position.ToVector2()
                float ballHoleDist = Vector2.Distance(ballPosition, holePosition);
                
                if (ballHoleDist < HOLE_RADIUS)
                {
                    score++;
                }
      
                mReleased = false;
            }

            // Reset the mouse released bool to true if mouse was unclicked
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(grassSprite, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(holeSprite, new Vector2(300, 300), Color.White);
            _spriteBatch.DrawString(gameFont, score.ToString(), new Vector2(100, 100), Color.Black);
            _spriteBatch.Draw(ballSprite, ballPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
