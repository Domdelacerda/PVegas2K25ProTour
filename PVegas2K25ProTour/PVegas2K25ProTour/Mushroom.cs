using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    public class Mushroom : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D mushroom_sprite;
        private Vector2 mushroom_pos;
        private Hitbox hitbox;

        private const float BOUNCINESS = 1.5f;

        public Mushroom(Vector2 mushroom_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox) : base(mushroom_pos, _sprite_batch, hitbox)
        {
            this.mushroom_pos = mushroom_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            mushroom_sprite = _content.Load<Texture2D>("Mushroom");
        }

        public override void Draw()
        {
            _sprite_batch.Draw(mushroom_sprite, mushroom_pos, Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        /// <summary>
        /// Unique collision behavior for the mushroom obstacle: bounces the
        /// ball away from the mushroom with a different speed value than it
        /// initially had
        /// </summary>
        /// <param name="ball">the ball to be bounced back</param>
        public override void collide(Ball ball)
        {
            Vector2 distance_vector = distanceVector(center(),
                    ball.center());
            distance_vector.Normalize();
            ball.setSpeed(distance_vector * ball.getSpeed().Length() *
                BOUNCINESS);
        }

        public override float radius()
        {
            return mushroom_sprite.Width / 2;
        }
    }
}
