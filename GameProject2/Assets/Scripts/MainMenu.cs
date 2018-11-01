using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Change scene to main game if play button is clicked
    }

    public void QuitGame()
    {

        Application.Quit();
        //Quit the game if quit button is clicked
    }
}
