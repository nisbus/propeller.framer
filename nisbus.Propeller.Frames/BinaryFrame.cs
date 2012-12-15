using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Frames
{
    /// <summary>
    /// A frame for holding binary information, ex. a single button status (on/off)
    /// </summary>
    public class BinaryFrame : AbstractPropellerFrame,IPropellerFrame
    {
        #region Fields

        private byte[] frameBuffer;

        #endregion

        public bool CurrentValue
        {
            get { return this.ToString() == "1"; }
        }

        #region Methods

        /// <summary>
        /// Determines whether an array of bytes represents the current frame type.
        /// </summary>
        /// <param name="buffer">The byte array.</param>
        /// <returns>
        /// 	<c>true</c> if the array of bytes matches the method of recognizing frames; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFrameType(byte[] buffer)
        {
            var str = base.DecodeBufferToString(buffer);
            var isFrame =  str == "1 \r" || str == "0 \r";
            return isFrame;
        }

        /// <summary>
        /// Returns a new BinaryFrame.
        /// </summary>
        /// <param name="buffer">The byte array to process into a binary frame.</param>
        /// <returns>A Binary frame</returns>
        public IPropellerFrame GetFrame(byte[] buffer)
        {
            if (IsFrameType(buffer))
            {
                return new BinaryFrame { frameBuffer = buffer };
            }
            else
                throw new ArgumentException("buffer does not match frame type");
        }

        #endregion

        public override string ToString()
        {
            var str = base.DecodeBufferToString(this.frameBuffer);
            return str.Substring(0, 1);
        }
    }
}
