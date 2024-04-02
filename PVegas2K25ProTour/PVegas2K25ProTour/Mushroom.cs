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
        private Vector2 scale;

        private const float BOUNCINESS = 1.5f;

        public Mushroom(Vector2 mushroom_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(mushroom_pos, 
                _sprite_batch, hitbox, scale)
        {
            this.mushroom_pos = mushroom_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
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
            _sprite_batch.Draw(mushroom_sprite, new
                Rectangle((int)mushroom_pos.X, (int)mushroom_pos.Y,
                (int)(mushroom_sprite.Width * scale.X),
                (int)(mushroom_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public override void Update(Ball ball)
        {
            if (hitbox.collisionCircleToCircle(ball, this))
            {
                collide(ball);
            }
        }

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
            ball.setSpeed(reflect(ball.getSpeed(), distance_vector) * 
                BOUNCINESS);
        }

        public override float radius()
        {
            return mushroom_sprite.Width / 2 * scale.X;
        }

        public override float width()
        {
            return mushroom_sprite.Width * scale.X;
        }

        public override float height()
        {
            return mushroom_sprite.Height * scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = mushroom_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}
