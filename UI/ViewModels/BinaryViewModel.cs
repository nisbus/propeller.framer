using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nisbus.Propeller.Base;
using nisbus.Propeller.Frames;
using System.ComponentModel;

namespace nisbus.Propeller.Demo.ViewModels
{
    public class BinaryViewModel : PropellerFrameViewModel, INotifyPropertyChanged
    {
        private bool currentValue;
        public bool CurrentValue
        {
            get { return currentValue; }
            set 
            { 
                currentValue = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentValue"));
            } 
        }
        
        public BinaryViewModel(PropellerSerialPort port)
            : base(port, typeof(BinaryFrame))
        {
            base.OnUpdated += delegate()
            {
                this.CurrentValue = (base.CurrentFrame as BinaryFrame).CurrentValue;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
