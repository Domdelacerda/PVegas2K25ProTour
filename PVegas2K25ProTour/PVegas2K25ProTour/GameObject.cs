using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class GameObject : Hitbox
    {
        private GraphicsDevice _device;
        private SpriteBatch _sprite_batch;

        private Texture2D object_sprite;
        private Vector2 start_pos;

        // Constructor with no image or starting position (default values
        // for both already exist in class)
        public GameObject(GraphicsDevice _device, SpriteBatch _sprite_batch)
        {
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

        // Constructor with no image supplied (content is supplied instead
        // so that the class loads its own image)
        public GameObject(Vector2 start_pos, GraphicsDevice _device,
            SpriteBatch _sprite_batch)
        {
            this.start_pos = start_pos;
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

        // Constructor with image supplied
        public GameObject(Texture2D object_sprite, Vector2 start_pos, 
            GraphicsDevice _device, SpriteBatch _sprite_batch)
        {
            this.object_sprite = object_sprite;
            this.start_pos = start_pos;
            this._device = _device;
            this._sprite_batch = _sprite_batch;
        }

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
