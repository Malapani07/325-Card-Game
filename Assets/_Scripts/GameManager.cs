using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    play,
}

public class GameManager : MonoBehaviour
{
    //GameManager Instance (singleton)
    public static GameManager instance;

    public List<GameObject> Deck;
    public GameState gamestate;
    public string trump = "spade";
    public int no_of_Uthris;
    public int Round=0;
    public int MaxRounds;
    [Header("All Players GameObject")]
    public Player1 player1;
    public Player2 player2;
    public Myplayer myplayer;
    Vector3 offset = new Vector3(0f, 0f, 0f);

    [Header("Hand Checker variables")]
    public List<GameObject> ThrownCard;
    int NoOfPositivesLoan;

    public Setting setting;
    public Sprite back_image1;
    public Sprite back_image2;
    public Sprite back_image3;
    public Sprite back_image4;
    public Sprite back_image5;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        MaxRounds = setting.maxRound;
        Deck = Shuffle(Deck);
        distribute(Deck, myplayer, player1, player2);
        StartnewRound();
        UiManager.instance.UpdateScores(myplayer,player1,player2);
    }


    //----------To Assign Goals to players-----------//
    void StartnewRound()
    {
        int ran =Random.Range(0, 3);
        if (ran == 0)
        {
            myplayer.Goal = 5;
            player2.Goal = 3;
            player1.Goal = 2;
            UiManager.instance.OnSelectingTrump();
        }
        else if (ran == 1)
        {
            player1.Goal = 5;
            myplayer.Goal = 3;
            player2.Goal = 2;
            FaceupMyCards();
            MakeTrump(player1.Player1Cards);
            player1.throwcard();
        }
        else if (ran == 2)
        {
            player2.Goal = 5;
            player1.Goal = 3;
            myplayer.Goal = 2;
            FaceupMyCards();
            MakeTrump(player2.Player2Cards);
            player2.throwcard();
        }

    }


    public void FaceupMyCards()
    {
        for (int i = 0; i < myplayer.Mycards.Count; i++)
        {
            myplayer.Mycards[i].GetComponent<Card>().FaceUp();
        }
    }




    // ---------------Shuffling Cards------------//

    public List<GameObject> Shuffle(List<GameObject> cards)
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            GameObject temp = cards[i];
            int r =Random.Range(0, cards.Count - 1);
            cards[i] = cards[r];
            cards[r] = temp;
        }
        return cards;
    }

    //--------------Distributing Cards-------------//

    public void distribute(List<GameObject> Cards, Myplayer myplayer, Player1 player1, Player2 player2)
    {
        offset = Vector3.zero;
        for (int i = 0; i < Cards.Count; i++)
        {
            if (i < 10)
            {
                GameObject card = Instantiate(Cards[i], myplayer.transform.position + offset, Quaternion.identity);
                card.transform.SetParent(myplayer.transform);
                card.GetComponent<Card>().back_image = DecideBackground(setting.BackgroundNum);
                card.AddComponent<BoxCollider2D>();
                card.GetComponent<Card>().whosecard = Whosecard.mycard;
                myplayer.Mycards.Add(card);
                if (i > 4) { card.GetComponent<Card>().FaceDown(); }
                offset += new Vector3(1.5f, 0f, 0f);
            }
            else if (i >= 10 && i < 20)
            {
                GameObject card = Instantiate(Cards[i], player1.transform.position, Quaternion.identity);
                card.transform.SetParent(player1.transform);
                card.GetComponent<Card>().back_image = DecideBackground(setting.BackgroundNum);
                card.GetComponent<Card>().FaceDown();
                card.GetComponent<Card>().whosecard = Whosecard.player1card;
                player1.Player1Cards.Add(card);
            }
            else if (i >= 20 && i < 30)
            {
                GameObject card = Instantiate(Cards[i], player2.transform.position, Quaternion.identity);
                card.transform.SetParent(player2.transform);
                card.GetComponent<Card>().back_image = DecideBackground(setting.BackgroundNum);
                card.GetComponent<Card>().FaceDown();
                card.GetComponent<Card>().whosecard = Whosecard.player2card;
                player2.Player2Cards.Add(card);
            }
        }
    }

    //---------------------Check Who wins the Hand----------------//

    public void Checkresult()
    {
        StartCoroutine(CheckResult(ThrownCard[0].GetComponent<Card>(), ThrownCard[1].GetComponent<Card>(), ThrownCard[2].GetComponent<Card>()));
    }

    private IEnumerator CheckResult(Card card0, Card card1, Card card2)
    {
        yield return new WaitForSeconds(.5f);
        // Three Same cards 
        if (card0.symbol == card1.symbol && card0.symbol == card2.symbol)
        {
            if (card0.value > card1.value)
            {
                if (card0.value > card2.value)
                {
                    ZerothCardWin(card0);
                }
                else
                {
                    SecondCardWin(card2);
                }
            }
            else
            {
                if (card1.value > card2.value)
                {
                    FirstCardWin(card1);
                }
                else
                {
                    SecondCardWin(card2);
                }
            }
        }

        // ThrownCards[1] symbol is same as ThrownCards[0] but not ThrownCards[2]
        else if (card0.symbol == card1.symbol && card0.symbol != card2.symbol)
        {
            if (card2.symbol == trump)
            {
                SecondCardWin(card2);
            }

            else if (card2.symbol != trump)
            {
                if (card0.value > card1.value)
                {
                    ZerothCardWin(card0);
                }
                else
                {
                    FirstCardWin(card1);
                }
            }
        }

        // ThrownCards[2] symbol is same as ThrownCards[0] but not ThrownCards[1] 
        else if (card0.symbol != card1.symbol && card0.symbol == card2.symbol)
        {
            if (card1.symbol == trump)
            {
                FirstCardWin(card1);
            }

            else if (card1.symbol != trump)
            {
                if (card0.value > card2.value)
                {
                    ZerothCardWin(card0);
                }
                else
                {
                    SecondCardWin(card2);
                }
            }
        }


        // NO Cards Same
        else if (card0.symbol != card1.symbol && card0.symbol != card2.symbol)
        {
            if (card1.symbol == trump && card2.symbol == trump)
            {
                if (card1.value > card2.value)
                {
                    FirstCardWin(card1);
                }
                else
                {
                    SecondCardWin(card2);
                }
            }


            else if (card1.symbol == trump && card2.symbol != trump)
            {
                FirstCardWin(card1);
            }
            else if (card1.symbol != trump && card2.symbol == trump)
            {
                SecondCardWin(card2);
            }

            else
            {
                ZerothCardWin(card0);
            }
        }

        if (no_of_Uthris == 10)
        {
            myplayer.Mycards.Clear();
            Round++;
            ShoWScore();
            
            //ClearingLoan();
        }
    }

    void DestroyCards()
    {
        for (int i = 0; i < ThrownCard.Count; i++)
        {
            Destroy(ThrownCard[i]);
        }
    }

    void ZerothCardWin(Card card0)
    {
        if (card0.whosecard == Whosecard.mycard)
        {
           
            myplayer.point++;
            DestroyCards();
            SoundManager.instance.sounds[0].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            myplayer.MyTurnStart();
        }
        else if (card0.whosecard == Whosecard.player1card)
        {
          
            player1.point++;
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player1.throwcard();
        }
        else if (card0.whosecard == Whosecard.player2card)
        {
           
            player2.point++;
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player2.throwcard();
        }
        UiManager.instance.UpdateScores(myplayer, player1, player2);
    }
    void FirstCardWin(Card card1)
    {
        if (card1.whosecard == Whosecard.mycard)
        {
          
            myplayer.point++;
            DestroyCards();
            SoundManager.instance.sounds[0].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            myplayer.MyTurnStart();
        }
        else if (card1.whosecard == Whosecard.player1card)
        {
           
            player1.point++;
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player1.throwcard();
        }
        else if (card1.whosecard == Whosecard.player2card)
        {
          
            player2.point++;
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player2.throwcard();
        }
        UiManager.instance.UpdateScores(myplayer, player1, player2);
    }
    void SecondCardWin(Card card2)
    {
        if (card2.whosecard == Whosecard.mycard)
        {
            
            myplayer.point++;
            DestroyCards();
            SoundManager.instance.sounds[0].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            myplayer.MyTurnStart();
        }
        else if (card2.whosecard == Whosecard.player1card)
        {
            
            player1.point++;  
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player1.throwcard();
        }
        else if (card2.whosecard == Whosecard.player2card)
        {
          
            player2.point++;          
            DestroyCards();
            SoundManager.instance.sounds[1].Play();
            ThrownCard.Clear();
            no_of_Uthris++;
            player2.throwcard();
        }
        UiManager.instance.UpdateScores(myplayer, player1, player2);
    }


    //-----------------Making Trump For Cpu Players-------------------//
    public void MakeTrump(List<GameObject> cards)
    {
        int hearts = 0;
        int spades = 0;
        int diamonds = 0;
        int clubs = 0;

        for (int i = 0; i < 5; i++)
        {
            if (cards[i].CompareTag("heart"))
            {
                hearts++;
            }
            else if (cards[i].CompareTag("spade"))
            {
                spades++;
            }
            else if (cards[i].CompareTag("diamond"))
            {
                diamonds++;
            }
            else if (cards[i].CompareTag("club"))
            {
                clubs++;
            }
        }

        if (hearts > spades)
        {
            if (diamonds > clubs)
            {
                if (hearts > diamonds)
                {
                    //heartwin
                    trump = "heart";
                }
                else
                {
                    //diamondwin
                    trump = "diamond";
                }
            }
            else
            {
                if (hearts > clubs)
                {
                    //heartwin
                    trump = "heart";
                }
                else
                {
                    //clubwin
                    trump = "club";
                }
            }
        }
        else
        {
            if (diamonds > clubs)
            {
                if (spades > diamonds)
                {
                    //spadewin
                    trump = "spade";
                }
                else
                {
                    //diamondwin
                    trump = "diamond";
                }
            }
            else
            {
                if (spades > clubs)
                {
                    //spadewin
                    trump = "spade";
                }
                else
                {
                    //clubwin
                    trump = "club";
                }
            }
        }
        UiManager.instance.UpdateTrumpImage();
    }

   /* void ClearingLoan()
    {
        myplayer.loan = myplayer.calculateLoan();
        player1.loan = player1.calculateLoan();
        player2.loan = player2.calculateLoan();

        if (myplayer.loan > 0)
        {
            NoOfPositivesLoan++;
        }
        if (player1.loan > 0)
        {
            NoOfPositivesLoan++;
        }
        if (player2.loan > 0)
        {
            NoOfPositivesLoan++;
        }

        if (NoOfPositivesLoan == 1)
        {
            if (myplayer.loan > 0)
            {
                Debug.Log("myplayer take card from P1 and P2");
            }
            else if (player1.loan > 0)
            {
                Debug.Log("player1 take card from MyP and P2");
            }
            else if (player2.loan > 0)
            {
                Debug.Log("Player2 take card from P1 And MyP");
            }
        }
        else
        {
            if (myplayer.loan < 0)
            {
                Debug.Log("myplayer Give Card to P1 and P2");
            }
            else if (player1.loan < 0)
            {
                Debug.Log("Player1 give Card to myP and P2");
            }
            else if (player2.loan < 0)
            {
                Debug.Log("player2 give Card to P1 and mtP");
            }
        }

    }
   */

    void ShoWScore()
    {
        UiManager.instance.BackButton.SetActive(false);
        UiManager.instance.ScorePanel.SetActive(true);
        UiManager.instance.MyPlayerBoard.WriteScoreMyP(myplayer);
        UiManager.instance.Player1Board.WriteScoreP1(player1);
        UiManager.instance.Player2Board.WriteScoreP2(player2);
    }

    public void StartnextRound()
    {
        if (Round == MaxRounds)
        {
            UiManager.instance.MyPlayerBoard.CalculateWinner();
        }
        else
        {
            UiManager.instance.BackButton.SetActive(true);
            //------shuffling and Distributing.
            Deck = Shuffle(Deck);
            distribute(Deck, myplayer, player1, player2);

            //------------Score ReAssigning---------------//
            myplayer.point = 0;
            player1.point = 0;
            player2.point = 0;
            UiManager.instance.UpdateScores(myplayer, player1, player2);

            //-----------------------------------------//
            no_of_Uthris = 0;

            //---------------------------------------//
            if (myplayer.Goal == 3)
            {
                myplayer.Goal = 5;
                player2.Goal = 2;
                player1.Goal = 3;
                UiManager.instance.OnSelectingTrump();
            }
            else if (myplayer.Goal == 2)
            {
                player1.Goal = 2;
                myplayer.Goal = 3;
                player2.Goal = 5;
                FaceupMyCards();
                MakeTrump(player2.Player2Cards);
                player2.throwcard();
            }
            else if (myplayer.Goal == 5)
            {
                player2.Goal = 3;
                player1.Goal = 5;
                myplayer.Goal = 2;
                FaceupMyCards();
                MakeTrump(player1.Player1Cards);
                player1.throwcard();
            }

        }

    }

    Sprite DecideBackground(int num)
    {
        if (num == 0) { return back_image1; }
        else if (num==1) { return back_image2; }
        else if (num == 2) { return back_image3; }
        else if (num == 3) { return back_image4; }
        else if(num == 4) { return back_image5; }
        else { return null; }
    }

}
