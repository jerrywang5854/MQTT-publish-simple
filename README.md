## Overview

This repository hosts a C# application designed to simulate multiple MQTT clients. Each client connects to an MQTT broker, sends a sequence of messages, and then disconnects, demonstrating the use of MQTT protocol in a high-load environment.

## Features

- **Multiple Simultaneous Connections**: Simulates up to 100 MQTT clients connecting concurrently to an MQTT broker.
- **Message Sending**: Each client sends a predefined number of messages (default is 5) with a configurable delay between messages.
- **Reliable Delivery**: Utilizes MQTT's QoS level 2 (Exactly Once) ensuring that messages are delivered reliably.
- **Detailed Logging**: Outputs logs for each message sent by the clients, as well as a summary upon completion of all messages sent.
- **Robust Error Handling**: Includes error handling to log and manage issues during client operations.

## Prerequisites

Before running the simulator, ensure you meet the following requirements:
- .NET Core SDK (version 3.1 or later recommended)
- MQTTnet library, which provides the necessary MQTT client and server capabilities
- An MQTT broker accessible via the specified broker IP and port

## Usage

Configure the MQTT broker settings in the `Main` method of the `Program` class by adjusting the `brokerIP`, `port`, `username`, and `userpassword` variables. These settings are crucial for establishing a connection to your MQTT broker.

To run the program, execute the following command from the command line at the root of your project directory:

```bash
dotnet run
