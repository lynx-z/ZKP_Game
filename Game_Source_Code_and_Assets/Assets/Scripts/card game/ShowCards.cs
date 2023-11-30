using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.U2D;
using System;
using System.IO;
using TMPro;
using System.Linq;

public class ShowCards : MonoBehaviour
{
    private float duration = 0.2f;
    private float elapsedTime = 0f;
    private bool showIsMoving = false;
    private bool showed = false;

    private bool selectedCardMove = false;
    private bool backToHandIsMoving = false;

    private bool selectedCardShowed = false;


    public GameObject deck; // parent game object
    public GameObject highlightObject; // selected object
    public GameObject[] cardList; // child object lists
    public GameObject[] showedAfterPickedCard;
    public GameObject[] disabledAfterPickedCard;

    // show card phase
    private Vector3 initalPostion = new Vector3(0, -3, 0);
    private Vector3 showPostion = new Vector3(-7, 2, 0);
    private float showPostionGapX = 1.1f;
    private float showPostionGapY = 1.6f;
    private float showPostionGapZ = 0.1f;

    // selected card phase
    private Vector3 selectedInitialPostion = new Vector3(0, 0, 0);
    // private Vector3 selectedCardPostion = new Vector3(7.5f, -4, 0);
    private Vector3 selectedCardPostion = new Vector3(-7.9f, -4f, 0);
    private string selectedCardName = null;

    // back other card to hand phase
    // private Vector3 handCardPosition = new Vector3(-7, -1, 10);
    private Vector3 handCardPosition = new Vector3(-6.5f, -4f, 10f);
    private float gapX = 0.3f;
    private float gapY = 0.6f;
    private float gapZ = 0.1f;


    // back other card to hand phase
    private Vector3 onTablePosition = new Vector3(-7, 2.3f, 10);
    private float tablePostionGapX = 1.1f;
    private float tablePostionGapY = 1.6f;
    private float tablePostionGapZ = 0.1f;

    private List<string> onHandCards = new List<string> {};
    private List<string> onTableCards = new List<string> {};
    

    // card names
    string spriteName = "Pixel_Card_";
    List<string> cardNameList = new List<string> {
        "1C","2C","3C","4C","5C","6C","7C","8C","9C","10C","11C","12C","13C",
        "1D","2D","3D","4D","5D","6D","7D","8D","9D","10D","11D","12D","13D",
        "1H","2H","3H","4H","5H","6H","7H","8H","9H","10H","11H","12H","13H",
        "1S","2S","3S","4S","5S","6S","7S","8S","9S","10S","11S","12S","13S"};
    List<string> completeCardNameList = new List<string> {};

    ////////////////////////////////////////////////////
    void Start()
    {
         // inital position
        // foreach (GameObject target in cardList)
        // {
        //     target.transform.position = initalPostion;
        // }

        //
        for (int i = 0; i < cardNameList.Count; i++)
        {
            completeCardNameList.Add(spriteName + cardNameList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // when start, move card from left bottom corner to desk
        if (showIsMoving){
            cardShowingAnimation();
        }

        if (selectedCardMove){
            selectedCardAnimation();
            // Wait for 1 second
            StartCoroutine(WaitAndExecute());
        }

        if (backToHandIsMoving){
            cardBackToHandAnimation();
            // set inital 
        }

    }

    // when click start
    public void showCards(){
        if (showed == false){
            // card back
            GameObject childObject = transform.Find("card_back")?.gameObject;
            childObject.SetActive(false);

            elapsedTime = 0f; // Reset the elapsed time
            showIsMoving = true;// Start the movement
            showed = true;

        }
    }


    // select phase
    public void setSelectedCard(string cardName){
        // set other unclickalbe
        disableAllCardClickable();

        selectedCardName = cardName;
        // selectedCardName = cardName.Split('_')[-1];
        Debug.Log(selectedCardName);
        GameObject target = cardList.Where(obj => obj.name == cardName).SingleOrDefault();
        selectedInitialPostion = target.transform.position;

        // set highlight
        highlightObject.SetActive(true);
        highlightObject.transform.SetParent(target.transform, false);
        highlightObject.transform.localPosition = new Vector3(0,0,1);


        selectedCardMove = true;

        // set prover text
        this.GetComponent<CardProverActions>().setSelectedCardName(cardName);
        this.GetComponent<CardProverActions>().setProverText();

        // add rest card to list
        initialList();

        // show element
        setElementsActive();
        // move animation

    }

    // initial list
    private void initialList(){
        foreach (GameObject target in cardList)
        {
            if (target.gameObject.name != selectedCardName){
                onHandCards.Add(target.gameObject.name);
            }
        }
        // reorderDeck();

    }


    // moveCard
    public void moveCard(GameObject card){
        // select
        string cardName = card.gameObject.name;

        // reveal select card
        if (cardName == selectedCardName){

            if (selectedCardShowed){
                selectedCardShowed = false;
                // move back to hand
                onTableCards.Remove(cardName);
                card.transform.position = selectedCardPostion;
            }else{
                // move to table
                selectedCardShowed = true;
                onTableCards.Add(cardName);
            }
            reorderDeck();
            return;
        }

        // normal card, move to table
        if (onHandCards.Contains(cardName)){
            onHandCards.Remove(cardName);
            onTableCards.Add(cardName);
        }else{
            onHandCards.Add(cardName);
            onTableCards.Remove(cardName);
        }

        // order deck
        reorderDeck();
    }


    // reorder deck
    public void reorderDeck(){
        // sort
        onHandCards.Sort((a, b) => completeCardNameList.IndexOf(a).CompareTo(completeCardNameList.IndexOf(b)));
        for (int i = 0; i < onHandCards.Count; i++) {
            GameObject cardObject = cardList.Where(obj => obj.name == onHandCards[i]).SingleOrDefault();

            // change the postion
            Vector3 position = handCardPosition;
            
            if (i < 26) {
                position.x += i * gapX;
                position.z -= i * gapZ;
            }else{
            // the second line
                position.y -= gapY;
                position.x += (i - 26) * gapX;
                position.z -= i * gapZ; 
            }

            cardObject.transform.position = position;
            // rotate
            onHandStyle(cardObject);
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

            
            // if (i < 26) {
            //     position.x += i * tablePostionGapX + tablePostionGapX; // rotate 45 offset
            //     position.z -= i * tablePostionGapZ;
            // }else{
            // // the second line
            //     position.y -= tablePostionGapY;
            //     position.x += (i - 26) * tablePostionGapX;
            //     position.z -= i * tablePostionGapZ; 
            // }

            cardObject.transform.position = position;
            // rotate
            // onTableStyle(cardObject);
        }
    }




    /////////////// Style ///////////////

    private void onTableStyle(GameObject cardObject){
        // roate 45
        Quaternion rotation = Quaternion.Euler(45, 45, 0);
        cardObject.transform.rotation = rotation;

        cardObject.transform.localScale = new Vector3(1.5f, 1, 1);
    }

    private void onHandStyle(GameObject cardObject){
        // roate 0
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        cardObject.transform.rotation = rotation;

        cardObject.transform.localScale = new Vector3(1, 1, 1);
    }

    /////////////// Animation ///////////////

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

            // clickable
            enableAllCardClickable();
        }
    }


    void selectedCardAnimation(){
        GameObject target = cardList.Where(obj => obj.name == selectedCardName).SingleOrDefault();
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        // Interpolate between the start and end positions
        target.transform.position = Vector3.Lerp(selectedInitialPostion, selectedCardPostion, t);

        // Check if the movement is complete
        if (t >= 1f)
        {
            target.GetComponent<CardAnimation>().setOriginalPosition(selectedCardPostion);
            selectedCardMove = false;
            // Reset the elapsed time
            elapsedTime = 0f;

        }

    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(1f);
        backToHandIsMoving = true;
    }


    void cardBackToHandAnimation(){
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;
        
        // Calculate the interpolation value based on the elapsed time and duration
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        // Interpolate between the start and end positions
        // exclude selected
        for (int i = 0; i < cardNameList.Count; i++){
            // GameObject target = cardList[i];
            if (spriteName+cardNameList[i] == selectedCardName){
                continue;
            }
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();

            Vector3 endPosition = handCardPosition;
            if (i < 26) {
                endPosition.x += i * gapX;
                endPosition.z -= i * gapZ;
            }else{
            // the second line
                endPosition.y -= gapY;
                endPosition.x += (i - 26) * gapX;
                endPosition.z -= i * gapZ; 
            }
            target.transform.position = Vector3.Lerp(target.transform.position, endPosition, t);
        }
        
        // Check if the movement is complete
        if (t >= 1f)
        {
            backToHandIsMoving = false;
            // Reset the elapsed time
            elapsedTime = 0f;

            reorderDeck();

            // clickable
            enableAllCardClickable();
        }
    }

    public void enableAllCardClickable(){
        // set clickable
        for (int i = 0; i < cardNameList.Count; i++){
        // GameObject target = cardList[i];
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();
            target.GetComponent<CardAnimation>().setClickable();
            target.GetComponent<CardAnimation>().setOriginalPosition(target.transform.position);
        }

    }

    public void disableAllCardClickable(){
        for (int i = 0; i < cardNameList.Count; i++){
        // GameObject target = cardList[i];
            GameObject target = cardList.Where(obj => obj.name == spriteName+cardNameList[i]).SingleOrDefault();
            target.GetComponent<CardAnimation>().setUnclickable();
            target.GetComponent<CardAnimation>().setOriginalPosition(target.transform.position);
        }
    }

    public void setElementsActive(){
        foreach (GameObject target in showedAfterPickedCard)
        {
            target.SetActive(true);
        }

        foreach (GameObject target in disabledAfterPickedCard)
        {
            target.SetActive(false);
        }
    }



    /////////////// Getter ///////////////
    public string getSelectedCardName(){
        return selectedCardName;
    }

    public GameObject[] getCardList(){
        return cardList;
    }

    public List<string> getOnHandCards(){
        return onHandCards;
    }

    public List<string> getOnTableCards(){
        return onTableCards;
    }

    public bool getShowed(){
        return showed;
    }

    public bool getSelectedCardShowed(){
        return selectedCardShowed;
    }


}
