using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    List<PlayerControler> playerList;
    public int[] endScores;    
    public int playersEnded;
    public LevelSpawner _levelSpawner;
    bool matchEnd = false;
    List<PlayerControler> playersResult;
   public List<Text> textScoresList;
    public Text winner;
    public GameObject playAgainBtn, exitBtn,emptyUI;
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
        textScoresList[playerList.Count - 1].gameObject.SetActive(true);
        aPlayer.setTextScore(textScoresList[playerList.Count - 1]);
    }

    public int getPlayerNumber(PlayerControler aPlayer)
    {
        return playerList.IndexOf(aPlayer);
    }

    public void endPlayer(PlayerControler aPlayer)
    {
        aPlayer.addScore(endScores[playersEnded]);
        playersEnded++;
        if (playersEnded == playerList.Count)
        {
            endMatch();
        }
    }

    private void endMatch()
    {
        matchEnd = true;
        playersResult = playerList;
        List<PlayerControler> SortedList = playersResult.OrderBy(o => o.getScore()).ToList();
        winner.text = "Winner - Player " + (getPlayerNumber(SortedList[SortedList.Count-1]) +1);
        winner.gameObject.SetActive(true);
        playAgainBtn.SetActive(true);
        exitBtn.SetActive(true);
        emptyUI.SetActive(true);

    }
}
