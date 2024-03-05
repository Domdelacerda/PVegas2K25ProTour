//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a generic class for all objects in the game that require a
// position, sprite, and a sprite batch to be drawn on
//-----------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// GameObject contains all information that objects in our game need to be
    /// drawn on screen
    /// </summary>-------------------------------------------------------------
    public class GameObject
    {
        private SpriteBatch _sprite_batch;
        private Texture2D object_sprite;
        private Vector2 start_pos;

        //---------------------------------------------------------------------
        // CONSTRUCTORS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Constructs a new game object with only the sprite batch to be drawn
        /// on specified, meaning that the child object already has a default
        /// position and sprite
        /// </summary>
        /// <param name="_sprite_batch">the sprite batch the object's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public GameObject(SpriteBatch _sprite_batch)
        {
            this._sprite_batch = _sprite_batch;
        }

        /// <summary>----------------------------------------------------------
        /// Constructs a new game object with the starting position and the 
        /// sprite batch to be drawn on specified, meaning that the child 
        /// object already has a default sprite
        /// </summary>
        /// <param name="start_pos">the position of the game object at the
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the object's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public GameObject(Vector2 start_pos, SpriteBatch _sprite_batch)
        {
            this.start_pos = start_pos;
            this._sprite_batch = _sprite_batch;
        }

        /// <summary>----------------------------------------------------------
        /// Constructs a new game object with the sprite, starting position, 
        /// and the sprite batch to be drawn on specified
        /// </summary>
        /// <param name="object_sprite">the sprite of the game object to be
        /// drawn on screen.</param>
        /// <param name="start_pos">the position of the game object at the
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the object's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public GameObject(Texture2D object_sprite, Vector2 start_pos, 
            SpriteBatch _sprite_batch)
        {
            this.object_sprite = object_sprite;
            this.start_pos = start_pos;
            this._sprite_batch = _sprite_batch;
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>
        /// Calculates the distance between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance between the two points.</returns>
        public float distance(Vector2 point1, Vector2 point2)
        {
            return Vector2.Distance(point1, point2);
        }

        /// <summary>
        /// Calculates the distance vector between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance vector between the two points.</returns>
        public Vector2 distanceVector(Vector2 point1, Vector2 point2)
        {
            Vector2 distance = point2;
            distance.X -= point1.X;
            distance.Y -= point1.Y;
            return distance;
        }

        /// <summary>
        /// Calculates the angle of a vector
        /// </summary>
        /// <param name="vector">the vector the angle is needed for.
        /// </param>
        /// <returns>the angle of the vector.</returns>
        public float vectorAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }
}
