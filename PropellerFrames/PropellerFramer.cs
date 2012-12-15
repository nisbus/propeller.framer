using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace nisbus.Propeller.Base
{
    public class PropellerFramer
    {
        private byte frameSeparator;

        /// <summary>
        /// Gets raised every time the class manages to process a whole IParallaxFrame from the received data
        /// </summary>
        public event Action<IPropellerFrame> OnFrameReceived;

        /// <summary>
        /// Holds the bytes that got left after the last frame was taken from the incoming buffer.
        /// </summary>
        private List<byte> lastBuffer;

        /// <summary>
        /// Gets or sets the frame types to process.
        /// </summary>
        /// <value>The frames.</value>
        public List<IPropellerFrame> Frames { get; set; }

        public PropellerFramer(byte frameSeparator ,List<IPropellerFrame> framesToListenTo)
        {
            this.frameSeparator = frameSeparator;
            lastBuffer = new List<byte>();
            this.Frames = framesToListenTo;
        }

        /// <summary>
        /// Frame the incoming data and raise the OnFrameReceived when the lastBuffer holds at least a frame type in the frames to listen to list.
        /// </summary>
        public void ProcessData(byte[] bytes)
        {
            lastBuffer.AddRange(bytes.ToList());
            List<byte> workingBuffer = new List<byte>();
            //Get all the bytes that constitute a whole frame
            var str = toStr(bytes);
            while (lastBuffer.IndexOf(frameSeparator) > 0)
            {
                if (lastBuffer.Count > lastBuffer.IndexOf(frameSeparator))
                    workingBuffer = lastBuffer.Take(lastBuffer.IndexOf(frameSeparator) + 1).ToList();
                else if (lastBuffer.Count == lastBuffer.IndexOf(frameSeparator))
                    workingBuffer = lastBuffer;


                if (workingBuffer.Count() > 0)
                {
                    foreach (var frame in Frames)
                    {
                        if (frame.IsFrameType(workingBuffer.ToArray()))
                        {
                            if (OnFrameReceived != null)
                                OnFrameReceived(frame.GetFrame(workingBuffer.ToArray()));
                        }
                    }
                    workingBuffer.Clear();
                }
                //Get the bytes from the last buffer that trail the last frame
                if (lastBuffer.IndexOf(frameSeparator) + 1 == lastBuffer.Count)
                    lastBuffer.Clear();
                else if (lastBuffer.IndexOf(frameSeparator) + 1 < lastBuffer.Count)
                    lastBuffer.RemoveRange(0, lastBuffer.IndexOf(frameSeparator) + 1);
            }
        }

        private string toStr(byte[] buffer)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(true);
            var str = encoding.GetString(buffer);
            return encoding.GetString(buffer);
        }
    }
}
