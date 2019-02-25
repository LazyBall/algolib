using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trees;

namespace Tests
{
    [TestClass]
    public class BinaryTreeTests
    {
        [TestMethod]
        public void CountIncreaseTest()
        {
            var bin = new BinaryTree<int, int>();
            for(int i=0; i<10; i++)
            {
                bin.Add(i, i);
            }
            Assert.AreEqual(10, bin.Count);
        }
    }
}
