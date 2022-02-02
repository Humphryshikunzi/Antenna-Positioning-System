import serial
ser = serial.Serial('COM9',9600)
while True:
	read_serial=ser.readline()
	print(read_serial)
