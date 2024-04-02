﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PVegas2K25ProTour
{
    public class SandPit : Obstacle
    {
        private SpriteBatch _sprite_batch;
        private Texture2D sand_pit_sprite;
        private Vector2 sand_pit_pos;
        private Hitbox hitbox;
        private Vector2 scale;

        private const float SAND_FRICTION = 0.955f;

        public SandPit(Vector2 sand_pit_pos, SpriteBatch _sprite_batch,
            Hitbox hitbox, Vector2 scale) : base(sand_pit_pos, 
                _sprite_batch, hitbox, scale)
        {
            this.sand_pit_pos = sand_pit_pos;
            this._sprite_batch = _sprite_batch;
            this.hitbox = hitbox;
            this.scale = scale;
        }

        //---------------------------------------------------------------------
        // GENERATED METHODS
        //---------------------------------------------------------------------
        public override void LoadContent(ContentManager _content)
        {
            sand_pit_sprite = _content.Load<Texture2D>("Sand");
        }

        public override void Draw()
        {
            _sprite_batch.Draw(sand_pit_sprite, new 
                Rectangle((int)sand_pit_pos.X, (int)sand_pit_pos.Y, 
                (int)(sand_pit_sprite.Width * scale.X),
                (int)(sand_pit_sprite.Height * scale.Y)), Color.White);
        }

        //---------------------------------------------------------------------
        // PROGRAMMER-WRITTEN METHODS
        //---------------------------------------------------------------------

        public override void Update(Ball ball)
        {
            if (hitbox.collisionPointToCircle(ball.center(), this))
            {
                collide(ball);
            }
        }

        /// <summary>
        /// Unique collision behavior for the sand pit obstacle: slows down the
        /// ball faster while on sand
        /// </summary>
        /// <param name="ball">the ball to be slowed down</param>
        public override void collide(Ball ball)
        {
            ball.setSpeed(ball.getSpeed() * SAND_FRICTION);
        }

        public override float radius()
        {
            return sand_pit_sprite.Width / 2 * scale.X;
        }

        public override float width()
        {
            return sand_pit_sprite.Width * scale.X;
        }

        public override float height()
        {
            return sand_pit_sprite.Height * scale.Y;
        }

        public override Vector2 center()
        {
            Vector2 center = sand_pit_pos;
            center.X += width() / 2;
            center.Y += height() / 2;
            return center;
        }
    }
}
