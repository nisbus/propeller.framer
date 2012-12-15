CON

  _CLKMODE = XTAL1 + PLL16X
  _XINFREQ = 5_000_000
  
OBJ
  SERIAL : "FullDuplexSerial"

PUB Start
  SERIAL.start(31, 30, 0, 9600)
  repeat
    OUTA [16..23] := INA[0..7]
    
    SERIAL.dec(INA[0])
    SERIAL.dec(INA[1])
    SERIAL.dec(INA[2])
    SERIAL.dec(INA[3])
    SERIAL.dec(INA[4])
    SERIAL.dec(INA[5])
    SERIAL.dec(INA[6])
    SERIAL.dec(INA[7])
    SERIAL.str(string(" ",13))
    'Wait 100 ms
    waitcnt(clkfreq/1000 + cnt)