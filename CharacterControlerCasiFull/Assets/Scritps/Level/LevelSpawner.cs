using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {

    public List<Module> spawnModules;
    public float[] spawners;
    public float levelHeight;
    public Module endModule;

	void Start () {
        spawners = new float[2] { transform.position.y, transform.position.y };       
        SpawnLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //0 left spawner , 1 right spawner the match the spawners list
    void setModule( int aPos)
    {
        int randomModule = Random.Range(0, spawnModules.Count);
        Vector3 positionToSpawn = new Vector3(transform.position.x, spawners[aPos]);
        Module newModule = Instantiate(spawnModules[randomModule],positionToSpawn,Quaternion.identity,transform);
        if (aPos == 1)
            newModule.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        spawners[aPos] += newModule.getHeight();
    }

    void SpawnLevel()
    {
        for (int i = 0; i < 2; i++)
        {
            while (spawners[i] < levelHeight)
            {
                setModule(i);
            }
        }
        float wheretoSpawnEnd = spawners[0] > spawners[1] ? spawners[0] : spawners[1];
        Vector3 spawnEnd = new Vector3(0, wheretoSpawnEnd, 0);
        Module newModule = Instantiate(endModule, spawnEnd, Quaternion.identity, transform);
    }
}
