using PVegas2K25ProTour;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace GameTest
{
    [TestClass]
    public class TestCoin
    {
        [TestMethod]
        public void testCoinCollision()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Ball golf_ball_reference = new_game.getBall();
            Vector2 golf_ball_center = golf_ball_reference.center();
            Coin coin = new Coin(golf_ball_center, new_game.getSpriteBatch());
            coin.LoadContent(new_game.Content);
            Assert.IsTrue(coin.Update(golf_ball_reference));
        }

        [TestMethod]
        public void testAwardMoney()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            int past_coins = new_game.getCoins();
            Coin coin = new Coin(Vector2.Zero, new_game.getSpriteBatch());
            new_game.addMoney(coin.moneyAmount());
            int current_coins = new_game.getCoins();
            Assert.IsTrue(past_coins < current_coins);
        }

        [TestMethod]
        public void testNullDraw()
        {
            using var new_game = new GameControl();
            new_game.RunOneFrame();
            Coin coin = new Coin(Vector2.Zero, new_game.getSpriteBatch());
            coin.Draw(null);
            Assert.IsTrue(true);
        }
    }
}
