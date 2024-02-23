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



        // [TestMethod]
        // public void TestDelete(LList myList)
        // {
                // This code will pass the test if the reference to the pointer is null and assume that the
                // garbage collector will remove any dangling pointers that weren't successfully deallocated
                // before the reference was set to null. 

                // Assert.IsNull(myList);
        // }
    }
}