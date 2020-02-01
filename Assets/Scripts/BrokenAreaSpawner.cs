using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
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
        SEVERE
    }

    [Header("Spawner Name"), SerializeField]
    private string spawnerName = "New Spawn Area";

    [Header("Spawning Range")]
    [Range(0.1f, 50f), SerializeField] private float spawningRangeX;
    [Range(0.1f, 50f), SerializeField] private float spawningRangeY;

    [Header("Broken Area Prefab"), SerializeField]
    private GameObject brokenAreaPrefab;

    [Header("Broken Area Count"), SerializeField]
    private List<BrokenArea> brokenAreaInstances = new List<BrokenArea>();

    [Header("Area Severity"), SerializeField]
    private SeverityState severityState;

    //IEnumerator coroutine variable
    private IEnumerator severityUpdate;

    //Max Limit of spawning
    private const uint maxInstanceLimit = 3;

    //Our transform, and Vector3
    //(UnityEngine.Vector3 was added because it conflicted with System.Numeric.Vector3)
    private Transform spawnerTransform;
    private Vector3 spawnerPosition;

    //Ray and RaycastHit to detect mouse
    Ray ray;
    RaycastHit hit;

    //IncrementRoutine
    private IEnumerator incrementRoutine;

    private void Awake()
    {
        spawnerTransform = GetComponent<Transform>();
        spawnerPosition = spawnerTransform.position;
    }

    // Start is called before the first frame update
    private void Start()
    {
        #region Severity Update Checking
        //Start severityUpdate checking
        severityUpdate = SeverityStateUpdate();
        incrementRoutine = IncrementRepairProgress();

        StartCoroutine(severityUpdate);
        StartCoroutine(incrementRoutine);
        #endregion
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateNewBrokenArea();
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
        while (true)
        {
            ChangeSeverityState(brokenAreaInstances.Count);
            
            yield return new WaitForEndOfFrame();
        }
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
    /// Increments the repair progress every frame.
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncrementRepairProgress()
    {
        while (true)
        {
            if (CheckMouseCollision() && hit.collider.tag == "BrokenArea")
            {
                BrokenArea effectedArea = hit.collider.gameObject.GetComponent<BrokenArea>();

                float increment = effectedArea.GetRepairIncrement();

                effectedArea.SetIsGettingFixed(Input.GetMouseButton(0));

                if (Input.GetMouseButton(0)) effectedArea.IncrementRepairProgressValue(increment);
            }

            yield return new WaitForEndOfFrame();
        }
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

    private void UpdateBAID()
    {
        uint baid = 0;

        for (int index = 0; index < brokenAreaInstances.Count; index++)
        {
            baid = (uint)index;
            brokenAreaInstances[index].SetBAID(baid);
        }
    }

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
        brokenAreaInstances.Remove(targetObj);
        Destroy(targetObj.gameObject);

        UpdateBAID();
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
    #endregion
}
