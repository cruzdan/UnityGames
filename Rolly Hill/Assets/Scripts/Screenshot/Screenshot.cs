using UnityEngine;
using System.Collections;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Screenshot : MonoBehaviour
{
    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        string path = Application.persistentDataPath + "/screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        ScreenCapture.CaptureScreenshot(path);
        UnityEngine.Debug.Log("Screenshot saved in: " + path);
    }

    public void TakeScreenshot()
    {
        StartCoroutine(Capture());
    }

    public void OpenFolder()
    {
        string path = Application.persistentDataPath;

#if UNITY_STANDALONE_WIN
        Process.Start("explorer.exe", path.Replace("/", "\\"));
#elif UNITY_STANDALONE_OSX
        Process.Start("open", path);
#elif UNITY_STANDALONE_LINUX
        Process.Start("xdg-open", path);
#elif UNITY_ANDROID
        Application.OpenURL("file://" + path);
#elif UNITY_WEBGL
        UnityEngine.Debug.Log("WebGL descarga directo");
#endif
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Screenshot))]
public class ScreenshotEditor : Editor
{
    private Screenshot screenshot;
    private void OnEnable()
    {
        screenshot = (Screenshot)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Take Screenshot"))
        {
            screenshot.TakeScreenshot();
        }
        if (GUILayout.Button("Open screenshot folder"))
        {
            screenshot.OpenFolder();
        }
    }
}
#endif
