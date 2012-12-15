using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace nisbus.Propeller.Frames.UnitTests
{
    /// <summary>
    /// Summary description for PropellerFramesUnitTests
    /// </summary>
    [TestClass]
    public class PropellerFramesUnitTests
    {
        public PropellerFramesUnitTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestCategory("H48CFrameTests")]
        [TestMethod]
        public void H48CRegExpMatchTest()
        {
            string match = "VREF=123X=111Y=12Z=345ThetaA=11ThetaB=33ThetaC=505 \r";
            H48CFrame frame = new H48CFrame();
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            Assert.IsTrue(frame.IsFrameType(buffer));
        }

        [TestCategory("H48CFrameTests")]
        [TestMethod]
        public void H48CRegExpTestWithNegativeValuesTest()
        {
            var match = "VREF=2049X=-12Y=-6Z=462ThetaA=205ThetaB=91ThetaC=359 \r";
            H48CFrame frame = new H48CFrame();
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            Assert.IsTrue(frame.IsFrameType(buffer));
        }

        [TestCategory("H48CFrameTests")]
        [TestMethod]
        public void TestCreateH48CFrameFromString()
        {
            string match = "VREF=123X=111Y=12Z=345ThetaA=11ThetaB=33ThetaC=505 \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            H48CFrame frame = new H48CFrame();
            H48CFrame result = frame.GetFrame(buffer) as H48CFrame;
            Assert.AreEqual(123, result.VRef,"VRef does not match");
            Assert.AreEqual(111, result.X, "X does not match");
            Assert.AreEqual(12, result.Y, "Y does not match");
            Assert.AreEqual(345, result.Z, "Z does not match");
            Assert.AreEqual(11, result.ThetaA, "ThetaA does not match");
            Assert.AreEqual(33, result.ThetaB, "ThetaB does not match");
            Assert.AreEqual(505, result.ThetaC, "ThetaC does not match");
        }

        [TestCategory("BinaryArrayFrameTests")]
        [TestMethod]
        public void ParseAStringOfBinariesToABinaryArrayFrame()
        {
            string match = "0100011101110 \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            BinaryArrayFrame frame = new BinaryArrayFrame();
            BinaryArrayFrame result = frame.GetFrame(buffer) as BinaryArrayFrame;
            var expected = new List<bool> { false,true,false,false,false,true,true,true,false,true,true,true,false};
            Assert.AreEqual(expected.Count, result.Bytes.Count);
            bool AllTheSame = true;
            for(int i = 0; i < result.Bytes.Count; i++)
            {
                if (expected[i] != result.Bytes[i])
                    AllTheSame = false;
            }
            Assert.AreEqual(AllTheSame, true);
        }

        [TestCategory("BinaryArrayFrameTests")]
        [TestMethod]
        public void ParseInvalidStringToBinaryArrayFrame_ResultsInException()
        {
            string match = "0100011101110aaaa \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            BinaryArrayFrame frame = new BinaryArrayFrame();
            ArgumentException exp = null;
            try
            {
                BinaryArrayFrame result = frame.GetFrame(buffer) as BinaryArrayFrame;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                    exp = ex as ArgumentException;
            }

            Assert.AreEqual(exp.Message, "buffer does not match frame type");
        }

        [TestCategory("BinaryFrameTests")]
        [TestMethod]
        public void ParseZeroToBinaryFrameTest()
        {
            string match = "0 \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            BinaryFrame frame = new BinaryFrame();
            BinaryFrame result = frame.GetFrame(buffer) as BinaryFrame;
            Assert.AreEqual(false, result.CurrentValue);
        }

        [TestCategory("BinaryFrameTests")]
        [TestMethod]
        public void ParseOneToBinaryFrameTest()
        {
            string match = "1 \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            BinaryFrame frame = new BinaryFrame();
            BinaryFrame result = frame.GetFrame(buffer) as BinaryFrame;
            Assert.AreEqual(true, result.CurrentValue);
        }

        [TestCategory("BinaryFrameTests")]
        [TestMethod]
        public void ParseInvalidStringToBinaryFrameTest()
        {
            string match = "a \r";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(match);
            BinaryFrame frame = new BinaryFrame();
            ArgumentException exp = null;
            try
            {
                BinaryFrame result = frame.GetFrame(buffer) as BinaryFrame;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                    exp = ex as ArgumentException;
            }

            Assert.AreEqual(exp.Message, "buffer does not match frame type");
        }


    }
}
