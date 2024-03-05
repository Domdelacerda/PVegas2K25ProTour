//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have an obstacle that balls can collide with and bounce off of
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Obstacle checks for collisions with a ball and upon collision inverts
    /// that ball's velocity to appear as though it bounces off
    /// </summary>-------------------------------------------------------------
    public class Obstacle : GameObject
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;
        private Texture2D line;
        private Vector2 obstacle_pos;
        private float angleOfLine = 0;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new obstacle using a specified position and sprite 
        /// batch
        /// </summary>
        /// <param name="obstacle_pos">the position of the obstacle at the 
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the obstacle's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Obstacle(Vector2 obstacle_pos, SpriteBatch _sprite_batch) : 
            base(obstacle_pos, _sprite_batch)
        {
            this.obstacle_pos = obstacle_pos;
            this._sprite_batch = _sprite_batch;
        }
    }
}
