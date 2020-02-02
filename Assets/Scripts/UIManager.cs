using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.XR.WSA.WebCam;

public class UIManager : MonoBehaviour
{
    //I will need any instance of BrokenAreaSpawner to use this function so that I can modify the UI
    public static UIManager Instance;

    //Now I have to reference my UI, which is just the Slider and the TMP
    [Header("UI Assets")]
    [SerializeField] private TextMeshProUGUI TMP_AREA;
    [SerializeField] private TextMeshProUGUI TMP_ALERT;
    [SerializeField] private TextMeshProUGUI TMP_HEALTHPERCENTAGE;
    [SerializeField] private Text T_SCORE;
    [SerializeField] private Text T_TIMER;
    [SerializeField] private Text T_HIGHSCORE;
    [SerializeField] private Slider S_PROGRESSIONBAR;
    [SerializeField] private Slider S_ROBOTHEALTH;


    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There was another instance of " + gameObject.name + ", so it was deleted. You may experience issues.");
            Destroy(gameObject);
        } 
        #endregion
    }

    #region Set Methods
    public void SetAreaTextInfo(string _value = "???")
    {
        if (TMP_AREA != null) TMP_AREA.text = _value;
    }

    public void SetProgressionInfo(float _value = 0f)
    {
        if (S_PROGRESSIONBAR != null) S_PROGRESSIONBAR.value = _value;
    }

    public void SetRobotHealthInfo(float _value = 0f)
    {
        if (S_ROBOTHEALTH != null) S_ROBOTHEALTH.value = _value;
    }

    public void SetHealthPercentageInfo(float _value = 0f)
    {
        if (TMP_HEALTHPERCENTAGE != null) TMP_HEALTHPERCENTAGE.text = _value + "%";
    }

    public void SetAlertText(string _value = "")
    {
        if (TMP_ALERT != null) TMP_ALERT.text = _value;
    }

    public void SetScoreText(string _value)
    {
        if (T_SCORE != null) T_SCORE.text = "Score:" + _value;
    }

    public void SetTimerText(string _value)
    {
        if (T_TIMER != null) T_TIMER.text = "Timer:\n" + _value;
    }

    public void SetTimerText(float _minutes, float _seconds)
    {
        if (T_TIMER != null) T_TIMER.text = "Timer:\n" + _minutes + ":" + _seconds.ToString("00.00", CultureInfo.InvariantCulture);
    }

    public void SetHighScoreText(string _value)
    {
        if (T_HIGHSCORE != null) T_HIGHSCORE.text = "Highscore:" + _value;
    }
    #endregion

}
