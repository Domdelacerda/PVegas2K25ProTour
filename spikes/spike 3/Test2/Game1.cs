using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;

namespace Test2
{
    public class Game1 : Game
    {
        Texture2D golfBallTexture;
        Vector2 ballPosition;
        float ballSpeed;
      
        Texture2D holeTexture;
        Vector2 holePosition;

        Texture2D line;
        private float angleOfLine;

        private static Texture2D _blankTexture;
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ;

            // ...

            

        }
        public void drawBorder()
        {

            //Drawing border
            _spriteBatch.Draw(line, new Rectangle(0, 0, 20, 500), null, Color.Black, 2*MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(780, 0, 20, 500), null, Color.Black, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(0, 0, 1000, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(0, 460, 1000, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
        }
        public void drawBall(float scale)
        {

            //Drawing ball
            _spriteBatch.Draw(golfBallTexture, ballPosition, null, Color.White, 0f, new Vector2(golfBallTexture.Width / 2,
             golfBallTexture.Height / 2), scale, SpriteEffects.None, 0f);
        }
        public void drawHole(float scale)
        {

            //Drawing hole
            _spriteBatch.Draw(holeTexture, holePosition, null, Color.White, 0f, new Vector2(holeTexture.Width / 2,
             holeTexture.Height / 2), scale, SpriteEffects.None, 0f);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
            _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;

            holePosition = new Vector2(100,50);

            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.Black } );
            angleOfLine = (float)0;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            golfBallTexture = Content.Load<Texture2D>("golf");
            holeTexture = Content.Load<Texture2D>("hole");
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
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            float scale = 0.2f; // used to scale images

            drawBorder();
            drawBall(scale);
            drawHole(scale);
       
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
