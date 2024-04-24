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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// GameControl creates a game scene that draws objects on screen and
    /// enables user input via the mouse. Update is called every frame to both
    /// detect user input and to update the state of each active game object
    /// </summary>-------------------------------------------------------------
    public class GameControl : Game
    {
        private const int DEFAULT_RES_WIDTH = 800;
        private const int DEFAULT_RES_HEIGHT = 480;

        private GraphicsDevice _device;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;
        private Renderer renderer;
        private int MAX_SCORE = 5000;
        private int MAX_COINS = 25;
        private bool clickedNext;

        //Settings variables for now
        Texture2D arrowTexture;
        private const int MAX_SETTINGS_VAL = 9;
        private const int MIN_SETTINGS_VAL = 2;
        private float _holeSize = 5;
        private float _sensitivity = 5;
        private float _volume = 5;
        private MouseState previousMouseState;
        private MouseState currentMouseState;


        private Vector2 mouse_pos;
        private bool dragging_mouse = false;
        private bool game_paused = false;

        List<Song> songs = new List<Song>();
        private List<SoundEffect> soundEffects;
        private bool songStart = false;
        private bool songStartLevel = false;
        private bool playedHole = false;
        private bool playedSwing = false;
        private int counter;

        private Ball golf_ball;
        private Shot shot;
        private Hole hole;
        private Hitbox hitbox;
        private LevelManager level_manager;
        private Texture2D background;
        private Texture2D cursor;
        SpriteFont font;
        MouseState prevMouseState;
        MouseState prevMouseStateVol;
        KeyboardState previousKeyState;

        private PlayerRecord playerRecord;
        private int totalHolesCompleted;
        private bool canIncrementHolesCompleted = true;
        private int totalStrokesLifetime;
        private int current_level = 0;
        bool isFirstContentLoad = true;

        Texture2D line;
        private float angleOfLine;
        private Vector2 game_resolution = new Vector2(DEFAULT_RES_WIDTH, 
            DEFAULT_RES_HEIGHT);

        private List<Button> _gameComponents;
        private String stateOfGame = "menu";
        private String previousGameState = "menu";
        Vector2 strokeCounter;

        private int coins = 0;
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

            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            renderer = new Renderer(_graphics.GraphicsDevice, 
                DEFAULT_RES_WIDTH, DEFAULT_RES_HEIGHT);
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = (int)game_resolution.X;
            _graphics.PreferredBackBufferHeight = (int)game_resolution.Y;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();
            renderer.setDestination();
            

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


        protected override void LoadContent()
        {
            // Load the current user name and stroke count
            playerRecord = SaveLoadSystem.Load<PlayerRecord>();

            //Set volume based on settings
            MediaPlayer.Volume = getVolumeVal() / 10;



            arrowTexture = Content.Load<Texture2D>("arrow");

            //Loading sounds used in game
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(Content.Load<SoundEffect>("holeSound"));
            soundEffects.Add(Content.Load<SoundEffect>("swing"));


            songs.Add(Content.Load<Song>("MainMenu"));
            songs.Add(Content.Load<Song>("Take a Swing"));

            // Load Saved Data

            playerRecord.isLevelOneUnlocked = true;
            coins = playerRecord.Coins;
            totalHolesCompleted = playerRecord.TotalHolesCompleted;
            totalStrokesLifetime = playerRecord.TotalStrokesLifetime;
            _sensitivity = playerRecord.swingSensitivityPreference;
            _holeSize = playerRecord.holeSize;
            
            // Load the graphics device
            _device = GraphicsDevice;
            _sprite_batch = new SpriteBatch(_device);
            font = Content.Load<SpriteFont>("File");
            
            if (stateOfGame == "menu")
            {
                mainMusicCheck();


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

                var LevelButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
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
                _gameComponents = new List<Button>()
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
                mainMusicCheck();

                var BackButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "<",
                };
                BackButton.Click += BackButton_Click;

                var OneButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 70),
                    Text = "Level 1",

                };
                OneButton.Click += OneButton_Click;

                var TwoButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(260, 70),
                    Text = "Level 2",

                };
                if (playerRecord.isLevelTwoUnlocked == false)
                {
                    TwoButton._isHoveringColour = Color.Black;
                    TwoButton.color = Color.Black;
                }
                TwoButton.Click += TwoButton_Click;
                var ThreeButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(520, 70),
                    Text = "Level 3",

                };
                if(playerRecord.isLevelThreeUnlocked == false)
                {
                    ThreeButton._isHoveringColour = Color.Black;
                    ThreeButton.color = Color.Black;
                }
                ThreeButton.Click += ThreeButton_Click;
                var FourButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 200),
                    Text = "Level 4",

                };
                if (playerRecord.isLevelFourUnlocked == false)
                {
                    FourButton._isHoveringColour = Color.Black;
                    FourButton.color = Color.Black;
                }
                FourButton.Click += FourButton_Click;
                var FiveButton = new Button(Content.Load<Texture2D>("button"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(260, 200),
                    Text = "Level 5",

                };
                if (playerRecord.isLevelFiveUnlocked == false)
                {
                    FiveButton._isHoveringColour = Color.Black;
                    FiveButton.color = Color.Black;
                }
                FiveButton.Click += FiveButton_Click;

                golf_ball.LoadContent(Content);
                _gameComponents = new List<Button>()
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
                mainMusicCheck();
                var BackButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "<",
                };
                BackButton.Click += BackButton_Click;

                var MoneyButton = new Button(Content.Load<Texture2D>("price tag"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(660, 0),

                    Text = playerRecord.Coins.ToString(),
                };

                

                Button Cosmetic1Button = null;
                Button Cosmetic2Button = null;
                Button Cosmetic3Button = null;

                if (playerRecord.isCosmeticOneUnlocked == false)
                {

                    Cosmetic1Button = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                    {
                        Position = new Vector2(130, 130),
                        Text = "50",
                    };
                }
                else
                {
                    Cosmetic1Button = new Button(Content.Load<Texture2D>("check"), Content.Load<Texture2D>("check"))
                    {
                        Position = new Vector2(130, 130),

                    };
                    Cosmetic1Button._isHoveringColour = Color.Green;
                }
                Cosmetic1Button.Click += Cosmetic1Button_Click;

                var Sunglasses = new Button(Content.Load<Texture2D>("Sunglasses"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(65, 130),
                    _isHoveringColour = Color.White,
                    PenColour = Color.Red,
                    Text = " "
                };
                if (playerRecord.currentCosmetic == "Sunglasses")
                {
                    Sunglasses.Text = "E";
                }

                if (playerRecord.isCosmeticTwoUnlocked == false)
                {

                    Cosmetic2Button = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                    {
                        Position = new Vector2(130, 260),
                        Text = "50",
                    };
                }
                else
                {
                    Cosmetic2Button = new Button(Content.Load<Texture2D>("check"), Content.Load<Texture2D>("check"))
                    {
                        Position = new Vector2(130, 260),

                    };
                    Cosmetic2Button._isHoveringColour = Color.Green;
                }
                Cosmetic2Button.Click += Cosmetic2Button_Click;

                var TopHat = new Button(Content.Load<Texture2D>("TopHat"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(65, 260),
                    _isHoveringColour = Color.White,
                    PenColour = Color.Red,
                    Text = " "
                };
                if (playerRecord.currentCosmetic == "TopHat")
                {
                    TopHat.Text = "E";
                }

                if (playerRecord.isCosmeticThreeUnlocked == false)
                {

                    Cosmetic3Button = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                    {
                        Position = new Vector2(130, 390),
                        Text = "50",
                    };
                    
                }
                else
                {
                    Cosmetic3Button = new Button(Content.Load<Texture2D>("check"), Content.Load<Texture2D>("check"))
                    {
                        Position = new Vector2(130, 390),

                    };
                    Cosmetic3Button._isHoveringColour = Color.Green;
                }
                Cosmetic3Button.Click += Cosmetic3Button_Click;

                var NoveltySodaDrinkHat = new Button(Content.Load<Texture2D>("NoveltySodaDrinkHat"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(65, 390),
                    _isHoveringColour = Color.White,
                    PenColour = Color.Red,
                    Text =  " "
                };
                if(playerRecord.currentCosmetic == "NoveltySodaDrinkHat")
                {
                    NoveltySodaDrinkHat.Text = "E";
                }


                golf_ball.LoadContent(Content);
                _gameComponents = new List<Button>()
                {
                    BackButton,
                    MoneyButton,
                    Cosmetic1Button,
                    Cosmetic2Button,
                    Cosmetic3Button,
                    NoveltySodaDrinkHat,
                    TopHat,
                    Sunglasses,
                };
            }
            if (stateOfGame == "Settings")
            {
                mainMusicCheck();
                var BackButton = new Button(Content.Load<Texture2D>("smallbutton"), Content.Load<SpriteFont>("Font"))
                {
                    Position = new Vector2(0, 0),
                    Text = "<",
                };
                BackButton.Click += BackButton_Click;
                _gameComponents = new List<Button>()
                {
                    BackButton
                };
            }
            else
            {
                // TODO: use this.Content to load your game content here
                background = Content.Load<Texture2D>("background");
                golf_ball = new Ball(_sprite_batch);
                golf_ball.setVirtualScale(renderer.getScale());
                golf_ball.setVirtualOffset(renderer.getOffset());
                golf_ball.LoadContent(Content);
                // USE THESE METHODS TO ALTER BALL COSMETICS
                //golf_ball.setHat(Content, null);
                golf_ball.setColor(Color.White);
                if(playerRecord.currentCosmetic != "null")
                {
                    golf_ball.setHat(Content, playerRecord.currentCosmetic);
                }
                
                shot = new Shot(_sprite_batch);
                shot.LoadContent(Content);
                hitbox = new Hitbox();
                hole = new Hole(new Vector2(100, 200), _sprite_batch,
                    hitbox, Vector2.One);
                hole.LoadContent(Content);

                level_manager = new LevelManager(golf_ball, hole, hitbox);
                level_manager.loadBorders((int)game_resolution.X, (int)game_resolution.Y);
                level_manager.generateLevelList();
                level_manager.loadCurrentLevel(_sprite_batch, Content);
                
            }
            for (int i = 0; i < _gameComponents.Count; i++)
            {
                _gameComponents[i].setLocalScale(renderer.getScale());
                _gameComponents[i].setOffset(renderer.getOffset());
            }
           
        }

        /*
         * This method subtracts the appropriate amount of coins for purchasing
         * a new hat cosmetic and saves the new player coin total to the save file. 
         */
        private void purchaseHat()
        {
            coins -= 50;
            playerRecord.Coins = coins;
            SaveLoadSystem.Save(playerRecord);
        }

        private void Cosmetic3Button_Click(object sender, EventArgs e)
        {
            if (playerRecord.Coins >= 50 || playerRecord.isCosmeticThreeUnlocked == true)
            {
                if (playerRecord.isCosmeticThreeUnlocked == false)
                {
                    purchaseHat();
                }
                golf_ball.setHat(Content, "NoveltySodaDrinkHat");
                playerRecord.currentCosmetic = "NoveltySodaDrinkHat";
                playerRecord.isCosmeticThreeUnlocked = true;
                saveGame();
                LoadContent();
            }
        }

        private void Cosmetic2Button_Click(object sender, EventArgs e)
        {
            if (playerRecord.Coins >= 50 || playerRecord.isCosmeticTwoUnlocked == true)
            {
                if (playerRecord.isCosmeticTwoUnlocked == false)
                {
                    purchaseHat();
                }
                golf_ball.setHat(Content, "TopHat");
                playerRecord.currentCosmetic = "TopHat";
                playerRecord.isCosmeticTwoUnlocked = true;
                saveGame();
                LoadContent();
            }
        }

        private void Cosmetic1Button_Click(object sender, EventArgs e)
        {
            if (playerRecord.Coins >= 50 || playerRecord.isCosmeticOneUnlocked == true)
            {
                if(playerRecord.isCosmeticOneUnlocked == false)
                {
                    purchaseHat();
                }

                golf_ball.setHat(Content, "Sunglasses");
                playerRecord.currentCosmetic = "Sunglasses";
                playerRecord.isCosmeticOneUnlocked = true;
                saveGame();
                LoadContent();
            }
        }
        private void ShopingButton_Click(object sender, EventArgs e)
        {
            stateOfGame = "store";
            LoadContent();
        }

        private void FiveButton_Click(object sender, EventArgs e)
        {
            if (songStartLevel == false)
            {
                playSong(1);
                songStart = false;
            }
            songStartLevel = true;

            if (playerRecord.isLevelFiveUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level_manager.setLevel(4);
                current_level = 4;
                stateOfGame = "play";
                level_manager.loadCurrentLevel(_sprite_batch, Content);
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
            if (songStartLevel == false)
            {
                playSong(1);
                songStart = false;
            }
            songStartLevel = true;

            if (playerRecord.isLevelFourUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level_manager.setLevel(3);
                current_level = 3;
                stateOfGame = "play";
                level_manager.loadCurrentLevel(_sprite_batch, Content);
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
            if (songStartLevel == false)
            {
                playSong(1);
                songStart = false;
            }
            songStartLevel = true;

            if (playerRecord.isLevelThreeUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level_manager.setLevel(2);
                current_level = 2;
                stateOfGame = "play";
                level_manager.loadCurrentLevel(_sprite_batch, Content);
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
            if (songStartLevel == false)
            {
                playSong(1);
                songStart = false;
            }
            songStartLevel = true;

            if (playerRecord.isLevelTwoUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level_manager.setLevel(1);
                current_level = 1;
                stateOfGame = "play";
                level_manager.loadCurrentLevel(_sprite_batch, Content);
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
            if (songStartLevel == false)
            {
                playSong(1);
                songStart = false;
            }
            songStartLevel = true;

            if (playerRecord.isLevelOneUnlocked)
            {
                // Yes, this is correct because level 1 has value: level = 0
                level_manager.setLevel(0);
                current_level = 0;
                stateOfGame = "play";
                level_manager.loadCurrentLevel(_sprite_batch, Content);
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
            previousGameState = stateOfGame;
            stateOfGame = "Settings";
            LoadContent();
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
        songStart = false;
        MediaPlayer.Stop();
        playSong(1);
        LoadContent();
    }

    private void windowClientSizeChanged(object sender, System.EventArgs e)
    {
        _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        _graphics.ApplyChanges();
        renderer.setDestination();
        golf_ball.setVirtualScale(renderer.getScale());
        golf_ball.setVirtualOffset(renderer.getOffset());
        for (int i = 0; i < _gameComponents.Count; i++)
        {
             _gameComponents[i].setLocalScale(renderer.getScale());
             _gameComponents[i].setOffset(renderer.getOffset());
        }
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
            if(IsKeyPressed())
            {
                
                stateOfGame = "Settings";
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }

            Window.ClientSizeChanged += windowClientSizeChanged;
            if (stateOfGame == "menu")
            {
                

                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }
            else if (stateOfGame == "levels")
            {
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }
            else if (stateOfGame == "store")
            {
                foreach (var component in _gameComponents)
                {
                    component.Update(gameTime);
                }
            }
            else if (stateOfGame == "Settings")
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
            addMoney(level_manager.Update());
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
                    SaveLoadSystem.Save(playerRecord);
                }

                if (nextLevelCheck())
                {
                    playedHole = false;
                    canIncrementHolesCompleted = true;
                    level_manager.loadNextLevel(_sprite_batch, Content);
                }
            }
            swingCounter();
            base.Update(gameTime);
        }
        private void swingCounter()
        {
            
            if(golf_ball.getStrokeCount() > counter)
            {
                counter++;
                soundEffects[1].Play();
            }
            else if (golf_ball.getStrokeCount() == 0)
            {
                counter = 0;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.setActive(Color.Black);

            // TODO: Add your drawing code here
            _sprite_batch.Begin();
            // Draw all obstacles in the obstacle list
            _sprite_batch.Draw(background, Vector2.Zero, Color.DarkOliveGreen);
            if (stateOfGame == "menu")
            {   
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                renderer.Draw(_sprite_batch, Color.Black);
                base.Draw(gameTime);
            }
            else if (stateOfGame == "levels")
            {
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                renderer.Draw(_sprite_batch, Color.Black);
                base.Draw(gameTime);
            }
            else if (stateOfGame == "store")
            {
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                renderer.Draw(_sprite_batch, Color.Black);
                base.Draw(gameTime);
            }
            else if (stateOfGame == "Settings")
            {
                drawSettingsScreen();
                foreach (var component in _gameComponents)
                {
                    {
                        component.Draw(gameTime, _sprite_batch);
                    }
                }
                _sprite_batch.End();
                renderer.Draw(_sprite_batch, Color.Black);
                base.Draw(gameTime);
            }
            else
            {
                level_manager.Draw(_sprite_batch);
                hole.Draw();
                shot.Draw();
                golf_ball.Draw();
                _sprite_batch.DrawString(Content.Load<SpriteFont>("Font"), "Stroke Count: " + golf_ball.getStrokeCount().ToString()
                   , strokeCounter, Color.Black);
                if(hole.getCollision() == true&&!coinAddLevel)
                {
                    if(playedHole == false)
                    {
                        soundEffects[0].Play();
                        playedHole = true;
                    }
                    drawVictoryScreen(shot.getStrokeCount());
                    golf_ball.setPosition(new Vector2(100000, 1000000));
                    coins += addCoins(golf_ball.getStrokeCount());
                    SaveLoadSystem.Save(playerRecord);
                    coinAddLevel = !coinAddLevel;
                }
                else if (hole.getCollision() == true)
                {
                    if (playedHole == false)
                    {
                        soundEffects[0].Play();
                        playedHole = true;
                    }
                    drawVictoryScreen(shot.getStrokeCount());
                    golf_ball.setPosition(new Vector2(100000, 1000000));
                }
                else if (hole.getCollision() == false)
                {
                    coinAddLevel = false;
                }
                _sprite_batch.End();
                renderer.Draw(_sprite_batch, Color.Black);
                base.Draw(gameTime);
            }
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

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

        /// <summary>----------------------------------------------------------
        /// Adds the parameter "amount" of coins to the local variable coins
        /// </summary>---------------------------------------------------------
        public void addMoney(float amount)
        {
            coins += (int)amount;
        }

        public void populateVictoryScreen(int number_of_shots)
        {
            //Finds the  center of the text
            Vector2 textMiddlePoint = font.MeasureString("You Won!") / 2;
            // Finds were to place "You Won!")
            Vector2 screen_center = new Vector2(game_resolution.X / 2, game_resolution.Y / 2);
            Vector2 win_text_pos = new Vector2(0, -100) + screen_center;
            Vector2 score_text_pos = new Vector2(-100, -40) + screen_center;
            Vector2 coins_text_pos = new Vector2(100, -40) + screen_center;
            Vector2 next_text_pos = new Vector2(0, 40) + screen_center;
            Vector2 next_button_size = new Vector2(150, 75);
            Vector2 next_button_pos = new Vector2(-(next_button_size.X / 2f), 0) + screen_center;

            //Methods to format the text 
            String score = "Score: " + calculateScore(number_of_shots).ToString();
            String coins = "Coins: " + calculateCoins(number_of_shots).ToString();


            //Populates the victory screen
            _sprite_batch.DrawString(font, "You Won!", win_text_pos, Color.Gold, 0, textMiddlePoint, 3.0f, SpriteEffects.None, 0.5f);
            _sprite_batch.DrawString(font, score, score_text_pos, Color.Black, 0, textMiddlePoint, 2.0f, SpriteEffects.None, 0.5f);
            _sprite_batch.DrawString(font, coins, coins_text_pos, Color.Black, 0, textMiddlePoint, 2.0f, SpriteEffects.None, 0.5f);

            _sprite_batch.Draw(line, new Rectangle((int)next_button_pos.X, (int)next_button_pos.Y, 
                (int)next_button_size.X, (int)next_button_size.Y), null, Color.White, 2 * MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0);

            _sprite_batch.DrawString(font, "Next Level", new Vector2(next_text_pos.X, next_text_pos.Y), 
                Color.Black, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 0.5f);
        }
        public void drawVictoryScreen(int number_of_shots)
        {
            Vector2 screen_center = new Vector2(game_resolution.X / 2, game_resolution.Y / 2);
            Vector2 win_screen_size = new Vector2(500, 300);
            Vector2 win_screen_pos = new Vector2(-(win_screen_size.X / 2f), -200) + screen_center;
            line.SetData(new[] { Color.DarkSlateGray });
            _sprite_batch.Draw(line, new Rectangle((int)win_screen_pos.X, (int)win_screen_pos.Y, 
                (int)win_screen_size.X, (int)win_screen_size.Y), null,
                Color.LightGray, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
            populateVictoryScreen(golf_ball.getStrokeCount());
        }

        public void playSong(int songChoice)
        {
            MediaPlayer.Play(songs[songChoice]);
        }

        //Settings class for now until we implement screen managment

        public float AdjustHoleVal()
        {
            _holeSize = playerRecord.holeSize;
            Vector2 screen_center = new Vector2(game_resolution.X / 2, game_resolution.Y / 2);
            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonClicked = currentMouseState.LeftButton == ButtonState.Pressed;

            Vector2 up_button_size = new Vector2(86 * renderer.getScale(), 
                86 * renderer.getScale());
            Vector2 up_button_pos = new Vector2(-43, 0) + screen_center;

            // Check if left button was clicked and released
            bool wasLeftButtonClickedAndReleased = isLeftButtonClicked && previousMouseState.LeftButton == ButtonState.Released;

            if (wasLeftButtonClickedAndReleased)
            {
                Rectangle upArrowRect = new Rectangle((Window.ClientBounds.Width / 6 + 95),
                                            (Window.ClientBounds.Height / 2 + 100),
                                            arrowTexture.Width / 15, arrowTexture.Height / 15);
                Rectangle downArrowRect = new Rectangle((Window.ClientBounds.Width / 6 - 35),
                                     (Window.ClientBounds.Height / 2 + 100),
                                     arrowTexture.Width / 15, arrowTexture.Height / 15);

                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                if (upArrowRect.Contains(mousePosition) && _holeSize <= MAX_SETTINGS_VAL)
                {
                    _holeSize += 1;
                }
                else if (downArrowRect.Contains(mousePosition) && _holeSize >= MIN_SETTINGS_VAL)
                {
                    _holeSize -= 1;
                }
                // Update the save data
                playerRecord.holeSize = (int)_holeSize;
                SaveLoadSystem.Save(playerRecord);
            }

            // Update the previous mouse state for the next frame
            previousMouseState = currentMouseState;

            return _holeSize;
        }

        public float AdjustVolumeVal()
        {
            _volume = playerRecord.volumePreference;
            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonClicked = currentMouseState.LeftButton == ButtonState.Pressed;

            // Check if left button was clicked and released
            bool wasLeftButtonClickedAndReleased = isLeftButtonClicked && prevMouseStateVol.LeftButton == ButtonState.Released;

            if (wasLeftButtonClickedAndReleased)
            {
                Rectangle upArrowRect = new Rectangle((Window.ClientBounds.Width / 2 + 15),
                                            (Window.ClientBounds.Height / 2 - 40),
                                            arrowTexture.Width / 15, arrowTexture.Height / 15);
                Rectangle downArrowRect = new Rectangle((Window.ClientBounds.Width / 2 - 120),
                                     (Window.ClientBounds.Height / 2 - 40),
                                     arrowTexture.Width / 15, arrowTexture.Height / 15);

                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                if (upArrowRect.Contains(mousePosition) && _volume <= MAX_SETTINGS_VAL)
                {
                    _volume += 1;

                }
                if (downArrowRect.Contains(mousePosition) && _volume >= MIN_SETTINGS_VAL)
                {
                    _volume -= 1;
                }
            }

            // Update the save data
            playerRecord.volumePreference = (int)_volume;
            SaveLoadSystem.Save(playerRecord);

            // Update the previous mouse state for the next frame
            prevMouseStateVol = currentMouseState;

            return _volume;
        }

        public float AdjustSensitivityVal()
        {
            _sensitivity = playerRecord.swingSensitivityPreference;
            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonClicked = currentMouseState.LeftButton == ButtonState.Pressed;

            // Check if left button was clicked and released
            bool wasLeftButtonClickedAndReleased = isLeftButtonClicked && prevMouseState.LeftButton == ButtonState.Released;

            if (wasLeftButtonClickedAndReleased)
            {
                Rectangle upArrowRect = new Rectangle((Window.ClientBounds.Width / 2 + 240),
                                     (Window.ClientBounds.Height / 2 + 100),
                                     arrowTexture.Width / 15, arrowTexture.Height / 15);
                Rectangle downArrowRect = new Rectangle((Window.ClientBounds.Width / 2 + 115),
                                            (Window.ClientBounds.Height / 2 + 100),
                                            arrowTexture.Width / 15, arrowTexture.Height / 15);

                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                if (upArrowRect.Contains(mousePosition) && _sensitivity <= MAX_SETTINGS_VAL)
                {
                    _sensitivity += 1;
                }
                if (downArrowRect.Contains(mousePosition) && _sensitivity >= MIN_SETTINGS_VAL)
                {
                    _sensitivity -= 1;
                }
                // Update the save data
                playerRecord.swingSensitivityPreference = (int)_sensitivity;
                SaveLoadSystem.Save(playerRecord);
            }

            // Update the previous mouse state for the next frame
            prevMouseState = currentMouseState;

            return _sensitivity;
        }
        public void checkFirstContentLoad()
        {
            if (isFirstContentLoad)
            {
                if (playerRecord.holeSize == 0 && playerRecord.swingSensitivityPreference == 0
                    && playerRecord.volumePreference == 0)
                {
                    Debug.WriteLine("No Previous Settings Save Data. Defaults set to 5.");
                    playerRecord.holeSize = 5;
                    playerRecord.swingSensitivityPreference = 5;
                    playerRecord.volumePreference = 5;
                }
            }
        }

        public void populateSettingsScreen()
        {
            checkFirstContentLoad();

            Vector2 textMiddlePoint = font.MeasureString("Settings") / 2;
            Vector2 screen_center = new Vector2(game_resolution.X / 2, game_resolution.Y / 2);
            Vector2 settings_text_pos = new Vector2(0, -175) + screen_center;

            Vector2 hole_text_pos = new Vector2(-200, 100) + screen_center;
            Vector2 hole_value_pos = new Vector2(-140, 150) + screen_center;
            Vector2 hole_down_arrow_pos = new Vector2(-240, 150) + screen_center;
            Vector2 hole_up_arrow_pos = new Vector2(-150, 150) + screen_center;

            Vector2 sensitivity_text_pos = new Vector2(160, 100) + screen_center;
            Vector2 sensitivity_value_pos = new Vector2(275, 150) + screen_center;
            Vector2 sensitivity_down_arrow_pos = new Vector2(175, 150) + screen_center;
            Vector2 sensitivity_up_arrow_pos = new Vector2(265, 150) + screen_center;



            String holeSize = AdjustHoleVal().ToString();

            String sensitivity = AdjustSensitivityVal().ToString();

            String volume = AdjustVolumeVal().ToString();

            //Populate Settings screen
            _sprite_batch.DrawString(font, "Settings", settings_text_pos,
                Color.Black, 0, textMiddlePoint, 3.0f, SpriteEffects.None, 0.5f);

            _sprite_batch.DrawString(font, "Hole Size", hole_text_pos,
                Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);

            _sprite_batch.DrawString(font, "Swing Sensitivity", sensitivity_text_pos,
               Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);

            _sprite_batch.DrawString(font, "Volume", new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 - 50),
              Color.Black, 0, textMiddlePoint, 2.2f, SpriteEffects.None, 0.5f);


            if (AdjustHoleVal() >= 1 && AdjustHoleVal() < 10)
            {
                _sprite_batch.DrawString(font, holeSize, new Vector2(hole_value_pos.X -
                    font.MeasureString(holeSize).X / 2, hole_value_pos.Y),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }
            else
            {
                _sprite_batch.DrawString(font, holeSize, new Vector2(hole_value_pos.X -
                    font.MeasureString(holeSize).X / 2, hole_value_pos.Y),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }
            if (AdjustVolumeVal() >= 1 && AdjustVolumeVal() < 10)
            {
                _sprite_batch.DrawString(font, volume, new Vector2(Window.ClientBounds.Width / 2 + 40, Window.ClientBounds.Height / 2),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }
            else
            {
                _sprite_batch.DrawString(font, volume, new Vector2(Window.ClientBounds.Width / 2 + 35, Window.ClientBounds.Height / 2),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }

            if (AdjustSensitivityVal() >= 1 && AdjustSensitivityVal() < 10)
            {
                _sprite_batch.DrawString(font, sensitivity, new Vector2(sensitivity_value_pos.X -
                    font.MeasureString(sensitivity).X / 2, sensitivity_value_pos.Y),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }
            else
            {
                _sprite_batch.DrawString(font, sensitivity, new Vector2(sensitivity_value_pos.X -
                    font.MeasureString(sensitivity).X / 2, sensitivity_value_pos.Y),
              Color.Black, 0, textMiddlePoint, 2f, SpriteEffects.None, 0.5f);
            }

            //Down arrow for hole size
            _sprite_batch.Draw(arrowTexture, hole_down_arrow_pos, null,
                Color.White, 0f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);

            //Up arrow for hole size
            _sprite_batch.Draw(arrowTexture, hole_up_arrow_pos, null,
                Color.White, 3.14f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);

            //Down arrow for sensitivity
            _sprite_batch.Draw(arrowTexture, sensitivity_down_arrow_pos, null,
                Color.White, 0f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);

            //Up arrow for sensitivity
            _sprite_batch.Draw(arrowTexture, sensitivity_up_arrow_pos, null,
                Color.White, 3.14f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);

            //Down arrow for Volume
            _sprite_batch.Draw(arrowTexture, new Vector2(Window.ClientBounds.Width / 2 - 60, Window.ClientBounds.Height / 2), null,
               Color.White, 0f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);

            //Up arrow for volume
            _sprite_batch.Draw(arrowTexture, new Vector2(Window.ClientBounds.Width / 2 + 40, Window.ClientBounds.Height / 2), null,
              Color.White, 3.14f, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), 0.04f, SpriteEffects.None, 0f);
        }
        public void drawSettingsScreen()
        {
            line.SetData(new[] { Color.DarkSlateGray });
            _sprite_batch.Draw(line, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,
                Color.LightGray, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
            populateSettingsScreen();
        }

        public float getHoleSize()
        {
            return _holeSize;
        }

        public float getSensitivityVal()
        {
            return _sensitivity;
        }
        public float getVolumeVal()
        {
            return _volume;
        }

        public bool IsKeyPressed()
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            bool isKeyPressed = currentKeyState.IsKeyDown(Keys.P);

            // Check if P was pressed and released
            bool wasKeyPressedAndReleased = isKeyPressed && !previousKeyState.IsKeyDown(Keys.P);

            // Update the previous key state for the next frame
            previousKeyState = currentKeyState;

            return wasKeyPressedAndReleased;
        }

        public void mainMusicCheck()
        {
            if((stateOfGame == "menu" || stateOfGame == "Settings" || stateOfGame == "levels" || stateOfGame == "store") && songStart == false)
            {
                playSong(0);
                 
            }
            songStart = true;
        }

        /// <summary>----------------------------------------------------------
        /// This method takes the score that the player earned for the current 
        /// level and if it is larger than the current high score for that level,
        /// updates the current high score to the newly earned high score. 
        /// </summary>---------------------------------------------------------
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

        /// <summary>----------------------------------------------------------
        /// This method checks to see which level the user is currently on
        /// and then unlocks the level that comes immediately after that level. 
        /// This method is designed to be called when the player scores a hole
        /// and is viewing the victory screen. 
        /// </summary>---------------------------------------------------------
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
            Vector2 screen_center = new Vector2(game_resolution.X / 2 * renderer.getScale(), 
                game_resolution.Y / 2 * renderer.getScale());
            Vector2 next_button_size = new Vector2(150 * renderer.getScale(), 75 * renderer.getScale());
            Vector2 next_button_pos = new Vector2(-75, 0) + screen_center;

            MouseState currentMouseState = Mouse.GetState();
            bool isLeftButtonClicked = currentMouseState.LeftButton == ButtonState.Pressed;

            // Check if left button was clicked and released
            bool wasLeftButtonClickedAndReleased = isLeftButtonClicked && prevMouseState.LeftButton == ButtonState.Released;

            if (wasLeftButtonClickedAndReleased)
            {
                Rectangle Rect = new Rectangle((int)next_button_pos.X, (int)next_button_pos.Y, 
                    (int)next_button_size.X, (int)next_button_size.Y);

                if (Rect.Contains(mouse_pos))
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

        /// <summary>----------------------------------------------------------
        /// Saves all of the local data to the player record and then calls 
        /// the saveLoadSystem to save the player record to the xml file. 
        /// </summary>---------------------------------------------------------
        public void saveGame()
        {
            playerRecord.Strokes = shot.getStrokeCount();
            playerRecord.Coins = coins;
            playerRecord.TotalHolesCompleted = totalHolesCompleted;
            playerRecord.TotalStrokesLifetime = totalStrokesLifetime;
            playerRecord.currentCosmetic = golf_ball.getHat(Content);
            SaveLoadSystem.Save(playerRecord);
        }
    }
}