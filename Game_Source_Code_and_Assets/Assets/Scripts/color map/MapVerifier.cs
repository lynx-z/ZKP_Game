using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class MapVerifier : MonoBehaviour
{

    // big map blocks
    public GameObject[] mapBlocks;
    public GameObject[] smallMaps;

    // save neighbour blocks
    public GameObject[] neighbourFirst;
    public GameObject[] neighbourSecond;

    Color blueColor;
    Color greenColor;
    Color redColor;

    private int revealTimes = 0;
    private int vaildRevealTimes = 0;

    private List<Color> colorStrategy1 = new List<Color>();



    // UI
    public Transform verifiertext;
    public Transform revealTimeText;

    public Button revealButton;
    public Button nextButton;


    void Start()
    {

        
    }

    // generate 6 maps, color should be same reference
    public void createMaps(){
        List<Color> colors = this.GetComponent<ColorCombation>().getColors();
        blueColor = colors[0];
        greenColor = colors[1];
        redColor = colors[2];

        colorStrategy1 = new List<Color>{blueColor, greenColor, redColor, greenColor, blueColor, redColor, blueColor, greenColor};
        generateSmallMaps(this.GetComponent<ColorCombation>().getCombinations(colorStrategy1));

    }

    void generateSmallMaps(List<List<Color>> resultCombinations){
        // for each small maps
            //combination_index = 0
            // for each block in big map (already in order)
                // get small block == big block name
                // small block color = combination[combination_index][big_block_index]

        int combination_index = 0;
        foreach (GameObject smallmap in smallMaps){
            for (int big_block_index = 0; big_block_index < mapBlocks.Length; big_block_index++){
                Transform childTransform = smallmap.transform.Find(mapBlocks[big_block_index].name);
                GameObject childObject = childTransform.gameObject;

                childObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = resultCombinations[combination_index][big_block_index];
            }
            combination_index++;
        }
    }


    public void reveal(){
        List<GameObject> selectedBlocks = BlockSelector.GetSelectedBlocks();
        if (selectedBlocks.Count !=2){
            return;
        }

        // randomly select a small map
        int smallmapIndex = UnityEngine.Random.Range(0, smallMaps.Length);
        GameObject targetSmallMap = smallMaps[smallmapIndex];

        GameObject neighbour_1 = selectedBlocks[0];
        GameObject neighbour_2 = selectedBlocks[1];
        
        Color color_1 = targetSmallMap.transform.Find(neighbour_1.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
        Color color_2 = targetSmallMap.transform.Find(neighbour_2.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;

        neighbour_1.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_1;
        neighbour_2.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_2;

        // check if neighbour
        bool isneighbour = false;
        for (int i = 0; i<neighbourFirst.Length; i++){
            if (neighbourFirst[i].name == neighbour_1.name){
                if (neighbourSecond[i].name == neighbour_2.name){
                    isneighbour = true;
                    break;
                }
            }

            if (neighbourSecond[i].name == neighbour_1.name){
                if (neighbourFirst[i].name == neighbour_2.name){
                    isneighbour = true;
                    break;
                }
            }
        }

        revealTimes++;

        // check if neighbour
        if (isneighbour){
            if (color_1 == color_2){
                vaildRevealTimes = 0;
            }else{
                vaildRevealTimes ++;
            }
        }

        // update text
        getConfidence();

        BlockSelector.disableClickable();
        nextButton.interactable = true;
        revealButton.interactable = false;
       


    }


    public void nextReveal(){
        BlockSelector.resetList();
        BlockSelector.enableClickable();
        nextButton.interactable = false;
        revealButton.interactable = true;

    }


    public void getConfidence(){
        TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
        revealtext.text = "The number of reveals: " + revealTimes.ToString();

        // 1-(1-1/E)^n, E = 12, n = times
        TextMeshProUGUI myTextMeshPro = verifiertext.GetComponent<TextMeshProUGUI>();

        string resultString;
        // get score
        float confidenceScore = 1f - Mathf.Pow(1f-(1f / 12f), vaildRevealTimes);
        string percentageString = (confidenceScore * 100f).ToString("F2") + "%";

        string confidenceString = "Confidence: " + percentageString;

        // string leakageString = "Leakage: " + LeakageDegree();
        resultString = confidenceString;

        // resultString = confidenceString +"\n"+leakageString;

        myTextMeshPro.text = resultString;
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
