using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.Ports;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Demo.ViewModels
{
    public class PortViewModel
    {
        #region Properties

        PropellerSerialPort port;
        private string selectedDevice;
        public string SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                if (!string.IsNullOrEmpty(selectedDevice))
                {
                    port.PortName = selectedDevice;
                    port.Open();
                }
            }
        }

        public ObservableCollection<string> DeviceList { get; set; }

        #endregion

        #region Constructor

        public PortViewModel(PropellerSerialPort port)
        {
            this.port = port;
            DeviceList = new ObservableCollection<string>();
            ListDevices();
        }

        #endregion

        #region Methods

        private void ListDevices()
        {
            DeviceList.Clear();
            //List serialports
            var portNames = SerialPort.GetPortNames();
            foreach (var portName in portNames)
            {
                DeviceList.Add(portName);
            }
        }

        #endregion
    }
}
