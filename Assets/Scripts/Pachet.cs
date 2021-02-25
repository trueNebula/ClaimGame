using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pachet : MonoBehaviour
{

  // teancul din care poti lua carti pe parcursul jocului
  string[] cartiLibere = {
                "1@", "2@", "3@", "4@", "5@", "6@", "7@", "8@", "9@", "10@", "J@", "Q@", "K@", 
                "1%", "2%", "3%", "4%", "5%", "6%", "7%", "8%", "9%", "10%", "J%", "Q%", "K%", 
                "1#", "2#", "3#", "4#", "5#", "6#", "7#", "8#", "9#", "10#", "J#", "Q#", "K#", 
                "1$", "2$", "3$", "4$", "5$", "6$", "7$", "8$", "9$", "10$", "J$", "Q$", "K$"
                };

  // teancul de carti date jos
  string[] teancArs;

  public string atuu;
  public string ultimaCarte;

  public int marime = 52;

  public TMP_Text textMana;

  public void Initiere(int playerNumber, GameObject[] playerOrder)
  {

    Amestecare(cartiLibere, marime);
    Impartire(cartiLibere, marime, playerNumber, playerOrder);
    atuu = cartiLibere[marime - 1];
    ultimaCarte = cartiLibere[marime - 2];
    cartiLibere[marime - 2] = null;
    marime -= 2;

  }

  // functie de amestecat
  void Amestecare(string[] cartiLibere, int marime){
    int steps = 200, i, j;
    string aux;

    while(steps > 0){  
      i = Random.Range(0, marime);
      j = Random.Range(0, marime);

      if(i != j){
        aux = cartiLibere[i];
        cartiLibere[i] = cartiLibere[j];
        cartiLibere[j] = aux;

        steps--;

      }

    }

  }

  // functie de impartit
  void Impartire(string[] cartiLibere, int marime, int playerNumber, GameObject[] playerOrder){
    for(int i = 0; i < playerNumber; i++)
      for(int j = 0; j < 5; j++){
        playerOrder[i].GetComponent<Player>().mana[j] = cartiLibere[marime - 1];
        cartiLibere[marime - 1] = null;
        marime--;

      }

  }

}
