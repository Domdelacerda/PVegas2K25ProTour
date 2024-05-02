//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Manage the levels in our game, including code for generating each
// level and the collision borders for the edge of the screen
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// LevelManager keeps track of the current level being played and contains
    /// code for loading, drawing, and updating each of the levels in the game
    /// </summary>-------------------------------------------------------------
    public class LevelManager
    {
        private Ball golf_ball;
        private Hole hole;
        private Hitbox hitbox;
        private bool removedCoin = false;

        private List<Action<SpriteBatch, ContentManager>> level_list;
        private int level = 0;
        private List<Obstacle> obstacle_list;
        private List<Coin> coin_list;
        private Obstacle[] borders;
        

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        public LevelManager(Ball golf_ball, Hole hole, Hitbox hitbox)
        {
            this.golf_ball = golf_ball;
            this.hole = hole;
            this.hitbox = hitbox;
            obstacle_list = new List<Obstacle>();
            level_list = new List<Action<SpriteBatch, ContentManager>>();
            borders = new Obstacle[4];
            coin_list = new List<Coin>();
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public int Update()
        {
            int money = 0;
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].Update(golf_ball);
                }
            }
            for (int i = 0; i < coin_list.Count; i++)
            {
                if (coin_list[i] != null)
                {
                    if (coin_list[i].Update(golf_ball))
                    {
                        money += coin_list[i].moneyAmount();
                        removeCoin(coin_list[i]);
                    }
                }
            }
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].Update(golf_ball);
            }
            return money;
        }

        public void Draw(SpriteBatch _sprite_batch)
        {
            for (int i = 0; i < obstacle_list.Count; i++)
            {
                if (obstacle_list[i] != null)
                {
                    obstacle_list[i].Draw();
                }
            }
            for (int i = 0; i < coin_list.Count; i++)
            {
                if (coin_list[i] != null)
                {
                    coin_list[i].Draw(_sprite_batch);
                }
            }
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Gets the current level the game is on
        /// </summary>
        /// <returns>the current level of the game.</returns>
        /// -------------------------------------------------------------------
        public int currentLevel()
        {
            return level;
        }

        /// <summary>----------------------------------------------------------
        /// Loads each of the borders at the edge of the screen
        /// </summary>
        /// <param name="window_width">the width of the window.</param>
        /// <param name="window_height">the height of the window.</param>
        /// -------------------------------------------------------------------
        public void loadBorders(int window_width, int window_height)
        {
            Obstacle left_border = new Obstacle(Vector2.Zero, null,
                hitbox, new Vector2(1, window_width));
            borders[0] = left_border;
            Obstacle right_border = new Obstacle(new Vector2
                (window_width, 0), null,
                hitbox, new Vector2(1, window_height));
            borders[1] = right_border;
            Obstacle top_border = new Obstacle(Vector2.Zero, null,
                hitbox, new Vector2(window_width, 1));
            borders[2] = top_border;
            Obstacle bottom_border = new Obstacle(new Vector2
                (0, window_height), null,
                hitbox, new Vector2(window_width, 1));
            borders[3] = bottom_border;
        }

        /// <summary>----------------------------------------------------------
        /// Clears all of the obstacles currently in the obstacle list, meaning
        /// that they will no longer receive updates or be drawn on screen
        /// </summary>---------------------------------------------------------
        public void clearObstacles()
        {
            obstacle_list.Clear();
        }

        /// <summary>----------------------------------------------------------
        /// Loads a preexisting obstacle's content and adds it to the obstacle
        /// list so that it can be updated and drawn each frame
        /// </summary>
        /// <param name="obstacle">the obstacle to be loaded.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void addObstacle(Obstacle obstacle, ContentManager content)
        {
            obstacle.LoadContent(content);
            obstacle_list.Add(obstacle);
        }

        /// <summary>----------------------------------------------------------
        /// Creates a brand new coin, loads its content, and adds it to the
        /// coin list so that it can be updated and drawn each frame
        /// </summary>
        /// <param name="position">the position to instantiate the coin at.
        /// </param>
        /// <param name="_sprite_batch">the sprite batch where the coin will
        /// be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        private void addCoin(Vector2 position, SpriteBatch _sprite_batch,
            ContentManager content)
        {
            Coin new_coin = new Coin(position, _sprite_batch);
            new_coin.LoadContent(content);
            coin_list.Add(new_coin);
        }

        /// <summary>----------------------------------------------------------
        /// Removes a specified coin object from the coin list
        /// </summary>
        /// <param name="coin">the coin to be removed from the list.</param>
        /// -------------------------------------------------------------------
        public void removeCoin(Coin coin)
        {
            coin_list.Remove(coin);
            removedCoin = true;
        }

        /// <summary>----------------------------------------------------------
        /// Checks if a coin has been removed
        /// </summary>---------------------------------------------------------
        public bool removeCoinCheck()
        {
            return removedCoin;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the value of removedCoin
        /// </summary>---------------------------------------------------------
        public void removeCoinCheckChange(bool input)
        {
            removedCoin = input;
        }

        /// <summary>----------------------------------------------------------
        /// Clears all of the coins currently in the coin list, meaning
        /// that they will no longer receive updates or be drawn on screen
        /// </summary>---------------------------------------------------------
        public void clearCoins()
        {
            coin_list.Clear();
        }

        /// <summary>----------------------------------------------------------
        /// Generates a list of all the levels currently playable in the game
        /// </summary>---------------------------------------------------------
        public void generateLevelList()
        {
            level_list.Add(loadLevelZero);
            level_list.Add(loadLevelOne);
            level_list.Add(loadLevelTwo);
            level_list.Add(loadLevelThree);
            level_list.Add(loadLevelFour);
            level_list.Add(loadLevelFive);
        }

        /// <summary>----------------------------------------------------------
        /// Load the next level in the sequence defined by the level list,
        /// inlcuding resetting the ball's stroke count and the hole's
        /// collision
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadNextLevel(SpriteBatch _sprite_batch, 
            ContentManager content)
        {
            level += 1;
            hole.setCollision(false);
            golf_ball.setStrokeCount(0);
            loadCurrentLevel(_sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Sets the level count to a specified value
        /// </summary>
        /// <param name="new_level">the index of the level to be set.</param>
        /// -------------------------------------------------------------------
        public void setLevel(int new_level)
        {
            level = new_level;
        }

        /// <summary>----------------------------------------------------------
        /// Load the current level specified by the level index
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadCurrentLevel(SpriteBatch _sprite_batch, 
            ContentManager content)
        {
            if (level < level_list.Count)
            {
                level_list[level].Invoke(_sprite_batch, content);
            }
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level zero (tutorial) by placing the ball, hole, and all
        /// obstacles and coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelZero(SpriteBatch _sprite_batch, 
            ContentManager content)
        {
            golf_ball.setPosition(new Vector2(600, 200));
            hole.setPosition(new Vector2(100, 200));
            addCoin(new Vector2(350, 200), _sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level one by placing the ball, hole, and all obstacles and
        /// coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelOne(SpriteBatch _sprite_batch, ContentManager content)
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 350));
            hole.setPosition(new Vector2(25, 100));
            SandPit pit1 = new SandPit(new Vector2(100, -175),
                _sprite_batch, hitbox, new Vector2(2f, 2f));
            addObstacle(pit1, content);
            SandPit pit2 = new SandPit(new Vector2(275, 250),
                _sprite_batch, hitbox, new Vector2(2f, 2f));
            addObstacle(pit2, content);
            addCoin(new Vector2(350, 220), _sprite_batch, content);
            addCoin(new Vector2(600, 100), _sprite_batch, content);
            addCoin(new Vector2(150, 350), _sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level two by placing the ball, hole, and all obstacles and
        /// coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelTwo(SpriteBatch _sprite_batch, ContentManager content)
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(200, 200));
            Lake lake1 = new Lake(new Vector2(-50, 0),
                _sprite_batch, hitbox, new Vector2(1f, 2.4f));
            addObstacle(lake1, content);
            Puddle puddle1 = new Puddle(new Vector2(400, -100),
                _sprite_batch, hitbox, new Vector2(1.5f, 1.5f));
            addObstacle(puddle1, content);
            Puddle puddle2 = new Puddle(new Vector2(350, 300),
                _sprite_batch, hitbox, new Vector2(1f, 1f));
            addObstacle(puddle2, content);
            Obstacle wall1 = new Obstacle(new Vector2(300, 185),
                _sprite_batch, hitbox, new Vector2(25, 100));
            addObstacle(wall1, content);
            addCoin(new Vector2(350, 220), _sprite_batch, content);
            addCoin(new Vector2(600, 210), _sprite_batch, content);
            addCoin(new Vector2(290, 10), _sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level three by placing the ball, hole, and all obstacles 
        /// and coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelThree(SpriteBatch _sprite_batch, ContentManager content)
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 200));
            hole.setPosition(new Vector2(100, 200));
            Mushroom mushroom1 = new Mushroom(new Vector2(200, 175),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(mushroom1, content);
            Mushroom mushroom2 = new Mushroom(new Vector2(100, 275),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(mushroom2, content);
            Mushroom mushroom3 = new Mushroom(new Vector2(100, 75),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(mushroom3, content);
            Mushroom mushroom4 = new Mushroom(new Vector2(450, 0),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(mushroom4, content);
            Mushroom mushroom5 = new Mushroom(new Vector2(600, 400),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(mushroom5, content);
            addCoin(new Vector2(240, 10), _sprite_batch, content);
            addCoin(new Vector2(460, 100), _sprite_batch, content);
            addCoin(new Vector2(170, 150), _sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level four by placing the ball, hole, and all obstacles 
        /// and coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelFour(SpriteBatch _sprite_batch, ContentManager content)
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 425));
            hole.setPosition(new Vector2(125, 275));
            Downslope slope1 = new Downslope(new Vector2(0, 200),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(slope1, content);
            Downslope slope2 = new Downslope(new Vector2(600, 200),
                _sprite_batch, hitbox, new Vector2(1, 1));
            addObstacle(slope2, content);
            Lake lake1 = new Lake(new Vector2(0, 0),
                _sprite_batch, hitbox, new Vector2(4, 0.5f));
            addObstacle(lake1, content);
            Lake lake2 = new Lake(new Vector2(200, 200),
                _sprite_batch, hitbox, new Vector2(2, 2));
            addObstacle(lake2, content);
            addCoin(new Vector2(350, 140), _sprite_batch, content);
            addCoin(new Vector2(600, 140), _sprite_batch, content);
            addCoin(new Vector2(130, 230), _sprite_batch, content);
        }

        /// <summary>----------------------------------------------------------
        /// Assemble level five by placing the ball, hole, and all obstacles 
        /// and coins in their place
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch where the objects in 
        /// the level will be drawn on.</param>
        /// <param name="content">the content manager where sprite data is
        /// loaded from.</param>
        /// -------------------------------------------------------------------
        public void loadLevelFive(SpriteBatch _sprite_batch, ContentManager content)
        {
            clearObstacles();
            clearCoins();
            golf_ball.ballStop();
            golf_ball.setPosition(new Vector2(700, 400));
            hole.setPosition(new Vector2(75, 350));
            Downslope slope1 = new Downslope(new Vector2(0, 0),
                _sprite_batch, hitbox, new Vector2(4, 1.5f));
            addObstacle(slope1, content);
            Lake lake1 = new Lake(new Vector2(200, 310),
                _sprite_batch, hitbox, new Vector2(2, 2f));
            addObstacle(lake1, content);
            Obstacle wall1 = new Obstacle(new Vector2(300, 150),
                _sprite_batch, hitbox, new Vector2(100, 25));
            addObstacle(wall1, content);
            Obstacle wall2 = new Obstacle(new Vector2(575, 250),
                _sprite_batch, hitbox, new Vector2(100, 25));
            addObstacle(wall2, content);
            addCoin(new Vector2(350, 190), _sprite_batch, content);
            addCoin(new Vector2(610, 130), _sprite_batch, content);
            addCoin(new Vector2(120, 300), _sprite_batch, content);
            addCoin(new Vector2(320, 90), _sprite_batch, content);
        }
    }
}