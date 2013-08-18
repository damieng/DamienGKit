using System;
using System.Collections.Generic;
using System.Linq;
using DamienG.Collections.Generic;
using NUnit.Framework;

namespace DamienG.Tests.Collections.Generic
{
    [TestFixture]
    public class ObservableListTests
    {
        [Test]
        public void AddFiresListChangedEvent()
        {
            const string expectedItem = "a string to add";
            var actualItem = "";
            var actualIndex = -1;

            var observableList = new ObservableList<string>();
            observableList.ListChanged += (s, e) =>
                                              {
                                                  actualItem = e.Item;
                                                  actualIndex = e.Index;
                                              };
            observableList.Add(expectedItem);

            Assert.AreEqual(expectedItem, actualItem);
            Assert.AreEqual(0, actualIndex);
            CollectionAssert.AreEqual(new[] { expectedItem }, observableList.ToArray());
        }

        [Test]
        public void AddWithNoEventListenerAddsItem()
        {
            var observableList = new ObservableList<Decimal> { 1m };
            CollectionAssert.AreEqual(new[] { 1m }, observableList.ToArray());
        }

        [Test]
        public void ClearFiresListClearedEvent()
        {
            var didFire = false;
            var observableList = new ObservableList<string> { "it doesn't matter" };

            observableList.ListCleared += (s, e) => { didFire = true; };
            observableList.Clear();

            Assert.IsTrue(didFire);
        }

        [Test]
        public void ClearWithNoEventListenerClearsList()
        {
            var observableList = new ObservableList<DateTime> { DateTime.Now, new DateTime(2001, 5, 4) };
            observableList.Clear();
            CollectionAssert.IsEmpty(observableList.ToArray());
        }

        [Test]
        public void ConstructorGivenAListReferencesIt()
        {
            var list = new List<long> { 1, 2, 3 };
            var observableList = new ObservableList<long>(list) { 4 };

            Assert.AreEqual(4, observableList.Count);
            Assert.AreEqual(4, list.Count);
        }

        [Test]
        public void ConstructorGivenAnEnumerableCopiesIt()
        {
            var list = new List<int> { 1, 2, 3 };
            var observableList = new ObservableList<int>(list.AsEnumerable()) { 4 };
            list.Remove(1);

            Assert.AreEqual(4, observableList.Count);
            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void ConstructorGivenNothingCreatesAList()
        {
            var observableList = new ObservableList<byte>();
            Assert.AreEqual(0, observableList.Count);
        }

        [Test]
        public void IndexOfGivenContainedValueReturnsCorrectIndex()
        {
            const string containedValue = "Charlie";
            const int expectedIndex = 2;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", containedValue, "Delta", "Echo" }.AsEnumerable());

            Assert.AreEqual(expectedIndex, observableList.IndexOf(containedValue));
        }

        [Test]
        public void IndexOfGivenUncontainedValueReturnsMinusOne()
        {
            const string uncontainedValue = "Charly";

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            Assert.AreEqual(-1, observableList.IndexOf(uncontainedValue));
        }

        [Test]
        public void IndexOfReturnsCorrectIndex()
        {
            var list = new List<short> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
            var observableList = new ObservableList<short>(list);
            foreach (var value in list)
                Assert.AreEqual(list.IndexOf(value), observableList.IndexOf(value));
        }

        [Test]
        public void IndexerSetToDiferentObjectDoesFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Cedilla";

            Assert.IsTrue(didFire);
        }

        [Test]
        public void IndexerSetToSameObjectDoesNotFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Charlie";

            Assert.IsFalse(didFire);
        }

        [Test]
        public void InsertFiresListChangedEvent()
        {
            const int expectedItem = 68030;
            var actualItem = -1;
            var actualIndex = -1;

            var observableList = new ObservableList<int> { 1, 2, 4, 8 };
            observableList.ListChanged += (s, e) =>
                                              {
                                                  actualItem = e.Item;
                                                  actualIndex = e.Index;
                                              };
            observableList.Insert(3, expectedItem);

            Assert.AreEqual(expectedItem, actualItem);
            Assert.AreEqual(3, actualIndex);
        }

        [Test]
        public void RemoveAtFiresListChangedEvent()
        {
            const string expectItem = "another string to remove";
            const int expectIndex = 1;
            var actualItem = "";
            var actualIndex = -1;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.Insert(expectIndex, expectItem);
            observableList.ListChanged += (s, e) =>
                                              {
                                                  actualItem = e.Item;
                                                  actualIndex = e.Index;
                                              };
            observableList.RemoveAt(expectIndex);

            Assert.AreEqual(expectItem, actualItem);
            Assert.AreEqual(expectIndex, actualIndex);
        }

        [Test]
        public void RemoveFiresListChangedEvent()
        {
            const string expectItem = "a string to remove";
            const int expectIndex = 4;
            var actualItem = "";
            var actualIndex = -1;

            var observableList = new ObservableList<string>(new[] { "A", "B", "C", "D", "E" }.AsEnumerable());
            observableList.Insert(expectIndex, expectItem);
            observableList.ListChanged += (s, e) =>
                                              {
                                                  actualItem = e.Item;
                                                  actualIndex = e.Index;
                                              };
            observableList.Remove(expectItem);

            Assert.AreEqual(expectItem, actualItem);
            Assert.AreEqual(expectIndex, actualIndex);
        }
    }
}