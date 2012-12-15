using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Frames
{
    /// <summary>
    /// A frame to manage the input from a H48C tri-axis accelerometer.
    /// See the H48C Pulse.spin program in the spin folder
    /// </summary>
    public class H48CFrame : AbstractPropellerFrame, IPropellerFrame
    {
       
        #region Properties

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the Z.
        /// </summary>
        /// <value>The Z.</value>
        public double Z { get; set; }
        
        /// <summary>
        /// Gets or sets the V ref.
        /// </summary>
        /// <value>The V ref.</value>
        public double VRef { get; set; }
        
        /// <summary>
        /// Gets or sets the theta A.
        /// </summary>
        /// <value>The theta A.</value>
        public double ThetaA { get; set; }
        
        /// <summary>
        /// Gets or sets the theta B.
        /// </summary>
        /// <value>The theta B.</value>
        public double ThetaB { get; set; }
        
        /// <summary>
        /// Gets or sets the theta C.
        /// </summary>
        /// <value>The theta C.</value>
        public double ThetaC { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether an array of bytes represents an H48CFrame.
        /// </summary>
        /// <param name="buffer">The byte array.</param>
        /// <returns>
        /// 	<c>true</c> if the array of bytes matches a H48CFrame; otherwise, <c>false</c>.
        /// <seealso cref="nisbus.Propeller.Base.AbstractPropellerFrame"/> for a concrete implementation.
        /// </returns>
        public bool IsFrameType(byte[] buffer)
        {
            return base.IsFrameType(buffer, @"VREF=-*\d*X=-*\d*Y=-*\d*Z=-*\d*ThetaA=-*\d*ThetaB=-*\d*ThetaC=-*\d* \r");
        }

        /// <summary>
        /// Returns a new H48CFrame.
        /// </summary>
        /// <param name="buffer">The byte array to process into a H48CFrame.</param>
        /// <returns>A H48CFrame</returns>
        public IPropellerFrame GetFrame(byte[] buffer)
        {
            if (IsFrameType(buffer))
            {
                var strings = base.DecodeBufferToString(buffer).Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                return new H48CFrame
                {
                    VRef = double.Parse(strings[1].Substring(0, strings[1].IndexOf("X")),CultureInfo.InvariantCulture),
                    X = double.Parse(strings[2].Substring(0, strings[2].IndexOf("Y")), CultureInfo.InvariantCulture),
                    Y = double.Parse(strings[3].Substring(0, strings[3].IndexOf("Z")), CultureInfo.InvariantCulture),
                    Z = double.Parse(strings[4].Substring(0, strings[4].IndexOf("ThetaA")), CultureInfo.InvariantCulture),
                    ThetaA = double.Parse(strings[5].Substring(0, strings[5].IndexOf("ThetaB")), CultureInfo.InvariantCulture),
                    ThetaB = double.Parse(strings[6].Substring(0, strings[6].IndexOf("ThetaC")), CultureInfo.InvariantCulture),
                    ThetaC = double.Parse(strings[7], CultureInfo.InvariantCulture)
                };
            }
            else
                throw new ArgumentException("buffer does not match frame type");
        }

        #endregion

        public override string ToString()
        {
            return string.Format("X {0}, Y {1}, Z {2}, Theta A {3}, Theta B {4}, Theta C {5}. (VREF {6})",this.X,this.Y, this.Z, this.ThetaA, this.ThetaB, this.ThetaC, this.VRef);
        }
    }
}
