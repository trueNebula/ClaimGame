using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Material defaultColor;
    public Material selectedColor;

    public string[] mana = new string[5];

    // Start is called before the first frame update
    void Start()
    {
      this.GetComponent<MeshRenderer>().material = defaultColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
