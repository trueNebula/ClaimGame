using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButtons : MonoBehaviour
{
    public GameObject button;
    public GameObject[] butoane = new GameObject[5];
    public GameManager gmanager;
    public GameObject ErrorMessage;
    public GameObject ErrorTurn1;
    GameObject error;

    float DIFERENTA = 250f;   //centru
    float initialpoz = -500f;
    

    //creare butoane
    void Start()
    {
        for(int i = 0; i <= 4; i++)
        {
            butoane[i] = Instantiate(button) as GameObject;
            butoane[i].transform.SetParent(this.transform, false);
            butoane[i].GetComponent<ButtonActions>().index = i;
            butoane[i].GetComponent<ButtonActions>().GaMa = gmanager;
        }
    }


    //spawnare butoane
    public void UIButtons(int marime)
    {
        
        initialpoz = -500f + 125f * (5 - marime);
        //pozitie pt carti existente in mana
        for(int i = 0; i < marime; i++)
        {
            butoane[i].transform.localPosition = new Vector3(initialpoz + i * DIFERENTA, -188f, 0f);
        }

        //pozitie arbitrara outOfBounds pt carti inexistente in mana
        for(int i = marime; i <= 4; i++)
        {
            butoane[i].transform.localPosition = new Vector3(5000f, 5000f, 5000f);
        }
    }

    //eroare
    public void DisplayErrorMessage()
    {
        float time = 2f;
        error = Instantiate(ErrorMessage) as GameObject;
        error.transform.SetParent(this.transform, false);
        Destroy (error, time);
    }

    public void DisplayClaimTurn1Message()
    {
        float time = 2f;
        error = Instantiate(ErrorTurn1) as GameObject;
        error.transform.SetParent(this.transform, false);
        Destroy (error, time);
    }
}
