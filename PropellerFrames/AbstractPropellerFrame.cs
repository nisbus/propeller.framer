using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nisbus.Propeller.Base
{
    /// <summary>
    /// All PropellerFrames should inherit this frame if you want to use regular expressions to match the incoming pattern to a frame.
    /// </summary>
    public class AbstractPropellerFrame
    {
        /// <summary>
        /// Decodes a byte array to string using UTF8 Encoding.
        /// </summary>
        /// <param name="buffer">the byte array to decode</param>
        /// <returns>The string from the byte array</returns>
        public string DecodeBufferToString(byte[] buffer)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(true);
            var str = encoding.GetString(buffer);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Determines whether a byte array matches a specific regex pattern.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="regexPattern">The regex pattern.</param>
        /// <returns>
        /// 	<c>true</c> if the byte array matches the regex patter, otherwise, <c>false</c>.
        /// </returns>
        public bool IsFrameType(byte[] buffer, string regexPattern)
        {
            Regex reg = new Regex(regexPattern);
            var match = reg.IsMatch(DecodeBufferToString(buffer));
            return match;
        }
    }
}
