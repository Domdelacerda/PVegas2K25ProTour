//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the shot class
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PVegas2K25ProTour;

namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all expected behaviors in the shot class to ensure that when code 
    /// is refactored that functionality does not change
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestShot
    {
        /// <summary>----------------------------------------------------------
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has more shot power, 
        /// which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testWindupShot()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's power at that point
            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            float close_shot_power = shot_reference.launchPower();

            // Set the mouse's position to a point farther from the center of
            // the golf ball and record the shot's power at that point
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            float far_shot_power = shot_reference.launchPower();

            Assert.IsTrue(far_shot_power > close_shot_power);
        }

        /// <summary>----------------------------------------------------------
        /// Examines 2 different points away from the ball--one farther and 
        /// one closer--to determine if the farther point has a larger
        /// arrow display, which it always should
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testArrowSize()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();

            // Set the mouse's position to a point close to the center of the
            // golf ball and record the shot's arrow length at that point
            Vector2 point_on_map = golf_ball_reference.center();
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            shot_reference.resizeArrow(golf_ball_reference.center());
            float close_shot_arrow_length = shot_reference.arrowLength();

            // Set the mouse's position to a point farther from the center of
            // the golf ball and record the shot's power at that point
            point_on_map.X += golf_ball_reference.radius();
            point_on_map.Y += golf_ball_reference.radius();
            shot_reference.windupShot(point_on_map,
                golf_ball_reference.center());
            shot_reference.resizeArrow(golf_ball_reference.center());
            float far_shot_arrow_length = shot_reference.arrowLength();

            Assert.IsTrue(far_shot_arrow_length > close_shot_arrow_length);
        }

        /// <summary>----------------------------------------------------------
        /// Determines if the shot arrow's length is zero (not visible) when
        /// the mouse is dragged to the center of the ball, meaning that the
        /// shot power is zero
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testArrowSizeZero()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();
            Vector2 golf_ball_center = golf_ball_reference.center();
            shot_reference.windupShot(golf_ball_center,
                golf_ball_center);
            shot_reference.resizeArrow(golf_ball_reference.center());
            float shot_arrow_length = shot_reference.arrowLength();

            Assert.IsTrue(shot_arrow_length == 0);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if a shot power greater than the maximum is truncated
        /// to the maximum value
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testClampShotPower()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Shot shot_reference = new_game.getShot();
            Vector2 far_point = new Vector2(10000f, 10000f);
            shot_reference.Update(true, far_point, golf_ball_reference);

            Assert.IsTrue(shot_reference.launchPower() ==
                shot_reference.maxShotPower());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the shot is able to change the ball's speed based
        /// on the magnitude of the shot
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testReleaseShot()
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

            shot_reference.releaseShot(golf_ball_reference);

            Assert.IsTrue(golf_ball_reference.getSpeed().X != 0 &&
                golf_ball_reference.getSpeed().Y != 0);
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the shot released check works when the shot is
        /// still being wound up, A.K.A. the shot hasn't been released, which
        /// should return false
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testShotReleasedFalse()
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

            Assert.IsFalse(shot_reference.shotReleased());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the shot released check works when the shot is
        /// wound up then released, which should return true
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testShotReleasedTrue()
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

            shot_reference.releaseShot(golf_ball_reference);

            Assert.IsTrue(shot_reference.shotReleased());
        }

        /// <summary>----------------------------------------------------------
        /// Checks to see if the shot released increments the Stroke counter by 1,
        /// expected value should be 1
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testShotCancelShot()
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

            shot_reference.cancelShot();

            Assert.IsTrue(shot_reference.launchPower() == 0);

        }

        [TestMethod]
        public void testShotStrokeCounter()
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

            int initial_stroke_count = shot_reference.getStrokeCount();
            shot_reference.Update(true, point_on_map, golf_ball_reference);
            shot_reference.Update(false, point_on_map, golf_ball_reference);

            Assert.IsTrue(shot_reference.getStrokeCount() == 
                1 + initial_stroke_count);
        }
    }
}