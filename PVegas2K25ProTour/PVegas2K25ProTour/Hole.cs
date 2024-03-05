//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a hole that the ball can enter in order to complete the level
// and move on to the next
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hole is a static object on the map that checks for collision with a
    /// ball to determine if the level is complete or not
    /// </summary>-------------------------------------------------------------
    public class Hole : GameObject
    {
        private SpriteBatch _sprite_batch;
        private Texture2D hole_sprite;
        private Vector2 hole_pos;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new hole using a specified position and sprite batch
        /// </summary>
        /// <param name="hole_pos">the position of the hole at the start of the
        /// game.</param>
        /// <param name="_sprite_batch">the sprite batch the hole's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Hole(Vector2 hole_pos, SpriteBatch _sprite_batch) : 
            base(hole_pos, _sprite_batch)
        {
            this.hole_pos = hole_pos;
            this._sprite_batch = _sprite_batch;
        }
    }
}
