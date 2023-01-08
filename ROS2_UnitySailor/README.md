# UnitySailor

This is an example of how the Unity simulation can be controlled using ROS2.

## Setting up the Container

To build the image from the Dockerfile, execute the following command in the directory of the Dockerfile:

    sudo docker build -t foxy -f Dockerfile .

The next command runs the image and establishes a connection between the local "docker_shared"-folder and the "shared" folder in the ROS workspace in the container.

    sudo docker run --name unity_sailor -it --rm -p 10000:10000 -v "$(pwd)/docker_shared:/home/dev_ws/shared" foxy /bin/bash

If everything worked, you are now presented with a bash prompt inside the container. If you need multiple prompts, you can always attach one with:

    sudo docker exec -ti unity_sailor bash

## Working with ROS inside the Container

The environment is sourced automatically for each new terminal.

The TCP endpoint can be started with:

    ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0

Messages can be published to a topic with the following command:

    ros2 topic pub /set_rudder_angle unity_sailor_msgs/msg/SetUnityRudderAngle "rudder_angle: 45"

To print out received messages from a topic:

    ros2 topic echo boat_posrot

By executing this command inside the `/shared` folder and by using the `--csv` flag, messages can be written to a csv file, which is saved to the shared directory.


## Helpful commands
- `docker system prune -a` clears everything related to docker from the system
- `ros2 interface list -m` lists all available ROS message types
