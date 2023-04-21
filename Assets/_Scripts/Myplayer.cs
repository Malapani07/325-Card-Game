using System.Collections.Generic;
using UnityEngine;

public class Myplayer : MonoBehaviour
{
    [SerializeField] public List<GameObject> Mycards;
    public int Goal;
    public int point;
    public int loan;
    private int count;

    void Start()
    {  
        Card.OnP1CardReach += MyTurnStart;
    }

    public void RemoveEvent()
    {
        Card.OnP1CardReach -= MyTurnStart;
    }

    public  void MyTurnStart()
    {
        count = 0;
        GameManager.instance.gamestate = GameState.play;
        if (GameManager.instance.ThrownCard.Count == 0)
        {
            foreach (GameObject card in Mycards)
            {
                if (card != null)
                {
                    card.GetComponent<BoxCollider2D>().enabled = true;
                    card.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
        else if(GameManager.instance.ThrownCard.Count > 0)
        {
           
            foreach (GameObject card in Mycards)
            {
                if (card != null)
                {
                    if (card.CompareTag(GameManager.instance.ThrownCard[0].tag))
                    {
                        card.GetComponent<BoxCollider2D>().enabled = true;
                        card.GetComponent<SpriteRenderer>().color = Color.white;
                        count++;
                    }
                    else
                    {
                        card.GetComponent<BoxCollider2D>().enabled = false;
                        card.GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                }
            }
            if (count == 0)
            {
                foreach (GameObject card in Mycards)
                {
                    if (card != null)
                    {
                        card.GetComponent<BoxCollider2D>().enabled = true;
                        card.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }

        }
        else
        {
            foreach (GameObject card in Mycards)
            {
                if (card != null)
                {
                    card.GetComponent<BoxCollider2D>().enabled = true;
                    card.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    public int calculateLoan()
    {
        return point - Goal;
    }
}
