using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Xml;
using Color = Microsoft.Xna.Framework.Color;
using Point = System.Drawing.Point;

namespace Spike1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
      
        private Sprite sprite;
        private Sprite sprite2;
        private Vector2 distance;
        private float rotation;
        private Sprite arrow;

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
            //Point point = new Point(200, 200);
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D texture = Content.Load<Texture2D>("Arrow");
            sprite = new Sprite(texture)
            {
                Position = new Vector2(400, 200),
                Rotation = 0f,
                Origin = new Vector2(texture.Bounds.Left, texture.Bounds.Center.Y)
            };

            Texture2D golfBall = Content.Load<Texture2D>("GolfBall");
            sprite2 = new Sprite(golfBall)
            {
                Position = new Vector2(400, 200),
                Rotation = 0f,
                Origin = new Vector2(texture.Bounds.Center.X, texture.Bounds.Center.Y)
                
            };

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            MouseState mouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            Vector2 dPos = sprite.Position - mousePosition;
            Vector2 min = new Vector2(1, 1);
            sprite.Rotation = (float)Math.Atan2(dPos.Y, dPos.X);
           
            //sprite.Scale.Y = mouse.Y;



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            _spriteBatch.Begin();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                sprite.Draw(_spriteBatch);
                
            }

            //sprite.Draw(_spriteBatch);
            sprite2.Draw(_spriteBatch);
            
            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}