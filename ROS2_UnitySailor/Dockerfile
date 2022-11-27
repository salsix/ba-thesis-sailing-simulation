FROM ros:foxy-ros-base

# Make ROS2 Workspace Dirss
RUN mkdir -p /home/dev_ws/src

# Copy ROS2 packages into workspace
COPY ./ros2_packages/ /home/dev_ws/src

# Copy my custom stuff into workspace
# COPY ./unity_sailor_msgs/ /home/dev_ws/src

# RUN apt update && apt-get -qq -y install curl && apt-get install -y ros-foxy-teleop-twist-keyboard


#Check out ROS-TCP-Endpoint, ROS2 version
RUN git clone https://github.com/Unity-Technologies/ROS-TCP-Endpoint /home/dev_ws/src/ros_tcp_endpoint -b ROS2v0.7.0

# Reference script with commands to source workspace
COPY ./source_ros.sh /home/dev_ws/source_ros.sh

# Change to workspace on sign in
RUN echo "cd home/dev_ws" >> ~/.bashrc

# Build the workspace
RUN cd home/dev_ws && . /opt/ros/foxy/setup.sh && colcon build

# Source the workspace on sign in
RUN echo ". install/local_setup.bash" >> ~/.bashrc
