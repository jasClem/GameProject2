using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Assigned to Main Menu in unity (scene 1)

    public AudioSource menuMusic;

    public Image blackImage;

    public GameObject blackImageOB;

    public bool fadeIn = true; //Fades in (or out) image


    IEnumerator Start()
    {

        if (fadeIn)
        {
            blackImage.canvasRenderer.SetAlpha(0.0f);
            //Set Image to empty (0.0) alpha value

            blackImageOB.SetActive(false);
            //disable black image
        }

        if (!fadeIn)
        {
            blackImage.canvasRenderer.SetAlpha(1.0f);
            //Set Image to full (1.0)

            blackImageOB.SetActive(true);
            //enable black image
        }

        FadeIn();//Fade in

        menuMusic.Play();

        yield return new WaitForSeconds(300.0f); //wait 5 min.

        FadeOut();//Fade out

        menuMusic.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); //Go back to introScreens
    }

    public void PlayGame()
    {
        FadeOut();//Fade out

        menuMusic.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Change scene to main game if play button is clicked
    }

    public void QuitGame()
    {
        FadeOut();//Fade out

        menuMusic.Stop();

        Debug.Log("Quit Game");
        //Display debug message for quitting the game

        Application.Quit();
        //Quit the game if quit button is clicked
    }

    void FadeIn()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(1.0f, 1.5f, false);
            //Fade Image to full (1.0) alpha value in (#) secoonds
            blackImageOB.SetActive(true);
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(0.0f, 2.0f, false);
            //Fade Image to empty (0.0) alpha value in (#) seconds
            blackImageOB.SetActive(false);
        }

    }

    void FadeOut()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(0.0f, 1.5f, false);
            //Fade Image to empty (0.0) alpha value in (#) seconds
            blackImageOB.SetActive(false);
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(1.0f, 2.0f, false);
            //Fade Image to full (1.0) alpha value in (#) secoonds
            blackImageOB.SetActive(true);

        }

    }
}
