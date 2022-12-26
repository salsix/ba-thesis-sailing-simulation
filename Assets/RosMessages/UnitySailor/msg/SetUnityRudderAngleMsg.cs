//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.UnitySailor
{
    [Serializable]
    public class SetUnityRudderAngleMsg : Message
    {
        public const string k_RosMessageName = "unity_sailor_msgs/SetUnityRudderAngle";
        public override string RosMessageName => k_RosMessageName;

        public float rudder_angle;

        public SetUnityRudderAngleMsg()
        {
            this.rudder_angle = 0.0f;
        }

        public SetUnityRudderAngleMsg(float rudder_angle)
        {
            this.rudder_angle = rudder_angle;
        }

        public static SetUnityRudderAngleMsg Deserialize(MessageDeserializer deserializer) => new SetUnityRudderAngleMsg(deserializer);

        private SetUnityRudderAngleMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.rudder_angle);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.rudder_angle);
        }

        public override string ToString()
        {
            return "SetUnityRudderAngleMsg: " +
            "\nrudder_angle: " + rudder_angle.ToString();
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