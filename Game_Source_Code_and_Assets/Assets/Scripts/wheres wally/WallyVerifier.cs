using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
public class WallyVerifier : MonoBehaviour
{

    public GameObject hole;

    private Vector3 holePosition = new Vector3 (-3.67f, 2.75f, 0f);
    private Vector3 holePosition_wrong = new Vector3 (-1.94f, -2.11f, 0f);

    private bool correctHole;

    public TextMeshProUGUI resultText;
    public Button belevebtn;
    public Button doubtbtn;


    // Start is called before the first frame update
    void Start()
    {
        createHole();

        
        
    }

    public void createHole(){
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0){
            // correct
            hole.transform.localPosition = holePosition;
            correctHole = true;
        }else{
            // wrong
            hole.transform.localPosition = holePosition_wrong;
            correctHole = false;
        }
    }

    public void believe(){
        if (correctHole){
            //success
            resultText.text = "Success.";
        }else{
            // fail
            resultText.text = "Fail.";
        }

        belevebtn.interactable = false;
        doubtbtn.interactable = false;


    }

    public void doubt(){
        if (correctHole){
            //fail
            resultText.text = "Fail.";
        }else{
            // success
            resultText.text = "Success.";
        }

        belevebtn.interactable = false;
        doubtbtn.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
