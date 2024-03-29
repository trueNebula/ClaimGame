﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool enableBots;
    public GameObject playerCard, button;
    public GameObject ErrorTurn;
    GameObject turnError;
    public int playerNumber;

    public Player[] playerOrder = new Player[6];
    public Player playerCapsule;

    public Pachet pachetManager;

    public Card teancCarti;

    public SpawnButtons UIselectcard;

    public int[] punctaje_jucatori = {0, 0, 0, 0, 0, 0};

    public int turn = 0, tura = 1, start_turn;
    float xmulti = 10f, zmulti = 8f;        //multiplicatori pt pozitia de spawnare
    public float tura_multiplier = 0.97f, tura_multiplier_increment = 0.96f;
    public float timeForBotWait;


    void Update()
    {
      if(timeForBotWait > -1f)
        {timeForBotWait -= Time.deltaTime; Debug.Log(timeForBotWait);}

      if(timeForBotWait < 0f && turn != 0)
      {
        //actualizarea texturii cartii de jos
        teancCarti.GetCardTexture(pachetManager.teancJos[pachetManager.marimeArs - 1], teancCarti.CarteJos);
        NextPlayer();
      }
    }

    // Start is called before the first frame update
    void Start()
    {
      playerNumber = GameVariables.numar_jucatori;
      
      enableBots = GameVariables.enableBots;
      //spawneaza jucatorii
      for(int i = 0; i < playerNumber; i++)
      {
        //pozitia este bazata pe cercul trigonometric, cu masa centrata la 0,0
        playerOrder[i] = Instantiate(playerCapsule, new Vector3(Mathf.Cos(2f * i * Mathf.PI / playerNumber) * xmulti, 1f, Mathf.Sin(2f * i * Mathf.PI / playerNumber) * zmulti), Quaternion.identity) as Player;

      }

      // randomizam jucatorul care incepe
      turn = Random.Range(0, playerNumber);
      start_turn = turn;

      //amestecare, impartire carti, stabilire atuu
      pachetManager.Initiere(playerNumber, playerOrder);

      //textura primei carti de jos
      teancCarti.GetCardTexture(pachetManager.teancJos[pachetManager.marimeArs - 1], teancCarti.CarteJos);       // Instantiate(carteJos, new Vector3(0f, 1f, 0f));
      teancCarti.GetCardTexture(pachetManager.atuu, teancCarti.Atuu);

      //culoarea capsulei este schimbata in cea definita pt jucatorul curent
      playerOrder[turn].GetComponent<MeshRenderer>().material = playerOrder[turn].GetComponent<Player>().selectedColor;


      if(enableBots){ // joci singleplayer
        if(turn == 0){ //e randul tau 
          //initializatul cartilor si afisajul texturilor acestora din mana jucatorului curent, iar apoi butoanele lor de selectare
          SpawnPlayerHand(); ShowHand(playerOrder[turn]); UIselectcard.UIButtons(playerOrder[turn].marimeMana);

        }

        else
          AI();


      }

      else{ // joci multiplayer
        //initializatul cartilor si afisajul texturilor acestora din mana jucatorului curent, iar apoi butoanele lor de selectare
          SpawnPlayerHand(); ShowHand(playerOrder[turn]); UIselectcard.UIButtons(playerOrder[turn].marimeMana);

      }


    }
    


    void ActualizareMana()
    {
      ShowHand(playerOrder[turn]); MoveHand();
      //actualizarea pozitiei si numarului de butoane de pe carti
      UIselectcard.UIButtons(playerOrder[turn].marimeMana);
      //actualizarea texturii cartii de jos
      teancCarti.GetCardTexture(pachetManager.teancJos[pachetManager.marimeArs - 1], teancCarti.CarteJos);
      //resetare pozitii la sf de rand
      ResetarePozitiiCarti();
    }


    //apelat de butonul pentru urmatoarea tura
    public void NextPlayer()
    {
      //calcularea pozitiei urmatorului jucator si schimbarea culorii de determinare a celui ce este la rand
      turn = (turn+1) % playerNumber;
      if(start_turn == turn)
      {  
        tura++; 
        tura_multiplier *= tura_multiplier_increment; //se inmulteste de fiecare data, pt a mari sansa, sunt subunitare; default: 0.92f
      }
      PlayerColorUpdate(playerOrder, turn);
      
      

      if(enableBots){ // joci singleplayer
        if(turn == 0){ //e randul tau 
          
          if(tura == 1)
            SpawnPlayerHand();
          
          
          //actualizarea texturilor pentru fiecare carte din lista de carti a teancului si afisarea corespunzatoare numarului lor
          ActualizareMana();

        }

        else
        {
          timeForBotWait = 1.2f;
          AI();
        }


      }

      else{ // joci multiplayer
        //actualizarea texturilor pentru fiecare carte din lista de carti a teancului si afisarea corespunzatoare numarului lor
        ShowHand(playerOrder[turn]); MoveHand();
        //actualizarea pozitiei si numarului de butoane de pe carti
        UIselectcard.UIButtons(playerOrder[turn].marimeMana);
        //actualizarea texturii cartii de jos
        teancCarti.GetCardTexture(pachetManager.teancJos[pachetManager.marimeArs - 1], teancCarti.CarteJos);
        //resetare pozitii la sf de rand
        ResetarePozitiiCarti();

      }

    }


    void AI(){ 
      // se uita in mana jucatorului playerOrder[turn]
      // determina cartea sau cartile de acelasi fel cu punctajul cel mai mare si da jos

      int[] punctajPeNrCarte = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // vector freq cu punctajul pe carte
      int punctajMaxim = 0, totalPuncteMana = 0;
      int ct = 0;
      char cartePunctajMaxim = '0';
      // bool aLuat = false;


      //stabilire totalul punctelor din mana si ce numar au cartile care adunate au maximul de pct. din mana si pot fi date jos
      for(int i = 0; i < playerOrder[turn].marimeMana; i++){
        if(playerOrder[turn].mana[i][0] != pachetManager.atuu[0]){
          int val = ValoareCarte(playerOrder[turn].mana[i][0]); // face vectorul de freq
          punctajPeNrCarte[val] += val;

          totalPuncteMana += val;

          if(punctajPeNrCarte[val] > punctajMaxim){   // determina cartea "maxima" curenta
            punctajMaxim = punctajPeNrCarte[val];
            cartePunctajMaxim = playerOrder[turn].mana[i][0];

          }

        }

      }

      if(tura > 1)    //nu pot da claim din prima tura
      {
        //determinare sansa de a da claim
        if(totalPuncteMana <= 10)
        {                                //pt tura 1, tura_multiplier = 0.93f, descrescator
          int sansa = (int)(totalPuncteMana * 15 * (tura_multiplier + 0.17f));      //cu cat cartea este mai mare, cu atat mai putine numere generate de la 0 la 100 vor fi mai mici
          if(sansa > 100) sansa = 100;      
          Debug.Log(100 - sansa); Debug.Log(totalPuncteMana); Debug.Log(turn + 1);     //minim 1 la 100 sansa
          if(Random.Range(0, 101) >= sansa)
            {
              Claim();
              return;
            }
        }
      }
     
        
      //aruncare carti din mana
      for(int i = 0; i < playerOrder[turn].marimeMana; i++){
        if(playerOrder[turn].mana[i][0] == cartePunctajMaxim){ 
          MutareCarteMana_Jos(playerOrder[turn], i);  // da jos cartile "maxime"
          ct++; //ct pt a lua a marime-ct -a carte de jos
          i--;
        }

      }


      bool extragePachet = ValoareCarte(pachetManager.teancJos[pachetManager.marimeArs - 1 - ct][0]) <= 7;
      if(extragePachet)
      {
        //ia de jos carte
        Player jucator = playerOrder[turn];
        jucator.mana[jucator.marimeMana] = pachetManager.teancJos[pachetManager.marimeArs - ct - 1]; //-1 - ct pt ca intai pune cartea jos, apoi o ia pe cea de sub
        jucator.marimeMana++; pachetManager.marimeArs--;
        pachetManager.teancJos[pachetManager.marimeArs - ct] = pachetManager.teancJos[pachetManager.marimeArs];
        pachetManager.teancJos[pachetManager.marimeArs] = null;
        
      }
      else      // ia din teanc o carte
      {
        if(pachetManager.marime == 0)
          RefacereCarti();

        Player jucator = playerOrder[turn];
        jucator.mana[jucator.marimeMana] = pachetManager.cartiLibere[pachetManager.marime - 1]; 
        pachetManager.cartiLibere[pachetManager.marime - 1] = null;
        jucator.marimeMana++; pachetManager.marime--;
      }

      //jucator.anim.Play("wait");


      // daca ultimaCarte < jos => ia ultimaCarte, altfel ia din pachet     -   sa o punem pt hard diff.

    }

    void PlayerColorUpdate(Player[] playerOrder, int turn)
    {

      // resetam culoarea jucatorului precedent
      int previous;

      if(turn == 0) previous = 5;
      else          previous = turn - 1;

      //culoarea jucatorului precedent se reseteaza
      playerOrder[previous].GetComponent<MeshRenderer>().material = playerOrder[previous].GetComponent<Player>().defaultColor;

      // schimbam culoarea jucatorului curent
      playerOrder[turn].GetComponent<MeshRenderer>().material = playerOrder[turn].GetComponent<Player>().selectedColor;

    }

    void SpawnPlayerHand()
    {
      //teanc carti are o lista de 5 obiecte goale, ce reprezinta cartile, care aici sunt initializate
      for(int i = 0; i <= 4; i++)
      {
        teancCarti.cartiJucatorCurent[i] = Instantiate(playerCard, new Vector3((i - 2f) * 4f, 1f, -3f), Quaternion.Euler(-90, 180, 0));
      }
      

    }

    void MoveHand()
    {
      //pt acest player, mana isi actualizeaza numarul de carti afisate
      int handSize = playerOrder[turn].marimeMana;
      float initialpoz = -2f + 0.5f * (5 - handSize), DIFERENTA = 1f;

      for(int i = 0; i < handSize; i++)
      {
        teancCarti.cartiJucatorCurent[i].transform.localPosition = new Vector3((initialpoz + i * DIFERENTA) * 4f, teancCarti.cartiJucatorCurent[i].transform.localPosition.y, teancCarti.cartiJucatorCurent[i].transform.localPosition.z);
      }

      for(int i = handSize; i <= 4; i++)
      {
        teancCarti.cartiJucatorCurent[i].transform.localPosition = new Vector3(50f, 50f, 50f);
      }
    }

    void ShowHand(Player player)
    {
      //pt acest player, teancul isi actualizeaza textura pt fiecare componenta, in functie de ce are playerul in mana
      for(int i = 0; i <= player.marimeMana - 1; i++)
        {
          teancCarti.GetCardTexture(player.mana[i], teancCarti.cartiJucatorCurent[i]);
        }

    }


    public void SelectieCarte(int cardIndex)
    {
      //marcheaza pozitiile selectate si modifica pozitia cartilor in joc
      teancCarti.isSelected[cardIndex] = true;
      Transform t = teancCarti.cartiJucatorCurent[cardIndex].GetComponent<Transform>();
      t.localPosition = new Vector3(t.localPosition.x, 1.5f, -2.2f);

    }

    void PozitieCartiLaDeselectie(int poz)
    {
      //rescrierea schimbarii pozitiei obiectelor carti la deselectie
      Transform tr = teancCarti.cartiJucatorCurent[poz].GetComponent<Transform>();
      tr.localPosition = new Vector3(tr.localPosition.x, 1f, -3f);

    }

    public void DeselectieCarte()
    {
      //se deselecteaza toate cartile marcate in vectorul auxiliar isSelected
      for(int i = 0; i <= 4; i++)
        if(teancCarti.isSelected[i] == true)
          {
            teancCarti.isSelected[i] = false;
            PozitieCartiLaDeselectie(i);
          }

    }

    void RefacereCarti()
    {
      //introducere atuu in carti
      pachetManager.teancJos[pachetManager.marimeArs] = pachetManager.atuu;
      pachetManager.marimeArs++;
      pachetManager.atuu = null;


      //amestecare teancJos
      pachetManager.Amestecare(pachetManager.teancJos, pachetManager.marimeArs);

      //reconstruire teanc de extras

      for(int i = 0; i < pachetManager.marimeArs; i++)
      {
        pachetManager.cartiLibere[i] = pachetManager.teancJos[i];
        pachetManager.teancJos[i] = null;
      }
      pachetManager.marime = pachetManager.marimeArs;
      pachetManager.marimeArs = 0;


      //stabilire ultima carte ca atuu
      pachetManager.atuu = pachetManager.cartiLibere[pachetManager.marime - 1];
      pachetManager.cartiLibere[pachetManager.marime - 1] = null;
      pachetManager.marime--;

      //pune jos urmatoarea carte
      pachetManager.teancJos[pachetManager.marimeArs] = pachetManager.cartiLibere[pachetManager.marime - 1];
      pachetManager.cartiLibere[pachetManager.marime - 1] = null;
      pachetManager.marime--;   pachetManager.marimeArs++;

      //actualizare textura atuu
      teancCarti.GetCardTexture(pachetManager.atuu, teancCarti.Atuu);

    }

    void MutareCarteMana_Jos(Player jucator, int pozitie)
    {

      //cartea jucatorului de pe pozitia (pozitie) este aruncata in teancul pt aruncat, se realizeaza la apelul ultimei functii 
      //prin aducerea ultimei carti peste cea de pe (pozitie) si apoi marcarea ultimei pozitii ca inexistente

      //pune cartea din mana de pe acea pozitie jos
      pachetManager.teancJos[pachetManager.marimeArs] = jucator.mana[pozitie]; pachetManager.marimeArs++;
      //se schimba valorile auxiliare ce determina daca o carte este selectata sau nu, deoarece functia schimba cartile
      bool aux = teancCarti.isSelected[pozitie];
      teancCarti.isSelected[pozitie] = teancCarti.isSelected[jucator.marimeMana - 1];
      teancCarti.isSelected[jucator.marimeMana - 1] = aux;
      //elimina cartea de pe pozitie din mana jucatorului si marcheaza null ultima carte
      jucator.GolireSlotMana(pozitie);

    }


    public void UltimaCarteOnClick()
    {
      bool ok = false;
      for(int i = 0; i < playerOrder[turn].marimeMana; i++)
        if(teancCarti.isSelected[i] == true)      //se iau la rand cartile din mana marcate ca si selectate
          {
            
            //se marcheaza ca deselectat, se modifica pozitia cartii deselectate, se muta acea carte jos
            teancCarti.isSelected[i] = false;
            PozitieCartiLaDeselectie(i);
            MutareCarteMana_Jos(playerOrder[turn], i);

            if(ok == false) //doar la prima carte gasita ca selectata, se face schimbul cu cea de jos
            {
              i--;
              //Ia carte de jos
              Player jucator = playerOrder[turn];
              jucator.mana[jucator.marimeMana] = pachetManager.teancJos[pachetManager.marimeArs - 2]; //-2 pt ca intai pune cartea jos, apoi o ia pe cea de sub
              jucator.marimeMana++; pachetManager.marimeArs--;
              pachetManager.teancJos[pachetManager.marimeArs - 1] = pachetManager.teancJos[pachetManager.marimeArs];
              pachetManager.teancJos[pachetManager.marimeArs] = null;
              
              ok = true;

            }

          }
        
      if(ok == true)  //daca exista carti selectate
      {
        ActualizareMana();
        NextPlayer();
      }
      else            //nu se poate lua carte fara sa fie selectate cel putin 1 pt aruncare
        UIselectcard.DisplayErrorMessage();
    }

    public void PachetExtrasOnClick()
    {
      bool ok = false;
      //daca nu mai sunt carti de extras
      if(pachetManager.marime == 0)
        RefacereCarti();

      for(int i = 0; i < playerOrder[turn].marimeMana; i++)
        if(teancCarti.isSelected[i] == true)      //se iau la rand cartile din mana marcate ca si selectate
          {
            
            //se marcheaza ca deselectat, se modifica pozitia cartii deselectate, se muta acea carte jos
            teancCarti.isSelected[i] = false;
            PozitieCartiLaDeselectie(i);
            MutareCarteMana_Jos(playerOrder[turn], i);

            if(ok == false) //doar la prima carte gasita ca selectata, se face schimbul cu cea din pachetul de extras
            {
              i--;
              //Ia carte din pachet
              Player jucator = playerOrder[turn];
              jucator.mana[jucator.marimeMana] = pachetManager.cartiLibere[pachetManager.marime - 1]; 
              pachetManager.cartiLibere[pachetManager.marime - 1] = null;
              jucator.marimeMana++; pachetManager.marime--;

              ok = true;

            }

          }
        
      if(ok == true)  //daca exista carti selectate
      {
        ActualizareMana();
        NextPlayer();
      }
      else            //nu se poate lua carte fara sa fie selectate cel putin 1 pt aruncare
        UIselectcard.DisplayErrorMessage();
    }

    //apelata la sfarsitul unui rand, pentru a 'asigura' deselectarea cartilor si resetarea indicelui butonului precedent
    void ResetarePozitiiCarti()
    {

      for(int i = 0; i <= 4; i++)
        PozitieCartiLaDeselectie(i);

      UIselectcard.butoane[0].GetComponent<ButtonActions>().Reset();

    }

    //conversie char -> numar
    int ValoareCarte(char c)
    {

      int n = 0;

      if(c >= '0' && c <= '9')  //de la 1 la 9
        n = c - '0';
      
      else
      {
        if(c == 'A')    //la 10
          n = 10;

        else            //de la J la K
          n = c -  'A' + 11;
      }

      return n;

    }

    void CalcularePunctaje()
    {
      char Atuu = pachetManager.atuu[0];
      Player jucator;


      //la fiecare jucator se retine punctajul intr un sir
      for(int j = 0; j <= 5; j++)
      {
        jucator = playerOrder[j];
        for(int i = 0; i < jucator.marimeMana; i++)     //fiecare carte care nu este atuu
          if(jucator.mana[i][0] != Atuu)
            punctaje_jucatori[j] += ValoareCarte(jucator.mana[i][0]);   //se aduna, prin conversie
      }
    }

    public void PlayerClaim()
    {
      if(turn != 0)     //daca nu esti tu, pt singleplayer(nu mai facem multi)
      {
        float time = 2f;
        turnError = Instantiate(ErrorTurn) as GameObject;
        turnError.transform.SetParent(UIselectcard.transform, false);
        Destroy (turnError, time);

        return ;
      }
      if(tura == 1)     //nu poti da Claim prima tura
      {
        UIselectcard.DisplayClaimTurn1Message();
        return;
      }

      //daca merge sa dai claim:
      Claim();
    }


    void Claim()
    {
      GameVariables.claimer = turn;
      CalcularePunctaje();

      //verificare castig
      bool teapa = false;
      int punctaj_apelator = punctaje_jucatori[turn];
      int min_punctaj = 69, id_min_pctj = 0;    //69 = max posibil

      for(int j = 0; j <= 5; j++)
        if(j != turn)
        {
          if(punctaje_jucatori[j] <= punctaj_apelator)  teapa = true;
          if(min_punctaj > punctaje_jucatori[j])        
          {
            min_punctaj = punctaje_jucatori[j];
            id_min_pctj = j;
          }
        }
      
      GameVariables.punctaje_adaos = new int[] {0, 0, 0, 0, 0, 0};    //resetare diferenta de punctaje

      if(teapa == true)    //daca jucatorul care a dat Claim nu are cele mai putine puncte
      {
        //jucator [turn] umfla 50
        GameVariables.punctaje_playeri[turn] += 50;
        GameVariables.punctaje_adaos[turn] = 50;
        GameVariables.current_winner = id_min_pctj;
      }
      else
      {
        //restul umfla punctajele lor

        for(int j = 0; j <= 5; j++)
          if(j != turn)
            {
              GameVariables.punctaje_playeri[j] += punctaje_jucatori[j]; 
              GameVariables.punctaje_adaos[j] = punctaje_jucatori[j];
            }

        GameVariables.current_winner = turn;
      }

      GameVariables.punctaj_claimer = punctaj_apelator;
      GameVariables.punctaj_castigator = min_punctaj;
      //Load Victory Scene
      SceneManager.LoadScene("VictoryMenu");
    }


  //end
}
