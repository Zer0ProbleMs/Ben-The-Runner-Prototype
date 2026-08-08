using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFuncs : MonoBehaviour
{
    [SerializeField] GameObject mainmenu;
    [SerializeField] GameObject settingsmenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void EnterSettings()
    {
        mainmenu.SetActive(false);
        settingsmenu.SetActive(true);
    }
    
    public void ExitSettings()
    {
        mainmenu.SetActive(true);
        settingsmenu.SetActive(false);
    }
}
