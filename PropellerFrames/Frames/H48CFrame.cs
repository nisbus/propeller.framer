using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Frames
{
    public class H48CFrame : AbstractPropellerFrame, IParallaxFrame
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double VRef { get; set; }
        public double ThetaA { get; set; }
        public double ThetaB { get; set; }
        public double ThetaC { get; set; }

        public bool IsFrameType(byte[] buffer)
        {
            return base.IsFrameType(buffer, @"VREF=-*\d*X=-*\d*Y=-*\d*Z=-*\d*ThetaA=-*\d*ThetaB=-*\d*ThetaC=-*\d* \r");
        }

        public IParallaxFrame GetFrame(byte[] buffer)
        {
            if (IsFrameType(buffer))
            {
                var strings = base.DecodeBufferToString(buffer).Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                var v = strings[1].Substring(0, strings[1].IndexOf("X"));
                var d = double.Parse(v, CultureInfo.InvariantCulture);

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

        public override string ToString()
        {
            return string.Format("X {0}, Y {1}, Z {2}, Theta A {3}, Theta B {4}, Theta C {5}. (VREF {6})",this.X,this.Y, this.Z, this.ThetaA, this.ThetaB, this.ThetaC, this.VRef);
        }
    }
}
