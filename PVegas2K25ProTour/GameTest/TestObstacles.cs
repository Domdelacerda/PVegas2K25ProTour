using PVegas2K25ProTour;
using Microsoft.Xna.Framework;

namespace GameTest
{
    [TestClass]
    public class TestObstacles
    {
        [TestMethod]
        public void testDownSlope()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball ball=new_game.getBall();
            Vector2 ogPos= ball.position();
            Downslope downslpoe = new Downslope(ball.position(),new_game.getSpriteBatch(),new Hitbox(),new Vector2(1,1));
            bool movedDown = ball.position().Y > ogPos.Y && ball.position().X > ogPos.X;
            Assert.IsTrue(movedDown);
        }

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
            Assert.IsTrue(backToOgSpot);
        }

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
            Assert.IsTrue(faster);
        }

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
            Assert.IsTrue(slower);
        }
    }
}
