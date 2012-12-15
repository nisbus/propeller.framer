using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Frames
{
    public class RawFrame : AbstractPropellerFrame, IPropellerFrame
    {
        private byte[] buffer;

        public bool IsFrameType(byte[] buffer)
        {
            return buffer.Length > 0;
        }

        public IPropellerFrame GetFrame(byte[] buffer)
        {
            return new RawFrame { buffer = buffer };
        }

        public override string ToString()
        {
            return base.DecodeBufferToString(buffer);
        }
    }
}
