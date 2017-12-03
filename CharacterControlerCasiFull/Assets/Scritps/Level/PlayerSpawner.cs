using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    public GameObject gentlemanPlayer;
    public GameObject girlPlayer;
    public GameObject villanPlayer;
    public GameObject robotPlayer;

    void Start ()
    {
        spawnPlayers();
    }

    private void spawnPlayers()
    {
        Debug.Log(PlayerPrefs.GetInt("PlayerModel1"));

        for (int i = 1; i < PlayerPrefs.GetInt("PlayerAmount")+ 1; i++)
        {
            if (PlayerPrefs.GetInt("PlayerModel" + i) == 1)
            {
                Instantiate(gentlemanPlayer, new Vector3(Random.Range(-4,4), 1, 0), Quaternion.identity);
            }
            if (PlayerPrefs.GetInt("PlayerModel" + i) == 2)
            {
                Instantiate(girlPlayer, new Vector3(Random.Range(-4, 4), 1, 0), Quaternion.identity);
            }
            if (PlayerPrefs.GetInt("PlayerModel" + i) == 3)
            {
                Instantiate(villanPlayer, new Vector3(Random.Range(-4, 4), 1, 0), Quaternion.identity);
            }
            if (PlayerPrefs.GetInt("PlayerModel" + i) == 4)
            {
                Instantiate(robotPlayer, new Vector3(Random.Range(-4, 4), 1, 0), Quaternion.identity);
            }
        }
    }
	
	void Update ()
    {
		
	}
}
