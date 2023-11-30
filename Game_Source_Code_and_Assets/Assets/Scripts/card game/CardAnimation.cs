using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    private float hoverAmount = 0.5f; // the amount to move the card upwards
    private Vector3 originalPosition;
    private Vector2 originalcolliderSize;

    private bool clickable = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position; // store the original position of the card
        originalcolliderSize = this.GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(clickable){
            GameObject parentObject = transform.parent.gameObject;
            // set SelectedCardName
            if (parentObject.GetComponent<ShowCards>().getSelectedCardName()== null){

                parentObject.GetComponent<ShowCards>().setSelectedCard(this.gameObject.name);
                // setUnclickable();
            }else{
                parentObject.GetComponent<ShowCards>().moveCard(this.gameObject);
                originalPosition = transform.position;
            }
        }
    }


    void OnMouseEnter()
    {
        if(clickable){
            // current position
            originalPosition = transform.position;
            MoveCardUp();
        }
    }

    void OnMouseExit()
    {
        if (clickable){
            MoveCardDown();
        }
    }


    void MoveCardUp()
    {

        // also set rotate 45 position
        transform.position = new Vector3(transform.position.x + transform.rotation.eulerAngles.x * hoverAmount * (0.7f/45f), transform.position.y + hoverAmount, transform.position.z);
        // transform.position = new Vector3(transform.position.x, transform.position.y + hoverAmount, transform.position.z);
// 
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(0f, hoverAmount/2 * (-1f));
        //  also set rotate 45 BoxCollider2D
        collider.size = new Vector2(originalcolliderSize.x, originalcolliderSize.y+hoverAmount + transform.rotation.eulerAngles.x * hoverAmount * (1f/45f));
        // collider.size = new Vector2(originalcolliderSize.x, originalcolliderSize.y+hoverAmount);
    }

    void MoveCardDown()
    {
        transform.position = originalPosition;
        resetColliderSize();

    }

    void resetColliderSize(){
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(0f, 0f);
        collider.size = originalcolliderSize;
    }

    public void setUnclickable(){
        this.clickable = false;
    }

    public void setClickable(){
        this.clickable = true;
    }

    public void setOriginalPosition(Vector3 newPosition){
        originalPosition = newPosition;
    }


}
