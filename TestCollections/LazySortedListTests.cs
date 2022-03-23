using System;
using System.Collections.Generic;
using System.Linq;
using ConvenientCollections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCollections
{
    [TestClass]
    public class LazySortedListTests
    {
        [TestMethod]
        public void ListInit_ContainsAllElements()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(Enumerable.Range(0, amount).Select(n => new KeyValuePair<int, int>(n, n)));

            Assert.AreEqual(amount, list.Count);

            for (int i = 0; i < amount; i++)
            {
                Assert.AreEqual(i, list[i]);
            }
        }

        [TestMethod]
        public void ListAddElements_ElementsExist()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(amount);

            for (int i = 0; i < amount; i++)
            {
                list.Add(i, i);
            }

            Assert.AreEqual(amount, list.Count);

            for (int i = 0; i < amount; i++)
            {
                Assert.AreEqual(i, list[i]);
            }
        }

        [TestMethod]
        public void ListAddElements_AreSortedByKey()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(amount);

            for (int i = 0; i < amount; i++)
            {
                list.Add(amount - i, i);
            }

            Assert.AreEqual(amount, list.Count);

            var keys = list.Keys;
            var sortedKeys = list.Keys.OrderBy(k => k);

            Enumerable.SequenceEqual(keys, sortedKeys);
        }

        [TestMethod]
        public void ListDeleteElement_Deletes()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(amount);

            for (int i = 0; i < amount; i++)
            {
                list.Add(amount - i, i);
            }

            Assert.AreEqual(amount, list.Count);

            Assert.IsTrue(amount > 12);
            list.Remove(amount / 10);
            list.Remove(amount / 12);

            Assert.IsFalse(list.ContainsKey(amount / 10));
            Assert.IsFalse(list.ContainsKey(amount / 12));

            Assert.AreEqual(amount - 2, list.Count);
        }

        [TestMethod]
        public void ListIndexer_SameAsTryGetValue()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(amount);

            for (int i = 0; i < amount; i++)
            {
                list.Add(i, i);
            }

            for (int i = 0; i < amount; i++)
            {
                Assert.IsTrue(list.TryGetValue(i, out int value), $"Value {i} not found.");
                var indexerValue = list[i];
                Assert.AreEqual(value, indexerValue);
            }
        }

        [TestMethod]
        public void ListRandomInsert_AllExist()
        {
            int amount = 10000;
            var list = new LazySortedList<int, int>(amount);

            var seed = new Random().Next(int.MaxValue);
            var random = new Random(seed);
            var randomNums = new List<int>(amount);

            for (int i = 0; i < amount; i++)
            {
                int rand = random.Next(int.MinValue, int.MaxValue);
                list.Add(rand, rand);
                randomNums.Add(rand);
            }


            for (int i = 0; i < amount; i++)
            {
                int rand = randomNums[i];
                Assert.IsTrue(list.ContainsKey(rand), $"List should have contained {rand} (seed {seed})");
            }
        }

    }
}