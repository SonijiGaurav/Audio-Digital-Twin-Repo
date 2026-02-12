# \# Audio-Driven Digital Engine Twin

# 

# A real-time Digital Twin project that visualizes audio data as mechanical engine physics.

# The system captures live Bluetooth audio streams via Python, calculates RPM/Vibration telemetry, and visualizes the stress on a 3D engine model in Unity.

# 

```mermaid
graph TD
    A[Real-World Audio] -->|Bluetooth| B(Python Script)
    B -->|FFT Analysis| C{Data Processing}
    C -->|Calculates RPM| D[JSON Data Bridge]
    D -.->|Atomic Write| E[Unity 3D Engine]
    E -->|Updates| F[3D Rotation]
    E -->|Updates| G[UI Dashboard]
    E -->|Updates| H[Physics/Heat]

# 

# \## 🔧 Tech Stack

# \* \*\*Data Acquisition:\*\* Python (SoundDevice, NumPy)

# \* \*\*Visualization:\*\* Unity 3D (C#)

# \* \*\*Communication:\*\* Local JSON Bridge (Atomic Writes)

# \* \*\*Physics:\*\* Procedural vibration \& thermodynamics simulation

# 

# \## 🚀 Key Features

# \* \*\*Real-Time Sync:\*\* Latency under 50ms between audio beat and engine reaction.

# \* \*\*Atomic Data Bridge:\*\* Solved file-locking race conditions between Python/Unity using atomic rename operations.

# \* \*\*Physics Simulation:\*\* Engine creates procedural vibration and heat buildup based on RPM stress.

# \* \*\*Live Analytics:\*\* Real-time line graph rendering of RPM history.

# 

# \## 🛠️ How to Run

# 1\.  \*\*Python:\*\* Run `audio\_engine.py` to start listening to system audio.

# 2\.  \*\*Unity:\*\* Open the project and press Play. The engine will automatically detect the data stream.

