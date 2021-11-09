using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_buttonmanager : MonoBehaviour
{


    
    

    public void m_playagain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void m_startgame()
    {
        SceneManager.LoadScene("Game1");
    }

    public void m_quit()
    {
        Application.Quit();
    }
}
