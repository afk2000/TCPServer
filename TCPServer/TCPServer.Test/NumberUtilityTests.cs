using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPServer.Library;

namespace TCPServer.Test
{
    [TestClass]
    public class NumberUtilityTests
    {
        [TestMethod]
        public void TestNumberFormatIsBad()
        {
            Assert.IsFalse(NumberUtility.IsValidFormat("123"));
        }

        [TestMethod]
        public void TestNumberFormatIsGood()
        {
            Assert.IsTrue(NumberUtility.IsValidFormat("000000123"));
        }

        [TestMethod]
        public void TestNumberFormatIsBadLonger()
        {
            Assert.IsFalse(NumberUtility.IsValidFormat("00000123"));
        }

        [TestMethod]
        public void TestNumberIsTerminateIsTrue()
        {
            Assert.IsTrue(NumberUtility.IsTerminate("terminate"));
        }

        [TestMethod]
        public void TestNumberIsTerminateIsFalse()
        {
            Assert.IsFalse(NumberUtility.IsTerminate("notterminate"));
        }

        // number collection tests
        [TestMethod]
        public void TestNoDupesAdded()
        {
            var numberCollection = new NumberCollectionSingleton();
            numberCollection.Add(1);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(1);
            Assert.IsTrue(numberCollection.GetTotalCount() == 2);
        }

        [TestMethod]
        public void TestNewUniqies()
        {
            var numberCollection = new NumberCollectionSingleton();
            numberCollection.Add(1);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(1);
            Assert.IsTrue(numberCollection.GetLatestNewUniques() == 2);
            Assert.IsTrue(numberCollection.GetLatestNewUniques() == 0);
        }

        [TestMethod]
        public void TestNewDupes()
        {
            var numberCollection = new NumberCollectionSingleton();
            numberCollection.Add(1);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(2);
            numberCollection.Add(1);
            Assert.IsTrue(numberCollection.GetLatestDuplicateCount() == 3);
            Assert.IsTrue(numberCollection.GetLatestDuplicateCount() == 0);
        }

        [TestMethod]
        public void TestCreateFile()
        {
            var fr = new FileRepo();
            fr.Append("hello");
            Assert.IsTrue(fr.LogFileExists());
        }


        [TestMethod]
        public void TestClearFile()
        {
            var fr = new FileRepo();
            fr.Clear();
            Assert.IsTrue(fr.LogFileExists());
        }

        [TestMethod]
        public void TestStringSplit()
        {
            var str = @"1234\n5678";
            var split = NumberUtility.GetStringArray(str);
            Assert.IsTrue(split.Length == 2);
        }

        //[TestMethod]
        //public void TestMoveFile()
        //{
        //    var fr = new FileRepo();
        //    fr.Clear(true);
        //    Assert.IsTrue(fr.LogFileExists());
        //}
    }
}
