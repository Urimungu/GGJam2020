using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;

public class Score : MonoBehaviour
{
    public static Score Instance;
    /*This is the score system. This will have variables that will be responsible to the scoring of different things
    such as killing an enemy, repairing a broken area, etc.*/

    [Header("Current Score"), SerializeField]
    private long currentScore;

    [Header("Highscore")]
    [SerializeField] private long currentHighScore;
    [SerializeField] private List<long> highScores = new List<long>();

    [Header("Increment Value"), SerializeField]
    private int incrementValue = 10; //Default

    [Header("Increment Interval"), SerializeField]
    private float intervalValue = 0.1f; //1 / 10th of a second

    [Header("Repair Points"), SerializeField]
    private int repairPoints = 1000;

    [Header("Enemy Points"), SerializeField]
    private int enemyPoints = 500;

    [Header("Multiplier"), SerializeField] private int muliplier = 1;

    //Score System Coroutine.
    private IEnumerator scoreSystemRoutine;
    private IEnumerator incrementorRoutine;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Coroutines
        scoreSystemRoutine = RunScoreSystem();
        incrementorRoutine = Incrementor(intervalValue);

        StartCoroutine(scoreSystemRoutine);
        StartCoroutine(incrementorRoutine); 
        #endregion
    }

    private IEnumerator RunScoreSystem()
    {
        while (true)
        {
            //Make sure high score updates if current score surpasses
            if (currentScore > currentHighScore)
                currentHighScore = currentScore;

            UIManager.Instance.SetScoreText(currentScore.ToString("0000000", CultureInfo.InvariantCulture));
            UIManager.Instance.SetHighScoreText(currentHighScore.ToString("0000000", CultureInfo.InvariantCulture));
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Incrementor(float _value)
    {
        while (true)
        {
            currentScore += incrementValue * muliplier;
            yield return new WaitForSeconds(_value);
        }
    }

    #region Send Methods
    public void SendEnemyScore()
    {
        currentScore += enemyPoints * muliplier;
    }

    public void SendRepairScore()
    {
        currentScore += repairPoints * muliplier;
    }

    #endregion

    #region Get Methods
    public long GetCurrentScore()
    {
        return currentScore;
    }

    public long GetCurrentHighScore()
    {
        return currentHighScore;
    }

    public List<long> GetHighScoreList()
    {
        return highScores;
    } 
    #endregion
}
