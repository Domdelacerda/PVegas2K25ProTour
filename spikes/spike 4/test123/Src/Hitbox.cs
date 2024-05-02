using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test123.Src
{
    
    public class Hitbox
    {
        private GraphicsDeviceManager gdm;
        private Texture2D rect;
        private Rectangle rect1 = new Rectangle();
        private Color[] data;
        public Hitbox(GraphicsDeviceManager gdm)
        {
            this.gdm = gdm;
        }
        public void Load(int w, int h) 
        {
            rect = new Texture2D(gdm.GraphicsDevice, w, h);
            data=new Color[w * h];
            int x = 0;
            while(x<data.Length)
            {
                data[x] = Color.White;
                x++;
            }
            rect.SetData(data);
        }
        public void Draw(SpriteBatch sb,Vector2 pos)
        {
            sb.Draw(rect, pos, Color.White);
        }
        public void Unload()
        {
            rect.Dispose();
        }
        public bool Collision()
        {
            //return rect.Intersects(Target.Mouse
            return false;
        }
    }
}
