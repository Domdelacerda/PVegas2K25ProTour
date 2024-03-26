//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a game scene that is able to display game objects on screen,
// accept user input, and update object properties every frame
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// GameControl creates a game scene that draws objects on screen and
    /// enables user input via the mouse. Update is called every frame to both
    /// detect user input and to update the state of each active game object
    /// </summary>-------------------------------------------------------------
    public class GameControl : Game
    {
        private GraphicsDevice _device;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;

        private Vector2 mouse_pos;
        private bool dragging_mouse = false;
        private bool game_paused = false;

        private Ball golf_ball;
        private Shot shot;

        Texture2D holeTexture;
        Vector2 holePosition;

        private PlayerRecord playerRecord;

        Texture2D line;
        private float angleOfLine;


        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public GameControl()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            line = new Texture2D(GraphicsDevice, 1, 1, false, 
                SurfaceFormat.Color);
            line.SetData(new[] { Color.Black });
            angleOfLine = (float)0;

            holePosition = new Vector2(100, 100);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load the current user name and stroke count
            playerRecord = SaveLoadSystem.Load<PlayerRecord>();
            Debug.WriteLine(playerRecord.Strokes + ", " + playerRecord.User);

            // Load the graphics device
            _device = GraphicsDevice;
            _sprite_batch = new SpriteBatch(_device);
            

            // TODO: use this.Content to load your game content here
            golf_ball = new Ball(_sprite_batch);
            golf_ball.LoadContent(Content);
            shot = new Shot(_sprite_batch);
            shot.LoadContent(Content);

            holeTexture = Content.Load<Texture2D>("Hole");
        }

        protected override void Update(GameTime gameTime)
        {
            // See if the user pressed Quit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // save and exit...
                saveGame();
                Exit();
            }


            // TODO: Add your update logic here
            MouseState mouse_state = Mouse.GetState();
            moveMouseTo(mouse_state.X, mouse_state.Y);
            updateDragState(isDraggingBall(mouse_state, golf_ball));
            shot.Update(dragging_mouse, mouse_pos, golf_ball);
            golf_ball.updateSpeed();
            golf_ball.updatePosition(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            drawHole();
            shot.Draw(golf_ball);
            golf_ball.Draw();
            drawBorder();
            _sprite_batch.End();

            base.Draw(gameTime);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public void drawBorder()
        {
            //Left border
            _sprite_batch.Draw(line, new Rectangle(0, 0, 20, Window.ClientBounds.Height), null, Color.Black, 2 * MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0f);

            //Right border
            _sprite_batch.Draw(line, new Rectangle(Window.ClientBounds.Width - 20, 0, 20, 500), null, Color.Black, 0, new Vector2(0, 0), SpriteEffects.None, 0f);

            //Top border
            _sprite_batch.Draw(line, new Rectangle(0, 0, Window.ClientBounds.Width, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0f);

            //Bottom border
            _sprite_batch.Draw(line, new Rectangle(0, Window.ClientBounds.Height - 20, Window.ClientBounds.Width, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0f);
        }

        public void drawHole()
        {
            //Drawing hole
            _sprite_batch.Draw(holeTexture, holePosition, null, Color.White, 0f, new Vector2(holeTexture.Width / 2,
            holeTexture.Height / 2), 1f, SpriteEffects.None, 0f);
        }


        /// <summary>----------------------------------------------------------
        /// Determines if the mouse is being dragged from the ball or not
        /// </summary>
        /// <param name="mouse"> the mouse state that input data is pulled
        /// from.</param>
        /// <returns> if the mouse is dragging the ball.</returns>
        /// -------------------------------------------------------------------
        public bool isDraggingBall(MouseState mouse, Ball ball)
        {
            bool drag_state = false;
            if (mouse.LeftButton == ButtonState.Released)
            {
                drag_state = false;
            }
            // If the player's left mouse button is down AND the mouse is over
            // the sprite OR if the player was already dragging the cursor from
            // the ball previously
            else if ((mouse.LeftButton == ButtonState.Pressed &&
                ball.isPointOverBall(mouse_pos) || dragging_mouse == true))
            {
                drag_state = true;
            }
            return drag_state;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the current position of the mouse
        /// </summary>
        /// <param name="x">the new horizontal position of the mouse.</param>
        /// <param name="y">the new vertical position of the mouse.</param>
        /// -------------------------------------------------------------------
        public void moveMouseTo(float x, float y)
        {
            mouse_pos.X = x;
            mouse_pos.Y = y;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the current dragging state of the mouse
        /// </summary>
        /// <param name="newDraggingState">the new power of the shot.</param>
        /// -------------------------------------------------------------------
        public void updateDragState(bool newDraggingState)
        {
            dragging_mouse = newDraggingState;
        }

        public bool isGamePaused()
        {
            return game_paused;
        }

        //---------------------------------------------------------------------
        // FOR TEST PURPOSES ONLY
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Gets the current ball active in the game scene
        /// </summary>
        /// <returns>the reference to the golf ball in this game.</returns>
        /// -------------------------------------------------------------------
        public Ball getBall()
        {
            return golf_ball;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the current shot active in the game scene
        /// </summary>
        /// <returns>the reference to the shot in this game.</returns>
        /// -------------------------------------------------------------------
        public Shot getShot()
        {
            return shot;
        }

        /// <summary>----------------------------------------------------------
        /// Exits the game and closes the game window; since Exit() cannot be
        /// used outside of GameControl, this method is how other classes can
        /// cause the game window to close
        /// </summary>---------------------------------------------------------
        public void quit()
        {
            saveGame();
            Exit();
        }


        public void saveGame()
        {
            playerRecord.Strokes = shot.getStrokeCount();
            SaveLoadSystem.Save(playerRecord);
        }
    }
}