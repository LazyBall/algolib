using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trees;

namespace Tests
{
    [TestClass]
    public class BinarySearchTreeTests
    {
        GeneralTests<BinarySearchTree<int, int>> general = new GeneralTests<BinarySearchTree<int, int>>();
        readonly int n = 1000;


        [TestMethod]
        public void TestContainsKey()
        {
            general.TestContainsKey(n);
        }

        [TestMethod]
        public void TestIndexerByKey()
        {
            general.TestIndexerByKey(n);
        }

        [TestMethod]
        public void TestAdd()
        {
            general.TestAdd(n);
        }

        [TestMethod]
        public void TestCountWhenAdd()
        {
            general.TestCountWhenAdd(n);
        }

        [TestMethod]
        public void TestCountWhenRemove()
        {
            general.TestCountWhenRemove(n, n / 2);
        }

        [TestMethod]
        public void TestRemoveWhenRightIsNull()
        {
            general.TestRemoveWhenRightIsNull();
        }

        [TestMethod]
        public void TestRemoveWhenRightLeftIsNull()
        {
            general.TestRemoveWhenRightLeftIsNull();
        }


        [TestMethod]
        public void TestRemove()
        {
            general.TestRemove();
        }

        [TestMethod]
        public void TestTraversal()
        {
            general.TestTraversal(n);
        }

        [TestMethod]
        public void TestAddNotRandom()
        {
            general.TestAddNotRandom();
        }

        [TestMethod]
        public void TestAddIncreasedEnum()
        {
            general.TestAddIncreasedEnum(n);
        }
    }
}