propeller.framer
================

Propeller framing in C#  
  
This is a library to do framing from a propeller chip in C#.  
Examples include a 3D cube using an accelerometer and some basic binary frames for ex. lights.  
Basically you just write a new frame type for the data your emitting from your chip and use that to visualize your data in WPF.  
  
<img src="https://raw.github.com/nisbus/propeller.framer/master/propeller%20UI.png"></img>  
  
A setup I use to test this interface is connect a H48C Accelerometer and load the H48C pulse.spin file onto the propeller.  
Connect the propeller USB to your PC and start the demo project.  
You will see a list of available devices and then pick your propeller from the list.  
Now when you rotate the propeller board the cube in the UI will rotate and the values will be printed out.  
  
#How it works:  
Your spin program must send out a fixed format of data (take a look at H48C pulse.spin (line 49-63)).  
A "frame" from the accelerometer will look something like this:
"VREF=43X=12Y=15Z=0ThetaA=1ThetaB=3ThetaC=7\n"
  
The H48CFrame in nisbus.Propeller.Frames you can see it implements the interface IPropellerFrame and is of the Abstract type AbstractPropellerFrame.  
It defines the properties of the output from the Propeller and has two methods:
  
  IsFrameType calls the abstract implementation which takes in a buffer and a regex.  
  It runs the regex on the data and returns a bool value.  
  
  If IsFrameType is true then GetFrame method is called which parses out the data into the frame object.  
  
The PropellerSerialPort is used to connect to the Propeller and calls the framer for all data received.
When the buffer the port is holding matches any of the frames you give to the constructor it will raise an event
with the frame.  
You start the PropellerSerialPort with the byte you use to separate packages from the propeller, port, baudrate and a list of PropellerFrame types you want to listen to.  
  
The easiest way to see something immediately is to start the mainwindow with only listening to the RawFrame and then check the show incoming data box in the UI.  
This will print out any data that you are sending from your propeller to the textbox in the UI.  

#Implemented frames
The frames in the library now are:

+RawFrame : gets raised for each buffer received from the serialport.
+BinaryFrame : Gets raised for each instance of 1 & 0 received (i.e. a single button)
+BinaryArrayFrame : Gets raised for arrays of binaries received (i.e. state of multiple buttons)
+H48CFrame : Gets raised for each iteration of the Accelerometers readings.

