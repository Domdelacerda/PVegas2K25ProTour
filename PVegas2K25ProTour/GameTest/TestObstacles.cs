using PVegas2K25ProTour;
using Microsoft.Xna.Framework;
//-----------------------------------------------------------------------------
// Team Name: Compu-Force
// Project: PVegas Tour 2K25 top-down golfing game
// Purpose: Continuously test the functionality of the obsticles
//-----------------------------------------------------------------------------
namespace GameTest
{
    /// <summary>--------------------------------------------------------------
    /// Tests all the different behaviors of obsticles in this test class
    /// </summary>-------------------------------------------------------------
    [TestClass]
    public class TestObstacles
    {
        /// <summary>----------------------------------------------------------
        /// Examines if the ball goes down the slope at all
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testDownSlope()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball ball=new_game.getBall();
            Vector2 ogPos= ball.position();
            Downslope downslpoe = new Downslope(ball.position(),new_game.getSpriteBatch(),new Hitbox(),new Vector2(1,1));
            bool movedDown = ball.position().Y > ogPos.Y && ball.position().X > ogPos.X;
            Assert.IsFalse(movedDown);
        }
        /// <summary>----------------------------------------------------------
        /// Examines if the ball gets put back to its starting spot
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testLakeAndPuddle()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball ball = new_game.getBall();
            Vector2 ogPos = ball.position();
            ball.setSpeed(new Vector2(1, 1));
            Lake lake=new Lake(new Vector2(150,150),new_game.getSpriteBatch(),new Hitbox(),new Vector2(1,1));
            ball.setPosition(lake.position());
            bool backToOgSpot = ball.position().Y == ogPos.Y && ball.position().X == ogPos.X;
            Assert.IsFalse(backToOgSpot);
        }
        /// <summary>----------------------------------------------------------
        /// Examines if the ball bounces of the mushroom
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testMushroom()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball ball = new_game.getBall();
            ball.setSpeed(new Vector2(1, 1));
            Vector2 og_speed=ball.getSpeed();
            Mushroom mushroom=new Mushroom(ball.position(),new_game.getSpriteBatch(),new Hitbox(),new Vector2(1,1));
            bool faster=og_speed.X < ball.getSpeed().X && og_speed.Y < ball.getSpeed().Y;
            Assert.IsFalse(faster);
        }
        /// <summary>----------------------------------------------------------
        /// Examines if the ball slows down due to the sand pit.
        /// </summary>---------------------------------------------------------
        [TestMethod]
        public void testSandPit()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball ball = new_game.getBall();
            ball.setSpeed(new Vector2(1,1));
            Vector2 og_speed = ball.getSpeed();
            SandPit mushroom = new SandPit(ball.position(), new_game.getSpriteBatch(), new Hitbox(), new Vector2(1, 1));
            bool slower = og_speed.X > ball.getSpeed().X && og_speed.Y > ball.getSpeed().Y;
            Assert.IsFalse(slower);
        }
    }
}
