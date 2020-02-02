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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(BrokenAreaSpawner spawner in FindObjectsOfType<BrokenAreaSpawner>())
            spawners.Add(spawner);

        serializedSpawners = spawners;
    }

    public static BrokenAreaSpawner GetAreaSpawnerByIndex(int _index)
    {
        return spawners[_index];
    }
}
