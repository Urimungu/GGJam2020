using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LeaderBoard : MonoBehaviour
{
    [Header("Leader Boardnames")]
    public List<TextMeshProUGUI> leaderBoardScores = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> leaderBoardNames = new List<TextMeshProUGUI>();

    private List<long> topScores = new List<long>();
    private List<string> names = new List<string>();

    private int totalScores;
    public string name;
    public TextMeshProUGUI iField;

    void Start()
    {
        if (Score.Instance.GetHighScoreList().Count <= 10)
            totalScores = Score.Instance.GetHighScoreList().Count;
        else if (Score.Instance.GetHighScoreList().Count > 10)
            totalScores = 10;
        else
            print("There is no score");

        for(int i = 0; i < totalScores; i++)
        {
            topScores[i] = Score.Instance.GetHighScoreList()[i];
            print(topScores[i]);
        }
    }

    private void Update()
    {
        name = iField.text;

        if (name == "")
            leaderBoardNames[0].text = "N/A";
        else
            leaderBoardNames[0].text = name;
    }

    private void OnApplicationQuit()
    {
        for(int i = 0; i < totalScores; i++)
        {
            names[i] = leaderBoardScores[i].text;
        }
    }
}
