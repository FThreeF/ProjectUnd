using UnityEngine;


public class SceneManager : MonoBehaviour
{
    public void loadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
