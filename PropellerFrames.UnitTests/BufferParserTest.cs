using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nisbus.Propeller.Base;
using nisbus.Propeller.Frames;

namespace nisbus.Propeller.UnitTests
{
    /// <summary>
    /// Summary description for BufferParserTest
    /// </summary>
    [TestClass]
    public class BufferParserTest
    {
        List<byte> lastBuffer = new List<byte>();
        List<IPropellerFrame> ProcessedFrames = new List<IPropellerFrame>();

        public BufferParserTest()
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

        [TestCategory("PropellerFramerTests")]
        [TestMethod]
        public void TestParsingPartialFrames_ShouldLeaveTheProcessedFramesAt_2()
        {
            PropellerFramer framer = new PropellerFramer(13, new List<IPropellerFrame> { new H48CFrame()});
            string bufferToTest = "VREF=123X=111Y=12Z=345ThetaA=11ThetaB=33ThetaC=505 \r" + "VREF=123X=111Y=12Z=345";
            UTF8Encoding encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(bufferToTest);
            framer.OnFrameReceived += delegate(IPropellerFrame f)
            {
                ProcessedFrames.Add(f);
            };
            framer.ProcessData(buffer);

            Assert.IsTrue(ProcessedFrames.Count == 1);
            bufferToTest = "ThetaA=11ThetaB=33ThetaC=505 \r";
           
            buffer = encoder.GetBytes(bufferToTest);
            framer.ProcessData(buffer);
            Assert.IsTrue(ProcessedFrames.Count == 2);
        }
    }
}
