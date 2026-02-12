import sounddevice as sd
import numpy as np
import json
import time
import os
import sys

# --- CONFIGURATION ---
SAMPLE_RATE = 44100
DURATION = 0.1        
GAIN = 10.0           
MIN_RPM = 800
MAX_RPM = 7000

# --- AUTOMATIC FILE PATH ---
# We use the path where the script is running
script_dir = os.path.dirname(os.path.abspath(__file__))
json_file = os.path.join(script_dir, "engine_data.json")
temp_file = os.path.join(script_dir, "engine_data.tmp")

print(f"--- ENGINE TWIN STARTED ---")
print(f"Saving data to: {json_file}")
print("1. Connect your phone via Bluetooth.")
print("2. Play music.")

# --- DEVICE SELECTION ---
print("\nAvailable Audio Devices:")
devices = sd.query_devices()
valid_devices = []
for i, dev in enumerate(devices):
    if dev['max_input_channels'] > 0:
        print(f"[{i}] {dev['name']}")
        valid_devices.append(i)

try:
    device_id = int(input("\nEnter Device ID (e.g., 1): "))
except:
    device_id = valid_devices[0]
    print(f"Invalid, defaulting to {device_id}")

print(f"\nListening... (Press Ctrl+C to stop)")

current_rpm = MIN_RPM

try:
    while True:
        # 1. RECORD AUDIO
        try:
            audio_data = sd.rec(int(DURATION * SAMPLE_RATE), samplerate=SAMPLE_RATE, channels=1, device=device_id, dtype='float32')
            sd.wait()
        except Exception as e:
            print(f"Audio Error: {e}")
            time.sleep(1)
            continue

        # 2. CALCULATE RPM
        volume = np.sqrt(np.mean(audio_data**2)) * GAIN
        volume = min(volume, 1.0)
        target_rpm = MIN_RPM + (volume * (MAX_RPM - MIN_RPM))
        current_rpm = (current_rpm * 0.7) + (target_rpm * 0.3)
        
        data = {
            "rpm": int(current_rpm),
            "volume": float(round(volume, 2))
        }

        # 3. ROBUST WRITE (The Fix)
        try:
            # Write to temp file first
            with open(temp_file, "w") as f:
                json.dump(data, f)
                f.flush()
                os.fsync(f.fileno())
            
            # Try to replace the real file. If Windows blocks it, we retry.
            max_retries = 5
            for attempt in range(max_retries):
                try:
                    os.replace(temp_file, json_file)
                    break # Success! Exit the retry loop
                except PermissionError:
                    # Unity is reading the file. Wait 10ms and try again.
                    time.sleep(0.01)
                except Exception as e:
                    print(f"Write Error: {e}")
                    break
                    
        except Exception as e:
            print(f"File Error: {e}")

        # 4. STATUS
        bars = "#" * int(volume * 20)
        print(f"\rRPM: {int(current_rpm)} | {bars.ljust(20)}", end="")

except KeyboardInterrupt:
    print("\nStopped.")