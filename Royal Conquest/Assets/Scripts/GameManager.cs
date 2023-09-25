using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeToSpawnWave = 20.0f;
    [SerializeField] float timeSinceLastWave = 0.0f;
    [SerializeField] GameObject minionPrefabBlue;
    [SerializeField] GameObject minionArcherPrefabBlue;
    [SerializeField] GameObject minionPrefabRed;
    [SerializeField] GameObject minionArcherPrefabRed;
    [SerializeField] GameObject[] spawnersMeleeBlue;
    [SerializeField] GameObject[] spawnersRangedBlue;
    [SerializeField] GameObject[] spawnersMeleeRed;
    [SerializeField] GameObject[] spawnersRangedRed;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        timeSinceLastWave += Time.deltaTime;
        if(timeSinceLastWave >= timeToSpawnWave)
            SpawnWave();
    }

    void SpawnWave()
    {
        timeSinceLastWave = 0.0f;
        foreach(GameObject location in spawnersMeleeBlue)
        {
            Instantiate(minionPrefabBlue, location.transform);
        }
        foreach(GameObject location in spawnersRangedBlue)
        {
            Instantiate(minionArcherPrefabBlue, location.transform);
        }
        foreach(GameObject location in spawnersMeleeRed)
        {
            Instantiate(minionPrefabRed, location.transform);
        }
        foreach(GameObject location in spawnersRangedRed)
        {
            Instantiate(minionArcherPrefabRed, location.transform);
        }
    }

    public void WinConditionMet(string winner)
    {
        foreach(Entity e in FindObjectsOfType<Entity>())
        {
            e.setEntityInControl(false);
        }
        if(winner == "Red")
            Debug.Log("Red team won!");
        if(winner == "Blue")
            Debug.Log("Blue team won!");
    }
}
