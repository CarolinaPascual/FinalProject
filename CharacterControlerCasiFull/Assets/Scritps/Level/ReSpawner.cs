using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawner : MonoBehaviour {


    #region Singleton stuff
    private static ReSpawner _inst;
    public static ReSpawner Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("ReSpawner").AddComponent<ReSpawner>();
            return _inst;
        }
    }
    #endregion

    private List<GameObject> placesToSpawn;

    void Start () {
        placesToSpawn = new List<GameObject>();
	}

    void Awake()
    {
        init();
    }
	

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ReSpawn")
        {
            placesToSpawn.Add(other.gameObject);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ReSpawn")
        {
            placesToSpawn.Remove(other.gameObject);
            Debug.Log("Remove");
        }
         
    }

    public GameObject getPlaceToSpawn()
    {
        int random = Random.Range(0, placesToSpawn.Count);
        GameObject aux = placesToSpawn[random];
        placesToSpawn.Remove(aux);
        return aux;
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

}
