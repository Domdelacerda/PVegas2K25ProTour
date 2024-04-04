//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Have a hole that the ball can enter in order to complete the level
// and move on to the next
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private Hitbox hitbox;
        private Vector2 hole_pos;
        private Vector2 scale;
        private bool collision = false;

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
        /// <param name="hitbox">the hitbox the game object uses for
        /// detecting collisions with other game objects.</param>
        /// <param name="_sprite_batch">the sprite batch the hole's sprite
        /// will be drawn in (the same as all other game objects in game 
        /// control).</param>
        /// -------------------------------------------------------------------
        public Hole(Vector2 hole_pos, SpriteBatch _sprite_batch, 
            Hitbox hitbox, Vector2 scale) : base(hole_pos, _sprite_batch, 
                hitbox, scale)
        {
            this.hole_pos = hole_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------

        public void LoadContent(ContentManager _content)
        {
            hole_sprite = _content.Load<Texture2D>("Hole");
        }

        public void Draw()
        {
            _sprite_batch.Draw(hole_sprite, hole_pos, Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>----------------------------------------------------------
        /// Checks for collisions with a ball game object every frame
        /// </summary>
        /// <param name="ball">the ball that can collide with this hole.
        /// </param>
        /// -------------------------------------------------------------------
        public void Update(Vector2 ball_center)
        {
            if (hitbox.collisionPointToCircle(ball_center, this) == true)
            {
                collide();
            }
        }

        /// <summary>----------------------------------------------------------
        /// On collision with a ball, enact unique collision behavior. The
        /// hole's collision behavior sets off a flag letting game control
        /// know that the current level has been completed
        /// </summary>---------------------------------------------------------
        public void collide()
        {
            setCollision(true);
        }

        /// <summary>----------------------------------------------------------
        /// Gets the position of the hole in the game view
        /// </summary>
        /// <returns>the position of the hole.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 position()
        {
            return hole_pos;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the new position of the hole in the game view
        /// </summary>
        /// <param name="new_position">the new position of the hole.</param>
        /// -------------------------------------------------------------------
        public void setPosition(Vector2 new_position)
        {
            hole_pos = new_position;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the radius of the hole from the size of its sprite and the
        /// scale factor
        /// </summary>
        /// <returns>the radius of the hole.</returns>
        /// -------------------------------------------------------------------
        public override float radius()
        {
            return hole_sprite.Width / 2 * scale.X;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the center of the hole from its position and radius
        /// </summary>
        /// <returns>the center of the hole.</returns>
        /// -------------------------------------------------------------------
        public override Vector2 center()
        {
            Vector2 center = hole_pos;
            center.X += radius();
            center.Y += radius();
            return center;
        }

        /// <summary>----------------------------------------------------------
        /// Gets the collision flag for the hole, used for determining if the
        /// player has reached the end of a level or not
        /// </summary>
        /// <returns>whether or not the ball has collided with the hole.
        /// </returns>
        /// -------------------------------------------------------------------
        public bool getCollision()
        {
            return collision;
        }

        /// <summary>----------------------------------------------------------
        /// Sets the collision flag for the hole, activated when the ball
        /// collides with the hole
        /// </summary>
        /// <param name="new_collision">the new collision state of the hole.\
        /// </param>
        /// -------------------------------------------------------------------
        public void setCollision(bool new_collision)
        {
            collision = new_collision;
        }
    }
}
