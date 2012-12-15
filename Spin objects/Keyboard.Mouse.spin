''********************************************
''*  Mouse and Keyboard generator v1.0       *
''*  Author: nisbus                          *
''*  Copyright (c) 2010, nisbus Inc.         *
''*  See end of file for terms of use.       *
''********************************************

{-----------------REVISION HISTORY-----------------
 v1.0 - 6/9/2010 first beta release.
}

{-----------------------------------------CONNECTIONS--------------------------------------
  The keyboard/mouse applications needs 15 pins to operate.
  10 for the keyboard, 3 for the mouse and 2 for the Serial port.
  Pins 3..12 are the ten keyboard buttons and will return a 10 bit variable that is checked
  for changes every 10ms (SEE DELAY Constant)
  Pins 30 and 31 are used by the Full duplex serial output for sending all data.
  Pins 0, 1 & 2  are used by the accelerometer for CS, DIO and CLK (see constants).
}


CON
  DELAY = 10 'Delay the next reading of the buttons by 10 milliseconds
  _CLKMODE = XTAL1 + PLL16X
  _XINFREQ = 5_000_000

  CS = 0
  DIO = 1
  CLK = 2

VAR
  long  buttonCog, accelerometerCog                     'cog flag/id
                                
  word buttons                  'current state of the buttons
  word lastButtons              'last state of the buttons
  word tmpButtons               'temporary storage for the buttons


  long vref,x,y,z,ThetaA,ThetaB,ThetaC 'Accelerometer variables to send
OBJ
  SERIAL : "FullDuplexSerial"
  H48C   : "H48C Tri-Axis Accelerometer"

  
'Starts a new cog for listening to the input of the buttons 
PUB Start : okay
  SERIAL.start(31, 30, 0, 9600)
  okay := buttonCog := cognew(LISTEN,@buttons)
  accelerometerCog := cognew(ACCELEROMETER, 205) 
  

'Listens to the buttons state and changes the buttons variable when the state changes  
PUB LISTEN 
  repeat
    tmpButtons := INA[3..12]
    if not (tmpButtons == lastButtons)
      buttons := tmpButtons
      lastButtons := tmpButtons
      SERIAL.dec(buttons)
      SERIAL.str(string(" ",13))
    waitcnt(clkfreq/1000 * DELAY + cnt)

PUB ACCELEROMETER
    repeat
     'vref := (H48C.vref*825)/1024   '<-- Here's how to get vref in mV

     vref := H48C.vref               '<-- Here's how to get vref in RAW
          

'Note: The returned value for X, Y, and Z is equal to the axis - Vref
        x := H48C.x   '<-- Here's how to get x 
        y := H48C.y   '<-- Here's how to get y
        z := H48C.z   '<-- Here's how to get z

'Note: The returned value is in Deg (0-359)
'      remove the '*45)/1024' to return the 13-Bit Angle
     ThetaA := (H48C.ThetaA*45)/1024   '<-- ThetaA is the angle relationship between X and Y
     ThetaB := (H48C.ThetaB*45)/1024   '<-- ThetaB is the angle relationship between X and Z
     ThetaC := (H48C.ThetaC*45)/1024   '<-- ThetaC is the angle relationship between Y and Z

     
      SERIAL.str(string("VREF="))
      SERIAL.dec(vref)
      SERIAL.str(string("X="))
      SERIAL.dec(x)
      SERIAL.str(string("Y="))
      SERIAL.dec(y)
      SERIAL.str(string("Z="))
      SERIAL.dec(z)
      SERIAL.str(string("ThetaA="))
      SERIAL.dec(ThetaA)
      SERIAL.str(string("ThetaB="))
      SERIAL.dec(ThetaB)
      SERIAL.str(string("ThetaC="))
      SERIAL.dec(ThetaC)
      SERIAL.str(string(" ",13))
      waitcnt(clkfreq/1000 * DELAY + cnt)