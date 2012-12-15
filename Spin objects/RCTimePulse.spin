VAR
  long RCValue

OBJ
  RC : "RCTIME"
  SERIAL : "FullDuplexSerial"
  Num : "Numbers"

PUB Start
  SERIAL.start(31, 30, 0, 9600)

  RCValue := 0
  RC.start(7,1,@RCValue )
  
  repeat
    SERIAL.str(Num.ToStr(RCValue, 13))
    waitcnt(clkfreq/1000 * 20 + cnt)