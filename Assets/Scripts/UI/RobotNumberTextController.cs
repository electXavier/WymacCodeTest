using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotNumberTextController : MonoBehaviour
{
    public Text robotNoText;

    //Initialize text component
    void Start()
    {
        robotNoText = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRobotNumber();
    }

    //Search for robot number using tag and update
    void UpdateRobotNumber()
    {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");

        robotNoText.text = "Robots in the scene: " + robots.Length;
    }
}
