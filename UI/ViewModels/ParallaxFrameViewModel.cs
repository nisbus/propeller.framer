using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Demo.ViewModels
{
    public abstract class ParallaxFrameViewModel
    {
        public bool StoreHistory { get; set; }
        public bool InsertNewFramesAtBeginning { get; set; }
        public List<IParallaxFrame> FrameHistory { get; set; }
        public IParallaxFrame CurrentFrame { get; set; }
        public ParallaxSerialPort Port;
        public event Action OnUpdated;
        private Type typeToProcess;

        public ParallaxFrameViewModel(ParallaxSerialPort port, Type typeToProcess)
        {
            this.typeToProcess = typeToProcess;
            this.Port = port;
            FrameHistory = new List<IParallaxFrame>();
            Port.OnFrameReceived += new Action<IParallaxFrame>(Port_OnFrameReceived);
        }

        void Port_OnFrameReceived(IParallaxFrame receivedFrame)
        {
            if (receivedFrame.GetType() == typeToProcess)
            {
                if (StoreHistory)
                    FrameHistory.Add(receivedFrame);
                CurrentFrame = receivedFrame;
                if (OnUpdated != null)
                    OnUpdated();
            }
        }
    }
}
