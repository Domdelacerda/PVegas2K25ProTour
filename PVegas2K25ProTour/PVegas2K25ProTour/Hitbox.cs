//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Checks for collisions with other hitboxes every frame
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using System.Runtime.Intrinsics.Arm;

namespace PVegas2K25ProTour
{
    /// <summary>--------------------------------------------------------------
    /// Hitbox determines if it is overlapping any other hitbox objects at any
    /// given time
    /// </summary>-------------------------------------------------------------
    public class Hitbox
    {
        private GraphicsDeviceManager gdm;
        public Hitbox(GraphicsDeviceManager gdm)
        {
            this.gdm = gdm;
        }
        public bool isBallCollidingWithHole(Ball ball, Hole hole)
        {
            float radius = ball.radius()*hole.radius();
            return (!((Vector2.DistanceSquared(ball.position(), hole.position()) > (radius * radius))));
        }
        public bool isBallOutOfbounds(Vector2 ball1Pos)
        {
            return (ball1Pos.X <= 0 || ball1Pos.X >= gdm.PreferredBackBufferWidth
                || ball1Pos.Y <= 0 || ball1Pos.Y >= gdm.PreferredBackBufferHeight);
        }
    }
}
