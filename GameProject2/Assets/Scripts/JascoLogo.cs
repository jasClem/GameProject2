using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JascoLogo : MonoBehaviour
{
    //Load logo images
    public Image jascoFade;
    public Image jascoBG;
    public Image jasFG;
    public Image jascoFG;
    public Image games;
    public Image blackImage;
    public bool fadeIn = true; //Fades in (or out) image

    //Load logo audio clips
    public AudioSource fadeSound;
    public AudioSource jascoSound;

    IEnumerator Start()
    {
        //Set Images to (0.0) alpha value
        jascoFade.canvasRenderer.SetAlpha(0.0f);
        jascoBG.canvasRenderer.SetAlpha(0.0f);
        jasFG.canvasRenderer.SetAlpha(0.0f);
        jascoFG.canvasRenderer.SetAlpha(0.0f);
        games.canvasRenderer.SetAlpha(0.0f);

        blackImage.canvasRenderer.SetAlpha(1.0f);
        //Set Image to empty (0.0) alpha value

        FadeIn();//Fade in

        //Animation - Faders/Alpha/Sound/Wait Times
        yield return new WaitForSeconds(0.5f);

        //Fade in fade image & play fade sound
        jascoFade.CrossFadeAlpha(1.0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        fadeSound.Play();

        //Display logo BG & fade out fade image
        jascoBG.canvasRenderer.SetAlpha(1.0f);
        jascoFade.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(1.5f);

        //Display first part of logo & play logo sound
        jasFG.canvasRenderer.SetAlpha(1.0f);
        jascoSound.Play();
        yield return new WaitForSeconds(0.5f);

        //Display full logo & remove partial logo
        jascoFG.canvasRenderer.SetAlpha(1.0f);
        jasFG.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForSeconds(1.0f);

        //Display games logo
        games.canvasRenderer.SetAlpha(1.0f);
        yield return new WaitForSeconds(3.0f);

        //Fade out logo BG
        jascoBG.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(0.5f);

        //Fade out logo
        jascoFG.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(0.5f);

        //Fade out games logo
        games.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        FadeOut();//Fade out

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load new scene
    }

    void FadeIn()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(1.0f, 1.0f, false);
            //Fade Image to full (1.0) alpha value in (#) secoonds
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(0.0f, 1.0f, false);
            //Fade Image to empty (0.0) alpha value in (#) seconds
        }

    }

    void FadeOut()
    {
        if (fadeIn == true)
        {
            blackImage.CrossFadeAlpha(0.0f, 0.5f, false);
            //Fade Image to empty (0.0) alpha value in (#) seconds
        }

        if (fadeIn == false)
        {
            blackImage.CrossFadeAlpha(1.0f, 0.25f, false);
            //Fade Image to full (1.0) alpha value in (#) secoonds

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
