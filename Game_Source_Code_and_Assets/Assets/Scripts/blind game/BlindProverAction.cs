using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;


public class BlindProverAction : MonoBehaviour
{

    public GameObject[] bigObjectLeft;
    public GameObject[] bigObjectRight;

    public GameObject[] smallObjectLeft;
    public GameObject[] smallObjectRight;

    GameObject bigGreenBall;
    GameObject bigRedBall;

    GameObject smallGreenBall;
    GameObject smallRedBall;

    // store origin position
    List <Vector3> bigObjectLeftPosition = new List <Vector3>();
    List <Vector3> bigObjectRightPosition = new List <Vector3>();
    List <Vector3> smallObjectLeftPosition = new List <Vector3>();
    List <Vector3> smallObjectRightPosition = new List <Vector3>();

    // public GameObject eyeCover;

    public GameObject blackoutPanel;
    private float fadeDuration = 0.5f; 
    private float waitDuration = 0.5f;

    private bool isSwitched = false;
    private bool previousIsSwitched = false;

    private int revealTimes = 0;

    // animation
    private float duration = 1f;
    private float elapsedTime = 0f;
    private bool isSwitching = false;
    Vector3 hidePosition = new Vector3(-7.5f, 4.2f, 0f);
    Vector3 hideScale = new Vector3(0.5f, 0.5f, 0f);
    Vector3 bigScale = new Vector3(2.5f, 2.5f, 0f);

    private Vector3 position1;
    private Vector3 position2;


    //UI
    public Button switchbtn;
    public Button notswitchbtn;
    public Button nextPicBtn;
    public Transform verifierText;

    // Start is called before the first frame update
    void Start()
    {
        bigGreenBall = bigObjectLeft[0];
        bigRedBall = bigObjectRight[0];

        smallGreenBall = smallObjectLeft[0];
        smallRedBall = smallObjectRight[0];


        //store origin position
        foreach (GameObject g_object in bigObjectLeft){bigObjectLeftPosition.Add(g_object.transform.position);}
        foreach (GameObject g_object in bigObjectRight){bigObjectRightPosition.Add(g_object.transform.position);}
        foreach (GameObject g_object in smallObjectLeft){smallObjectLeftPosition.Add(g_object.transform.position);}
        foreach (GameObject g_object in smallObjectRight){smallObjectRightPosition.Add(g_object.transform.position);}
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwitching){
            BallAnimation();
        }
        
    }


    int pictureIndex = 0;
    public void changePictures(){
        pictureIndex ++;
        if (pictureIndex == bigObjectLeft.Length){
            pictureIndex = 0;
        }

        // set previous not active
        bigGreenBall.SetActive(false);
        bigRedBall.SetActive(false);
        smallGreenBall.SetActive(false);
        smallRedBall.SetActive(false);

        // change variable
        bigGreenBall = bigObjectLeft[pictureIndex];
        bigRedBall = bigObjectRight[pictureIndex];

        smallGreenBall = smallObjectLeft[pictureIndex];
        smallRedBall = smallObjectRight[pictureIndex];

        bigGreenBall.SetActive(true);
        bigRedBall.SetActive(true);
        smallGreenBall.SetActive(true);
        smallRedBall.SetActive(true);

        // set to origin position
        bigGreenBall.transform.position = bigObjectLeftPosition[pictureIndex];
        bigRedBall.transform.position = bigObjectRightPosition[pictureIndex];
        smallGreenBall.transform.position = smallObjectLeftPosition[pictureIndex];
        smallRedBall.transform.position = smallObjectRightPosition[pictureIndex];


        revealTimes = 0;
        previousIsSwitched = false;
        isSwitched = false;

        getConfidence(verifierText);
        showBalls();

        Debug.Log(isSwitched);

    }

    public void showBalls(){
        // StartCoroutine(FadeCoroutine());
        // smallBallSwitch();
        // BigRandomlySwitched();
        smallBallSwitch();
        BigRandomlySwitched();
        isSwitching = true;

        switchbtn.interactable = false;
        notswitchbtn.interactable = false;
        nextPicBtn.interactable = false;

    }

    // call first then big ball
    void smallBallSwitch(){
        if (previousIsSwitched){
            switchposition(smallGreenBall, smallRedBall);
        }
    }

    // randamly switch two big balls
    void BigRandomlySwitched(){
        // red -> position 1, green -> position2 
        revealTimes ++;
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 1){
            // switch
            isSwitched = true;
            previousIsSwitched = true;
            position1 = bigGreenBall.transform.position;
            position2 = bigRedBall.transform.position;

            // switchposition(bigRedBall, bigGreenBall);
            
        }else{
            //not switch 
            isSwitched = false;
            previousIsSwitched = false;
            position1 = bigRedBall.transform.position;
            position2 = bigGreenBall.transform.position;
        }
    }

    // switch two game object position
    void switchposition(GameObject object1, GameObject object2){
        Vector3 tempPosition = object1.transform.position;
        object1.transform.position = object2.transform.position;
        object2.transform.position = tempPosition;
    }

    //////////////////////////// black screen ////////////////////////////
    void BallAnimation(){

        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);

        if (t<= 0.5f){
            bigRedBall.transform.position = Vector3.Lerp(bigRedBall.transform.position, hidePosition, 2*t);
            bigGreenBall.transform.position = Vector3.Lerp(bigGreenBall.transform.position, hidePosition, 2*t);

            //scale
            bigRedBall.transform.localScale = Vector3.Lerp(bigRedBall.transform.localScale, hideScale, 2*t);
            bigGreenBall.transform.localScale = Vector3.Lerp(bigGreenBall.transform.localScale, hideScale, 2*t);

        }else{
            bigRedBall.transform.position = Vector3.Lerp(hidePosition, position1, 2 * (t - 0.5f));
            bigGreenBall.transform.position = Vector3.Lerp(hidePosition, position2, 2 * (t - 0.5f));

            // scale
            bigRedBall.transform.localScale = Vector3.Lerp(hideScale, bigScale, 2 * (t - 0.5f));
            bigGreenBall.transform.localScale = Vector3.Lerp(hideScale, bigScale, 2 * (t - 0.5f));

        }

        // Check if the movement is complete
        if (t >= 1f)
        {
            isSwitching = false;
            // Reset the elapsed time
            elapsedTime = 0f;

            switchbtn.interactable = true;
            notswitchbtn.interactable = true;
            nextPicBtn.interactable = true;

        }
    }

    private IEnumerator FadeCoroutine()
    {
        //eyeCover
        // eyeCover.SetActive(true);


        blackoutPanel.SetActive(true);

        // Fade in (transition to black)
        yield return StartCoroutine(FadeToColor(Color.black, fadeDuration / 2f));

        // Wait for a moment with the screen fully black
        yield return new WaitForSeconds(waitDuration);

        // wait, then show 
        smallBallSwitch();
        BigRandomlySwitched();

        // Fade out (transition back to normal)
        yield return StartCoroutine(FadeToColor(Color.clear, fadeDuration / 2f));

        //eyecover
        // eyeCover.SetActive(false);

        blackoutPanel.SetActive(false);
    }

    private IEnumerator FadeToColor(Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = blackoutPanel.GetComponent<SpriteRenderer>().color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackoutPanel.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        blackoutPanel.GetComponent<SpriteRenderer>().color = targetColor;
    }



    ////////////////////// prover select switched or not //////////////////////

    public void ProverSelectSwitch(){
        if (!isSwitched){
            revealTimes = 0;
        }
    }

    public void ProverSelectNotSwitch(){
        if (isSwitched){
            revealTimes = 0;
        }
    }

    public void getConfidence(Transform btn_text){
        // 1-(1-1/E)^n, E = 2, n = times

        TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();

        string resultString;
        float confidenceScore = 1f - Mathf.Pow(1f-(1f / 2f), revealTimes);
        string percentageString = (confidenceScore * 100f).ToString("F2") + "%";

        resultString = "Confidence: " + percentageString;
        myTextMeshPro.text = resultString;

    }

}
