using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public Animation anim;
    public Material defaultColor;
    public Material selectedColor;

    public string[] mana = new string[5];

    public int marimeMana = 5;


    public void GolireSlotMana(int poz)
    {
        mana[poz] = mana[marimeMana - 1];
        mana[marimeMana - 1] = null;
        marimeMana--;
    }
}
