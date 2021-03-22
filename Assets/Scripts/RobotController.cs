using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    //Variables for commands
    public TextAsset textAsset; // Text asset to get robots command
    private List<string> commandList; //List holds the whole commands
    private string currentCommand;// current command grabbed from text file
    private int commandIndex; // To keep track of current command line
    private int commandsSize;// Size of the total commands

    // Varables to track robot's state
    private enum State
    {
        START,
        ROTATE,
        MOVE,
        WAIT,
        STOP,
        DESTROY,
        INITIALIZE
    };

    private State robotState;

    //Flag variables to check robot's state
    private bool isWaiting;
    private bool isMoving;
    private bool isRotating;
    

    //Variables for command parameter
    private Vector3 startPos; // x, y, z of the spawn position
    private Quaternion currRotation; //current rotation of the robot
    private Vector3 rotation; // x = D degrees, y = DS degrees per sec, z = dD delta degrees
    private float velocity, waitTime; // moveSpeed = V for robot move speed, waitTime = T for robot's waiting time 

    //variables per fps
    int frameCount;
    float dt, fps;
    float updateRate = 4.0f; //4 updates per sec

    // Use OnEnable rather than Start() to make sure initialization happends every new cycle
    void OnEnable()
    {
        //if text asset is empty throw error and disable object
        if(textAsset == null)
        {
            Debug.LogError("Error: Text asset is empty can not load any commands");
            this.gameObject.SetActive(false);
        }
        else
        {
            //Initialize command variables
            //Get command from text and get commands and size of the commands
            commandList = CommandExtractor.LoadCommand(textAsset);
            commandsSize = commandList.Count - 1;
            commandIndex = 0;

            //Initialize robot state to default
            robotState = State.INITIALIZE;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            isRotating = false;
            isMoving = false;
            isWaiting = false;

            //Initialize to calculate fps
            frameCount = 0;
            dt = fps = 0.0f;
        }   
    }

    /* 
     * From update robot follows it's command by it's state set from CheckCommand()
     */
    void Update()
    {
        if(!isWaiting)
            CheckCommand();

        if (robotState != State.DESTROY)
        {
            switch (robotState)
            {
                case State.START:
                    Spawn();
                    break;
                case State.ROTATE:
                    isRotating = true;
                    Rotate();
                    break;
                case State.WAIT:
                    if (isRotating)
                        Rotate();
                    if (isMoving)
                        Move();
                    if(!isWaiting)
                        StartCoroutine(Wait());
                    break;
                case State.MOVE:
                    isMoving = true;
                    Move();
                    break;
                case State.STOP:
                    isMoving = false;
                    isRotating = false;
                    break;
            }
        }
        else
        {
            //DESTROY command disable robot
            this.gameObject.SetActive(false);
        }
            
        //calculate fps
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if(dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        //Debug.Log("Current State: " + robotState + ", Current Command: " + currentCommand);
        //Debug.Log("FPS: " + fps);
    }

    /*This function checks the command from the list extracted
     */
    void CheckCommand()
    {
        
        if (commandList == null || commandIndex == commandsSize)
        {
            robotState = State.DESTROY;
            return;
        } 

        currentCommand = commandList.ElementAt(commandIndex);

        //index to track current enum
        int i = 0;
        bool isStateChanged = false;
        foreach (string c in Enum.GetNames(typeof(State)))
        {
            if(currentCommand.Contains(c))
            {
                robotState = (State)i;
                SetVariable(currentCommand);
                isStateChanged = true;
                break;
            }
            i++;
        }
        if(isStateChanged)
            Debug.LogError("Error: Unknown command ignoring command");

        commandIndex++;
    }

    /*
     * This Function sets varables depending on the command
     */
    void SetVariable(string command)
    {
        string[] line, parameter;
        try
        {
            if (command.Contains("START"))
            {
                line = currentCommand.Split(' ');
                if(line.Length != 2)
                {
                    Debug.LogError("Error: Parameter not found");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }
                parameter = line[1].Split(',');
                if (parameter.Length != 3)
                {
                    Debug.LogError("Error: Parameter should be form of 3 numbers");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }
                startPos.x = float.Parse(parameter[0]);
                startPos.y = float.Parse(parameter[1]);
                startPos.z = float.Parse(parameter[2]);
            }
            else if (command.Contains("ROTATE"))
            {
                line = currentCommand.Split(' ');
                if (line.Length != 2)
                {
                    Debug.LogError("Error: Parameter not found");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }
                parameter = line[1].Split(',');
                if (parameter.Length != 2)
                {
                    Debug.LogError("Error: Parameter should be form of 2 numbers");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }

                rotation.x = float.Parse(parameter[0]);
                rotation.y = float.Parse(parameter[1]);
                rotation.z = 0.0f;

                currRotation = transform.rotation;
            }
            else if (command.Contains("MOVE"))
            {
                line = currentCommand.Split(' ');
                if (line.Length != 2)
                {
                    Debug.LogError("Error: Parameter not found");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }

                velocity = float.Parse(line[1]);
            }
            else if (command.Contains("WAIT"))
            {
                line = currentCommand.Split(' ');
                if (line.Length != 2)
                {
                    Debug.LogError("Error: Parameter not found");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                    return;
                }

                waitTime = float.Parse(line[1]);
            }
            else if(command.Contains("STOP") || command.Contains("DESTROY"))
            {
                line = currentCommand.Split(' ');
                if (line.Length > 1)
                {
                    Debug.LogError("Error: Parameter should not exist");
                    robotState = State.DESTROY;//this can be removed if just want to ignore command
                }                   
            }
        }catch(Exception e)
        {
            Debug.LogException(e);
        }
        
    }

    //Set robot's position refering to command
    void Spawn()
    {
        transform.position = startPos;
    }

    //This function allows robot to rotate around Y axis by command provided
    void Rotate()
    {
        //check if ds is 0 if yes rotate immediatley
        if(rotation.y == 0.0f)
        {
            transform.rotation = currRotation * Quaternion.AngleAxis(rotation.x, Vector3.up);
        }
        else
        {
            //check if parameter D is negative if yes turn left if not turn right
            if (rotation.x < 0)
            {
                rotation.z -= rotation.y / fps;
                rotation.z = Mathf.Max(rotation.z, rotation.x);
            }
            else
            {
                rotation.z += rotation.y / fps;
                rotation.z = Mathf.Min(rotation.z, rotation.x);
            }
            
            transform.rotation = currRotation * Quaternion.AngleAxis(rotation.z, Vector3.up);
        }
    }

    //This function waits for given time and change flag when given time is over
    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    //This function allows robot to move forward with given parameter speed
    void Move()
    {
        //divide speed by fps to make velocity per second
        Vector3 targetPos = transform.position + transform.forward * velocity / fps;

        transform.position = targetPos;
    }


}
