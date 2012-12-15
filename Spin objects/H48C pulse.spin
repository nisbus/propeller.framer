{{
  H48C output to the PropPlug
}}
CON

  _CLKMODE = XTAL1 + PLL16X
  _XINFREQ = 5_000_000

        CS = 0
       DIO = 1
       CLK = 2
       DELAY = 20

VAR

    long vref,x,y,z,ThetaA,ThetaB,ThetaC

OBJ
  SERIAL : "FullDuplexSerial"
  H48C   : "H48C Tri-Axis Accelerometer"

PUB Start

  H48C.start(CS,DIO,CLK)
  'DIRA[23]~~
  'DIRA[3..7]~
  
  
  SERIAL.start(31, 30, 0, 9600)

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
            
    
   