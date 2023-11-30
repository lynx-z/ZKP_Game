using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;

using TMPro;
// same object which showcards.cs attached
public class CardProverActions : MonoBehaviour
{
    public GameObject deck; // parent game object
    public GameObject proverTextObj;

    private string selectedCardName = null;
    private List<string> onHandCards = new List<string> {};
    private List<string> onTableCards = new List<string> {};



    // Start is called before the first frame update
    void Start()
    {
        // selectedCardName = deck.GetComponent<ShowCards>().getSelectedCardName();
        onHandCards = deck.GetComponent<ShowCards>().getOnHandCards();
        onTableCards = deck.GetComponent<ShowCards>().getOnTableCards();


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // check if card is red
    private bool isRed(string cardName){
        if (cardName.Contains("H") || cardName.Contains("D")){
            return true;
        }else{
            return false;
        }
    }

    // get card name
    private string getCardName(string cardName){
        // get card color
        string name;
        if (cardName.Contains("H")){
            name = "Hearts";
        }else if (cardName.Contains("D")){
            name = "Diamonds";
        }else if (cardName.Contains("S")){
            name = "Spades";
        }else{
            name = "Clubs";
        }

        // get card number
        string cardText;
        string cardNumber = cardName.Split('_')[2];
        cardNumber = cardNumber.Remove(cardNumber.Length - 1);
        switch (cardNumber)
        {
            case "1":
                cardText = "ace";
                break;
            case "2":
                cardText = "two";
                break;
            case "3":
                cardText = "three";
                break;
            case "4":
                cardText = "four";
                break;
            case "5":
                cardText = "five";
                break;
            case "6":
                cardText = "six";
                break;
            case "7":
                cardText = "seven";
                break;
            case "8":
                cardText = "eight";
                break;
            case "9":
                cardText = "nine";
                break;
            case "10":
                cardText = "ten";
                break;
            case "11":
                cardText = "jack";
                break;
            case "12":
                cardText = "queen";
                break;
            case "13":
                cardText = "king";
                break;
            default:
                cardText = "unknown";
                break;
        }

        string result = cardText + " of "+ name;
        return result;
    }


    // get result
    public void getScore(Transform btn_text){
        // selectedCardName = deck.GetComponent<ShowCards>().getSelectedCardName();
        if (selectedCardName == null){return;}
        // red = true, black = false
        TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();

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
        if (deck.GetComponent<ShowCards>().getSelectedCardShowed()){
            resultString = "You have shown your card, so I know\nyour card is " +  getCardName(selectedCardName) +".";
            myTextMeshPro.text = resultString;
            return;
        }

        //
        if (correctNumber < 26){
            // resultString = resultString + "Not enough info";
            if (isRed(selectedCardName)){resultString = resultString + "It's not convincing.\nYour card could be black.";}
            else{resultString = resultString + "It's not convincing, your card could be red.";}
            myTextMeshPro.text = resultString;
            return;

        }else{
            // resultString = resultString + "Enough info";
            float possibility =  1f/(26f-extraNumber);
            string possibilityString = (possibility * 100f).ToString("F2") + "%";
            resultString = resultString + "I believe you now. The probability of\nme guessing your card is " + possibilityString;
            myTextMeshPro.text = resultString;
            return;
        }
    }

    // update prover text
    public void setProverText(){
        // selectedCardName = deck.GetComponent<ShowCards>().getSelectedCardName();
        if (selectedCardName == null){return;}

        TextMeshProUGUI proverText =  proverTextObj.GetComponent<TextMeshProUGUI>();

        if (isRed(selectedCardName)){
            proverText.text = "I picked a red card.";
        }else{
            proverText.text = "I picked a black card.";
        }
    }

    //
    public void setSelectedCardName(string name){
        selectedCardName = name;
    }




}
