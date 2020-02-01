using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Increment Value"), SerializeField] private float repairIncrementValue = 0.1f;

    [Header("Name of Spawner"), SerializeField] private string spawnerName;

    [Header("Being fixed?"), SerializeField] private bool isGettingFixed;

    //Reference to spawer so we can remove the object if repaired
    private BrokenAreaSpawner spawnerObj;

    //Coroutine variables
    private IEnumerator incrementRoutine;

    //If area is fixed
    private bool isFixed = false;

    // Start is called before the first frame update
    private void Start()
    {
        //Get the spawner that this object came from
        spawnerObj = GetSpawnerOrigin();
        spawnerName = spawnerObj.GetSpawnerName();

        //Set coroutine variable
        incrementRoutine = IncrementRepairProgress();
        StartCoroutine(incrementRoutine);
    }

    // Update is called once per frame
    private void Update()
    {
        RepairStatusUpdate();
    }

    private BrokenAreaSpawner GetSpawnerOrigin()
    {
        return GetComponentInParent<BrokenAreaSpawner>();
    }

    private IEnumerator IncrementRepairProgress()
    {
        while (isGettingFixed)
        {
            repairProgressValue += repairIncrementValue;
            yield return new WaitForEndOfFrame();

            if (isGettingFixed == false) break;
        }
    }

    private void RepairStatusUpdate()
    {
        if (repairProgressValue > 0.99f)
        {
            isFixed = true;
            spawnerObj.RemoveBrokenArea((int)baid);
        }
    }

    #region Set Methods
    public void SetBAID(uint _value)
    {
        baid = _value;
    }
    #endregion
}
