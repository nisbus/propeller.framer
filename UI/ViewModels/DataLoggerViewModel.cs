using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using nisbus.Propeller.Base;

namespace nisbus.Propeller.Demo.ViewModels
{
    /// <summary>
    /// Manages all incoming frames 
    /// </summary>
    public class DataLoggerViewModel : PropellerFrameViewModel
    {
        #region Fields

        private Dispatcher uiDispatcher;

        #endregion

        #region Properties

        public ObservableCollection<string> ReadData { get; set; }

        #endregion

        #region Constructor

        public DataLoggerViewModel(PropellerSerialPort port) : base(port, typeof(IPropellerFrame))
        {
            ReadData = new System.Collections.ObjectModel.ObservableCollection<string>();
            uiDispatcher = Dispatcher.CurrentDispatcher;
            base.OnUpdated += delegate
            {
                if (base.StoreHistory)
                {
                    uiDispatcher.BeginInvoke(DispatcherPriority.Background, new ThreadStart(() =>
                    {
                        ReadData.Insert(0, string.Format("Received {0} : {1}", base.CurrentFrame.GetType().Name, base.CurrentFrame.ToString()));
                    }));
                }
            };
        }

        #endregion
    }
}
