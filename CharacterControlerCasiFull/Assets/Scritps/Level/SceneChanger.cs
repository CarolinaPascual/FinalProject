using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public static SceneChanger inst;
    [HideInInspector]

    private void Awake()
    {
        inst = this;
    }

    public void goToPlayScene()
    {
        Debug.Log("PLAY");
        SceneManager.LoadScene("LevelSpawnerScene");
        CInputManager.Inst.resetBools();
    }

    public void goTomenuScene()
    {
        SceneManager.LoadScene("mainMenu");
        CInputManager.Inst.resetBools();
    }

    public void goToSelectScene()
    {
        SceneManager.LoadScene("CharacterSelect");
        CInputManager.Inst.resetBools();
    }

    public void exit()
    {
        Application.Quit();
    }

}
