using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void Play()
    {
       
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        
        Application.Quit();
    }

    public void Youtube()
    {
        
        Application.OpenURL("https://youtube.com/@malapani1");
    }

    public void Instagram()
    {
        
        Application.OpenURL("https://www.instagram.com/malapani07/");
    }




}
