Suppose you have a robot that can receive commands from a text file in order to command it. These commands will tell the robot to go forwards, backwards and rotate. 

The goal of the project is to spawn a robot, programmatically load and process the RobotCommands.txt file to move the robot based on the commands within it.

The avaiable commands are:
---------------------------------------
START X,Y,Z
	Parameter count 3.
	Sets the Robot immediately to world co-ordinates X,Y,Z
	
ROTATE D,DS
	Parameter count 2.
	Rotate the robot around the Y axis D degrees at DS degrees per second.
	The rotation is relative from the robot's current rotation.
	Zero DS means to rotate immediatley.

MOVE V
	Parameter count 1.
	Set the robot moving in the robot's current forward direction at V units per second.
	
WAIT T
	Parameter count 1.
	Pauses the execution of the command processor for T seconds
	If the robot is moving or rotating it will continue to move/rotate, whilst the command processor is 'waiting'.
	
STOP
	Parameter count none.
	Stop Moving and or Rotating the robot.
	
DESTROY
	Parameter count none.
	Destroy and remove the robot from the scene.

Open the test project in Unity. The test project is built with Unity 2019.2.3f1.
In the Assets folder there is a file called "RobotCommands.txt" and robot prefab called "Robot". 
	
---------------------------------------
You must: 
---------------------------------------
-Implement a script that spawns a 'Robot' each time the player presses Enter. 

-Load the robot with the commands from the RobotCommands.txt file.

-Write a command parser to parse and execute the list of commands in order to move the robot about.
	Notes:
	* There will only be one Command per line.
	* Ignore lines starting with "//".
	* Ignore blank lines
	* Commands may have none, one, two or three parameters.
	* Commands are separated from their parameter list by whitespace.
	* Multiple parameters are separated by a comma ",".
	* Robot can still be moving and or rotating whilst 'waiting' to execute the next command.
	* Stop execution once all lines have been read and executed.
	* Only six commands to implement ;)
	
-Programmatically move and rotate the robot based on commands. No need for Rigidbodies/Physics etc. Setting transform rotations and positions directly will be ok.

-Create a Ui That displays the total the number of Robots that have been spawned.


---------------------------------------

What we are looking for:
* Code that is well thought out and tested.
* Clean, well commented and readable code.
* Error handling for unknown commands, bad parameters etc.
* Extensibility should be considered.

Please include a readme with any additional information you would like to include. You may wish to use it to explain any design decisions.



