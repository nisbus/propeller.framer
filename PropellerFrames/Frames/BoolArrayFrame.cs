using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace Propeller.Frames
{
    public class BoolArrayFrame : AbstractPropellerFrame, IParallaxFrame
    {
        public List<bool> Bytes { get; set; }

        public BoolArrayFrame()
        {
            Bytes = new List<bool>();
        }

        public bool IsFrameType(byte[] buffer)
        {
            return base.IsFrameType(buffer,@"\d*\r");
        }

        public IParallaxFrame GetFrame(byte[] buffer)
        {
            var str = base.DecodeBufferToString(buffer);
            str =  str.Substring(0,str.LastIndexOf("\r"));
            var returnFrame = new BoolArrayFrame();
            foreach (var s in str)
            {
                returnFrame.Bytes.Add(StrToBool(s));
            }
            return returnFrame;
        }
        
        public override string ToString()
        {
            var returnString = string.Empty;
            foreach (var b in Bytes)
                returnString += b.ToString()+",";
            return returnString;
        }

        private bool StrToBool(char s)
        {
            return s == '1';
        }
    }
}
