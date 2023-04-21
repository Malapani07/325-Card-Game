using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{

    [SerializeField]public GameObject ScoreText;
    [SerializeField] public Text P1Total;
    [SerializeField] public Text MyTotal;
    [SerializeField] public Text P2Total;

    [SerializeField] public GameObject WinPanel;
    [SerializeField] public GameObject LosePanel;
    [SerializeField] public GameObject DrawPanel;
    public void WriteScoreP1(Player1 p1)
    {
        var scoretext = Instantiate(ScoreText);
        scoretext.transform.SetParent(transform, false);
        scoretext.GetComponent<Text>().text = p1.point.ToString();
        P1Total.text = (int.Parse(P1Total.text) + p1.point).ToString();
    }
    public void WriteScoreP2(Player2 p2)
    {
        var scoretext = Instantiate(ScoreText);
        scoretext.transform.SetParent(transform, false);
        scoretext.GetComponent<Text>().text = p2.point.ToString();
        P2Total.text = (int.Parse(P2Total.text) + p2.point).ToString();
    }
    public void WriteScoreMyP(Myplayer MyP)
    {
        var scoretext = Instantiate(ScoreText);
        scoretext.transform.SetParent(transform, false);
        scoretext.GetComponent<Text>().text = MyP.point.ToString();
        MyTotal.text = (int.Parse(MyTotal.text) + MyP.point).ToString();
    }

    public void CalculateWinner()
    {
        if (int.Parse(MyTotal.text) > int.Parse(P1Total.text))
        {
            if (int.Parse(MyTotal.text) > int.Parse(P2Total.text))
            {           
                WinPanel.SetActive(true);
                SoundManager.instance.sounds[2].Play();
            }
            else if(int.Parse(MyTotal.text)== int.Parse(P2Total.text))
            {
                DrawPanel.SetActive(true);
                SoundManager.instance.sounds[4].Play();
            }
            else
            {
                LosePanel.SetActive(true);
                SoundManager.instance.sounds[3].Play();
            }
        }
        else if(int.Parse(MyTotal.text) == int.Parse(P1Total.text))
        {
            if (int.Parse(MyTotal.text) > int.Parse(P2Total.text))
            {
                DrawPanel.SetActive(true);
                SoundManager.instance.sounds[4].Play();
            }
            else
            {
                LosePanel.SetActive(true);
                SoundManager.instance.sounds[3].Play();
            }
        }
        else
        {         
            LosePanel.SetActive(true);
            SoundManager.instance.sounds[3].Play();
        }
    }

}
