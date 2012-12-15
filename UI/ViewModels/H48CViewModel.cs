using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using nisbus.Propeller.Base;
using nisbus.Propeller.Frames;
using Common.Logging;


namespace nisbus.Propeller.Demo.ViewModels
{
    public class H48CViewModel : PropellerFrameViewModel, INotifyPropertyChanged
    {
        ILog logger = LogManager.GetLogger("H48CViewModel");

        List<H48CFrame> Last10Frames = new List<H48CFrame>();
        int lastframeIndex = -1;
        int averageSmoother = 1000;

        #region Properties

        private DateTime lastUpdated;
        public DateTime LastUpdated 
        {
            get { return lastUpdated; }
            set 
            { 
                lastUpdated = value;
                Update("LastUpdated");
            }
        }
        private double x;
        public double X 
        {
            get { return x; }
            set 
            {
                x = value;
                Update("X");
            }
        }
        private double y;
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                Update("Y");
            }
        }
        private double z;
        public double Z
        {
            get { return z; }
            set
            {
                z = value;
                Update("Z");
            }
        }
        private double thetaA;
        public double ThetaA
        {
            get { return thetaA; }
            set
            {
                thetaA = value;
                Update("ThetaA");
            }
        }
        private double thetaB;
        public double ThetaB
        {
            get { return thetaB; }
            set
            {
                thetaB = value;
                Update("ThetaB");
            }
        }
        private double thetaC;
        public double ThetaC
        {
            get { return thetaC; }
            set
            {
                thetaC = value;
                Update("ThetaC");
            }
        }
        private double vref;
        public double Vref
        {
            get { return vref; }
            set
            {
                vref = value;
                Update("Vref");
            }
        }

        #endregion

        public H48CViewModel(PropellerSerialPort port) : base(port, typeof(H48CFrame)) 
        {
            Last10Frames = new List<H48CFrame>();
            base.OnUpdated += delegate
            {
                Update(base.CurrentFrame as H48CFrame);
            };
        }

        public void Update(H48CFrame newFrame)
        {
            /*
            if (Last10Frames.Count < averageSmoother)
                Last10Frames.Add(newFrame);
            else 
            {
                if (lastframeIndex == averageSmoother-1)
                    lastframeIndex = -1;
                lastframeIndex++;
                Last10Frames[lastframeIndex] = newFrame;
            }


            this.X = System.Math.Round(Last10Frames.Average(x => x.X));
            this.Y = System.Math.Round(Last10Frames.Average(x => x.Y));
            this.Z = System.Math.Round(Last10Frames.Average(x => x.Z));
            this.ThetaA = System.Math.Round(Last10Frames.Average(x => x.ThetaA));
            this.ThetaB = System.Math.Round(Last10Frames.Average(x => x.ThetaB));
            this.ThetaC = System.Math.Round(Last10Frames.Average(x => x.ThetaC));
            this.Vref = System.Math.Round(Last10Frames.Average(x => x.VRef));
             */
            this.X = newFrame.X;
            this.Y = newFrame.Y;
            this.Z = newFrame.Z;
            this.ThetaA = newFrame.ThetaA;
            this.ThetaB = newFrame.ThetaB;
            this.ThetaC = newFrame.ThetaC;
            this.Vref = newFrame.VRef;

            this.LastUpdated = DateTime.UtcNow;
            logger.Debug(newFrame.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Update(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
}
