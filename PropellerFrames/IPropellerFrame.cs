using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nisbus.Propeller.Base
{
    /// <summary>
    /// An interface that all frames need to implement to be used with other propeller base classes.
    /// <seealso cref="nisbus.Propeller.Base.AbstractPropellerFrame"/> for added functionality to embed in inherited frames.
    /// </summary>
    public interface IPropellerFrame
    {
        /// <summary>
        /// Determines whether an array of bytes represents the current frame type.
        /// </summary>
        /// <param name="buffer">The byte array.</param>
        /// <returns>
        /// 	<c>true</c> if the array of bytes matches the method of recognizing frames; otherwise, <c>false</c>.
        /// 	<seealso cref="nisbus.Propeller.Base.AbstractPropellerFrame"/> for a concrete implementation.
        /// </returns>
        bool IsFrameType(byte[] buffer);

        /// <summary>
        /// Should return a new Frame of the type of the inherited frame.
        /// </summary>
        /// <param name="buffer">The byte array to process into a frame.</param>
        /// <returns>A Propeller frame</returns>
        IPropellerFrame GetFrame(byte[] buffer);
    }
}
