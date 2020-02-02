using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderBoard : MonoBehaviour
{
    [Header("Leader Boardnames")]
    public List<TextMeshProUGUI> leaderBoardNames = new List<TextMeshProUGUI>();
    private long topScore;

    void Start()
    {
        topScore = Score.Instance.GetCurrentScore;
    }
}
