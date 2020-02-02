using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;

public class BrokenArea : MonoBehaviour
{
    /*BrokenArea will take the name of the spawner.
     There will be a repairProgressValue.
     A boolean of isGettingFixed will be toggled if the player interacts with
     this brokenArea.
     */

    [Header("Broken Area Identifier (BAID)"), SerializeField]
    private uint baid;

    [Header("Progress"), Range(0f, 100f), SerializeField] private float repairProgressValue;

    [Header("Increment Value"), SerializeField] private float repairIncrementValue = 1f;

    [Header("Name of Spawner"), SerializeField] private string spawnerName;

    [Header("Severity"), SerializeField] private string severityState;

    [Header("Being fixed?"), SerializeField] private bool isGettingFixed;

    [Header("Interactable"), SerializeField]
    private bool isInteractable;

    //Reference to spawer so we can remove the object if repaired
    private BrokenAreaSpawner spawnerObj;

    //Coroutine variables
    private IEnumerator repairStatusEnumerator;
    private IEnumerator playerDistanceRoutine;

    //If area is fixed
    private bool isFixed = false;


    //Reference to player
    [SerializeField] private GameObject player;

    //Distance
    [SerializeField] private float gdistance;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Get the spawner that this object came from
        spawnerObj = GetSpawnerOrigin();
        spawnerName = spawnerObj.GetSpawnerName();

        //Set coroutine variables
        repairStatusEnumerator = RepairStatusUpdate();
        playerDistanceRoutine = CheckPlayerDistance();

        StartCoroutine(repairStatusEnumerator);
        StartCoroutine(playerDistanceRoutine);
    }


    /// <summary>
    /// Check if the brokenArea has been repaired
    /// </summary>
    private IEnumerator RepairStatusUpdate()
    {
        while (true)
        {
            //Get the severity
            severityState = spawnerObj.GetSeverityLevelAsString();

            if (repairProgressValue > 99f)
            {
                isFixed = true;
                Score.Instance.SendRepairScore();
                spawnerObj.RemoveBrokenArea((int)baid);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Continuously check on player distance.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPlayerDistance()
    {
        #region Wait Each Frame
        while (true)
        {
            #region Assure that player is not null
            while (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                yield return new WaitForEndOfFrame();
            } 
            #endregion

            Vector3 distance = Vector3.zero;

            //Calculate distance of player
            float dist = Vector3.Distance(transform.position, player.transform.position);

            var reparable = (spawnerObj.GetSeverityState() != BrokenAreaSpawner.SeverityState.IRREPARABLE);

            //Check if player is within reachable distance
            if (Mathf.Abs(dist) < spawnerObj.GetReachableDistance() && reparable)
                isInteractable = true;
            else
                isInteractable = false;

            yield return new WaitForEndOfFrame();
        }
        #endregion
    }

    #region Set Methods
    /// <summary>
    /// Set the ID of the broken Area spanwned
    /// </summary>
    /// <param name="_value"></param>
    public void SetBAID(uint _value)
    {
        baid = _value;
    }

    public void SetIsGettingFixed(bool _flag)
    {
        isGettingFixed = _flag;
    }

    public void IncrementRepairProgressValue(float _value)
    {
        repairProgressValue += _value;
    }
    #endregion

    #region Get Methods
    /// <summary>
    /// This will pass in the name of what part of the robot you are repairing.
    /// </summary>
    /// <returns></returns>
    public string GetGeneralArea()
    {
        return spawnerName;
    }

    /// <summary>
    /// Returns what spawner the brokenArea spawned from
    /// </summary>
    /// <returns></returns>
    public BrokenAreaSpawner GetSpawnerOrigin()
    {
        return GetComponentInParent<BrokenAreaSpawner>();
    }

    /// <summary>
    /// Returns the repairProgressValues into a value between 0 and 1
    /// </summary>
    public float GetRepairProgress(bool _inPercentage = false)
    {
        float value = 0;
        switch (_inPercentage)
        {
            case false:
                value = repairProgressValue;
                break;

            case true:
                value = (repairProgressValue / 100f);
                break;
        }

        return value;
    }

    public float GetRepairIncrement()
    {
        return repairIncrementValue;
    }

    public string GetSeverity()
    {
        return severityState;
    }

    public bool GetIsInteractable()
    {
        return isInteractable;
    }
    #endregion
}
