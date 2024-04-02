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
        private Hole hole;
        private Hitbox hitbox;
        private List<Obstacle> obstacle_list;
        private Obstacle[] borders;
        private List<Action> levels_list;

        private PlayerRecord playerRecord;
        private int level = 0;

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
            obstacle_list = new List<Obstacle>();
            levels_list = new List<Action>();
            borders = new Obstacle[4];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load the current user name and stroke count
            //playerRecord = SaveLoadSystem.Load<PlayerRecord>();
            //Debug.WriteLine(playerRecord.Strokes + ", " + playerRecord.User);

            // Load the graphics device
            _device = GraphicsDevice;
            _sprite_batch = new SpriteBatch(_device);
            

            // TODO: use this.Content to load your game content here
            golf_ball = new Ball(_sprite_batch);
            golf_ball.LoadContent(Content);
            shot = new Shot(_sprite_batch);
            shot.LoadContent(Content);
            hitbox = new Hitbox(_graphics);
            hole = new Hole(new Vector2(100, 200), _sprite_batch,
                hitbox);
            hole.LoadContent(Content);
            loadBorders();
            // Update all content in the obstacle list
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].LoadContent(Content);
                }
            }
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].LoadContent(Content);
            }
            levels_list.Add(loadLevelZero);
            levels_list.Add(loadLevelOne);
            levels_list.Add(loadLevelTwo);
            levels_list.Add(loadLevelThree);
            levels_list.Add(loadLevelFour);
            levels_list.Add(loadLevelFive);

            levels_list[level].Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            // See if the user pressed Quit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // save and exit...
                //saveGame();
                Exit();
            }


            // TODO: Add your update logic here
            MouseState mouse_state = Mouse.GetState();
            moveMouseTo(mouse_state.X, mouse_state.Y);
            updateDragState(isDraggingBall(mouse_state, golf_ball));
            shot.Update(dragging_mouse, mouse_pos, golf_ball);
            hole.Update(golf_ball);
            golf_ball.updateSpeed();
            golf_ball.updatePosition(gameTime);
            // Update all items in the obstacle list
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].Update(golf_ball);
                }
            }
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].Update(golf_ball);
            }
            if (hole.getCollision() == true)
            {
                level += 1;
                hole.setCollision(false);
                if (level < levels_list.Count)
                {
                    levels_list[level].Invoke();
                }
                else
                {
                    // Display win screen
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            // Draw all obstacles in the obstacle list
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].Draw();
                }
            }
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].Draw();
            }
            hole.Draw();
            shot.Draw();
            golf_ball.Draw();
            //drawBorder();
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

        public void loadBorders()
        {
            Obstacle left_border = new Obstacle(Vector2.Zero, _sprite_batch,
                hitbox, new Vector2(1, Window.ClientBounds.Height));
            borders[0] = left_border;
            Obstacle right_border = new Obstacle(new Vector2
                (Window.ClientBounds.Width, 0), _sprite_batch,
                hitbox, new Vector2(1, Window.ClientBounds.Height));
            borders[1] = right_border;
            Obstacle top_border = new Obstacle(Vector2.Zero, _sprite_batch, 
                hitbox, new Vector2(Window.ClientBounds.Width, 1));
            borders[2] = top_border;
            Obstacle bottom_border = new Obstacle(new Vector2
                (0, Window.ClientBounds.Height), _sprite_batch,
                hitbox, new Vector2(Window.ClientBounds.Width, 1));
            borders[3] = bottom_border;
        }

        public void clearObstacles()
        {
            obstacle_list.Clear();
        }

        public void addObstacle(Obstacle obstacle)
        {
            obstacle.LoadContent(Content);
            obstacle_list.Add(obstacle);
        }

        public void loadLevelZero()
        {
            golf_ball.setPosition(new Vector2(600, 200));
            hole.setPosition(new Vector2(100, 200));
        }

        public void loadLevelOne()
        {
            clearObstacles();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 350));
            hole.setPosition(new Vector2(25, 100));
            SandPit pit1 = new SandPit(new Vector2(100, -175),
                _sprite_batch, new Hitbox(_graphics), new Vector2(2f, 2f));
            addObstacle(pit1);
            SandPit pit2 = new SandPit(new Vector2(275, 250),
                _sprite_batch, new Hitbox(_graphics), new Vector2(2f, 2f));
            addObstacle(pit2);
        }

        public void loadLevelTwo()
        {
            clearObstacles();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(200, 200));
            Lake lake1 = new Lake(new Vector2(-50, 0),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1f, 2.4f));
            addObstacle(lake1);
            Puddle puddle1 = new Puddle(new Vector2(400, -100),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1.5f, 1.5f));
            addObstacle(puddle1);
            Puddle puddle2 = new Puddle(new Vector2(350, 300),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1f, 1f));
            addObstacle(puddle2);
            Obstacle wall1 = new Obstacle(new Vector2(300, 185),
                _sprite_batch, new Hitbox(_graphics), new Vector2(25, 100));
            addObstacle(wall1);
        }

        public void loadLevelThree()
        {
            clearObstacles();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(100, 200));
            Mushroom mushroom1 = new Mushroom(new Vector2(200, 175),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(mushroom1);
            Mushroom mushroom2 = new Mushroom(new Vector2(100, 275),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(mushroom2);
            Mushroom mushroom3 = new Mushroom(new Vector2(100, 75),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(mushroom3);
            Mushroom mushroom4 = new Mushroom(new Vector2(450, 0),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(mushroom4);
            Mushroom mushroom5 = new Mushroom(new Vector2(600, 400),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(mushroom5);
        }

        public void loadLevelFour()
        {
            clearObstacles();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 425));
            hole.setPosition(new Vector2(125, 275));
            Downslope slope1 = new Downslope(new Vector2(0, 200),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(slope1);
            Downslope slope2 = new Downslope(new Vector2(600, 200),
                _sprite_batch, new Hitbox(_graphics), new Vector2(1, 1));
            addObstacle(slope2);
            Lake lake1 = new Lake(new Vector2(0, 0),
                _sprite_batch, new Hitbox(_graphics), new Vector2(4, 0.5f));
            addObstacle(lake1);
            Lake lake2 = new Lake(new Vector2(200, 200),
                _sprite_batch, new Hitbox(_graphics), new Vector2(2, 2));
            addObstacle(lake2);
        }

        public void loadLevelFive()
        {
            clearObstacles();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 400));
            hole.setPosition(new Vector2(75, 350));
            Downslope slope1 = new Downslope(new Vector2(0, 0),
                _sprite_batch, new Hitbox(_graphics), new Vector2(4, 1.5f));
            addObstacle(slope1);
            Lake lake1 = new Lake(new Vector2(200, 310),
                _sprite_batch, new Hitbox(_graphics), new Vector2(2, 2f));
            addObstacle(lake1);
            Obstacle wall1 = new Obstacle(new Vector2(300, 150),
                _sprite_batch, new Hitbox(_graphics), new Vector2(100, 25));
            addObstacle(wall1);
            Obstacle wall2 = new Obstacle(new Vector2(575, 250),
                _sprite_batch, new Hitbox(_graphics), new Vector2(100, 25));
            addObstacle(wall2);
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