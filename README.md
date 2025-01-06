<h1 align="center">Record/Replay System for Unity</h1>

---

A system for Recording and Replaying game data.   
Allows you to Record ***any*** sequence of data, Serialize, and Replay it.  
***Work even In Build!***

---

## Quality Assessment

`Architecture:` Allows recording scenes with many elements with minimal manual effort.

`Optimization:` The system is optimized to avoid boxing/unboxing when accessing or setting values.

---

## Installation

1. **Install the package:**
   - Open Unity Package Manager.
   - Select **Add package from Git URL**.
   - Enter: `https://github.com/StepanLem/Unity-Replay-System.git`

2. **Download Sample scene and scripts (You need it)**
   - Open the **Unity Package Manager**.  
   - Locate the installed `Unity-Replay-System` package.  
   - Navigate to the **Samples** tab in the Package Manager.  
   - Download all samples.  

3. **Fix Odin API Compatibility Level Issue:**
   - If you encounter "Odin Inspector is incapable of compiling source code against the .NET Standard 2.0 API surface":
     - Go to **Edit > Project Settings > Player > Other Settings**.
     - Set **API Compatibility Level** to `.NET Framework`.

4. **Install** `Unity Interface Support`**:**
   - Manually install [Unity Interface Support](https://github.com/TheDudeFromCI/Unity-Interface-Support?tab=readme-ov-file) in your project. In order to do this, enter Git URL in UnityPackageManager: `https://github.com/TheDudeFromCI/Unity-Interface-Support.git?path=/Packages/net.wraithavengames.unityinterfacesupport`
  
5. **Fix `Failed to find entry-points` error:**  
   - If you encounter "Failed to find entry-points":
      - Close the project.
      - Navigate to the project folder.
      - Delete the "Library" folder (this folder is compiled locally, so deleting it will not affect your project).
      - When you restart the project, the Library will be regenerated automatically, and the error should be resolved.

---

## How to Test the Sample Recording Scene
1. Call `ReplayController.StartRecording()`.
2. Modify component values linked to `ReplayableValue`.
3. Call `ReplayController.StopRecording()`.
4. To play the recording, call `ReplayController.StartPlaybacking()`.

---

## How to Use

1. **Create a Custom ReplayableValue**
   - Inherit from `BaseReplayableValue<T>`.
   - Implement logic to read and write data from a specific component.
   - Example: Check `Samples/ReplayableValues`.

2. **Create or Customize a ReplayController**
   - Use the provided `ReplayController` or create a new one.
   - The default `ReplayController` can be extended to meet your project’s requirements.
   - Example: Check `Samples/ReplayController`.

3. **Add RecordableObject (and PlaybackableObject)**
   - Attach a `RecordableObject` to the root recording object in the scene.
   - Assign it to the `ReplayController`.
   - Do the same with `PlaybackableObject`.

4. **Connect ReplayableValues**
   - Attach and Link `ReplayableValues` to the components from which they read data.
   - If `ReplayableValue` is not a child of `RecordableObject`/`PlaybackableObject`, manually assign it.

5. **Add MonoTicker Components**
   - Add necessary `MonoTicker` components to the scene.
   - Assign them to the `ReplayableValues`.

---


## License
This project is licensed under the MIT License, see [LICENSE](https://github.com/StepanLem/Unity-Replay-System/blob/main/LICENSE.md).

[OdinSerializer](https://github.com/StepanLem/Unity-Replay-System/tree/main/OdinSerializer) folder is licensed under the Apache-2.0 License, see [LICENSE](https://github.com/StepanLem/Unity-Replay-System/blob/main/OdinSerializer/LICENSE) for more information. Odin Serializer belongs to [Team Sirenix](https://github.com/TeamSirenix)
