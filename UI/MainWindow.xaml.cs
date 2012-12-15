using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using WpfCubeControl;
using nisbus.Propeller.Demo.ViewModels;
using nisbus.Propeller.Base;
using nisbus.Propeller.Frames;
using log4net;

namespace PortScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        private System.Drawing.Point point;
        private int X = -1;
        private int Y = -1;
         */
        public PortViewModel vm { get; set; }
        public CubeBuilder builder { get; set; }
        public H48CViewModel H48CModel { get; set; }
        private PropellerSerialPort port;
        public DataLoggerViewModel DataVM { get; set; }
        public BinaryViewModel binaryVM { get; set; }
        public BoolArrayViewModel boolArrayVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                port = new PropellerSerialPort(13, "COM13", 9600, new List<IPropellerFrame> { /*new H48CFrame(), new BinaryFrame(), */new BinaryArrayFrame()/*, new RawFrame()*/ });
                
                DataVM = new DataLoggerViewModel(port);
                //H48CModel = new H48CViewModel(port);
                //binaryVM = new BinaryViewModel(port);
                boolArrayVM = new BoolArrayViewModel(port);
                vm = new PortViewModel(port);

                this.DataContext = vm;


                //BinaryControl.DataContext = binaryVM;
                ListenerControl.DataContext = DataVM;
                KeyboardControl.DataContext = boolArrayVM;
                boolArrayVM.OnKeyPressed += delegate(string key)
                {
                    if (key == "SPACE")
                        TextKeyInput.Text += " ";
                    else if (key == "BACKSPACE")
                        TextKeyInput.Text = TextKeyInput.Text.Remove(TextKeyInput.Text.Length - 2);
                    else if (key == "ENTER")
                        TextKeyInput.Text += '\n';
                    else
                        TextKeyInput.Text += key;
                };

                //H48CControl.DataContext = H48CModel;
                //Cube.DataContext = H48CModel;
                //Cube.Render();
            }
        }
    }
}
