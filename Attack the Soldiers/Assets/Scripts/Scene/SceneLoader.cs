using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadOfflineScene(int index)
    {
        string sceneName = GetOfflineSceneName(index);
        SceneManager.LoadScene(sceneName);
    }

    public string GetOfflineSceneName(int index)
    {
        string indexString;
        if (index < 10)
        {
            indexString = "00" + index.ToString();
        }
        else if (index < 100)
        {
            indexString = "0" + index.ToString();
        }
        else
        {
            indexString = index.ToString();
        }
        return Constants.OFFLINE_SCENE_PREFIX + indexString;
    }
}