﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public int index = 0;
    public GameManager GaMa;
    public static int currentIndex = -1;

    GameObject error;


    void ErrorTurnMessage()
    {
        float time = 2f;
        error = Instantiate(GaMa.ErrorTurn) as GameObject;
        error.transform.SetParent(this.transform.parent, false);
        Destroy (error, time);
    }

    //selectare carte noua in mana
    public void ButtonSelectCard()
    {
        if(GaMa.turn != 0)
        {
            ErrorTurnMessage();
            return;
        }
        if(currentIndex != -1)      //prima selectie nu deselecteaza nimic, deoarece nu a fost nimic selectat anterior
        if(GaMa.playerOrder[GaMa.turn].mana[currentIndex][0] != GaMa.playerOrder[GaMa.turn].mana[index][0]) //daca cartea selectata are
            GaMa.DeselectieCarte();                             //acelasi numar cu cea ce trebuie deselectata, nu se mai face deselectia

        //selectia noua carte
        currentIndex = index;
        if(GaMa.teancCarti.isSelected[currentIndex] == false)
            GaMa.SelectieCarte(currentIndex);
        
    }


    //resetare index la inceputul randului unui alt jucator
    public void Reset()
    {
        currentIndex = -1;
    }
}
