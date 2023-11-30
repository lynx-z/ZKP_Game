using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapController : MonoBehaviour
{

    private static GameObject selectedSmallMap;

    private static Color selectedColor = Color.white;
    private static Color defaultColor = Color.black;

    private static List<GameObject> selectedSmallMapList = new List<GameObject>();

    private static bool clickable = true;

    // Start is called before the first frame update
    void Start()
    {
        selectedSmallMapList = new List<GameObject>();


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        // select one map

        // // Reset the color of the previously selected small map
        // if (selectedSmallMap != null)
        // {
        //     SmallMapController previousController = selectedSmallMap.GetComponent<SmallMapController>();
        //     previousController.SetColor(defaultColor);
        // }

        // // Set the clicked small map as the selected one
        // selectedSmallMap = gameObject;

        // // Set the color of the clicked small map
        // SetColor(selectedColor);

        if (clickable){
            if (selectedSmallMapList.Contains(this.gameObject)){
                selectedSmallMapList.Remove(this.gameObject);
                this.SetColor(defaultColor);
            }else{
                selectedSmallMapList.Add(this.gameObject);
                this.SetColor(selectedColor);
            }
        }


    }


    private void SetColor(Color color)
    {
       this.transform.Find("map border").gameObject.GetComponent<SpriteRenderer>().color = color;
    }


    public static GameObject GetSelectedSmallMap()
    {
        return selectedSmallMap;
    }

    public static void enableClickable(){
        clickable = true;

    }

    public static List<GameObject> GetSelectedSmallMapList(){
        if (selectedSmallMapList.Count ==0){return new List<GameObject>();}
        clickable = false;
        return selectedSmallMapList;
    }


}
