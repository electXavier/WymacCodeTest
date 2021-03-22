using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;
    }

    // Use this for initialization
    void Start()
    {
        //Initialize object pooling class
        ObjectPool.SharedInstance.PoolObjects();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    //This function reads user's input
    //just enter for now
    private void GetInput()
    {
        if (Input.GetButtonDown("Submit"))
            OnEnter();
    }

    //When press enter grab robot from the pool
    private void OnEnter()
    {
        GameObject robot = ObjectPool.SharedInstance.GetPooledObject("Robot");
        robot.SetActive(true);
    }
}
