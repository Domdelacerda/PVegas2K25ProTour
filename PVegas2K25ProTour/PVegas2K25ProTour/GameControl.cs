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
using System.ComponentModel;
using PVegas2K25ProTour.Controls;

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
        private RenderTarget2D _render_target;
        private int MAX_SCORE = 5000;
        private int MAX_COINS = 25;
        private bool clickedNext;

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
        SpriteFont font;
        MouseState prevMouseState;

        private PlayerRecord playerRecord;
        private int level = 0;
        private int totalHolesCompleted;
        private bool canIncrementHolesCompleted = true;
        private int totalStrokesLifetime;
        private int current_level = 0;

        Texture2D line;
        private float angleOfLine;

        private Vector2 game_resolution = new Vector2(800, 480);
        private Rectangle render_target_rect;

        private List<Component> _gameComponents;
        private String stateOfGame = "menu";
        private String previousGameState = "menu";
        Vector2 strokeCounter;

        private int coins = 0;
        private List<Coin> coinList;
        private bool coinAddLevel=false;

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
            coinList=new List<Coin>();

            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            _render_target = new RenderTarget2D(_graphics.GraphicsDevice, 
                160, 144);

            Exiting += OnExiting;
            base.Initialize();
        }

        private void OnExiting(object sender, EventArgs args)
        {
            // Handle window closing
            // You can put your cleanup code here before the game exits
            // Save upon exiting... 
            saveGame();
        }

        Rectangle GetRenderTargetDestination(Vector2 resolution, int preferredBackBufferWidth, int preferredBackBufferHeight)
        {
            float resolutionRatio = resolution.X / resolution.Y;
            float screenRatio;
            Vector2 bounds = new Vector2(preferredBackBufferWidth, preferredBackBufferHeight);
            screenRatio = bounds.X / bounds.Y;
            float scale;
            Rectangle rectangle = new Rectangle();

            if (resolutionRatio < screenRatio)
                scale = bounds.Y / resolution.Y;
            else if (resolutionRatio > screenRatio)
                scale = bounds.X / resolution.X;
            else
            {
                // Resolution and window/screen share aspect ratio
                rectangle.Size = new Point((int)bounds.X, (int)bounds.Y);
                return rectangle;
            }
            rectangle.Width = (int)(resolution.X * scale);
            rectangle.Height = (int)(resolution.Y * scale);
            return CenterRectangle(new Rectangle(0, 0, (int)bounds.X, (int)bounds.Y), rectangle);
        }

        static Rectangle CenterRectangle(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            Point delta = outerRectangle.Center - innerRectangle.Center;
            innerRectangle.Offset(delta);
            return innerRectangle;
        }

        protected override void LoadContent()
        {
            // Load the current user name and stroke count
            playerRecord = SaveLoadSystem.Load<PlayerRecord>();


            // Load Saved Data
            
            playerRecord.isLevelOneUnlocked = true;
            coins = playerRecord.Coins;
            totalHolesCompleted = playerRecord.TotalHolesCompleted;
            totalStrokesLifetime = playerRecord.TotalStrokesLifetime;
 
            
            // Load the graphics device
            _device = GraphicsDevice;
            _sprite_batch = new SpriteBatch(_device);
            font = Content.Load<SpriteFont>("File");

            _graphics.PreferredBackBufferWidth = (int)game_resolution.X;
            _graphics.PreferredBackBufferHeight = (int)game_resolution.Y;
            _graphics.ApplyChanges();
            _render_target = new RenderTarget2D(GraphicsDevice, 
                (int)game_resolution.X, (int)game_resolution.Y, false, 
                SurfaceFormat.Color, DepthFormat.None, 0, 
                RenderTargetUsage.PreserveContents);
            render_target_rect = GetRenderTargetDestination
                (game_resolution, _graphics.PreferredBackBufferWidth, 
                _graphics.PreferredBackBufferHeight);

            if (stateOfGame == "menu")
            {

                var playButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "Play",
                };
                playButton.Click += PlayButton_Click;
                var quitButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 260),
                    Text = "Quit",
                };
                // TODO: use this.Content to load your game content here
                quitButton.Click += QuitButton_Click;
                golf_ball = new Ball(_sprite_batch);
                golf_ball = new Ball(_sprite_batch);
                var settingsButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<Texture2D>("settings"))
                {
                    Position = new Vector2(730, 0 ),
                    //Text = "Settings",
                };
                settingsButton.Click += SettingsButton_Click;

                var shopingButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<Texture2D>("store"))
                {
                    Position = new Vector2(660, 0),
                    //Text = "Settings",
                };
                shopingButton.Click += ShopingButton_Click;

                var LevelButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(0, 130),
                    Text = "Level",
                };
                LevelButton.Click += LevelButton_Click;

                var DeleteButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<Texture2D>("delete"))
                {
                    Position = new Vector2(730, 324),
                    
                };
                DeleteButton.Click += DeleteButton_Click;

                golf_ball.LoadContent(Content);
                _gameComponents = new List<Component>()
            {
                playButton,
                quitButton,
                settingsButton,
                shopingButton,
                LevelButton,
                DeleteButton,
            };
            }
            if (stateOfGame == "levels")
            {
                var BackButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "<",
                };
                BackButton.Click += BackButton_Click;

                var OneButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(0, 70),
                    Text = "Level 1",

                };
                OneButton.Click += OneButton_Click;

                var TwoButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(260, 70),
                    Text = "Level 2",

                };
                TwoButton.Click += TwoButton_Click;
                var ThreeButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(520, 70),
                    Text = "Level 3",

                };
                ThreeButton.Click += ThreeButton_Click;
                var FourButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(0, 200),
                    Text = "Level 4",

                };
                FourButton.Click += FourButton_Click;
                var FiveButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(260, 200),
                    Text = "Level 5",

                };
                FiveButton.Click += FiveButton_Click;

                golf_ball.LoadContent(Content);
                _gameComponents = new List<Component>()
                {
                    BackButton,
                    OneButton,
                    TwoButton,
                    ThreeButton,
                    FourButton,
                    FiveButton
                };
            }
            if (stateOfGame == "store")
            {
                var BackButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font/Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "<",
                };
                BackButton.Click += BackButton_Click;
                golf_ball.LoadContent(Content);
                _gameComponents = new List<Component>()
                {
                    BackButton
                };
            }
            else
            {
                // TODO: use this.Content to load your game content here
                golf_ball = new Ball(_sprite_batch);
                golf_ball.LoadContent(Content);
                // USE THESE METHODS TO ALTER BALL COSMETICS
                golf_ball.setHat(Content, "Sunglasses");
                golf_ball.setColor(Color.Aqua);

                shot = new Shot(_sprite_batch);
                shot.LoadContent(Content);
                hitbox = new Hitbox();
                hole = new Hole(new Vector2(100, 200), _sprite_batch,
                    hitbox, Vector2.One);
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
                for (int i = 0; i < coinList.Count; i++)
                {
                    if (coinList[i] != null)
                    {
                        coinList[i].LoadContent(Content);
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
        }
        private void ShopingButton_Click(object sender, EventArgs e)
        {
            stateOfGame = "store";
            LoadContent();
        }

        private void FiveButton_Click(object sender, EventArgs e)
        {
            if (playerRecord.isLevelFiveUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level = 4;
                stateOfGame = "play";
                LoadContent();
            }
            else
            {
                // Implement some code here for what happens if level
                // 5 is not unlocked
                Debug.WriteLine("Level 5 not unlocked!!");
            }

        }
        private void FourButton_Click(object sender, EventArgs e)
        {
            if (playerRecord.isLevelFourUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level = 3;
                stateOfGame = "play";
                LoadContent();
            }
            else
            {
                // Implement some code here for what happens if level
                // 4 is not unlocked
                Debug.WriteLine("Level 4 not unlocked!!");
            }

        }
        private void ThreeButton_Click(object sender, EventArgs e)
        {
            if (playerRecord.isLevelThreeUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level = 2;
                stateOfGame = "play";
                LoadContent();
            }
            else
            {
                // Implement some code here for what happens if level
                // 3 is not unlocked
                Debug.WriteLine("Level 3 not unlocked!!");
            }
        }

        private void TwoButton_Click(object sender, EventArgs e)
        {
            if (playerRecord.isLevelTwoUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level = 1;
                stateOfGame = "play";
                LoadContent();
            }
            else
            {
                // Implement some code here for what happens if level
                // 2 is not unlocked
                Debug.WriteLine("Level 2 not unlocked!!");
            }
        }

        private void OneButton_Click(object sender, EventArgs e)
        {
            if (playerRecord.isLevelOneUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level = 0;
                stateOfGame = "play";
                LoadContent();
            }
            else
            {
                // Implement some code here for what happens if level
                // 1 is not unlocked
                Debug.WriteLine("Level 1 not unlocked!!");
            }
        }

    private void SettingsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    private void LevelButton_Click(object sender, EventArgs e)
    {
            previousGameState = stateOfGame;
            stateOfGame = "levels";
            LoadContent();
    }
    private void BackButton_Click(object sender, EventArgs e)
    {
        stateOfGame = previousGameState;
        LoadContent();
    }
        private void QuitButton_Click(object sender, System.EventArgs e)
    {
        Exit();
    }
    private void PlayButton_Click(object sender, System.EventArgs e)
    {
        stateOfGame = "play";
        LoadContent();
    }

    private void DeleteButton_Click(object sender, System.EventArgs e)
    {
            //add delete progress conditions here
            SaveLoadSystem.DeleteSaveFile();
    }

        protected override void Update(GameTime gameTime)
        {
            // See if the user pressed Quit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (stateOfGame == "menu")
            {
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }

            if (stateOfGame == "levels")
            {
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }
            if(stateOfGame == "store")
            {
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }

            // TODO: Add your update logic here
            MouseState mouse_state = Mouse.GetState();
            moveMouseTo(mouse_state.X, mouse_state.Y);
            updateDragState(isDraggingBall(mouse_state, golf_ball));
            shot.Update(dragging_mouse, mouse_pos, golf_ball);
            hole.Update(golf_ball.center());
            golf_ball.Update(gameTime);
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].Update(golf_ball);
                }
            }
            for (int i = 0; i < coinList.Count; i++)
            {
                if (coinList[i] != null)
                {
                    if (coinList[i].Update(golf_ball))
                    {
                        addMoney(coinList[i].moneyAmount());
                        removeCoin(coinList[i]);
                    }
                }
            }
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].Update(golf_ball);
            }
            if (hole.getCollision() == true)
            {   
                // uses a flag to ensure stats counters are only
                // updated once per level
                if (canIncrementHolesCompleted)
                {
                    Debug.WriteLine("Updating stats counters...");
                    current_level++;
                    saveLevelScore(golf_ball.getStrokeCount(), current_level);
                    totalHolesCompleted++;

                    //Unlock the next level
                    unlockNextLevel(current_level);

                    // Note, user's lifetime strokes only update after a
                    // level is completed
                    totalStrokesLifetime += golf_ball.getStrokeCount();
                    canIncrementHolesCompleted = false;
                }

                if (nextLevelCheck())
                {
                    // reset flag back to true
                    canIncrementHolesCompleted = true;
                    level += 1;
                    hole.setCollision(false);
                    if (level < levels_list.Count)
                    {

                        golf_ball.setStrokeCount(0);
                        levels_list[level].Invoke();
                    }
                    else
                    {
                        // Display win screen
                        Exit();
                    }
                    
                }
                   
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(_render_target);
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            // Draw all obstacles in the obstacle list
            if (stateOfGame == "menu")
            {
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                base.Draw(gameTime);
            }
            else if(stateOfGame == "levels")
            {
                
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                base.Draw(gameTime);
            }
            else if(stateOfGame == "store")
            {
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                base.Draw(gameTime);
            }
            else
            {
            
                for (int i = 0; i < obstacle_list.Count; i++)
                {
                    if (obstacle_list[i] != null)
                    {
                        obstacle_list[i].Draw();
                    }
                }
                for (int i = 0; i < coinList.Count; i++)
                {
                    if (coinList[i] != null)
                    {
                        coinList[i].Draw(_sprite_batch);
                    }
                }
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i].Draw();
                }
                hole.Draw();
                shot.Draw();
                golf_ball.Draw();
                _sprite_batch.DrawString(Content.Load<SpriteFont>("Font"), "Stroke Count: " + golf_ball.getStrokeCount().ToString()
                   , strokeCounter, Color.Black);
                if(hole.getCollision() == true&&!coinAddLevel)
                {
                    drawVictoryScreen(shot.getStrokeCount());
                    golf_ball.setPosition(new Vector2(100000, 1000000));
                    coins += addCoins(golf_ball.getStrokeCount());
                    coinAddLevel = !coinAddLevel;
                }
                else if (hole.getCollision() == true)
                {
                    drawVictoryScreen(shot.getStrokeCount());
                    golf_ball.setPosition(new Vector2(100000, 1000000));
                }
                else if (hole.getCollision() == false)
                {
                    coinAddLevel = false;
                }
                //drawBorder();
                _sprite_batch.End();

                //GraphicsDevice.SetRenderTarget(null);
                //GraphicsDevice.Clear(Color.DarkOliveGreen);

                /*
                _sprite_batch.Begin();
                _sprite_batch.Draw(_render_target, render_target_rect, Color.White);
                _sprite_batch.End();
                */

                base.Draw(gameTime);
            }
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
        public void addCoin(Coin coin)
        {
            coin.LoadContent(Content);
            coinList.Add(coin);
        }
        public void removeCoin(Coin coin)
        {
            coinList.Remove(coin);
        }
        public void clearCoins()
        {
            coinList.Clear();
        }

        public void loadLevelZero()
        {
            golf_ball.setPosition(new Vector2(600, 200));
            hole.setPosition(new Vector2(100, 200));
            Coin coin1 = new Coin(new Vector2(350, 200),_sprite_batch);
            addCoin(coin1);
        }

        public void loadLevelOne()
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 350));
            hole.setPosition(new Vector2(25, 100));
            SandPit pit1 = new SandPit(new Vector2(100, -175),
                _sprite_batch, new Hitbox(), new Vector2(2f, 2f));
            addObstacle(pit1);
            SandPit pit2 = new SandPit(new Vector2(275, 250),
                _sprite_batch, new Hitbox(), new Vector2(2f, 2f));
            addObstacle(pit2);
            Coin coin1 = new Coin(new Vector2(350, 220), _sprite_batch);
            addCoin(coin1);
            Coin coin2 = new Coin(new Vector2(600, 100), _sprite_batch);
            addCoin(coin2);
            Coin coin3 = new Coin(new Vector2(150, 350), _sprite_batch);
            addCoin(coin3);
        }

        public void loadLevelTwo()
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(200, 200));
            Lake lake1 = new Lake(new Vector2(-50, 0),
                _sprite_batch, new Hitbox(), new Vector2(1f, 2.4f));
            addObstacle(lake1);
            Puddle puddle1 = new Puddle(new Vector2(400, -100),
                _sprite_batch, new Hitbox(), new Vector2(1.5f, 1.5f));
            addObstacle(puddle1);
            Puddle puddle2 = new Puddle(new Vector2(350, 300),
                _sprite_batch, new Hitbox(), new Vector2(1f, 1f));
            addObstacle(puddle2);
            Obstacle wall1 = new Obstacle(new Vector2(300, 185),
                _sprite_batch, new Hitbox(), new Vector2(25, 100));
            addObstacle(wall1);
            Coin coin1 = new Coin(new Vector2(350, 220), _sprite_batch);
            addCoin(coin1);
            Coin coin2 = new Coin(new Vector2(600, 210), _sprite_batch);
            addCoin(coin2);
            Coin coin3 = new Coin(new Vector2(290, 10), _sprite_batch);
            addCoin(coin3);
        }

        public void loadLevelThree()
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(100, 200));
            Mushroom mushroom1 = new Mushroom(new Vector2(200, 175),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(mushroom1);
            Mushroom mushroom2 = new Mushroom(new Vector2(100, 275),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(mushroom2);
            Mushroom mushroom3 = new Mushroom(new Vector2(100, 75),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(mushroom3);
            Mushroom mushroom4 = new Mushroom(new Vector2(450, 0),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(mushroom4);
            Mushroom mushroom5 = new Mushroom(new Vector2(600, 400),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(mushroom5);
            Coin coin1 = new Coin(new Vector2(240, 10), _sprite_batch);
            addCoin(coin1);
            Coin coin2 = new Coin(new Vector2(460, 100), _sprite_batch);
            addCoin(coin2);
            Coin coin3 = new Coin(new Vector2(170, 150), _sprite_batch);
            addCoin(coin3);
        }

        public void loadLevelFour()
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 425));
            hole.setPosition(new Vector2(125, 275));
            Downslope slope1 = new Downslope(new Vector2(0, 200),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(slope1);
            Downslope slope2 = new Downslope(new Vector2(600, 200),
                _sprite_batch, new Hitbox(), new Vector2(1, 1));
            addObstacle(slope2);
            Lake lake1 = new Lake(new Vector2(0, 0),
                _sprite_batch, new Hitbox(), new Vector2(4, 0.5f));
            addObstacle(lake1);
            Lake lake2 = new Lake(new Vector2(200, 200),
                _sprite_batch, new Hitbox(), new Vector2(2, 2));
            addObstacle(lake2);
            Coin coin1 = new Coin(new Vector2(350, 140), _sprite_batch);
            addCoin(coin1);
            Coin coin2 = new Coin(new Vector2(600, 140), _sprite_batch);
            addCoin(coin2);
            Coin coin3 = new Coin(new Vector2(130, 230), _sprite_batch);
            addCoin(coin3);
        }

        public void loadLevelFive()
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 400));
            hole.setPosition(new Vector2(75, 350));
            Downslope slope1 = new Downslope(new Vector2(0, 0),
                _sprite_batch, new Hitbox(), new Vector2(4, 1.5f));
            addObstacle(slope1);
            Lake lake1 = new Lake(new Vector2(200, 310),
                _sprite_batch, new Hitbox(), new Vector2(2, 2f));
            addObstacle(lake1);
            Obstacle wall1 = new Obstacle(new Vector2(300, 150),
                _sprite_batch, new Hitbox(), new Vector2(100, 25));
            addObstacle(wall1);
            Obstacle wall2 = new Obstacle(new Vector2(575, 250),
                _sprite_batch, new Hitbox(), new Vector2(100, 25));
            addObstacle(wall2);
            Coin coin1 = new Coin(new Vector2(350, 190), _sprite_batch);
            addCoin(coin1);
            Coin coin2 = new Coin(new Vector2(610, 130), _sprite_batch);
            addCoin(coin2);
            Coin coin3 = new Coin(new Vector2(120, 300), _sprite_batch);
            addCoin(coin3);
            Coin coin4 = new Coin(new Vector2(320, 90), _sprite_batch);
            addCoin(coin4);
        }

        public int calculateScore(int number_of_shots)
        {
            //scaling value to be determined
            int score = MAX_SCORE - number_of_shots * 343;

            if (score < 0)
            {
                score = 0;
            }
            return score;
        }
        public int calculateCoins(int number_of_shots)
        {
            //scaling value to be determined
            int coins = MAX_COINS - number_of_shots * 10;
            
            if (coins < 0)
            {
                coins = 0;
            }
            //this.coins += coins;
            return (int)this.coins;
        }
        public int addCoins(int number_of_shots)
        {
            //scaling value to be determined
            int coins = MAX_COINS - number_of_shots * 10;

            if (coins < 0)
            {
                coins = 0;
            }
            return coins;
        }

        public void populateVictoryScreen(int number_of_shots)
        {
            //Finds the  center of the text
            Vector2 textMiddlePoint = font.MeasureString("You Won!") / 2;
            // Finds were to place "You Won!")
            Vector2 position1 = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 5);

            //Used to position the score and coin amount on victory screen
            Vector2 position2 = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 3);

            //Methods to format the text 
            String score = "Score: " + calculateScore(number_of_shots).ToString();
            String coins = "Coins: " + calculateCoins(number_of_shots).ToString();


            //Populates the victory screen
            _sprite_batch.DrawString(font, "You Won!", position1, Color.Gold, 0, textMiddlePoint, 3.0f, SpriteEffects.None, 0.5f);
            _sprite_batch.DrawString(font, score, position2 - new Vector2(100, -40), Color.Black, 0, textMiddlePoint, 2.0f, SpriteEffects.None, 0.5f);
            _sprite_batch.DrawString(font, coins, position2 + new Vector2(100, 40), Color.Black, 0, textMiddlePoint, 2.0f, SpriteEffects.None, 0.5f);

            _sprite_batch.Draw(line, new Rectangle(Window.ClientBounds.Width / 2 - 75, Window.ClientBounds.Height - 240, 150, 75), null, Color.White, 2 * MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0);

            _sprite_batch.DrawString(font, "Next Level", new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height - 200), Color.Black, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 0.5f);
        }
        public void drawVictoryScreen(int number_of_shots)
        {
            line.SetData(new[] { Color.DarkSlateGray });
            _sprite_batch.Draw(line, new Rectangle(Window.ClientBounds.Width / 6 + 10, Window.ClientBounds.Height / 10, 500, 300), null,
                Color.LightGray, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
            populateVictoryScreen(golf_ball.getStrokeCount());
        }

        /**
         * It would be nice to base this method on an abstraction that can
         * have one instance per level but we simply do not have time to 
         * implement that, thus, here is a concretion that functions. 
         */
        public void saveLevelScore(int number_of_shots, int currentLevel)
        {
            Debug.WriteLine("num of shots: " + number_of_shots);
            
            int current_score = calculateScore(number_of_shots);

            if (current_score > playerRecord.playerScoreLevelOne && currentLevel == 1)
                playerRecord.playerScoreLevelOne = current_score;
            else if (current_score > playerRecord.playerScoreLevelTwo && currentLevel == 2)
                playerRecord.playerScoreLevelTwo = current_score;
            else if (current_score > playerRecord.playerScoreLevelThree && currentLevel == 3)
                playerRecord.playerScoreLevelThree = current_score;
            else if (current_score > playerRecord.playerScoreLevelFour && currentLevel == 4)
                playerRecord.playerScoreLevelFour = current_score;
            else if (current_score > playerRecord.playerScoreLevelFive && currentLevel == 5)
                playerRecord.playerScoreLevelFive = current_score;
        }

        public void unlockNextLevel(int currentLevel)
        {
            if (currentLevel == 1)
                playerRecord.isLevelTwoUnlocked = true;
            else if (currentLevel == 2)
                playerRecord.isLevelThreeUnlocked = true;
            else if (currentLevel == 3)
                playerRecord.isLevelFourUnlocked = true;
            else if (currentLevel == 4)
                playerRecord.isLevelFiveUnlocked = true;
            else
                Debug.WriteLine("NO MORE LEVELS!");
        }

        public bool nextLevelCheck()
        {
            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonClicked = currentMouseState.LeftButton == ButtonState.Pressed;

            // Check if left button was clicked and released
            bool wasLeftButtonClickedAndReleased = isLeftButtonClicked && prevMouseState.LeftButton == ButtonState.Released;

            if (wasLeftButtonClickedAndReleased)
            {
                Rectangle Rect = new Rectangle(Window.ClientBounds.Width / 2 - 75, Window.ClientBounds.Height - 240, 150, 75);

                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                if (Rect.Contains(mousePosition))
                {
                    return true;
                }

            }

            // Update the previous mouse state for the next frame
            prevMouseState = currentMouseState;
            return false;


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
        /// Gets the game's sprite batch used for drawing sprites
        /// </summary>
        /// <returns>the reference to the sprite batch in this game.</returns>
        /// -------------------------------------------------------------------
        public SpriteBatch getSpriteBatch()
        {
            return _sprite_batch;
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
            playerRecord.Coins = coins;
            playerRecord.TotalHolesCompleted = totalHolesCompleted;
            playerRecord.TotalStrokesLifetime = totalStrokesLifetime;
            SaveLoadSystem.Save(playerRecord);
        }

        public void addMoney(float amount)
        {
            coins += (int)amount;
        }
    }
}