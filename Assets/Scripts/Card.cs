using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Texture MainTexture;
    public Texture[] Romb = new Texture[13]; 
    public Texture[] Inima = new Texture[13];
    public Texture[] Trefla = new Texture[13];
    public Texture[] Spade = new Texture[13];
    public Renderer m_Renderer;
    //obiectele pt texturile cartilor
    public GameObject CarteJos, Atuu;
    public GameObject[] cartiJucatorCurent = new GameObject[5];
    public bool[] isSelected = new bool[] {false, false, false, false, false};



    // convertim un caracter c ca numar din baza 16 in baza 10
    private int ConvertBase16(char c){
      int n = 0;

      if(c >= '0' && c <= '9')
        n = c - '0';
      
      else
        n = c -  'A' + 10;


      return n;

    }




    public void GetCardTexture(string carte, GameObject obiectCarte)
    {
      // locul din array unde se afla textura cartii
      int index = ConvertBase16(carte[0]) - 1;

      // @ = Inima
      // % = Trefla
      // # = Romb
      // $ = Spade
      switch(carte[1]){
        case '@':
          MainTexture = Inima[index];
          break;

        case '%':
          MainTexture = Trefla[index];
          break;

        case '#':
          MainTexture = Romb[index];
          break;

        case '$':
          MainTexture = Spade[index];
          break;

        default:
          MainTexture = Inima[0];
          break;
      }

      obiectCarte.GetComponent<Renderer>().materials[1].SetTexture("_BaseMap", MainTexture);

    }


    //resetare vector auxiliar de selectie
    public void Reset()
    {
      for(int i = 0; i <= 4; i++)
        isSelected[i] = false;
    }

}
