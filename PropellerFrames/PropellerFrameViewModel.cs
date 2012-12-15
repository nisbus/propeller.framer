using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace nisbus.Propeller.Base
{
    /// <summary>
    /// Use this as a base class when creating viewmodels for MVVM patterns.
    /// </summary>
    public abstract class PropellerFrameViewModel : INotifyPropertyChanged
    {
        #region Properties

        private bool storeHistory;
        /// <summary> 
        /// Gets or sets a value indicating whether the viewmodel should keep all incoming messages (of the viewmodels type) in the FrameHistory list.
        /// </summary>
        /// <value><c>true</c> if FrameHistory is updated with each frame; otherwise, <c>false</c>.</value>
        public bool StoreHistory 
        {
            get { return storeHistory; }
            set { storeHistory = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether new incoming frames should be inserted at position 0 in the FrameHistory or added to the back.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if frames should be inserted at index 0; otherwise, <c>false</c>.
        /// </value>
        public bool InsertNewFramesAtBeginning { get; set; }
        /// <summary>
        /// A list of all frames received by the viewmodel (if StoreHistory == true).
        /// </summary>
        /// <value>The frame history.</value>
        public List<IPropellerFrame> FrameHistory { get; set; }

        /// <summary>
        /// The last frame received on the port (of the viewmodels type).
        /// </summary>
        /// <value>The current frame.</value>
        public IPropellerFrame CurrentFrame { get; set; }

        /// <summary>
        /// The port to listen to
        /// </summary>
        public PropellerSerialPort Port;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the serialport receives the current frame type.
        /// </summary>
        public event Action OnUpdated;

        #endregion

        #region Fields

        /// <summary>
        /// The type of frame the inherited class manages
        /// </summary>
        private Type typeToProcess;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PropellerFrameViewModel"/> class.
        /// </summary>
        /// <param name="port">The port to listen to.</param>
        /// <param name="typeToProcess">The type to process.</param>
        public PropellerFrameViewModel(PropellerSerialPort port, Type typeToProcess)
        {
            this.typeToProcess = typeToProcess;
            this.Port = port;
            FrameHistory = new List<IPropellerFrame>();
            Port.OnFrameReceived += new Action<IPropellerFrame>(Port_OnFrameReceived);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processing for each received frame on the serial port.
        /// </summary>
        /// <param name="receivedFrame">The received frame.</param>
        void Port_OnFrameReceived(IPropellerFrame receivedFrame)
        {
            //Enables using IPropellerFrame as the constructor for the viewmodel so that all IPropellerFrames raise the OnUpdated event
            //or Check if the received frame is of the correct type
            if (typeToProcess.IsInterface || receivedFrame.GetType() == typeToProcess)
            {
                if (StoreHistory)
                    FrameHistory.Add(receivedFrame);
                CurrentFrame = receivedFrame;
                if (OnUpdated != null)
                    OnUpdated();                
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void Updated(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
