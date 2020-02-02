using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BrokenSpawnerManager : MonoBehaviour
{
    public static BrokenSpawnerManager Instance;

    //Simply has a list of all existing spawners, as well as returning them
    private static List<BrokenAreaSpawner>  spawners = new List<BrokenAreaSpawner>();

    [SerializeField] private List<BrokenAreaSpawner> serializedSpawners;

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

    public static BrokenAreaSpawner GetAreaSpawnerByIndex(int _index)
    {
        return spawners[_index];
    }

    public List<BrokenAreaSpawner> GrabAllBrokenAreaSpawners()
    {
        return serializedSpawners;
    }
}
