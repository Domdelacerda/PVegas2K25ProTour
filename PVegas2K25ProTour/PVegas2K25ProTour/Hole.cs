using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class Hole : GameObject
    {
        private GraphicsDevice _device;
        private SpriteBatch _sprite_batch;

        private Texture2D hole_sprite;
        private Vector2 hole_pos;

        public Hole(Texture2D hole_sprite, Vector2 hole_pos, 
            GraphicsDevice _device, SpriteBatch _sprite_batch) : 
            base(hole_sprite, hole_pos, _device, _sprite_batch)
        {
            this.hole_sprite = hole_sprite;
            this.hole_pos = hole_pos;
            this._device = _device;
        }
    }
}
