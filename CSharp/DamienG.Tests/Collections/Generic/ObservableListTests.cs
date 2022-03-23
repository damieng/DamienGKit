using System;
using System.Collections.Generic;
using System.Linq;
using DamienG.Collections.Generic;
using Xunit;

namespace DamienG.Tests.Collections.Generic
{
    public class ObservableListTests
    {
        [Fact]
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

            Assert.Equal(expectedItem, actualItem);
            Assert.Equal(0, actualIndex);
            Assert.Single(observableList.ToArray(), expectedItem);
        }

        [Fact]
        public void AddWithNoEventListenerAddsItem()
        {
            var observableList = new ObservableList<Decimal> { 1m };
            Assert.Equal(new[] { 1m }, observableList.ToArray());
        }

        [Fact]
        public void ClearFiresListClearedEvent()
        {
            var didFire = false;
            var observableList = new ObservableList<string> { "it doesn't matter" };

            observableList.ListCleared += (s, e) => { didFire = true; };
            observableList.Clear();

            Assert.True(didFire);
        }

        [Fact]
        public void ClearWithNoEventListenerClearsList()
        {
            var observableList = new ObservableList<DateTime> { DateTime.Now, new DateTime(2001, 5, 4) };
            observableList.Clear();
            Assert.Empty(observableList.ToArray());
        }

        [Fact]
        public void ConstructorGivenAListReferencesIt()
        {
            var list = new List<long> { 1, 2, 3 };
            var observableList = new ObservableList<long>(list) { 4 };

            Assert.Equal(4, observableList.Count);
            Assert.Equal(4, list.Count);
        }

        [Fact]
        public void ConstructorGivenAnEnumerableCopiesIt()
        {
            var list = new List<int> { 1, 2, 3 };
            var observableList = new ObservableList<int>(list.AsEnumerable()) { 4 };
            list.Remove(1);

            Assert.Equal(4, observableList.Count);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void ConstructorGivenNothingCreatesAList()
        {
            var observableList = new ObservableList<byte>();
            Assert.Empty(observableList);
        }

        [Fact]
        public void IndexOfGivenContainedValueReturnsCorrectIndex()
        {
            const string containedValue = "Charlie";
            const int expectedIndex = 2;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", containedValue, "Delta", "Echo" }.AsEnumerable());

            Assert.Equal(expectedIndex, observableList.IndexOf(containedValue));
        }

        [Fact]
        public void IndexOfGivenUncontainedValueReturnsMinusOne()
        {
            const string uncontainedValue = "Charly";

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            Assert.Equal(-1, observableList.IndexOf(uncontainedValue));
        }

        [Fact]
        public void IndexOfReturnsCorrectIndex()
        {
            var list = new List<short> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
            var observableList = new ObservableList<short>(list);
            foreach (var value in list)
                Assert.Equal(list.IndexOf(value), observableList.IndexOf(value));
        }

        [Fact]
        public void IndexerSetToDiferentObjectDoesFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Cedilla";

            Assert.True(didFire);
        }

        [Fact]
        public void IndexerSetToSameObjectDoesNotFireListChangedEvent()
        {
            var didFire = false;

            var observableList = new ObservableList<string>(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo" }.AsEnumerable());
            observableList.ListChanged += (s, e) => { didFire = true; };
            observableList[2] = "Charlie";

            Assert.False(didFire);
        }

        [Fact]
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

            Assert.Equal(expectedItem, actualItem);
            Assert.Equal(3, actualIndex);
        }

        [Fact]
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

            Assert.Equal(expectItem, actualItem);
            Assert.Equal(expectIndex, actualIndex);
        }

        [Fact]
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

            Assert.Equal(expectItem, actualItem);
            Assert.Equal(expectIndex, actualIndex);
        }
    }
}