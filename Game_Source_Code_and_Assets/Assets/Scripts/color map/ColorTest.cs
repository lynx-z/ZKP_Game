using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ColorTest : MonoBehaviour
{
    private List<int> colorList;
    private List<List<string>> combinations;
    private List<List<string>> result = new List<List<string>>(){};

    private void Start()
    {
        colorList = new List<int> { 0, 1, 2, 0 };

        combinations = new List<List<string>>(){
            new List<string> {"blue", "red", "green"},
            new List<string> {"blue", "green", "red"},
            new List<string> {"red", "blue", "green"},
            new List<string> {"red", "green", "blue"},
            new List<string> {"green", "red", "blue"},
            new List<string> {"green", "blue", "red"}
        };
        printResult();
    }


    void printResult(){
        foreach (List<string> eachCombination in combinations){
            List<string> resultCombination = new List<string> {};
            foreach (int colorInt in  colorList){
                resultCombination.Add(eachCombination[colorInt]);
            }

            result.Add(resultCombination);
        }

        foreach (List<string> resultCombination in result)
        {
            string colors = string.Join(", ", resultCombination);
            Debug.Log(colors);
        }

    }

}
