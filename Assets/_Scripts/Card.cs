using System;
using System.Collections;
using UnityEngine;

public enum Whosecard
{
    none,
    mycard,
    player1card,
    player2card
}
public class Card : MonoBehaviour
{
    [Header("Card Properties"),SerializeField]
    public string symbol;
    public int value;
    public Whosecard whosecard;

    public Sprite front_image;
    public Sprite back_image;
    [Header("moving Properties")]
    [SerializeField] float MoveSpeed;
    [SerializeField] Vector2 mySloat;
    [SerializeField] Vector2  player1Sloat;
    [SerializeField] Vector2 player2Sloat;
    Vector2 target;

    //event called when the respective Card Reached to their sloat //
    public static event Action OnMyCardReach;
    public static event Action OnP1CardReach;
    public static event Action OnP2CardReach;


    private void OnMouseDown()
    {
        if (GameManager.instance.gamestate == GameState.play)
        {
            GameManager.instance.gamestate=GameState.wait;
            StartCoroutine(MoveCard());
        }
    }

    public IEnumerator MoveCard()
    {
        Chosetarget();
        this.FaceUp();
        while (this.transform.position != (Vector3)target)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, target, MoveSpeed * Time.deltaTime);
            yield return null;
        }
        GameManager.instance.ThrownCard.Add(this.gameObject);
        WhoseCardReached();
        StopCoroutine(this.MoveCard());
    }

    void WhoseCardReached()
    {
        if (GameManager.instance.ThrownCard.Count < 3)
        {
            if (this.whosecard == Whosecard.mycard) { OnMyCardReach?.Invoke(); }
            else if (this.whosecard == Whosecard.player1card) { OnP1CardReach?.Invoke(); }
            else if (this.whosecard == Whosecard.player2card) { OnP2CardReach?.Invoke(); }
        }
        else
        {
            // if third card is thrown then check the result.
            GameManager.instance.Checkresult();
        }
    }

    public void Chosetarget()
    {
        if (this.whosecard == Whosecard.mycard) { this.target = mySloat; }
        else if (this.whosecard == Whosecard.player1card) { this.target = player1Sloat; }
        else if (this.whosecard == Whosecard.player2card) { this.target = player2Sloat; }
    }

    public void FaceDown()
    {
        this.GetComponent<SpriteRenderer>().sprite = back_image;
    }
    public void FaceUp()
    {
        this.GetComponent<SpriteRenderer>().sprite = front_image;
    }


}
