//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the hitbox class
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using PVegas2K25ProTour;
using System.Runtime.CompilerServices;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in the hitbox class to ensure that when 
    /// code is refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestHitbox
    {
        private const float OBSTACLE_SIZE = 50f;

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle that overlaps another circle detects
        /// collision with it, which it should. In this case, a puddle is
        /// placed on top of another
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToCircleCenter()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle_1 = new Puddle(Vector2.Zero, 
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_1.LoadContent(new_game.Content);
            Puddle puddle_2 = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_2.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionCircleToCircle
                (puddle_1, puddle_2));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle that is on the boundary of another circle
        /// collide with each other, which they should. In this case, two 
        /// puddles that are a full diameter apart are used for the comparison
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToCircleEdge()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle_1 = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_1.LoadContent(new_game.Content);
            Puddle puddle_2 = new Puddle(new Vector2(0, puddle_1.radius() * 2),
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_2.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionCircleToCircle
                (puddle_1, puddle_2));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle that is slightly past the boundary of
        /// another circle are not colliding with each other
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToCircleFalse()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle_1 = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_1.LoadContent(new_game.Content);
            Puddle puddle_2 = new Puddle(new Vector2(0, puddle_1.radius() * 2
                + 1),
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle_2.LoadContent(new_game.Content);

            Assert.IsFalse(hitbox_reference.collisionCircleToCircle
                (puddle_1, puddle_2));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a point at the center of a circle is colliding
        /// with the circle, which it always should be
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionPointToCircleCenter()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionPointToCircle
                (puddle.center(), puddle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a point on the radius of a circle is colliding
        /// with the circle, which it always should be
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionPointToCircleEdge()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 edge = puddle.center();
            edge.X += puddle.radius();

            Assert.IsTrue(hitbox_reference.collisionPointToCircle
                (edge, puddle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a point slightly off the radius of a circle is
        /// colliding with a circle, which it never should be
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionPointToCircleFalse()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 edge = puddle.center();
            edge.X += puddle.radius() + 1;

            Assert.IsFalse(hitbox_reference.collisionPointToCircle
                (edge, puddle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle and a rectangle are colliding if their
        /// centers are in the same position, which should alawys be true
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToRectCenter()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Obstacle obstacle = new Obstacle(puddle.center(), 
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One
                * OBSTACLE_SIZE);
            obstacle.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionCircleToRect
                (puddle, obstacle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle and a rectangle are colliding when the 
        /// circle is on the edge of the rectangle, which should always be true
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToRectEdgeTrue()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 obstacle_offset = puddle.center();
            obstacle_offset.X += puddle.radius();
            Obstacle obstacle = new Obstacle(obstacle_offset,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One
                * OBSTACLE_SIZE);
            obstacle.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionCircleToRect
                (puddle, obstacle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle and a rectangle are colliding when the 
        /// circle is just past the edge of the rectangle, which should always
        /// be false
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToRectEdgeFalse()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 obstacle_offset = puddle.center();
            obstacle_offset.X += puddle.radius() + 1;
            Obstacle obstacle = new Obstacle(obstacle_offset,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One
                * OBSTACLE_SIZE);
            obstacle.LoadContent(new_game.Content);

            Assert.IsFalse(hitbox_reference.collisionCircleToRect
                (puddle, obstacle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle and a rectangle are colliding when the 
        /// circle is on the corner of the rectangle, which should always be
        /// true
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToRectCornerTrue()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 obstacle_offset = puddle.center();
            obstacle_offset.X += puddle.radius() * MathF.Cos(
                (float)Math.PI / 4f);
            obstacle_offset.Y += puddle.radius() * MathF.Sin(
                (float)Math.PI / 4f);
            Obstacle obstacle = new Obstacle(obstacle_offset,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One
                * OBSTACLE_SIZE);
            obstacle.LoadContent(new_game.Content);

            Assert.IsTrue(hitbox_reference.collisionCircleToRect
                (puddle, obstacle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a circle and a rectangle are colliding when the 
        /// circle is just past the corner of the rectangle, which should 
        /// always be false
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testCollisionCircleToRectCornerFalse()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Puddle puddle = new Puddle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            puddle.LoadContent(new_game.Content);
            Vector2 obstacle_offset = puddle.center();
            obstacle_offset.X += puddle.radius() * MathF.Cos(
                (float)Math.PI / 4f) + 1;
            obstacle_offset.Y += puddle.radius() * MathF.Sin(
                (float)Math.PI / 4f) + 1;
            Obstacle obstacle = new Obstacle(obstacle_offset,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One
                * OBSTACLE_SIZE);
            obstacle.LoadContent(new_game.Content);

            Assert.IsFalse(hitbox_reference.collisionCircleToRect
                (puddle, obstacle));
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is equal to the normal vector of collision, which
        /// means that the angle between incidence and normal should be the
        /// same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testVectorReflectionStraight()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle = new Obstacle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = Vector2.UnitX;
            Vector2 normal = Vector2.UnitX;
            Vector2 reflection = obstacle.reflectVector(incidence,
                normal);
            float angle_in = obstacle.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI -
                obstacle.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a vector is reflected correctly when its angle
        /// of incidence is different from the normal vector of collision, 
        /// which means that the angle between incidence and normal should be 
        /// the same as the angle between normal and reflection
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testVectorReflectionSkew()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Hitbox hitbox_reference = new Hitbox();
            Obstacle obstacle = new Obstacle(Vector2.Zero,
                new_game.getSpriteBatch(), hitbox_reference, Vector2.One);
            Vector2 incidence = -Vector2.One;
            Vector2 normal = Vector2.UnitY;
            Vector2 reflection = obstacle.reflectVector(incidence,
                normal);
            float angle_in = obstacle.angleBetweenVectors(incidence,
                normal);
            float angle_ir = (float)Math.PI -
                obstacle.angleBetweenVectors(normal, reflection);
            Assert.IsTrue(angle_in == angle_ir);
        }
    }
}
