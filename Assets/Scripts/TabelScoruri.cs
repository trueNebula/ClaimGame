using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TabelScoruri : MonoBehaviour
{
    TMP_Text scoruri, winner;

    float timp;

    void Start()
    {
        timp = 8f;
    }
    void OnEnable()
    {
        scoruri = this.gameObject.transform.GetChild(1).transform.GetComponent<TMP_Text>();
        winner = this.gameObject.transform.GetChild(2).transform.GetComponent<TMP_Text>();


        winner.text = "The winner is: player " + (GameVariables.current_winner + 1);

        int n = GameVariables.numar_jucatori;
        for(int i = 0; i < n; i++)
        scoruri.text += "Player " + (i + 1) + ":" + "     " + GameVariables.punctaje_playeri[i] + "\n";
    }

    public void BackToGame()
    {
        timp = 0f;
        if(GameVariables.enableBots == true)
                SceneManager.LoadScene("Singleplayer");
            else
                SceneManager.LoadScene("MainGame");
    }

    void Update()
    {
        timp -= Time.deltaTime;

        if(timp < 0f)   
        {
            if(GameVariables.enableBots == true)
                SceneManager.LoadScene("Singleplayer");
            else
                SceneManager.LoadScene("MainGame");
        }
    }
}
