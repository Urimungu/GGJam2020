using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BrokenSpawnerManager : MonoBehaviour
{
    public static BrokenSpawnerManager Instance;

    public BrokenAreaGroup brokenAreaGroup;

    //Simply has a list of all existing spawners, as well as returning them
    private static List<BrokenAreaSpawner>  spawners = new List<BrokenAreaSpawner>();

    [SerializeField] private List<BrokenAreaSpawner> serializedSpawners;

    private IEnumerator checkNoHealthRoutine;

    void Awake()
    {
        Instance = this;
        
        #region Find all Broken Area Spawners
        foreach (BrokenAreaSpawner spawner in FindObjectsOfType<BrokenAreaSpawner>())
            spawners.Add(spawner);

        serializedSpawners = new List<BrokenAreaSpawner>();

        serializedSpawners = spawners; 
        #endregion
    }

    void Start()
    {
        checkNoHealthRoutine = CheckNoHealth();
        StartCoroutine(checkNoHealthRoutine);
    }

    public static BrokenAreaSpawner GetAreaSpawnerByIndex(int _index)
    {
        return spawners[_index];
    }

    public List<BrokenAreaSpawner> GrabAllBrokenAreaSpawners()
    {
        return serializedSpawners;
    }

    private IEnumerator CheckNoHealth()
    {
        while (true)
        {
            if(brokenAreaGroup.GetHealth() <= 0)
                GameManager.Manager.LoseSinglePlayer();

            yield return new WaitForEndOfFrame();
        }
    }
}
