using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class WallyResult : MonoBehaviour
{
    public GameObject picture;
    public GameObject board;
    public GameObject targetPoint;

    private Collider2D pictureCollider;
    private Collider2D boardCollider;
    private Collider2D targetPointCollider;

    private Vector3 correctholePosition = new Vector3(-3.67f, 2.75f, 0);

    // Start is called before the first frame update
    void Start()
    {
        pictureCollider = picture.GetComponent<Collider2D>();
        boardCollider = board.GetComponent<Collider2D>();
        targetPointCollider = targetPoint.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // check if picture is in the board
    public bool isInside(Collider2D smallerCollider, Collider2D largerCollider){
        bool isInside = smallerCollider.bounds.min.x >= largerCollider.bounds.min.x &&
                        smallerCollider.bounds.max.x <= largerCollider.bounds.max.x &&
                        smallerCollider.bounds.min.y >= largerCollider.bounds.min.y &&
                        smallerCollider.bounds.max.y <= largerCollider.bounds.max.y;
        return isInside;

    }

    // change result word
    public void getResult(Transform verifier_text){
        TextMeshProUGUI myTextMeshPro = verifier_text.GetComponent<TextMeshProUGUI>();

        string resultString = "";
        // if not correct hole
        if (Vector3.Distance(targetPoint.transform.localPosition, correctholePosition) > 0.1){
            resultString =  "I do not see the puffin, so I do not believe there is a puffin in the picture";
             myTextMeshPro.text = resultString;
             return;
        }

        // big board and cover totally 
        if (isInside(pictureCollider, boardCollider)){
            resultString = "I see the puffin, but do not know the exact position";
        }else{

            if (isInside(targetPointCollider, boardCollider)){
                resultString = "I see the puffin, and I also know its position";
            }else{
                resultString = "I do not see the puffin, so I do not believe there is a puffin in the picture";
            }


        }
        myTextMeshPro.text = resultString;

    }




    


}
