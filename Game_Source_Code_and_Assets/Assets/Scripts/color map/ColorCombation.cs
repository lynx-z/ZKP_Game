using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ColorCombation : MonoBehaviour
{

    public GameObject[] colorObjects;

    private List<Color> colors = new List<Color>{};
    private List<List<Color>> combinations = new List<List<Color>>{};

    // permute color
    public GameObject[] leftColors;
    public GameObject[] rightColors;

    private int colorIndex = 0; // same as combinations, start from 1, not 0



    // Start is called before the first frame update
    void Start()
    {
        // color list
        foreach (GameObject colorObject in colorObjects){
            colors.Add(colorObject.GetComponent<SpriteRenderer>().color);
        }

        // create 6 combinations
        combinations.Add(new List<Color>{colors[0], colors[1], colors[2]});
        combinations.Add(new List<Color>{colors[0], colors[2], colors[1]});
        combinations.Add(new List<Color>{colors[1], colors[0], colors[2]});
        combinations.Add(new List<Color>{colors[1], colors[2], colors[0]});
        combinations.Add(new List<Color>{colors[2], colors[1], colors[0]});
        combinations.Add(new List<Color>{colors[2], colors[0], colors[1]});

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // return a combination color list
    public List<List<Color>>  getCombinations(List<Color> mapColorList){
        // Debug.Log(mapColorList.Count);
        List<List<Color>> resultCombinations = new List<List<Color>>{};
        List<int> colorInt = new List<int>{};

        // get int list
        foreach(Color mapColor in mapColorList){
            int index = colors.IndexOf(mapColor);

            colorInt.Add(index);
        }

        // get all combinations
        foreach (List<Color> eachCombination in combinations){
            List<Color> resultCombination = new List<Color> {};
            foreach (int cInt in  colorInt){
                resultCombination.Add(eachCombination[cInt]);
            }
            resultCombinations.Add(resultCombination);
        }

        return resultCombinations;
    }


    // randomly fill maps
    public void autoFillMap(GameObject[] blocks){
        foreach(GameObject block in blocks){
            int randomNumber = UnityEngine.Random.Range(0, colorObjects.Length);

            block.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = colorObjects[randomNumber].GetComponent<SpriteRenderer>().color;
        }
    }


    public List<Color> getColors(){
        return colors;

    }



    public void showCombination(){

        for(int i = 0; i< 3;i++){
            leftColors[i].GetComponent<SpriteRenderer>().color = colors[i];
        }

        for(int i = 0; i< 3;i++){
            rightColors[i].GetComponent<SpriteRenderer>().color = combinations[colorIndex][i];
        }

        colorIndex++;

    }


}
