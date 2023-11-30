using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.U2D;
using System;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class CardAIprover : MonoBehaviour
{
    public GameObject deck; // parent game object
    public GameObject highlightObject; // selected object
    public GameObject[] cardList; // child object lists

    public Button[] buttonList;

    public Sprite cardBackSprite;

    private Vector3 showStartCardPostion = new Vector3(-7.7f, 2.6f, 0f);


    // selected card
    private string selectedCardName;
    private Vector3 selectedCardPostion = new Vector3(-1.3f, 4f, 0f);

    // private Vector3 handCardPosition = new Vector3(-7, -1, 10);
    private Vector3 handCardPosition = new Vector3(0f, 4f, 10f);
    private float gapX = 0.3f;
    private float gapY = 0.4f;
    private float gapZ = 0.1f;


    // back other card to hand phase
    private Vector3 onTablePosition = new Vector3(-7f, 1.5f, 10f);
    private float tablePostionGapX = 1.1f;
    private float tablePostionGapY = 1.6f;
    private float tablePostionGapZ = 0.1f;


    // card names
    List<string> completeCardNameList = new List<string> {};
    string spriteName = "Pixel_Card_";
    List<string> cardNameList = new List<string> {
        "1C","2C","3C","4C","5C","6C","7C","8C","9C","10C","11C","12C","13C",
        "1D","2D","3D","4D","5D","6D","7D","8D","9D","10D","11D","12D","13D",
        "1H","2H","3H","4H","5H","6H","7H","8H","9H","10H","11H","12H","13H",
        "1S","2S","3S","4S","5S","6S","7S","8S","9S","10S","11S","12S","13S"};


    // 
    List<string> onHandCards = new List<string> {};
    List<string> onTableCards = new List<string> {};



    //animation
    private float duration = 0.2f;
    private float elapsedTime = 0f;
    private bool showIsMoving = false;
    private bool showed = false;

    private bool secondMove = false;
    private bool thirdMove = false;
    private bool fourthMove = false;
    private bool hasExecutedThirdMove = false;

    private bool selectedCardShowed = false;

    private Vector3 initalPostion = new Vector3(0, -3, 0);
    private Vector3 showPostion = new Vector3(-7, 2, 0);
    private float showPostionGapX = 1.1f;
    private float showPostionGapY = 1.6f;
    private float showPostionGapZ = 0.1f;


    Dictionary<string, Vector3> cardFinalPositionDic = new Dictionary<string, Vector3>();




    // Start is called before the first frame update
    void Start()
    {
        disableButtons();

        for (int i = 0; i < cardNameList.Count; i++)
        {completeCardNameList.Add(spriteName + cardNameList[i]);}

        //


        

        // //select a card
        // randomlySelectCard();
        // moveSelectCard();
        // setShowCase();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showIsMoving){
            cardShowingAnimation();
            StartCoroutine(WaitAndExecute());
        }

        if (secondMove){
            cardBackShowingAnimation();
            StartCoroutine(WaitAndExecuteSecond());
        }

        if (thirdMove && !hasExecutedThirdMove){
            randomlySelectCard();
            moveSelectCard();
            setShowCase();
            hasExecutedThirdMove = true;

            fourthMove = true;
        }

        if (fourthMove){
            activateButtons();
            cardFourthAnimation();
        }

        
    }


    private void randomlySelectCard(){
        selectedCardName = "Pixel_Card_" + "1C";
    }


    // move select card
    private void moveSelectCard(){
        GameObject target = cardList.Where(obj => obj.name == selectedCardName).SingleOrDefault();
        // target.transform.position = selectedCardPostion;
        cardFinalPositionDic[target.name] = selectedCardPostion;
        target.GetComponent<SpriteRenderer>().sprite = cardBackSprite;

        // set highlight
        highlightObject.SetActive(true);
        highlightObject.transform.SetParent(target.transform, false);
        highlightObject.transform.localPosition = new Vector3(0,0,1);


    }

    //
    private void setShowCase(){

        List<string> redList =  new List<string> {
        "1D","2D","3D","4D","5D","6D","7D","8D","9D","10D","11D","12D","13D",
        "1H","2H","3H","4H","5H","6H","7H","8H","9H","10H","11H","12H","13H",
        };

        List<string> blackList =  new List<string> {
        "2C","3C","4C","5C","6C","7C","8C","9C","10C","11C","12C","13C",
        "1S","2S","3S","4S","5S","6S","7S","8S","9S","10S","11S","12S","13S"
        };

        // 0. not enough
        // 1. enough
        int randomNumber = UnityEngine.Random.Range(0, 2);
        // Debug.Log(randomNumber);

        if (randomNumber == 0){
            // not enough red card

            //add red first
            int pickedRedNumber = UnityEngine.Random.Range(0, 26);
            List<string> selectedItems = new List<string>();
            List<string> remainingItems = new List<string>(redList);

            for (int i = 0; i < pickedRedNumber; i++)
            {
                if (remainingItems.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, remainingItems.Count);
                    string selectedItem = remainingItems[randomIndex];
                    selectedItems.Add(selectedItem);
                    remainingItems.RemoveAt(randomIndex);
                }
                else{break;}
            }

            foreach (string item in selectedItems){onTableCards.Add(item);}
            foreach (string item in remainingItems){onHandCards.Add(item);}


            //add black
            int pickedBlackNumber = UnityEngine.Random.Range(5, 15);
            selectedItems = new List<string>();
            remainingItems = new List<string>(blackList);

            for (int i = 0; i < pickedBlackNumber; i++)
            {
                if (remainingItems.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, remainingItems.Count);
                    string selectedItem = remainingItems[randomIndex];
                    selectedItems.Add(selectedItem);
                    remainingItems.RemoveAt(randomIndex);
                }
                else{break;}
            }

            foreach (string item in selectedItems){onTableCards.Add(item);}
            foreach (string item in remainingItems){onHandCards.Add(item);}


        }else{
            foreach (string item in redList){onTableCards.Add(item);}

            //add black
            int pickedBlackNumber = UnityEngine.Random.Range(0, 10);
            List<string> selectedItems = new List<string>();
            List<string> remainingItems = new List<string>(blackList);

            for (int i = 0; i < pickedBlackNumber; i++)
            {
                if (remainingItems.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, remainingItems.Count);
                    string selectedItem = remainingItems[randomIndex];
                    selectedItems.Add(selectedItem);
                    remainingItems.RemoveAt(randomIndex);
                }
                else{break;}
            }

            foreach (string item in selectedItems){onTableCards.Add(item);}
            foreach (string item in remainingItems){onHandCards.Add(item);}
        }


        for (int i = 0; i < onHandCards.Count; i++)
        {onHandCards[i] = spriteName + onHandCards[i];}

        for (int i = 0; i < onTableCards.Count; i++)
        {onTableCards[i] = spriteName + onTableCards[i];}


        reorderDeck(onHandCards, onTableCards);
    }




    // reorder deck
    public void reorderDeck(List<string> onHandCards, List<string> onTableCards){
        // sort
        onHandCards.Sort((a, b) => completeCardNameList.IndexOf(a).CompareTo(completeCardNameList.IndexOf(b)));
        for (int i = 0; i < onHandCards.Count; i++) {
            GameObject cardObject = cardList.Where(obj => obj.name == onHandCards[i]).SingleOrDefault();


            // change the postion
            Vector3 position = handCardPosition;

            if (i < 18) {
                position.x += i * gapX;
                position.z -= i * gapZ;
            }else if(i < 36){
                position.x += (i-18) * gapX;
                position.y -= 1f * gapY;
                position.z -= i * gapZ;
            }else if(i < 54){
                position.x += (i-36) * gapX;
                position.y -= 2f * gapY;
                position.z -= i * gapZ;
            }else{
                position.x += (i-54) * gapX;
                position.y -= 3f * gapY;
                position.z -= i * gapZ;
            }

            cardFinalPositionDic[cardObject.name] = position;
            // cardObject.transform.position = position;
            
            // change card back here
            cardObject.GetComponent<SpriteRenderer>().sprite = cardBackSprite;

            // rotate
        }

        // sort
        onTableCards.Sort((a, b) => completeCardNameList.IndexOf(a).CompareTo(completeCardNameList.IndexOf(b)));
        for (int i = 0; i < onTableCards.Count; i++) {
            GameObject cardObject = cardList.Where(obj => obj.name == onTableCards[i]).SingleOrDefault();

            // change the postion
            Vector3 position = onTablePosition;

            if (i < 13) {
                position.x += i * tablePostionGapX;
            }else if(i < 26){
                position.x += (i-13) * tablePostionGapX;
                position.y -= 1f * tablePostionGapY;
                position.z -= 1f * tablePostionGapZ;
            }else if(i < 39){
                position.x += (i-26) * tablePostionGapX;
                position.y -= 2f * tablePostionGapY;
                position.z -= 2f * tablePostionGapZ;
            }else{
                position.x += (i-39) * tablePostionGapX;
                position.y -= 3f * tablePostionGapY;
                position.z -= 3f * tablePostionGapZ;
            }

            // cardObject.transform.position = position;
            cardFinalPositionDic[cardObject.name] = position;

        }
    }







    // get result //

    // check if card is red
    private bool isRed(string cardName){
        if (cardName.Contains("H") || cardName.Contains("D")){
            return true;
        }else{
            return false;
        }
    }

    // get result
    private string getScore(){
        // selectedCardName = deck.GetComponent<ShowCards>().getSelectedCardName();
        // red = true, black = false
        // TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();

        int extraNumber = 0;
        int correctNumber = 0;
        string resultString = "";

        foreach(string cardName in onTableCards){
            if (isRed(selectedCardName) == isRed(cardName)){
                extraNumber += 1;
            }else{
                correctNumber += 1;
            }
        }

        // check if selected card showed
        // if (deck.GetComponent<ShowCards>().getSelectedCardShowed()){
        //     resultString = "You have shown your card, so I know\nyour card is " +  getCardName(selectedCardName) +".";
        //     myTextMeshPro.text = resultString;
        //     return;
        // }

        //
        if (correctNumber < 26){

            // resultString = resultString + "Not enough info";

            if (isRed(selectedCardName)){resultString = resultString + "The prover's reveal is not convincing because the prover did not reveal all the black cards.";}
            else{resultString = resultString + "The prover's reveal is not convincing because the prover did not reveal all the red cards.";}


            // myTextMeshPro.text = resultString;
            return resultString;

        }else{
            // resultString = resultString + "Enough info";
            float possibility =  1f/(26f-extraNumber);
            string possibilityString = (possibility * 100f).ToString("F2") + "%";
            resultString = resultString + "The prover shows enough cards to justify his statement. The probability of you guessing his card is " + possibilityString;
            // myTextMeshPro.text = resultString;
            return resultString;
        }
    }


    private bool ifEnoughInfo(){
        int extraNumber = 0;
        int correctNumber = 0;

        foreach(string cardName in onTableCards){
            if (isRed(selectedCardName) == isRed(cardName)){
                extraNumber += 1;
            }else{
                correctNumber += 1;
            }
        }

        if (correctNumber < 26){
            return false;}
        else{
            return true;
        }

    }


    public void selectTrust(Transform btn_text){
        TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();
        string resultString = "";

        if (ifEnoughInfo()) {
            resultString = "Success.\n" + getScore();
        }else{
            resultString = "Fail.\n" + getScore();
        }

        myTextMeshPro.text = resultString;

    }

    public void selectNotTrust(Transform btn_text){
        TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();
        string resultString = "";

        if (!ifEnoughInfo()) {
            resultString = "Success.\n" + getScore();
        }else{
            resultString = "Fail.\n"+ getScore();
        }

        myTextMeshPro.text = resultString;


    }

    public void disableButtons(){
        foreach(Button button in buttonList){
            button.interactable = false;
        }
    }

    public void activateButtons(){
        foreach(Button button in buttonList){
            button.interactable = true;
        }

    } 







    /////////////// Animation ///////////////
    // when click start
    public void showCards(){
        if (showed == false){
            // card back
            GameObject childObject = deck.transform.Find("card_back")?.gameObject;
            childObject.SetActive(false);

            elapsedTime = 0f; // Reset the elapsed time
            showIsMoving = true;// Start the movement
            showed = true;

        }
    }


    void cardShowingAnimation(){
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        // Interpolate between the start and end positions
        for (int i = 0; i < cardNameList.Count; i++){
            // GameObject target = cardList[i];
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();
            Vector3 endPosition = showPostion;
            if (i < 13) {
                endPosition.x += i * showPostionGapX;
            }else if(i < 26){
                endPosition.x += (i-13) * showPostionGapX;
                endPosition.y -= 1f * showPostionGapY;
                endPosition.z -= 1f * showPostionGapZ;
            }else if(i < 39){
                endPosition.x += (i-26) * showPostionGapX;
                endPosition.y -= 2f * showPostionGapY;
                 endPosition.z -= 2f * showPostionGapZ;
            }else{
                endPosition.x += (i-39) * showPostionGapX;
                endPosition.y -= 3f * showPostionGapY;
                endPosition.z -= 3f * showPostionGapZ;
            }
            target.transform.position = Vector3.Lerp(initalPostion, endPosition, t);
        }
        
        // Check if the movement is complete
        if (t >= 1f)
        {
            showIsMoving = false;
            // Reset the elapsed time
            elapsedTime = 0f;
        }
    }

    void cardBackShowingAnimation(){
        // card back
        GameObject childObject = deck.transform.Find("card_back")?.gameObject;
        childObject.SetActive(true);
        childObject.transform.position = showStartCardPostion;

        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        // Interpolate between the start and end positions
        for (int i = 0; i < cardNameList.Count; i++){
            // GameObject target = cardList[i];
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();
            // Vector3 endPosition = initalPostion;
            target.transform.position = Vector3.Lerp(target.transform.position, showStartCardPostion, t);
        }
        
        // Check if the movement is complete
        if (t >= 1f)
        {
            secondMove = false;
            // Reset the elapsed time
            elapsedTime = 0f;


        }
    }

    void cardFourthAnimation(){
        // card back
        GameObject childObject = deck.transform.Find("card_back")?.gameObject;
        childObject.SetActive(false);

        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        // Interpolate between the start and end positions
        for (int i = 0; i < cardNameList.Count; i++){
            // GameObject target = cardList[i];
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();
            // Vector3 endPosition = initalPostion;
            target.transform.position = Vector3.Lerp(target.transform.position, cardFinalPositionDic[target.name], t);
        }
        
        // Check if the movement is complete
        if (t >= 1f)
        {
            fourthMove = false;
            // Reset the elapsed time
            elapsedTime = 0f;


        }
    }


    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(1f);
        secondMove = true;
    }


    IEnumerator WaitAndExecuteSecond()
    {
        yield return new WaitForSeconds(1f);
        thirdMove = true;
    }

}
