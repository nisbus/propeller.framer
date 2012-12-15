using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Frames
{
    /// <summary>
    /// Holds a list of bools that can be handy for processing multiple pins ex. INA[1..5].
    /// </summary>
    public class BinaryArrayFrame : AbstractPropellerFrame, IPropellerFrame
    {
        #region Properties

        /// <summary>
        /// Gets or sets the values from the byte array.
        /// </summary>
        /// <value>The bytes.</value>
        public List<bool> Bytes { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryArrayFrame"/> class.
        /// </summary>
        public BinaryArrayFrame()
        {
            Bytes = new List<bool>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether an array of bytes represents a BoolArrayFrame.
        /// </summary>
        /// <param name="buffer">The byte array.</param>
        /// <returns>
        /// 	<c>true</c> if the array of bytes matches the method of recognizing frames; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFrameType(byte[] buffer)
        {
            return base.IsFrameType(buffer, @"^[01]+ \r$");
        }

        /// <summary>
        /// Returns a new BoolArrayFrame from the incoming byte array.
        /// </summary>
        /// <param name="buffer">The byte array to process into a BoolArrayFrame.</param>
        /// <returns>A BoolArrayFrame</returns>
        public IPropellerFrame GetFrame(byte[] buffer)
        {
            if (IsFrameType(buffer))
            {
                var str = base.DecodeBufferToString(buffer);
                str = str.Substring(0, str.LastIndexOf(" \r"));
                var returnFrame = new BinaryArrayFrame();
                foreach (var s in str)
                {
                    returnFrame.Bytes.Add(StrToBool(s));
                }
                return returnFrame;
            }
            else
                throw new ArgumentException("buffer does not match frame type");
        }

        private bool StrToBool(char s)
        {
            return s == '1';
        }

        #endregion

        public override string ToString()
        {
            var returnString = string.Empty;
            foreach (var b in Bytes)
                returnString += b.ToString()+",";
            return returnString;
        }
    }
}
