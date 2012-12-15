using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace nisbus.Propeller.Base
{
    /// <summary>
    /// The ParallaxSerialPort is for listening to the Propeller on USB (PropPlug)
    /// </summary>
    public class PropellerSerialPort
    {
        #region Events

        /// <summary>
        /// Gets raised every time the class manages to process a whole IParallaxFrame from the received data
        /// </summary>
        public event Action<IPropellerFrame> OnFrameReceived;

        /// <summary>
        /// Gets raised when the Serial port receives an error
        /// </summary>
        public event Action<SerialError> OnError;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the port.
        /// </summary>
        /// <value>The name of the port.</value>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the serial port.
        /// </summary>
        /// <value>The serial port.</value>
        public SerialPort SerialPort { get; set; }

        /// <summary>
        /// Gets a value indicating whether the serialport is open.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the serialport is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsPortOpen
        {
            get { return SerialPort.IsOpen; }
        }

        #endregion

        #region Fields

        private PropellerFramer framer;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PropellerSerialPort"/> class.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="framesToListenTo">The frame types to listen to.</param>
        public PropellerSerialPort(byte separatorByte, string portName, int baudRate, List<IPropellerFrame> framesToListenTo)
        {
            framer = new PropellerFramer(separatorByte, framesToListenTo);

            this.SerialPort = new SerialPort(portName, baudRate);
            this.SerialPort.Encoding = new UTF8Encoding(true);
            
            SerialPort.DataReceived += delegate
            {
                ProcessData();
            };
            SerialPort.ErrorReceived += delegate(object sender, SerialErrorReceivedEventArgs e)
            {
                if (OnError != null)
                    OnError(e.EventType);
            };

            framer.OnFrameReceived += delegate(IPropellerFrame f)
            {
                if (OnFrameReceived != null)
                    OnFrameReceived(f);
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the serial port.
        /// </summary>
        public void Open()
        {
            if (SerialPort.IsOpen)
                SerialPort.Close();
            SerialPort.Open();
        }

        /// <summary>
        /// Closes the serial port.
        /// </summary>
        public void Close()
        {
            SerialPort.Close();
            SerialPort.Dispose();
        }

        /// <summary>
        /// Writes the specified string to the serial port.
        /// </summary>
        /// <param name="stringToWrite">The string to write.</param>
        public void Write(string stringToWrite)
        {
            SerialPort.Write(stringToWrite);
        }

        /// <summary>
        /// Frame the incoming data.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void ProcessData()
        {
            int bytesToRead = SerialPort.BytesToRead;
            byte[] bytes = new byte[bytesToRead];
            SerialPort.Read(bytes, 0, bytesToRead);
            framer.ProcessData(bytes);
        }

        #endregion
    }
}
