using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{

    public Image blackImage;

    public float fadeTime = 6.5f; //Time beteen fades

    public bool fadeIn = true; //Fades in (or out) image


    IEnumerator Start()
    {

        if (fadeIn)
            blackImage.canvasRenderer.SetAlpha(0.0f);
        //Set Image to empty (0.0) alpha value

        if(!fadeIn)
            blackImage.canvasRenderer.SetAlpha(1.0f);
        //Set Image to full (1.0)

        yield return new WaitForSeconds(1.5f);//Wait (1.5) seconds

        FadeIn();//Fade in

        yield return new WaitForSeconds(fadeTime);//Wait (6.5) seconds

        FadeOut();//Fade out

        yield return new WaitForSeconds(1.5f);//Wait (1.5) seconds

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load new scene
    }

    void FadeIn()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(1.0f, 0.25f, false);
            //Fade Image to full (1.0) alpha value in (0.25) secoonds
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(0.0f, 0.5f, false);
            //Fade Image to empty (0.0) alpha value in (0.5) seconds
        }

    }

    void FadeOut()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(0.0f, 0.5f, false);
            //Fade Image to empty (0.0) alpha value in (0.5) seconds
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(1.0f, 0.25f, false);
            //Fade Image to full (1.0) alpha value in (0.25) secoonds

        }

    }

    void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
