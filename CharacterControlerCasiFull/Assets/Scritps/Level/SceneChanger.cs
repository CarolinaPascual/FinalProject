using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

public	void goToPlayScene()
    {
        SceneManager.LoadScene("LevelSpawnerScene");
    }

    public void goTomenuScene()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void exit()
    {
        Application.Quit();
    }
}
