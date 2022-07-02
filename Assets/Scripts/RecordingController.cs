using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Recorder;

public class RecordingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private RecorderWindow GetRecorderWindow()
    {
        return (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow));
    }
   
    public void StartRecording() {
        RecorderWindow recorderWindow = GetRecorderWindow();
        if(!recorderWindow.IsRecording())
            recorderWindow.StartRecording();
    }
    public void StopRecording() {
        RecorderWindow recorderWindow = GetRecorderWindow();
        if(recorderWindow.IsRecording())
            recorderWindow.StopRecording();
    }
}
