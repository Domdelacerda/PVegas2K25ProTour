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
        private Hitbox hitbox;
        private Vector2 scale;

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
            scale = Vector2.One;
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
            scale = Vector2.One;
        }

        /// <summary>----------------------------------------------------------
        /// Constructs a new game object with the sprite, starting position, 
        /// and the sprite batch to be drawn on specified
        /// </summary>
        /// <param name="start_pos">the position of the game object at the
        /// start of the game.</param>
        /// <param name="_sprite_batch">the sprite batch the object's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// <param name="hitbox">the hitbox the game object uses for
        /// detecting collisions with other game objects.</param>
        /// <param name="scale">the scale of the game object when it
        /// is drawn.</param>
        /// -------------------------------------------------------------------
        public GameObject(Vector2 start_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale)
        {
            this.start_pos = start_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Gets the position where the object is drawn
        /// </summary>
        /// <returns>the position of the object.</returns>
        /// -------------------------------------------------------------------
        public virtual Vector2 position()
        {
            return start_pos;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the radius of the object from the size of its sprite
        /// </summary>
        /// <returns>the radius of the object.</returns>
        /// -------------------------------------------------------------------
        public virtual float radius()
        {
            float radius = 0f;
            if (object_sprite != null)
            {
                radius = (object_sprite.Width / 2) * scale.X;
            }
            return radius;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the width of the object from the size of its sprite and
        /// it's scale multiplier
        /// </summary>
        /// <returns>the width of the object.</returns>
        /// -------------------------------------------------------------------
        public virtual float width()
        {
            float width = 0f;
            if (object_sprite != null)
            {
                width = object_sprite.Width * scale.X;
            }
            return width;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the height of the object from the size of its sprite and
        /// it's scale multiplier
        /// </summary>
        /// <returns>the height of the object.</returns>
        /// -------------------------------------------------------------------
        public virtual float height()
        {
            float height = 0f;
            if (object_sprite != null)
            {
                height = object_sprite.Height * scale.Y;
            }
            return height;
        }

        /// <summary>----------------------------------------------------------
        /// Obtains the center of a game object using its position and 
        /// dimensions
        /// </summary>
        /// <returns>the center of the object.</returns>
        /// -------------------------------------------------------------------
        public virtual Vector2 center()
        {
            Vector2 center = start_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the distance between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance between the two points.</returns>
        /// -------------------------------------------------------------------
        public float distance(Vector2 point1, Vector2 point2)
        {
            return Vector2.Distance(point1, point2);
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the distance vector between two provided points
        /// </summary>
        /// <param name="point1">the starting point.</param>
        /// <param name="point2">the ending point.</param>
        /// <returns>the distance vector between the two points.</returns>
        /// -------------------------------------------------------------------
        public Vector2 distanceVector(Vector2 point1, Vector2 point2)
        {
            Vector2 distance = point1;
            distance.X -= point2.X;
            distance.Y -= point2.Y;
            return distance;
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the angle of a vector from the positive x axis
        /// </summary>
        /// <param name="vector">the vector the angle is needed for.
        /// </param>
        /// <returns>the angle of the vector.</returns>
        /// -------------------------------------------------------------------
        public float vectorAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>----------------------------------------------------------
        /// Calculates the angle between two provided vectors
        /// </summary>
        /// <param name="vector1">the starting angle.</param>
        /// <param name="vector2">the final angle.</param>
        /// <returns>the sngle between two vectors.</returns>
        /// -------------------------------------------------------------------
        public float angleBetweenVectors(Vector2 vector1,
            Vector2 vector2)
        {
            vector1.Normalize();
            vector2.Normalize();
            return (float)Math.Acos(Vector2.Dot(vector1, vector2));
            /*
            Vector2 inverted_distance = new Vector2(vector2.Y - vector1.Y, 
                vector2.X - vector1.X);
            return (float)Math.Atan2(inverted_distance.X, inverted_distance.Y);
            */
        }
    }
}