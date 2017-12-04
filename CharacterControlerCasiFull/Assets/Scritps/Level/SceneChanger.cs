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

    public void goToCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void goToInstructionsScene()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void exit()
    {
        Application.Quit();
    }

}
