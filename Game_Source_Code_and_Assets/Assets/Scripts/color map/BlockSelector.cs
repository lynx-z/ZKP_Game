using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    private static List <GameObject> selectedBlocks = new List <GameObject>();
    private static bool clickable = true;

    // private static Color selectedColor = Color.red;
    private static Color selectedColor = new Color(154f/255f, 154f/255f, 154f/255f, 255f/255f);
    private static Color defaultColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {

        if (clickable){
            if (selectedBlocks.Contains(this.gameObject)){
                selectedBlocks.Remove(this.gameObject);
                SetColor(defaultColor);
                
            }else{
                if (selectedBlocks.Count <2){
                    selectedBlocks.Add(this.gameObject);
                    SetColor(selectedColor);
                }else{
                    // move first
                    GameObject removedObject = selectedBlocks[0];
                    removedObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = defaultColor;
                    selectedBlocks.RemoveAt(0);

                    selectedBlocks.Add(this.gameObject);
                    SetColor(selectedColor);
                }
            }
        }

    }



    public static List<GameObject> GetSelectedBlocks()
    {
        return selectedBlocks;
    }

    private void SetColor(Color color)
    {
       this.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color;
    }

    public static void enableClickable(){
        clickable = true;
    }

    public static void disableClickable(){
        clickable = false;
    }

    public static void resetList(){
        selectedBlocks[0].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = defaultColor;
        selectedBlocks[1].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = defaultColor;
        selectedBlocks = new List <GameObject>();

    }
}
