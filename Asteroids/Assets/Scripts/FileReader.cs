using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileReader
{
    //get the file begging for the application path
    public static string[] GetContentFromFile(string filePath)
    {
        string[] content;
        content = File.ReadAllLines(Application.dataPath + "/" +  filePath);
        return content;
    }
    //get the file from the StreamingAssets folder
    public static string[] GetContentFromFileBuild(string filePath)
    {
        string[] content;
        content = File.ReadAllLines(Application.streamingAssetsPath + "/" + filePath);
        return content;
    }
}
