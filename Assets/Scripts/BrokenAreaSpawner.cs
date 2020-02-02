using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class BrokenAreaSpawner : MonoBehaviour
{
    public static BrokenAreaSpawner Instance;

    /*BrokenAreaSpawner will be responsible for the actual spawning of areas that needs
     to be repaired.
     
    A spawnRange variable can be set, and this will be the value in which the brokenArea will spawn away
    from the actual positioning of the spawner.
    
    The spawner will keep track of how many difference brokenArea there are, however the limit is 3.
    
    A BrokenAreaManager will be the one factor of notifying for what spawner, or what area, needs to be fixed
    in general. Depending on the amount spawned relative to the spawner's position will state the severity of that area.
    
    One brokenArea being mild, two being bad, three being severe.*/

    public enum SeverityState
    {
        NONE,
        MILD,
        HIGH,
        SEVERE,
        IRREPARABLE
    }

    //Alert
    public string[] alert =
    {
        "",
        "You're too far to fix this area.",
        "This area is irreparable.",
        "Broken area has been repaired!!!"
    };

    [Header("Spawner Name"), SerializeField]
    private string spawnerName = "New Spawn Area";

    [Header("Spawning Range")]
    [Range(0.1f, 50f), SerializeField]
    private float spawningRangeX;

    [Range(0.1f, 50f), SerializeField] private float spawningRangeY;

    [Header("Broken Area Prefab"), SerializeField]
    private GameObject brokenAreaPrefab;

    [Header("Broken Area Count"), SerializeField]
    private List<BrokenArea> brokenAreaInstances = new List<BrokenArea>();

    [Header("Area Severity"), SerializeField]
    private SeverityState severityState;

    [Header("Reachable Distance")]
    [Range(0.1f, 10f), SerializeField]
    private float reachableDistance;

    //If the player is with fixable distance, this will be true
    [SerializeField] private bool interactable = false;

    //IEnumerator coroutine variables
    private IEnumerator severityUpdateRoutine;
    private IEnumerator incrementRoutine;

    private IEnumerator maxDamageRoutine;

    //Max Limit of spawning
    private const uint maxInstanceLimit = 4;

    //Our transform, and Vector3
    //(UnityEngine.Vector3 was added because it conflicted with System.Numeric.Vector3)
    private Transform spawnerTransform;
    private Vector3 spawnerPosition;

    //Ray and RaycastHit to detect mouse
    private Ray ray;
    private RaycastHit hit;

    private bool isRepairing;

    //Damage variable. This will reset once it hits 100, but it'll spawn a BrokenArea
    private int areaDamage = 0;

    //Constant for 0
    private const uint reset = 0;

    //Constant for maxDamage
    private const uint maxDamage = 100;

    private void Awake()
    {
        spawnerTransform = GetComponent<Transform>();
        spawnerPosition = spawnerTransform.position;
    }

    // Start is called before the first frame update
    private void Start()
    {
        #region Coroutines
        //Start severityUpdate checking
        severityUpdateRoutine = SeverityStateUpdate();
        incrementRoutine = IncrementRepairProgress();
        maxDamageRoutine = CheckMaxDamage();

        StartCoroutine(severityUpdateRoutine);
        StartCoroutine(incrementRoutine);
        StartCoroutine(maxDamageRoutine);
        #endregion
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            BrokenSpawnerManager.GetAreaSpawnerByIndex(0).SetDamage(10);
    }

    /// <summary>
    /// Promptly change the state of severity of an area.
    /// </summary>
    /// <param name="_value">0 being no severity, 3 being severe</param>
    private void ChangeSeverityState(int _value)
    {
        //Change the severity of an area.
        severityState = (SeverityState)_value;
    }

    /// <summary>
    /// Continuously check for severity update
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeverityStateUpdate()
    {
        #region Wait Each Frame
        while (true)
        {
            ChangeSeverityState(brokenAreaInstances.Count);
            yield return new WaitForEndOfFrame();
        }
        #endregion
    }

    /// <summary>
    /// Increments the repair progress every frame.
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncrementRepairProgress()
    {
        if (severityState != SeverityState.IRREPARABLE)
        {
            #region Wait Each Frame

            while (true)
            {
                if (CheckMouseCollision() && hit.collider.tag == "BrokenArea")
                {
                    BrokenArea effectedArea = hit.collider.gameObject.GetComponent<BrokenArea>();

                    float increment = effectedArea.GetRepairIncrement();

                    effectedArea.SetIsGettingFixed(Input.GetMouseButton(0));
                    UpdateIsRepairing(Input.GetMouseButton(0));
                    effectedArea.SetIsGettingFixed(Input.GetMouseButton(0));


                    //Update textUI
                    UIManager.Instance.SetAreaTextInfo(effectedArea.GetGeneralArea() + " (" +
                                                       effectedArea.GetSeverity() +
                                                       ")");
                    UIManager.Instance.SetProgressionInfo(effectedArea.GetRepairProgress(true));

                    if (Input.GetMouseButton(0))
                    {
                        switch (effectedArea.GetIsInteractable())
                        {
                            case false:
                                effectedArea.GetSpawnerOrigin().SendAlert(1);
                                break;
                            case true:
                                effectedArea.IncrementRepairProgressValue(increment);
                                effectedArea.GetSpawnerOrigin().SendAlert(reset);
                                break;
                        }
                    }
                    else
                    {
                        UpdateIsRepairing(false);
                        UIManager.Instance.SetAreaTextInfo("???");
                        UIManager.Instance.SetProgressionInfo((float)reset);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            #endregion
        }
    }

    /// <summary>
    /// Handles Changes to the IsRepairing State
    /// </summary>
    /// <returns></returns>
    private void UpdateIsRepairing(bool newState)
    {
        var changed = isRepairing != newState;
        if (changed)
            Message.Publish(newState ? (object)new RepairStarted() : new RepairStopped());
        isRepairing = newState;
    }

    /// <summary>
    /// Continuously check if max damage
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckMaxDamage()
    {
        #region Wait Each Frame
        while (true)
        {
            if (areaDamage >= maxDamage)
            {
                areaDamage = (int)reset;
                GenerateNewBrokenArea();
                LoseCondition.Instance.SendSignalToUpdateCounter();
            }

            yield return new WaitForEndOfFrame();
        }
        #endregion
    }

    /// <summary>
    /// Check if mouse is hovering over object.
    /// </summary>
    /// <returns></returns>
    private bool CheckMouseCollision()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit);
    }

    /// <summary>
    /// Clear brokenAreaInstance list
    /// </summary>
    private void ClearList()
    {
        brokenAreaInstances.Clear();
    }

    /// <summary>
    /// Refresh our instance list.
    /// </summary>
    private void InstanceListRefresh()
    {
        //We will check for child objects in parent
        BrokenArea[] childBrokenArea = GetComponentsInChildren<BrokenArea>();

        //We'll loop through our array, and add them to our list.
        foreach (BrokenArea area in childBrokenArea)
            brokenAreaInstances.Add(area);

        UpdateBAID();
    }

    /// <summary>
    /// Updates the ID of a broken area. This is very important when it comes to refreshing our list.
    /// </summary>
    private void UpdateBAID()
    {
        uint baid = 0;

        for (int index = 0; index < brokenAreaInstances.Count; index++)
        {
            baid = (uint)index;
            brokenAreaInstances[index].SetBAID(baid);
        }
    }

    /// <summary>
    /// Generate a random location relative to spawner
    /// </summary>
    private void RandomizeAndSpawnBrokenArea()
    {
        //Generate X and Y values
        float generatedXPos = Random.Range(-GetSpawningRangeX(), GetSpawningRangeX());
        float generatedYPos = Random.Range(-GetSpawningRangeY(), GetSpawningRangeY());

        //Create a new vector3
        Vector3 xAxis = spawnerTransform.right * generatedXPos;
        Vector3 yAxis = spawnerTransform.up * generatedYPos;

        Vector3 brokenAreaPosition = xAxis + yAxis;

        //Now instantiate our brokenArea
        GameObject newBrokenArea = Instantiate(brokenAreaPrefab.gameObject, spawnerTransform);
        newBrokenArea.transform.position += brokenAreaPosition;
    }

    /// <summary>
    /// Send an alert!
    /// </summary>
    /// <param name="_index"></param>
    private void SendAlert(uint _index)
    {
        UIManager.Instance.SetAlertText(alert[_index]);
    }

    /// <summary>
    /// Generate a new area that needs to be fixed by the player.
    /// </summary>
    public void GenerateNewBrokenArea()
    {
        /*With the spawnRange variable, I only want
         the X and Y axis (I think. Kind of trying to visualize).
         
         Two variables will generate a random number between 1 and the spawnRange.
         Those two variables will be generatedXPos and generatedYPos.
         
         After that, we create our brokenArea prefab, as well as add it to our brokenAreaInstances list.*/

        //This will be a test;
        if (brokenAreaInstances.Count < maxInstanceLimit)
        {
            ClearList();

            //Now we actually spawn our brokenAreas this time!!!
            RandomizeAndSpawnBrokenArea();

            //Refresh list
            InstanceListRefresh();

            #region Send Signal to Group
            try
            {
                BrokenAreaGroup hi = GetComponentInParent<BrokenAreaGroup>();
                if (hi != null)
                    hi.SignalCountUpdate();
            }
            catch
            {
                throw new AbandonedMutexException();
            }
            #endregion
        }
    }

    /// <summary>
    /// If something got repair, pass in that value relative to the spawner.
    /// </summary>
    /// <param name="_index"></param>
    public void RemoveBrokenArea(int _index)
    {
        BrokenArea targetObj = brokenAreaInstances[_index];

        //Destroy and remove object from list
        Message.Publish(new RepairCompleted());
        brokenAreaInstances.Remove(targetObj);
        Destroy(targetObj.gameObject);

        UpdateBAID();

        SendAlert(3);
    }

    public void SetDamage(int _value)
    {
        areaDamage += _value;
    }

    #region Get Methods

    public string GetSpawnerName()
    {
        return spawnerName;
    }

    public float GetSpawningRangeX()
    {
        return spawningRangeX;
    }
    public float GetSpawningRangeY()
    {
        return spawningRangeY;
    }

    public string GetSeverityLevelAsString()
    {
        return Enum.GetName(typeof(SeverityState), (int)severityState);
    }

    public SeverityState GetSeverityState()
    {
        return severityState;
    }

    public float GetReachableDistance()
    {
        return reachableDistance;
    }
    #endregion
}
