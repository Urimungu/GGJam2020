using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardStruct {
    public string Name;
    public int Rank;
    public long PlayerScore;

    public LeaderBoardStruct()
    {
        Name = "N/A";
        Rank = 0;
        PlayerScore = 000000;
    }

    public LeaderBoardStruct(string name, int rank, long score)
    {
        Name = name;
        Rank = rank;
        PlayerScore = score;
    }
}
