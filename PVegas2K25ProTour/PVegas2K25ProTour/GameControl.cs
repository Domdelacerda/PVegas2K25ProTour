using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class GameControl : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;
        private bool game_paused = false;
        private Texture2D golf_ball;

        public GameControl()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.ToggleFullScreen();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sprite_batch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            golf_ball = Content.Load<Texture2D>("GolfBall");
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
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            _sprite_batch.Draw(golf_ball, new Vector2(0, 0), Color.White);
            _sprite_batch.End();

            base.Draw(gameTime);
        }

        public bool isGamePaused()
        {
            return game_paused;
        }
    }
}