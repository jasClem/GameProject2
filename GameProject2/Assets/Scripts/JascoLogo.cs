using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JascoLogo : MonoBehaviour
{

    public Image jascoFade;
    public Image jascoBG;
    public Image jasFG;
    public Image jascoFG;
    public Image games;
    //Load logo images

    public AudioSource fadeSound;
    public AudioSource jascoSound;
    //Load audio clips


    IEnumerator Start()
    {
        jascoFade.canvasRenderer.SetAlpha(0.0f);
        jascoBG.canvasRenderer.SetAlpha(0.0f);
        jasFG.canvasRenderer.SetAlpha(0.0f);
        jascoFG.canvasRenderer.SetAlpha(0.0f);
        games.canvasRenderer.SetAlpha(0.0f);
        //Set Images to empty (0.0) alpha value

        yield return new WaitForSeconds(1.5f);

        jascoFade.CrossFadeAlpha(1.0f, 0.5f, false);

        fadeSound.Play();

        yield return new WaitForSeconds(0.5f);

        jascoBG.canvasRenderer.SetAlpha(1.0f);

        jascoFade.CrossFadeAlpha(0.0f, 0.75f, false);

        yield return new WaitForSeconds(1.5f);

        jasFG.canvasRenderer.SetAlpha(1.0f);
        jascoSound.Play();

        yield return new WaitForSeconds(0.5f);

        jascoFG.canvasRenderer.SetAlpha(1.0f);
        jasFG.canvasRenderer.SetAlpha(0.0f);

        yield return new WaitForSeconds(1.0f);

        games.canvasRenderer.SetAlpha(1.0f);

        yield return new WaitForSeconds(3.0f);

        jascoBG.CrossFadeAlpha(0.0f, 2.0f, false);

        yield return new WaitForSeconds(1.5f);

        jascoFG.CrossFadeAlpha(0.0f, 2.0f, false);

        yield return new WaitForSeconds(0.5f);

        games.CrossFadeAlpha(0.0f, 1.5f, false);

        yield return new WaitForSeconds(1.5f);
    }

}
