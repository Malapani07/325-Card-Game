using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public List<GameObject> Player1Cards;
    public int Goal;
    public int point;
    public int loan;

    public List<GameObject> selectedcards;

    // Start is called before the first frame update
    void Start()
    {
        Card.OnP2CardReach += throwcard;
    }

    public void RemoveEvent()
    {
        Card.OnP2CardReach -= throwcard;
    }

    public int calculateLoan()
    {
        return point - Goal;
    }

    public void throwcard()
    {
        if (GameManager.instance.no_of_Uthris < 10)
        {
            if (GameManager.instance.ThrownCard.Count == 0)
            {
                GameObject card = Select1Card();
                throwCardlogic(card);
                Player1Cards.Remove(card);
            }
            else if (GameManager.instance.ThrownCard.Count == 1)
            {
                GameObject card = TagChecker2throw();
                throwCardlogic(card);
                Player1Cards.Remove(card);
                selectedcards.Clear();
            }
            else if (GameManager.instance.ThrownCard.Count == 2)
            {
                GameObject card = TagChecker3throw();
                throwCardlogic(card);
                Player1Cards.Remove(card);
                selectedcards.Clear();
            }
        }
    }


    //------------------------first throw--------------------//

    GameObject Select1Card()
    {
        int a =Random.Range(0, Player1Cards.Count);
        return Player1Cards[a];
    }

    //later may be change name
    void throwCardlogic(GameObject card)
    {
        StartCoroutine(card.GetComponent<Card>().MoveCard());
        Player1Cards.Remove(card);
    }


    //.......................2nd Card throw functions................................//

    GameObject TagChecker2throw()
    {
        string trump = GameManager.instance.trump;
        var thrownCard0 = GameManager.instance.ThrownCard[0].GetComponent<Card>().symbol;
        for (int i = 0; i < Player1Cards.Count; i++)
        {
            if (Player1Cards[i] != null && Player1Cards[i].CompareTag(thrownCard0))
            {
                selectedcards.Add(Player1Cards[i]);
            }
        }
        if (selectedcards.Count > 0)
        {
            return having_sameCard_as_0ThrownCard(sort(selectedcards));
        }


        if (selectedcards.Count <= 0)
        {

            for (int i = 0; i < Player1Cards.Count; i++)
            {
                if (Player1Cards[i] != null && Player1Cards[i].CompareTag(trump))
                {
                    selectedcards.Add(Player1Cards[i]);
                }
            }
            if (selectedcards.Count > 0)
            {
                selectedcards = sort(selectedcards);
                return selectedcards[0];
            }
        }

        if (selectedcards.Count <= 0)
        {
            for (int i = 0; i < Player1Cards.Count; i++)
            {
                if (Player1Cards[i] != null)
                {
                    selectedcards.Add(Player1Cards[i]);
                }
            }
            selectedcards = sort(selectedcards);
            return selectedcards[0];
        }

        return null;
    }

    // NEED IMPROVEMENT //

    GameObject having_sameCard_as_0ThrownCard(List<GameObject> SelectedCards)
    {
        if (SelectedCards[SelectedCards.Count - 1].GetComponent<Card>().value < GameManager.instance.ThrownCard[0].GetComponent<Card>().value)
        {
            return SelectedCards[0];
        }

        return SelectedCards[SelectedCards.Count - 1];
    }


    //...................3rd Card throws functions....................................//
    GameObject TagChecker3throw()
    {


        for (int i = 0; i < Player1Cards.Count; i++)
        {
            if (Player1Cards[i] != null && Player1Cards[i].CompareTag(GameManager.instance.ThrownCard[0].tag))
            {
                selectedcards.Add(Player1Cards[i]);
            }
        }

        if (selectedcards.Count > 0)
        {
            return if_having_sameCard_as_0ThrownCard(sort(selectedcards));
        }

        if (selectedcards.Count <= 0)
        {
            for (int i = 0; i < Player1Cards.Count; i++)
            {
                if (Player1Cards[i] != null && Player1Cards[i].CompareTag(GameManager.instance.trump))
                {
                    selectedcards.Add(Player1Cards[i]);
                }
            }
            if (selectedcards.Count > 0)
            {
                return not_having_sameCard_as_0ThrownCard(sort(selectedcards));
            }
        }


        if (selectedcards.Count <= 0)
        {
            for (int i = 0; i < Player1Cards.Count; i++)
            {
                if (Player1Cards[i] != null)
                {
                    selectedcards.Add(Player1Cards[i]);
                }
            }
            selectedcards = sort(selectedcards);
            return selectedcards[0];
        }
        return null;
    }

    GameObject if_having_sameCard_as_0ThrownCard(List<GameObject> SelectedCards)
    {
        if (GameManager.instance.ThrownCard[1].GetComponent<Card>().symbol != GameManager.instance.trump && SelectedCards.Count > 0)
        {
            if (GameManager.instance.ThrownCard[1].GetComponent<Card>().value > GameManager.instance.ThrownCard[0].GetComponent<Card>().value)
            {
                for (int a = 0; a < SelectedCards.Count; a++)
                {
                    if (SelectedCards[a].GetComponent<Card>().value > GameManager.instance.ThrownCard[1].GetComponent<Card>().value)
                    {

                        return SelectedCards[a];
                    }
                }
                return SelectedCards[0];
            }
            else
            {
                for (int b = 0; b < SelectedCards.Count; b++)
                {
                    if (SelectedCards[b].GetComponent<Card>().value > GameManager.instance.ThrownCard[0].GetComponent<Card>().value)
                    {
                        return SelectedCards[b];
                    }

                }
                return SelectedCards[0];
            }
        }

        else if (GameManager.instance.ThrownCard[1].CompareTag(GameManager.instance.trump) && SelectedCards.Count > 0)
        {
            return SelectedCards[0];
        }
        return null;
        // Debug.Log("MISTAKE PLAYER1 LINE 117 IN RETURN STATEMENT CONDITION");
    }

    GameObject not_having_sameCard_as_0ThrownCard(List<GameObject> SelectedCards)
    {
        for (int a = 0; a < SelectedCards.Count; a++)
        {
            if (SelectedCards[a].GetComponent<Card>().value > GameManager.instance.ThrownCard[1].GetComponent<Card>().value)
            {
                return SelectedCards[a];
            }
        }

        for (int i = 0; i < Player1Cards.Count; i++)
        {
            if (Player1Cards[i] != null)
            {
                selectedcards.Add(Player1Cards[i]);
            }
        }
        selectedcards = sort(selectedcards);
        return selectedcards[0];
    }



    //................SORTING FUNCTION..........................//
    List<GameObject> sort(List<GameObject> SelectedCards)
    {
        GameObject temp;
        int length = SelectedCards.Count - 1;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (SelectedCards[j].GetComponent<Card>().value > SelectedCards[j + 1].GetComponent<Card>().value)
                {
                    temp = SelectedCards[j];
                    SelectedCards[j] = SelectedCards[j + 1];
                    SelectedCards[j + 1] = temp;
                }
            }

        }

        return SelectedCards;
    }

}
