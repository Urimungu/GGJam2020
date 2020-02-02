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
    [SerializeField] private Text T_SCORE;
    [SerializeField] private Text T_TIMER;
    [SerializeField] private Text T_HIGHSCORE;
    [SerializeField] private Slider S_PROGRESSIONBAR;


    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
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
        TMP_AREA.text = _value;
    }

    public void SetProgressionInfo(float _value = 0f)
    {
        S_PROGRESSIONBAR.value = _value;
    }

    public void SetAlertText(string _value = "")
    {
        TMP_ALERT.text = _value;
    }

    public void SetScoreText(string _value)
    {
        T_SCORE.text = "Score:" + _value;
    }

    public void SetTimerText(string _value)
    {
        T_TIMER.text = "Timer:\n" + _value;
    }

    public void SetTimerText(float _minutes, float _seconds)
    {
        T_TIMER.text = "Timer:\n" + _minutes + ":" + _seconds.ToString("00.00", CultureInfo.InvariantCulture);
    }

    public void SetHighScoreText(string _value)
    {
        T_HIGHSCORE.text = "Highscore:" + _value;
    }
    #endregion

}
