using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace Propeller.Frames
{
    public class BinaryFrame : AbstractPropellerFrame, IParallaxFrame
    {
        private byte[] frameBuffer; 
        public bool IsFrameType(byte[] buffer)
        {
            return base.IsFrameType(buffer, @"0 \r|1 \r");
        }
        
        public IParallaxFrame GetFrame(byte[] buffer)
        {
            return new BinaryFrame { frameBuffer = buffer };
        }

        public override string ToString()
        {
            var str = base.DecodeBufferToString(this.frameBuffer);
            return str.Substring(0, str.LastIndexOf("\r"));
        }
    }
}
