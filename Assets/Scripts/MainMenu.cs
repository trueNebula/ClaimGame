using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void SinglePlayer(){
      GameVariables.enableBots = true;
      SceneManager.LoadScene("Singleplayer");
      
    }

    public void MultiPlayer()
    {   
        GameVariables.enableBots = false;
        SceneManager.LoadScene("MainGame");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
