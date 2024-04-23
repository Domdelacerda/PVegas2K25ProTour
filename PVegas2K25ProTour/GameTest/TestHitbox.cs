//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the hitbox class
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PVegas2K25ProTour;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in the hitbox class to ensure that when 
    /// code is refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestHitbox
    {
        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is equal to the normal vector of collision, which
        /// means that the angle between incidence and normal should be the
        /// same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestVectorReflectionStraight()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle_reference = new Obstacle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = Vector2.UnitX;
            Vector2 normal = Vector2.UnitX;
            Vector2 reflection = obstacle_reference.reflectVector(incidence,
                normal);
            float angle_in = obstacle_reference.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI -
                obstacle_reference.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is different from the normal vector of collision, 
        /// which means that the angle between incidence and normal should be 
        /// the same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void TestVectorReflectionSkew()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle_reference = new Obstacle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = -Vector2.One;
            Vector2 normal = Vector2.UnitY;
            Vector2 reflection = obstacle_reference.reflectVector(incidence,
                normal);
            float angle_in = obstacle_reference.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI -
                obstacle_reference.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }
    }
}
