About Me: A system for recording and replaying game data.


Quality Assessment
{
Architecture: Nice. Allows recording scenes with many elements with minimal manual effort.
Optimization: Nice. Successfully avoided unnecessary boxing/unboxing when reading/writing data.
}


Dependecies:
Unity Interface Support (GitHub Page: https://github.com/TheDudeFromCI/Unity-Interface-Support?tab=readme-ov-file)

Project Setup:
0.0 Install this package via PackageManager => Add package from git URL => https://github.com/StepanLem/ReplaySystem.git
0.1 To Fix "Odin Inspector is incapable of compiling source code against the .NET Standard 2.0 API surface. You can change the API Compatibility Level in the Player settings": 
Go to Edit => ProjectSettings => Player => OtherSettings => "Api Compatability Setting" set to .NetFramework
0.2 Install UnityInterfaceSupport in your project. The link to the GitHub with Setup instructions is provided above
 

1.Create your own ReplayableValue : BaseReplayableValue<T> that reads and writes data from a some component.
Example: Check the Demo folder or see Replayable_Position_OfTransform.

2.You can create your own ReplayController that interacts with RecordableObject.
Example: Check ReplayController. It works as is but may need to be extended or customized to suit the specific requirements of your project.

3.Add a RecordableObject to the root recording object in the scene and assign it to the ReplayController.

4.Connect ReplayableValues with the components from which they read data.
If a ReplayableValue is not a child of RecordableObject(PlaybackableObject), manually assign it to the required RecordableObject.

5.Add the necessary MonoTicker components to the scene and assign them to the ReplayableValue.
// If your project uses a DI-Container, you can inject them through it.


Workflow:
Call ReplayController.StartRecording();
Modify the component values linked to ReplayableValue;
Call ReplayController.StopRecording();
To play the recording, call ReplayController.StartPlaybacking();


TODO
{
Null checks and safeguards.

Stop recording/playback when a component is destroyed.

Enable dynamically adding recordable/playbackable elements to the replay system, even during an active recording/playback session.
}
