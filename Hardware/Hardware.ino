#include <Time.h>  
#include <LiquidCrystal.h>
#include <SoftwareSerial.h>

#define NOTE_B0  31
#define NOTE_C1  33
#define NOTE_CS1 35
#define NOTE_D1  37
#define NOTE_DS1 39
#define NOTE_E1  41
#define NOTE_F1  44
#define NOTE_FS1 46
#define NOTE_G1  49
#define NOTE_GS1 52
#define NOTE_A1  55
#define NOTE_AS1 58
#define NOTE_B1  62
#define NOTE_C2  65
#define NOTE_CS2 69
#define NOTE_D2  73
#define NOTE_DS2 78
#define NOTE_E2  82
#define NOTE_F2  87
#define NOTE_FS2 93
#define NOTE_G2  98
#define NOTE_GS2 104
#define NOTE_A2  110
#define NOTE_AS2 117
#define NOTE_B2  123
#define NOTE_C3  131
#define NOTE_CS3 139
#define NOTE_D3  147
#define NOTE_DS3 156
#define NOTE_E3  165
#define NOTE_F3  175
#define NOTE_FS3 185
#define NOTE_G3  196
#define NOTE_GS3 208
#define NOTE_A3  220
#define NOTE_AS3 233
#define NOTE_B3  247
#define NOTE_C4  262
#define NOTE_CS4 277
#define NOTE_D4  294
#define NOTE_DS4 311
#define NOTE_E4  330
#define NOTE_F4  349
#define NOTE_FS4 370
#define NOTE_G4  392
#define NOTE_GS4 415
#define NOTE_A4  440
#define NOTE_AS4 466
#define NOTE_B4  494
#define NOTE_C5  523
#define NOTE_CS5 554
#define NOTE_D5  587
#define NOTE_DS5 622
#define NOTE_E5  659
#define NOTE_F5  698
#define NOTE_FS5 740
#define NOTE_G5  784
#define NOTE_GS5 831
#define NOTE_A5  880
#define NOTE_AS5 932
#define NOTE_B5  988
#define NOTE_C6  1047
#define NOTE_CS6 1109
#define NOTE_D6  1175
#define NOTE_DS6 1245
#define NOTE_E6  1319
#define NOTE_F6  1397
#define NOTE_FS6 1480
#define NOTE_G6  1568
#define NOTE_GS6 1661
#define NOTE_A6  1760
#define NOTE_AS6 1865
#define NOTE_B6  1976
#define NOTE_C7  2093
#define NOTE_CS7 2217
#define NOTE_D7  2349
#define NOTE_DS7 2489
#define NOTE_E7  2637
#define NOTE_F7  2794
#define NOTE_FS7 2960
#define NOTE_G7  3136
#define NOTE_GS7 3322
#define NOTE_A7  3520
#define NOTE_AS7 3729
#define NOTE_B7  3951
#define NOTE_C8  4186
#define NOTE_CS8 4435
#define NOTE_D8  4699
#define NOTE_DS8 4978

int melody[] = {
	NOTE_C4, NOTE_G3 };

// note durations: 4 = quarter note, 8 = eighth note, etc.:
int noteDurations[] = {
	8, 16 };

const int TX_BT = 6;
const int RX_BT = 7;

bool Lights_ON = false;
bool Display_ON = true;

char ch;
int initialPos = 0;
int messagePos = 0;
int cursorPos = 16;
String message;

SoftwareSerial btSerial(TX_BT, RX_BT);
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

void setup()  {
	Serial.begin(9600);
	btSerial.begin(9600);
	lcd.begin(16, 2);
	setTime(12, 04, 30, 04, 03, 2015);

}

void loop(){
	if (timeStatus() == timeSet) {
		refreshDate();
	}
	else {
		lcd.print("Time not set");
	}
	if (btSerial.available()) {
		int commandSize = (int)btSerial.read();
		char command[commandSize];
		int commandPos = 0;
		while (commandPos < commandSize) {
			if (btSerial.available()) {
				command[commandPos] = (char)btSerial.read();
				commandPos++;
			}
		}
		command[commandPos] = 0;
		processCommand(command);
	}
	if (message.length() > 0)
	{
		printMessage();
	}
}

void printMessage()
{
	lcd.setCursor(0, 1);
	lcd.print("                ");
	if (message.length() - messagePos >= 16)
	{
		if (cursorPos == 0)
		{
			lcd.setCursor(0, 1);
			lcd.print(message.substring(messagePos, messagePos + 16));
			messagePos++;
		}
		else
		{
			lcd.setCursor(cursorPos, 1);
			lcd.print(message.substring(0, initialPos));
			cursorPos--;
			initialPos++;
		}
	}
	else
	{
		lcd.setCursor(0, 1);
		lcd.print("                ");
		initialPos = 0;
		messagePos = 0;
		cursorPos = 16;
	}

	for (int thisNote = 0; thisNote < 2; thisNote++) {

		// to calculate the note duration, take one second 
		// divided by the note type.
		//e.g. quarter note = 1000 / 4, eighth note = 1000/8, etc.
		int noteDuration = 1000 / noteDurations[thisNote];
		tone(8, melody[thisNote], noteDuration);

		// to distinguish the notes, set a minimum time between them.
		// the note's duration + 30% seems to work well:
		delay(noteDuration*1.80);
		// stop the tone playing:
		noTone(8);
	}
}

void refreshDate(){

	int h = hourFormat12(); // Get the hours right now and store them in an integer called h
	int m = minute(); // Get the minutes right now and store them in an integer called m
	int s = second(); // Get the seconds right now and store them in an integer called s
	int d = day();
	int mon = month();
	int y = year();
	lcd.setCursor(0, 0); // Set the cursor at the column zero, upper row...
	if (h < 10){   // Add a zero, if necessary, as above
		lcd.print(0);
	}
	lcd.print(h);   // Display the current hour
	lcd.setCursor(2, 0); // Move to the next column
	lcd.print(":");  // And print the colon
	lcd.setCursor(3, 0); // Move to the next column
	if (m < 10){   // Add a zero, if necessary, as above
		lcd.print(0);
	}
	lcd.print(m);   // Display the current minute
	lcd.setCursor(5, 0); // Move to the next column
	lcd.print(":");  // And print the colon
	lcd.setCursor(6, 0); // Move to the next column
	if (s < 10){   // Add a zero, if necessary, as above
		lcd.print(0);
	}
	lcd.print(s);   // Display the current second
	lcd.setCursor(9, 0);
	if (isAM())
	{
		lcd.print("AM");
	}
	else{
		lcd.print("PM");
	}
	//lcd.setCursor(9, 0);
	//lcd.print(d);
	//lcd.setCursor(10, 0);
	//lcd.print("/");
	//lcd.setCursor(11, 0);
	//lcd.print(m);
	//lcd.setCursor(12, 0);
	//lcd.print("/");
	//lcd.setCursor(13, 0);
	//lcd.print(y);	
}

//Process the incoming command from Windows Phone.
void processCommand(char* command) {
	if (strcmp(command, "TURN_ON_LIGHT") == 0) {
		analogWrite(9, 150);
		Lights_ON = true;
		sendMessage("LIGHT:ON");
	}
	else if (strcmp(command, "TURN_OFF_LIGHT") == 0) {
		analogWrite(9, 0);
		Lights_ON = false;
		sendMessage("LIGHT:OFF");
	}
	else if (strcmp(command, "TURN_ON_DISPLAY") == 0){
		lcd.display();
		Display_ON = true;
		sendMessage("DISPLAY:ON");
	}
	else if (strcmp(command, "TURN_OFF_DISPLAY") == 0){
		lcd.noDisplay();
		Display_ON = false;
		sendMessage("DISPLAY:OFF");
	}
	else if (strstr(command, "TIME") != NULL){
		String time = String(command);
		unsigned long parsedTime = time.substring(5).toInt();
		setTime(parsedTime);
	}
	else if (strcmp(command, "SYNC_DATA") == 0){
		String lights = Lights_ON ? "ON" : "OFF";
		String display = Display_ON ? "ON" : "OFF";
		String time = String(now());
		String data = String("LIGHT:") + lights + ";" + String("DISPLAY:") + display + ";" + String("TIME:") + time;
		sendMessage(data);
	}
	else{
		lcd.setCursor(0, 1);
		lcd.print("                ");
		message = String(command);
	}
}

//Send a message back to the Windows Phone.
void sendMessage(String message) {
	int messageLen = message.length();
	if (messageLen < 256) {
		btSerial.write(messageLen);
		btSerial.print(message);
	}
}


void serialEvent()
{
	if (Serial.available())
	{
		ch = Serial.read();
		if (ch == 'N')
		{
			analogWrite(9, 150);
		}
		if (ch == 'F')
		{
			analogWrite(9, 0);
		}

	}
}



