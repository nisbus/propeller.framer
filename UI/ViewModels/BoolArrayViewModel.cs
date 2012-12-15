using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using nisbus.Propeller.Base;
using nisbus.Propeller.Frames;
using System.Text.RegularExpressions;
using nisbus.Propeller.Demo.Utils;
using System.Windows.Threading;
using System.Threading;

namespace nisbus.Propeller.Demo.ViewModels
{
    public class BoolArrayViewModel : PropellerFrameViewModel
    {
        #region Events

        public Action<string> OnKeyPressed;

        #endregion

        #region Fields
        Dispatcher dispatch;
        private DateTime lastKeyPressTime = DateTime.Now;
        public bool Value1 { get; set; }
        public bool Value2 { get; set; }
        public bool Value3 { get; set; }
        public bool Value4 { get; set; }
        public bool Value5 { get; set; }
        public bool Value6 { get; set; }
        public bool Value7 { get; set; }
        public bool Value8 { get; set; }
        private string text;
        public string TextValue
        {
            get { return text; }
            set
            {
                int ms = 0;
                try
                {
                    ms = DateTime.Now.Subtract(lastKeyPressTime).Milliseconds;
                }
                catch
                { }
                if (ms > 100 && !string.IsNullOrEmpty(value) && (text != value ))
                {
                    lastKeyPressTime = DateTime.Now;
                    text = value;
                    if (OnKeyPressed != null)
                    {
                        dispatch.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() =>
                        {
                            OnKeyPressed(text);
                        }));
                    }
                }
            }
        }
        public List<bool> CurrentState 
        {
            get
            {
                if (base.CurrentFrame != null)
                    return (base.CurrentFrame as BinaryArrayFrame).Bytes;
                else
                {
                    return new List<bool>();
                }
            }
        }

        #endregion

        #region Constructor

        public BoolArrayViewModel(PropellerSerialPort port) : base(port, typeof(BinaryArrayFrame)) 
        {
            this.dispatch = Dispatcher.CurrentDispatcher;
            base.OnUpdated += new Action(BoolArrayViewModel_OnUpdated);
        }

        #endregion

        #region Methods

        void BoolArrayViewModel_OnUpdated()
        {
            try
            {
                Value1 = CurrentState[0];
                Value2 = CurrentState[1];
                Value3 = CurrentState[2];
                Value4 = CurrentState[3];
                Value5 = CurrentState[4];
                Value6 = CurrentState[5];
                Value7 = CurrentState[6];
                Value8 = CurrentState[7];
                TextValue = TranslateToText(CurrentState);
                base.Updated(string.Empty);
            }
            catch (Exception ex)
            {
                TextValue = ex.Message;
            }
        }

        private string TranslateToText(List<bool> CurrentState)
        {
            return KeyTranslator.Translate(CurrentState);
        }

        #endregion
    }
}
