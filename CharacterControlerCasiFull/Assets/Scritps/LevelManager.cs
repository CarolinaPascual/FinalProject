using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    List<PlayerControler> playerList;
    public int firstPoints, secondPoints, thirdPoints, fourthPoints;
    public LevelSpawner _levelSpawner;
    #region Singleton stuff
    private static LevelManager _inst;
    public static LevelManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("LevelManager").AddComponent<LevelManager>();
            return _inst;
        }
    }
    #endregion
    void Awake()
    {
        init();
        playerList = new List<PlayerControler>();
    }
    private void init()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _inst = this;

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addToList(PlayerControler aPlayer)
    {
        playerList.Add(aPlayer);
    }

    public int getPlayerNumber(PlayerControler aPlayer)
    {
        return playerList.IndexOf(aPlayer);
    }
}
