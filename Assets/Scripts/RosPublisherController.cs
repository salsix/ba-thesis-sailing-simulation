using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnitySailor;
using UnityBoatTrim = RosMessageTypes.UnitySailor.UnityBoatTrimMsg;
using UnityBoatPosRot = RosMessageTypes.UnitySailor.UnityBoatPosRotMsg;

public class RosPublisherController : MonoBehaviour
{
    ROSConnection ros;

    public GameObject rudder;
    public GameObject rig;

    private float rudderRotation;
    private float rigRotation;

    private float publishMessageFrequency = 1f;
    private float timeElapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<UnityBoatTrim>("boat_trim");
        ros.RegisterPublisher<UnityBoatPosRot>("boat_posrot");
        rudderRotation = rudder.GetComponent<RudderController>().currentRot;
        rigRotation = rig.GetComponent<RigController>().currentRot;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            timeElapsed = 0;
            PublishBoatTrim();
            PublishBoatPosition();
        }
    }

    void PublishBoatTrim()
    {
        var msg = new UnityBoatTrim
        {
            rudder_rotation = rudderRotation,
            rig_rotation = rigRotation
        };

        ros.Publish("boat_trim", msg);
    }

    void PublishBoatPosition()
    {
        var msg = new UnityBoatPosRot
        {
            pos_x = transform.position.x,
            pos_y = transform.position.y,
            pos_z = transform.position.z,
            rot_x = transform.rotation.x,
            rot_y = transform.rotation.y,
            rot_z = transform.rotation.z,
            rot_w = transform.rotation.w
        };

        ros.Publish("boat_posrot", msg);
    }
}
