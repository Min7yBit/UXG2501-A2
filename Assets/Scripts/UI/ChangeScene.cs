using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneFunc(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
