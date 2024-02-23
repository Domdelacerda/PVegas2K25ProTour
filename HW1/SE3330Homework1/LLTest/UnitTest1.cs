using SE3330Homework1;

namespace LLTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreate()
        {
            LList newList = new LList(null);
            Assert.IsNotNull(newList);
            Assert.IsNull(newList.GetHead());
        }

        [TestMethod]
        public void TestIsEmptyTrue()
        {
            LList newList1 = new LList(null);
            Assert.IsTrue(newList1.isEmpty());
        }
        [TestMethod]
        public void TestIsEmptyFalse()
        {
            LLNode newNode = new LLNode(1, null);
            LList newList1 = new LList(newNode);
            Assert.IsFalse(newList1.isEmpty());
        }

        // Andrew's Code:
        [TestMethod]

        public void TestDisplay_Null()
        {

            LList nList = new LList(null);

            Assert.IsNull(nList.displayList());

        }

        [TestMethod]

        public void TestDisplay_NotNull()
        {

            LLNode node1 = new LLNode(2, new LLNode(3, new LLNode(5, null)));

            LList nList = new LList(node1);

            int[] nodeArray = nList.displayList();

            int x = 0;

            int[] ans = { 2, 3, 5 };

            while (nodeArray.Length > x)

            {

                Assert.IsNotNull(nodeArray[x]);

                Assert.IsTrue(nodeArray[x] == ans[x]);

                x++;

            }

        }



        [TestMethod]
        public void TestDelete()
        {
            LList myList = new LList(null);

            myList.DeleteList();
            

            Assert.IsNull(myList.GetHead());
        }
    }
}