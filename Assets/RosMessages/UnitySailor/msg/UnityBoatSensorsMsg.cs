//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.UnitySailor
{
    [Serializable]
    public class UnityBoatSensorsMsg : Message
    {
        public const string k_RosMessageName = "unity_sailor_msgs/UnityBoatSensors";
        public override string RosMessageName => k_RosMessageName;

        public float boat_sail_angle;
        public float boat_rudder_angle;
        public float boat_forward_speed;
        public float relative_wind_dir;
        public float absolute_wind_speed;

        public UnityBoatSensorsMsg()
        {
            this.boat_sail_angle = 0.0f;
            this.boat_rudder_angle = 0.0f;
            this.boat_forward_speed = 0.0f;
            this.relative_wind_dir = 0.0f;
            this.absolute_wind_speed = 0.0f;
        }

        public UnityBoatSensorsMsg(float boat_sail_angle, float boat_rudder_angle, float boat_forward_speed, float relative_wind_dir, float absolute_wind_speed)
        {
            this.boat_sail_angle = boat_sail_angle;
            this.boat_rudder_angle = boat_rudder_angle;
            this.boat_forward_speed = boat_forward_speed;
            this.relative_wind_dir = relative_wind_dir;
            this.absolute_wind_speed = absolute_wind_speed;
        }

        public static UnityBoatSensorsMsg Deserialize(MessageDeserializer deserializer) => new UnityBoatSensorsMsg(deserializer);

        private UnityBoatSensorsMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.boat_sail_angle);
            deserializer.Read(out this.boat_rudder_angle);
            deserializer.Read(out this.boat_forward_speed);
            deserializer.Read(out this.relative_wind_dir);
            deserializer.Read(out this.absolute_wind_speed);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.boat_sail_angle);
            serializer.Write(this.boat_rudder_angle);
            serializer.Write(this.boat_forward_speed);
            serializer.Write(this.relative_wind_dir);
            serializer.Write(this.absolute_wind_speed);
        }

        public override string ToString()
        {
            return "UnityBoatSensorsMsg: " +
            "\nboat_sail_angle: " + boat_sail_angle.ToString() +
            "\nboat_rudder_angle: " + boat_rudder_angle.ToString() +
            "\nboat_forward_speed: " + boat_forward_speed.ToString() +
            "\nrelative_wind_dir: " + relative_wind_dir.ToString() +
            "\nabsolute_wind_speed: " + absolute_wind_speed.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
