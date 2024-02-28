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
    internal class Obstacle : GameObject
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D line;
        private float angleOfLine = 0;

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.Black });
            base.Initialize();
        }

        public void drawBorder()
        {
            //Drawing border 
            _spriteBatch.Draw(line, new Rectangle(0, 0, 20, 500), null, Color.Black, 2 * MathHelper.Pi, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(620, 0, 20, 500), null, Color.Black, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(0, 0, 1000, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);
            _spriteBatch.Draw(line, new Rectangle(0, 460, 1000, 20), null, Color.Black, angleOfLine, new Vector2(0, 0), SpriteEffects.None, 0);

        }
    }
}
