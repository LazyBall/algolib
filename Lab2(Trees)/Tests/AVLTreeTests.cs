using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using Trees;

namespace Tests
{
    [TestClass]
    public class AVLTreeTests
    {

        [TestMethod]
        public void TestAdd()
        {
            var avl = new AVLTree<int, int>();
            for(int i=0; i<10; i++)
            {
                avl.Add(i+1, i+1);
            }
        }

        [TestMethod]
        public void TestRemove()
        {
            var avl = new AVLTree<int, int>();
            for (int i = 0; i < 10; i++)
            {
                avl.Add(i + 1, i + 1);
            }

            avl.Remove(4);
            
        }


    }
}
