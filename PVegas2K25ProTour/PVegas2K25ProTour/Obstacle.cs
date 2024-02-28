using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PVegas2K25ProTour
{
    public class Obstacle : GameObject
    {
        private GraphicsDevice _device;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sprite_batch;
        private Texture2D line;
        private Vector2 obstacle_pos;
        private float angleOfLine = 0;

        public Obstacle(Texture2D line, Vector2 obstacle_pos, 
            GraphicsDevice _device, SpriteBatch _sprite_batch) : 
            base(line, obstacle_pos, _device, _sprite_batch)
        {
            this.line = line;
            this.obstacle_pos = obstacle_pos;
            this._device = _device;
        }
        /*
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            line = new Texture2D(GraphicsDevice, 1, 1, false, 
                SurfaceFormat.Color);
            line.SetData(new[] { Color.Black });
            base.Initialize();
        }

        public void drawBorder()
        {
            //Drawing border 
            _sprite_batch.Draw(line, new Rectangle(0, 0, 20, 500), null, 
                Color.Black, 2 * MathHelper.Pi, new Vector2(0, 0), 
                SpriteEffects.None, 0);
            _sprite_batch.Draw(line, new Rectangle(620, 0, 20, 500), null,
                Color.Black, 0, new Vector2(0, 0), 
                SpriteEffects.None, 0);
            _sprite_batch.Draw(line, new Rectangle(0, 0, 1000, 20), null,
                Color.Black, angleOfLine, new Vector2(0, 0), 
                SpriteEffects.None, 0);
            _sprite_batch.Draw(line, new Rectangle(0, 460, 1000, 20), null,
                Color.Black, angleOfLine, new Vector2(0, 0), 
                SpriteEffects.None, 0);
        
        }
        */
    }
}
