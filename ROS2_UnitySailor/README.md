# UnitySailor

This is an example of how the simulation can be controlled using ROS2. 


// TODO: anleitung überarbeiten
IN LOCAL CONSOLE:

sudo docker build -t foxy -f Dockerfile .

sudo docker run --name unity_sailor -it --rm -p 10000:10000 foxy /bin/bash
sudo docker run --name unity_sailor -it --rm -p 10000:10000 -v $PWD/docker_shared:/home/dev_ws/shared foxy /bin/bash

sudo docker exec -ti unity_sailor bash

IN DOCKER:

source install/setup.bash

ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0

ros2 topic echo --csv pos_rot

SEGEL STEUERUNG:
ros2 topic pub /twist unity_sailor_msgs/msg/UnitySailTwist "twist: 30"


( ros2 interface list -m )

# HELPERS
docker system prune -a