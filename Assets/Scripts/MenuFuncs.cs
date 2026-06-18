using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFuncs : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
}
