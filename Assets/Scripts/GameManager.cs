using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject playerCapsule;
    public int playerNumber = 6;

    public GameObject[] playerOrder = new GameObject[6];
    private GameObject gigel;

    public Pachet pachetManager;

    public Card carteJos;

    int turn = 0;

    // Start is called before the first frame update
    void Start()
    {

      for(int i = 0; i < playerNumber; i++){
        gigel = Instantiate(playerCapsule, new Vector3(Mathf.Cos(2f * i * Mathf.PI / playerNumber) * 10f, 1f, Mathf.Sin(2f * i * Mathf.PI / playerNumber) * 8f), Quaternion.identity);
        playerOrder[i] = gigel;

      }

      pachetManager.Initiere(playerNumber, playerOrder);

      carteJos.GetCardTexture(pachetManager.ultimaCarte);
      // Instantiate(carteJos, new Vector3(0f, 1f, 0f));

      // randomizam jucatorul care incepe
      turn = Random.Range(0, playerNumber);



    }
    
  
    public void NextPlayer()
    {
      PlayTurn(playerOrder, turn);

      turn = (turn+1) % playerNumber;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayTurn(GameObject[] playerOrder, int turn){

      // resetam culoarea jucatorului precedent
      int previous;

      if(turn == 0)
        previous = 5;
      
      else
        previous = turn - 1;

      playerOrder[previous].GetComponent<MeshRenderer>().material = playerOrder[previous].GetComponent<Player>().defaultColor;

      // schimbam culoarea jucatorului curent
      playerOrder[turn].GetComponent<MeshRenderer>().material = playerOrder[turn].GetComponent<Player>().selectedColor;

      

    }

}
