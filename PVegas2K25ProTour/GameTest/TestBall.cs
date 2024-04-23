//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the ball class
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PVegas2K25ProTour;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in the ball class to ensure that when code 
    /// is refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestBall
    {
        /// <summary>----------------------------------------------------------
        /// Checks to see if the point at the center of the ball overlaps the 
        /// ball itself, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testIsPointOverBallCenter()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_center = golf_ball_reference.center();
            bool overlap =
                golf_ball_reference.isPointOverBall(golf_ball_center);

            Assert.IsTrue(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the point at the edge of the ball overlaps the 
        /// ball itself, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testIsPointOverBallEdge()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_edge = golf_ball_reference.center();
            golf_ball_edge.Y += (golf_ball_reference.radius());
            bool overlap = golf_ball_reference.isPointOverBall(golf_ball_edge);

            Assert.IsTrue(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a point off of the ball overlaps the ball itself, 
        /// which it never should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testIsPointNotOverBall()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 point_on_map = golf_ball_reference.center();
            // Add the size of the radius to each coordinate so that the point
            // is in the top right corner of the ball's square sprite but is
            // not actually overlapping the circular ball itself
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            bool overlap = golf_ball_reference.isPointOverBall(point_on_map);

            Assert.IsFalse(overlap);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if calling the ballStop method sets the ball's speed
        /// to zero
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testBallStop()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot

            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());

            // Release the Shot to give it some movement
            shot_reference.releaseShot(golf_ball_reference);

            // Stop the ball
            golf_ball_reference.ballStop();

            Assert.IsTrue(golf_ball_reference.getSpeed() == Vector2.Zero);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the ball loses speed with each call of update by
        /// comparing its initial speed to its speed one frame later
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testUpdateBallSpeed()
        {
            Vector2 ball_speed_1;
            Vector2 ball_speed_2;
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot

            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());

            shot_reference.releaseShot(golf_ball_reference);

            ball_speed_1 = golf_ball_reference.getSpeed();
            // update reduces speed by drag reduction scale
            golf_ball_reference.updateSpeed();
            ball_speed_2 = golf_ball_reference.getSpeed();

            Assert.IsTrue(ball_speed_1.Length() > ball_speed_2.Length());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the ball's position is changed eahc frame based on
        /// the value of ball speed
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testUpdateBallPosition()
        {
            Vector2 ball_pos_1;
            Vector2 ball_pos_2;
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // set mouse position and ball center position to different 
            // then call windup shot

            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());

            shot_reference.releaseShot(golf_ball_reference);

            ball_pos_1 = golf_ball_reference.center();
            // update reduces speed by drag reduction scale
            golf_ball_reference.Update(new GameTime(new_game.TargetElapsedTime, 
                new_game.MaxElapsedTime));
            ball_pos_2 = golf_ball_reference.center();

            Assert.AreNotEqual(ball_pos_1, ball_pos_2);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the ball's speed is truncated down  to the max 
        /// speed if it has a speed value greater than the maximum
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testTruncateSpeedUpper()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the ball's speed to double its maximum
            golf_ball_reference.setSpeed(Vector2.UnitX * 
                golf_ball_reference.getMaxSpeed() * 2);
            golf_ball_reference.truncateSpeedUpper();

            Assert.IsTrue(golf_ball_reference.getSpeed().Length() == 
                golf_ball_reference.getMaxSpeed());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the ball's speed is truncated down to zero if it 
        /// has a speed value less than the minimum
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testTruncateSpeedLower()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the ball's speed to half its minimum
            golf_ball_reference.setSpeed(Vector2.UnitX *
                golf_ball_reference.getMinSpeed() * 0.5f);
            golf_ball_reference.truncateSpeedLower();

            Assert.IsTrue(golf_ball_reference.getSpeed().Length() == 0);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the ball is put back in its previous position
        /// after being reset
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testResetBall()
        {
            Vector2 ball_pos_1 = Vector2.Zero;
            Vector2 ball_pos_2 = Vector2.One;
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            golf_ball_reference.setPreviousPosition(ball_pos_1);
            golf_ball_reference.setPosition(ball_pos_2);
            golf_ball_reference.resetPosition();

            Assert.IsTrue(golf_ball_reference.position() == ball_pos_1);
        }
    }
}