using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pachet : MonoBehaviour
{

  // teancul din care poti lua carti pe parcursul jocului
  // A = 10
  // B = Juvete
  // C = Queen
  // D = King
  public string[] cartiLibere = {
                "1@", "2@", "3@", "4@", "5@", "6@", "7@", "8@", "9@", "A@", "B@", "C@", "D@", 
                "1%", "2%", "3%", "4%", "5%", "6%", "7%", "8%", "9%", "A%", "B%", "C%", "D%", 
                "1#", "2#", "3#", "4#", "5#", "6#", "7#", "8#", "9#", "A#", "B#", "C#", "D#", 
                "1$", "2$", "3$", "4$", "5$", "6$", "7$", "8$", "9$", "A$", "B$", "C$", "D$"
                };

  // teancul de carti date jos
  public string[] teancJos = new string[52];
  public int marimeArs = 0;

  public string atuu;

  public int marime = 52;

  public TMP_Text textMana;


  //folosit pt start - pregatire tenac carti
  public void Initiere(int playerNumber, Player[] playerOrder)
  {

    Amestecare(cartiLibere, marime);
    Impartire(cartiLibere, playerNumber, playerOrder);
    atuu = cartiLibere[marime - 1]; 
    teancJos[marimeArs] = cartiLibere[marime - 2];
    cartiLibere[marime - 1] = null; cartiLibere[marime - 2] = null;
    marime -= 2; marimeArs++;

  }

  // functie de amestecat
  public void Amestecare(string[] carti, int marime){
    int steps = 500, i, j;
    string aux;

    while(steps > 0){  
      i = Random.Range(0, marime);
      j = Random.Range(0, marime);

      if(i != j){
        aux = carti[i];
        carti[i] = carti[j];
        carti[j] = aux;

        steps--;

      }

    }

  }

  // functie de impartit carti la  playeri
  void Impartire(string[] cartiLibere, int playerNumber, Player[] playerOrder){
    for(int i = 0; i < playerNumber; i++)
      for(int j = 0; j < 5; j++){
        playerOrder[i].GetComponent<Player>().mana[j] = cartiLibere[marime - 1];
        cartiLibere[marime - 1] = null;
        marime--;

      }

  }

}
