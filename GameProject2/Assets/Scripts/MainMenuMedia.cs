using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuMedia : MonoBehaviour
{
    //Assigned to Main Menu in unity (scene 1)

    public AudioSource menuMusic;

    public Image blackImage;

    public GameObject blackImageOB;


    IEnumerator Start()
    {
        blackImageOB.SetActive(true);

        blackImage.CrossFadeAlpha(0.0f, 2.0f, false);
        //Fade Image to empty (0.0) alpha value in (#) seconds

        menuMusic.Play();

        yield return new WaitForSeconds(300.0f); //wait 5 min.

        blackImage.CrossFadeAlpha(1.0f, 2.0f, false);
        //Fade Image to full (1.0) alpha value in (#) secoonds

        blackImageOB.SetActive(true);

        menuMusic.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); //Go back to introScreens
    }

    public void PlayGame()
    {
        blackImage.CrossFadeAlpha(1.0f, 2.0f, false);
        //Fade Image to full (1.0) alpha value in (#) secoonds

        blackImageOB.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        menuMusic.Stop();
    }

    public void QuitGame()
    {
        blackImage.CrossFadeAlpha(1.0f, 2.0f, false);
        //Fade Image to full (1.0) alpha value in (#) secoonds

        blackImageOB.SetActive(true);

        menuMusic.Stop();

        Debug.Log("Quit Game");
        //Display debug message for quitting the game

        Application.Quit();
        //Quit the game if quit button is clicked
    }

}
