using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;


public class SailboatController : MonoBehaviour
{
    public GameObject rudder;
    private Rigidbody rudderRB;
    private Rigidbody boatRB;
    private bool driving;

    public WindZone wind;
    private float[] tblWindAngles;
    private int[] tblWindSpeeds;
    private float[][] lookupTable;

    public GameObject rig;
    private RigController rigController;

    // wind angle and speed
    private float TWA;
    private float TWS;
    private float AWA;
    private float AWS;

    public Text dispTWS;
    public Text dispTWA;
    public Text dispSpeed;

    // Start is called before the first frame update
    void Start()
    {
        boatRB = this.GetComponent<Rigidbody>();
        rudderRB = rudder.GetComponent<Rigidbody>();

        BuildPolarTable();

        rigController = rig.GetComponent<RigController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) driving = !driving;

        // if (Input.GetKey("left")) Turn(true);
        // if (Input.GetKey("right")) Turn(false);
    }

    void FixedUpdate() {
        if (driving)
        {
            TWA = Vector3.Angle(wind.transform.forward, transform.right);

            if (-transform.InverseTransformDirection(boatRB.velocity).x <= GetTargetSpeed(12, TWA)) {
                boatRB.AddForce(-transform.right * 50000f);
            }
        }

        UpdateDisplays();
    }

    // applies a force to the rudder to turn the boat depending on the forward speed
    void Turn(bool turnLeft) {
        float turnSpeed = turnLeft ? -10000f : 10000f;
        rudderRB.AddForce(transform.up * turnSpeed);
    }

    // returns the target speed by wind speed and angle from the polar chart IN UNITS/S
    float GetTargetSpeed(float windSpeed, float windAngle) {
        float targetKn;

        // if wind angle to steep, the boat cannot sail
        if (windAngle < tblWindAngles.First()) return 0;
        // if data for big angle not in table, just assume biggest angle available
        if (windAngle > tblWindAngles.Last()) windAngle = tblWindAngles.Last();
        // if wind speed bigger than tableMax, assume wind speed = tableMax
        if (windSpeed > tblWindSpeeds.Last()) windSpeed = tblWindSpeeds.Last();

        int nearestWindSpeed = tblWindSpeeds.OrderBy(x => Math.Abs(x - windSpeed)).First();
        int windSpeedIndex = Array.BinarySearch(tblWindSpeeds, nearestWindSpeed);
        var windAngleIndex = Array.BinarySearch(tblWindAngles, windAngle);

        // if windangle not in table, take linear interpolation
        if (windAngleIndex < 0) {
            int leftIndex = ~windAngleIndex - 1;
            int rightIndex = ~windAngleIndex;

            float x1 = tblWindAngles[leftIndex];
            float x2 = tblWindAngles[rightIndex];
            float y1 = lookupTable[leftIndex][windSpeedIndex];
            float y2 = lookupTable[rightIndex][windSpeedIndex];

            targetKn = y1 + (windAngle - x1) * ((y2 - y1)/(x2 - x1));
        } else {
            targetKn = lookupTable[windAngleIndex][windSpeedIndex];
        }

        // reduce speed if sail position not optimal
        Debug.Log(GetSailEfficiency());

        // convert knots to m/s: * 0.51
        return targetKn * 0.51f;


    }

    float GetSailEfficiency() {
        float sailAngle = rigController.currentRot;

        float relWindAngle = TWA;
        float targetAngle = 0;

        if (relWindAngle <= 55) {
            targetAngle = 15;
        } else if (relWindAngle <= 150) {
            targetAngle = 15f + (relWindAngle - 55f) * ((90f - 15f)/(150f - 55f));
        } else {
            targetAngle = 90;
        }

        //TODO: optimize this

        return 1 - Math.Abs(targetAngle - sailAngle)/targetAngle;

    }
    
    // reads the polar chart csv and saves it to memory
    void BuildPolarTable() {
        string path = "/home/jonathan/Downloads/polar-table-first235.csv";

        string[] lines = System.IO.File.ReadAllLines(path);

        tblWindSpeeds = lines[0].Split(';').Skip(1).Select(int.Parse).ToArray();

        tblWindAngles = new float[lines.Length - 1];
        lookupTable = new float[lines.Length - 1][]; 

        int idx = 0;
        foreach(string line in lines.Skip(1)) {
            string[] values = line.Split(';');
            tblWindAngles[idx] = Int32.Parse(values[0]);
            lookupTable[idx] = values.Skip(1).Select(float.Parse).ToArray();

            idx++;
        }
    }

    void UpdateDisplays() {
        dispTWA.text = "<size=18>TWA</size>\n<b>" + Vector3.Angle(wind.transform.forward, transform.right).ToString("0.0") + "</b><size=12>deg</size>";
        dispTWS.text = "<size=18>TWS</size>\n<b>" + 12 + "</b><size=12>kn</size>";
        dispSpeed.text = "<size=18>Speed</size>\n<b>" + (-transform.InverseTransformDirection(boatRB.velocity).x).ToString("0.0") + "</b><size=12>kn</size>";
    }
}