using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.WSA.WebCam;

public class UIManager : MonoBehaviour
{
    //I will need any instance of BrokenAreaSpawner to use this function so that I can modify the UI
    public static UIManager Instance;

    //Now I have to reference my UI, which is just the Slider and the TMP
    [Header("UI Assets")]
    [SerializeField] private TextMeshProUGUI TMP_AREA;
    [SerializeField] private TextMeshProUGUI TMP_ALERT;
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
    #endregion

}
