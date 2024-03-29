﻿using System.Collections;
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
        timp = 7f;
    }
    void OnEnable()
    {
        scoruri = this.gameObject.transform.GetChild(1).transform.GetComponent<TMP_Text>();
        winner = this.gameObject.transform.GetChild(2).transform.GetComponent<TMP_Text>();


        winner.text = "The winner is: player " + (GameVariables.current_winner + 1);
        if(GameVariables.current_winner != GameVariables.claimer)   //daca cine a dat claimcastiga, nu mai arata de 2 ori acelasi pctj.
            winner.text += " (" + (GameVariables.punctaj_castigator) + ")";
        winner.text += ".\nThe claimer is: player " + (GameVariables.claimer + 1) + " (" + (GameVariables.punctaj_claimer) + ").";


        int n = GameVariables.numar_jucatori;
        for(int i = 0; i < n; i++)
        scoruri.text += "Player " + (i + 1) + ":" + "     " + GameVariables.punctaje_playeri[i] + " (+" + GameVariables.punctaje_adaos[i] + ")\n";
    }

    public void BackToGame()
    {
        timp = -1f;
    }

    void Update()
    {
        if(timp < 0f)   
        {
            if(GameVariables.enableBots == true)
                SceneManager.LoadScene("Singleplayer");
            else
                SceneManager.LoadScene("MainGame");
        }

        timp -= Time.deltaTime;
    }
}
