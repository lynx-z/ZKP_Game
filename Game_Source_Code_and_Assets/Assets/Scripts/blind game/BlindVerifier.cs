using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.UI;
using TMPro;

public class BlindVerifier : MonoBehaviour
{
    public GameObject bigRedBall;
    public GameObject bigGreenBall;


    // private GameObject ballobject1;
    // private GameObject ballobject2;

    private Vector3 position1;
    private Vector3 position2;

    private bool isSwitched = false;
    private bool ballMove = false;

    private float duration = 0.2f;
    private float elapsedTime = 0f;
    private Vector3 centerPosition;
    // private float radius = 5f;

    private int revealTimes = 0;


    // UI
    public Transform statusText;
    public Transform verifierText;
    public Transform proverText;

    public Button switchbutton;
    public Button showbutton;
    public Button continuebutton;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 bigRedBallpos = bigRedBall.transform.position;
        Vector3 bigGreenBallpos = bigGreenBall.transform.position;

        centerPosition = new Vector3((bigRedBallpos.x + bigGreenBallpos.x)/2, (bigRedBallpos.y + bigGreenBallpos.y)/2, (bigRedBallpos.z + bigGreenBallpos.z)/2);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (ballMove){
            BallAnimation();
        }
        
    }

    public void switchBalls(){
        switchbutton.interactable = false;
        if (isSwitched){
            isSwitched = false;
            position1 = bigRedBall.transform.position;
            position2 = bigGreenBall.transform.position;
            ballMove = true;

        }else{
            // switch 
            isSwitched = true;
            position1 = bigRedBall.transform.position;
            position2 = bigGreenBall.transform.position;
            ballMove = true;

        }

        updateStatus();
    }

    void updateStatus(){
        TextMeshProUGUI sText = statusText.GetComponent<TextMeshProUGUI>();
        if (isSwitched){
            sText.text = "You have switeched balls";

        }else{
            sText.text = "You have not switeched balls";

        }
    }

    public void showToprover(){
        revealTimes +=1;

        //
        TextMeshProUGUI pText = proverText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI vText = verifierText.GetComponent<TextMeshProUGUI>();

        if (isSwitched){
            pText.text = "You have switeched balls";
        }else{
            pText.text = "You have not switeched balls";
        }

        float confidenceScore = 1f - Mathf.Pow(1f-(1f / 2f), revealTimes);
        string percentageString = (confidenceScore * 100f).ToString("F2") + "%";
        vText.text = "Confidence: " + percentageString;

        showbutton.interactable = false;
        switchbutton.interactable = false;
        continuebutton.interactable = true;




    }

    public void continuenNext(){
        showbutton.interactable = true;
        switchbutton.interactable = true;
        continuebutton.interactable = false;

        isSwitched = false;
        updateStatus();

        TextMeshProUGUI pText = proverText.GetComponent<TextMeshProUGUI>();
        pText.text = "Let me see ...";

    }
    




    // void BallAnimation(){

    //     // Increment the elapsed time
    //     elapsedTime += Time.deltaTime;
        
    //     // Calculate the interpolation value based on the elapsed time and duration
    //     float t = Mathf.Clamp01(elapsedTime / duration);
        
    //     bigRedBall.transform.position = Vector3.Lerp(position1, position2, t);
    //     bigGreenBall.transform.position = Vector3.Lerp(position2, position1, t);

    //     // Check if the movement is complete
    //     if (t >= 1f)
    //     {
    //         switchbutton.interactable = true;
    //         ballMove = false;
    //         // Reset the elapsed time
    //         elapsedTime = 0f;
    //     }
    // }


   void BallAnimation()
{
    // Increment the elapsed time
    elapsedTime += Time.deltaTime;

    // Calculate the interpolation value based on the elapsed time and duration
    float t = Mathf.Clamp01(elapsedTime / duration);

    // Calculate the center of the semi-circle (midpoint of position1 and position2)
    Vector3 center = (position1 + position2) / 2f;

    // Calculate the radius of the semi-circle (half the distance between position1 and position2)
    float radius = Vector3.Distance(position1, position2) / 2f;

    // Calculate the angles for the semi-circle motion (0 to 180 degrees for upper semi-circle and 180 to 360 degrees for lower semi-circle)
    float startAngle = 0f;
    float endAngle = 180f;

    // If you want the balls to rotate in the opposite direction, uncomment the following line:
    // float temp = startAngle; startAngle = endAngle; endAngle = temp;

    // Calculate the current angle based on the interpolation value
    float currentAngle = Mathf.Lerp(startAngle, endAngle, t);

    // Convert the angle to radians
    float radianAngle = currentAngle * Mathf.Deg2Rad;

    // Calculate the position of bigGreenBall on the upper semi-circle
    bigGreenBall.transform.position = new Vector3(center.x + radius * Mathf.Cos(radianAngle),
                                                  center.y + radius * Mathf.Sin(radianAngle),
                                                  bigGreenBall.transform.position.z);

    // Calculate the position of bigRedBall on the lower semi-circle
    bigRedBall.transform.position = new Vector3(center.x + radius * Mathf.Cos(Mathf.PI + radianAngle),
                                                center.y + radius * Mathf.Sin(Mathf.PI + radianAngle),
                                                bigRedBall.transform.position.z);

    // Check if the movement is complete
    if (t >= 1f)
    {
        switchbutton.interactable = true;
        ballMove = false;
        // Reset the elapsed time
        elapsedTime = 0f;
    }
}











}
