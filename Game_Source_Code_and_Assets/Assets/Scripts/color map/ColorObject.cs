using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private Vector3 originalPosition;

    private bool isHitting = false;
    private Collision2D hittedObject;

    // Start is called before the first frame update
    void Start()
    {
        // set color as layer "color"
        originalPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = mousePosition;

    }

    void OnMouseUp()
    {
        // isDragging = false;

        if (isHitting){
            hittedObject.gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
        }
        transform.position = originalPosition;
    }

    // set map element tag as 'map'
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("map")){
            isHitting = true;
            hittedObject = collision;

        }
        
    }

    void OnCollisionStay2D(Collision2D collision){
        if (collision.gameObject.CompareTag("map")){
            isHitting = true;
            hittedObject = collision;

        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("map")){
            isHitting = false;
        }
    }




}
