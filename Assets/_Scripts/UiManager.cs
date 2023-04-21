using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public Text Player1Score;
    public Text Player2Score;
    public Text MyPlayerScore;

    // Trump UI 
    public Image TrumpImage;
    public Sprite heart;
    public Sprite diamond;
    public Sprite club;
    public Sprite spade;
    public GameObject TrumpselectingPanel;

    //--- Score-Management-----//
    public ScoreBoard MyPlayerBoard;
    public ScoreBoard Player1Board;
    public ScoreBoard Player2Board;

    [SerializeField] public GameObject ScorePanel;
    [SerializeField] public GameObject BackButton;


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


    public void UpdateScores(Myplayer myp, Player1 p1, Player2 p2)
    {
        MyPlayerScore.text =myp.point.ToString() + "/" + myp.Goal.ToString();
        Player1Score.text=p1.point.ToString() + "/" + p1.Goal.ToString();
        Player2Score.text=p2.point.ToString() + "/" + p2.Goal.ToString();
    }

    public void UpdateTrumpImage()
    {
        if (GameManager.instance.trump == "heart")
        {
            TrumpImage.sprite = heart;
        }
        else if (GameManager.instance.trump == "spade")
        {
            Debug.Log("a");
            TrumpImage.sprite = spade;
        }     
        else if(GameManager.instance.trump == "diamond")
        {
            TrumpImage.sprite = diamond;
        }
        else if (GameManager.instance.trump == "club")
        {
            TrumpImage.sprite = club;
        }
      
    }


    public void MakeTrumpHeart()
    {
        GameManager.instance.trump = "heart";
        UpdateTrumpImage();
        GameManager.instance.gamestate = GameState.play;
        GameManager.instance.FaceupMyCards();
    }
    public void MakeTrumpSpade()
    {
        GameManager.instance.trump = "spade";
        UpdateTrumpImage();
        GameManager.instance.gamestate = GameState.play;
        GameManager.instance.FaceupMyCards();
    }
    public void MakeTrumpDiamond()
    {
        GameManager.instance.trump = "diamond";
        UpdateTrumpImage();
        GameManager.instance.gamestate = GameState.play;
        GameManager.instance.FaceupMyCards();
    }
    public void MakeTrumpClub()
    {
        GameManager.instance.trump = "club";
        UpdateTrumpImage();
        GameManager.instance.gamestate = GameState.play;
        GameManager.instance.FaceupMyCards();
    }

    public void OnSelectingTrump()
    {
        TrumpselectingPanel.SetActive(true);
    }

    public void Next()
    {
       GameManager.instance.StartnextRound();
    }

    public void Menu()
    {
        GameManager.instance.player1.RemoveEvent();
        GameManager.instance.player2.RemoveEvent();
        GameManager.instance.myplayer.RemoveEvent();
        SceneManager.LoadScene("Menu");
    }
}
