using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace PVegas2K25ProTour
{
    public class Downslope : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D slope_sprite;
        private Vector2 slope_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float INCLINE = 2.5f;

        public Downslope(Vector2 slope_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(slope_pos,
                _sprite_batch, hitbox, scale)
        {
            this.slope_pos = slope_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            slope_sprite = _content.Load<Texture2D>("Downslope");
        }

        public override void Draw()
        {
            _sprite_batch.Draw(slope_sprite, new Rectangle((int)slope_pos.X,
                (int)slope_pos.Y, (int)(slope_sprite.Width * scale.X),
                (int)(slope_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToRect(ball.center(), this))
            {
                ball.setRolling(true);
                collide(ball);
            }
            else
            {
                ball.setRolling(false);
            }
        }

        /// <summary>
        /// Unique collision behavior for the mushroom obstacle: bounces the
        /// ball away from the mushroom with a different speed value than it
        /// initially had
        /// </summary>
        /// <param name="ball">the ball to slide down the slope.</param>
        public override void collide(Ball ball)
        {
            if (ball.getSpeed().Length() != 0)
            {
                ball.setSpeed(new Vector2(ball.getSpeed().X, 
                    ball.getSpeed().Y + INCLINE));
            }
        }

        public override float radius()
        {
            return slope_sprite.Width / 2 * scale.X;
        }

        public override float width()
        {
            return slope_sprite.Width * scale.X;
        }

        public override float height()
        {
            return slope_sprite.Height * scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = slope_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}