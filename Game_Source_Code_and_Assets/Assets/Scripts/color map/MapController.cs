using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;

using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    // big map blocks
    public GameObject[] mapBlocks;

    // save neighbour blocks
    public GameObject[] neighbourFirst;
    public GameObject[] neighbourSecond;

    // save to small maps
    public GameObject[] smallMaps;

    // big map block colors
    private List<Color> mapBlockColor = new List<Color>();  // same order as mapBlocks
    private List<List<Color>> smallMapBlockColorList;
    private int permuteIndex = 0;
    private GameObject pickedSmallMap;

    private int revealTimes = 0;
    private GameObject selectedSmallMap;

    //use for calculate leakage
    private int[] smallMapRecord = new int[6] { 0, 0, 0, 0, 0, 0};


    // result 
    private int maxRevealTime = 50;
    private int revealCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // inital
        for (int i = 0; i < mapBlocks.Length; i++){
            mapBlockColor.Add(Color.white);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //hide elemnts
    public GameObject colorObject;
    public Button savebtn;
    public Button showbtn;
    public Button fillbtn;

    public Transform verifiertext;
    public Transform revealTimeText;

    public TextMeshProUGUI vtext;
    public TextMeshProUGUI proverText;


    // phase 3
    public Button pickBtn;
    public Button revealBtn;
    public Button keepBtn;


    // confirm the color 
    // public void saveMapColor(){

    //     // save color
    //     for(int i = 0; i < mapBlocks.Length; i++){
    //         mapBlockColor[i] = mapBlocks[i].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
    //     }

    //     // if not filled
    //     if (mapBlockColor.Contains(Color.white)){
    //         Debug.Log("Not filled");
    //         return;
    //     }

    //     //set prover text



    //     //set active
    //     colorObject.SetActive(false);
    //     savebtn.interactable = false;
    //     fillbtn.interactable = false;
    //     showbtn.interactable = true;


    //     // set back to white
    //     for(int i = 0; i < mapBlocks.Length; i++){
    //         mapBlocks[i].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = Color.white;
    //     }

    //     // get color combination
    //     generateSmallMaps(this.GetComponent<ColorCombation>().getCombinations(mapBlockColor));
    //     SmallMapController.enableClickable();
    // }



    //confirm the color 
    public void saveMapColor(){

        // save color
        for(int i = 0; i < mapBlocks.Length; i++){
            mapBlockColor[i] = mapBlocks[i].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
        }

        // if not filled
        if (mapBlockColor.Contains(Color.white)){
            Debug.Log("Not filled");
            return;
        }

        //set prover text

        //set active
        colorObject.SetActive(false);


        // set back to white
        for(int i = 0; i < mapBlocks.Length; i++){
            mapBlocks[i].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = Color.white;
        }

        // get color combination
        // generateSmallMaps(this.GetComponent<ColorCombation>().getCombinations(mapBlockColor));
        // SmallMapController.enableClickable();
        smallMapBlockColorList = this.GetComponent<ColorCombation>().getCombinations(mapBlockColor);
        permuteSmallMaps();
    }

    public void permuteSmallMaps(){
        if (permuteIndex >=6){
            return;
        }
        GameObject smallmap = smallMaps[permuteIndex];
        smallmap.SetActive(true);
        for (int big_block_index = 0; big_block_index < mapBlocks.Length; big_block_index++){
            Transform childTransform = smallmap.transform.Find(mapBlocks[big_block_index].name);
            GameObject childObject = childTransform.gameObject;
            childObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = smallMapBlockColorList[permuteIndex][big_block_index];
            }
        permuteIndex++;

        // show stragty
        this.GetComponent<ColorCombation>().showCombination();

        // show on big map
        foreach(GameObject mapBlock in mapBlocks){
            Color smallmapBlockColor = smallmap.transform.Find(mapBlock.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
            mapBlock.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = smallmapBlockColor;
        }




    }


    // void generateSmallMaps(List<List<Color>> resultCombinations){
    //     // for each small maps
    //         //combination_index = 0
    //         // for each block in big map (already in order)
    //             // get small block == big block name
    //             // small block color = combination[combination_index][big_block_index]
        

    //     int combination_index = 0;
    //     foreach (GameObject smallmap in smallMaps){
    //         for (int big_block_index = 0; big_block_index < mapBlocks.Length; big_block_index++){
    //             Transform childTransform = smallmap.transform.Find(mapBlocks[big_block_index].name);
    //             GameObject childObject = childTransform.gameObject;

    //             childObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = resultCombinations[combination_index][big_block_index];
    //         }
    //         combination_index++;
    //     }

    // }

    public void pickMap(){
        // pick a small map
        int smallmapIndex = UnityEngine.Random.Range(0, permuteIndex);
        GameObject targetSmallMap = smallMaps[smallmapIndex];
        if (targetSmallMap==null){return;}
        pickedSmallMap = targetSmallMap;

        // reset color
        foreach (GameObject smallmap in smallMaps){
            smallmap.transform.Find("map border").gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        // set rest as shadow color
        foreach(GameObject mapBlock in mapBlocks){
            Color smallmapBlockColor = targetSmallMap.transform.Find(mapBlock.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
            Color reducedOpacityColor = new Color(smallmapBlockColor.r, smallmapBlockColor.g, smallmapBlockColor.b, smallmapBlockColor.a * 0.2f);
            mapBlock.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = reducedOpacityColor;
        }

        // set small map border
        targetSmallMap.transform.Find("map border").gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        // set prover text
        proverText.text = "I have randomly picked a map.";
        vtext.text = "I have selected two adjacent fields, please click reveal to show me. ";

    }

    public void reveal(){
        GameObject targetSmallMap = pickedSmallMap;

        // select a random neighbour
        int randomNumber = UnityEngine.Random.Range(0, neighbourFirst.Length);
        GameObject neighbour_1 = neighbourFirst[randomNumber];
        GameObject neighbour_2 = neighbourSecond[randomNumber];

        // get smallmap block color
        Color color_1 = targetSmallMap.transform.Find(neighbour_1.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
        Color color_2 = targetSmallMap.transform.Find(neighbour_2.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;

        // set as small block color
        neighbour_1.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_1;
        neighbour_2.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_2;

        // calculate score
        if (color_1 == color_2){
            revealTimes = 0;
        }else{
            revealTimes ++;
        }

        // get confidence
        revealCounter ++;
        getConfidence(verifiertext);

        // end condition
        if (revealTimes == 0){
            TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
            revealtext.text = revealtext.text + "\nThe verifier found two blocks of the same color!";
            pickBtn.interactable = false;
            revealBtn.interactable = false;
            keepBtn.interactable = false;

        }

        if (revealTimes == maxRevealTime){
            TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
            revealtext.text = revealtext.text + "\nYou convinced your opponent!";
            pickBtn.interactable = false;
            revealBtn.interactable = false;
            keepBtn.interactable = false;
        }




    }

    public void keepRevealing(){
        StartCoroutine(RunKeep());
    }



    IEnumerator RunKeep(){
        while (revealCounter <= maxRevealTime)
        {
            pickMap();
            reveal();
            if (revealTimes == 0){
                TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
                revealtext.text = revealtext.text + "\nThe verifier found two blocks of the same color!";
                break;
            }

            if (revealTimes == maxRevealTime){
                TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
                revealtext.text = revealtext.text + "\nYou convinced your opponent!";
                break;
            }

            yield return new WaitForSeconds(0.35f);
        }

    }







    public void saveSelect(){
        List <GameObject> targetSmallMapList = SmallMapController.GetSelectedSmallMapList();
        if (targetSmallMapList.Count == 0){return;}

        showbtn.interactable = false;

        StartCoroutine(RunFunctionEverySecond());

    }

    // run 30 times
    IEnumerator RunFunctionEverySecond(){
        while (revealCounter <= maxRevealTime)
        {
            // Call your function here
            revealOnePair();
            getConfidence(verifiertext);

            if (revealTimes == 0){
                TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
                revealtext.text = revealtext.text + "\nThe verifier found two blocks of the same color!";
                break;
            }

            if (revealTimes == maxRevealTime){
                TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
                revealtext.text = revealtext.text + "\nYou convinced your opponent!";
                break;
            }


            revealCounter++;
            yield return new WaitForSeconds(0.35f);
        }

    }



    // reveal one pair neighbour 
    public void revealOnePair(){

        // get selected smallmap
        // GameObject targetSmallMap = SmallMapController.GetSelectedSmallMap();

        // randomly get a small map
        List <GameObject> targetSmallMapList = SmallMapController.GetSelectedSmallMapList();
        if (targetSmallMapList.Count == 0){return;}
        int smallmapIndex = UnityEngine.Random.Range(0, targetSmallMapList.Count);
        GameObject targetSmallMap = targetSmallMapList[smallmapIndex];


        if (targetSmallMap==null){return;}
        // save smallmap index
        string targetSmallMapIndexString = targetSmallMap.name.Split(' ')[2];
        int targetSmallMapIndex = int.Parse(targetSmallMapIndexString);


        // count times
        smallMapRecord[targetSmallMapIndex] = smallMapRecord[targetSmallMapIndex] + 1;
        // Debug.Log("Array Contents: " + string.Join(", ", smallMapRecord));

        // reset all white
        for(int i = 0; i < mapBlocks.Length; i++){
            mapBlocks[i].GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = Color.white;
        }
        
        // foreach (GameObject smallmap in smallMaps){
        //     smallmap.transform.Find("map border").gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        // }

        // set rest as shadow color
        foreach(GameObject mapBlock in mapBlocks){
            Color smallmapBlockColor = targetSmallMap.transform.Find(mapBlock.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
            Color reducedOpacityColor = new Color(smallmapBlockColor.r, smallmapBlockColor.g, smallmapBlockColor.b, smallmapBlockColor.a * 0.2f);
            mapBlock.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = reducedOpacityColor;

        }

        // select a random neighbour
        int randomNumber = UnityEngine.Random.Range(0, neighbourFirst.Length);
        GameObject neighbour_1 = neighbourFirst[randomNumber];
        GameObject neighbour_2 = neighbourSecond[randomNumber];


        // select a random smallmap
        // GameObject targetSmallMap = smallMaps[UnityEngine.Random.Range(0, smallMaps.Length)];

        // highlight the small map
        // targetSmallMap.transform.Find("map border").gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        // get smallmap block color
        Color color_1 = targetSmallMap.transform.Find(neighbour_1.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;
        Color color_2 = targetSmallMap.transform.Find(neighbour_2.name).gameObject.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color;

        // set as small block color
        neighbour_1.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_1;
        neighbour_2.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = color_2;




        // calculate score
        if (color_1 == color_2){
            revealTimes = 0;
        }else{
            revealTimes ++;
        }

    }



    public void callAutoFill(){
        this.GetComponent<ColorCombation>().autoFillMap(mapBlocks);
    }


    //leakage
    private string getLeakage(){
        // max standardDeviation
        float MaxStandardDeviation = 15f;
        float MinStandardDeviation = 0.2f;
        

        float mean = 0f;
        foreach (float value in smallMapRecord)
        {
            mean += value;
        }
        mean /= smallMapRecord.Length;

        float squaredDifferencesSum = 0f;
        foreach (float value in smallMapRecord)
        {
            float difference = value - mean;
            squaredDifferencesSum += difference * difference;
        }

        float variance = squaredDifferencesSum / smallMapRecord.Length;
        float standardDeviation = Mathf.Sqrt(variance);

        float leakage = Mathf.Max(MinStandardDeviation, standardDeviation)/Mathf.Max(MaxStandardDeviation, standardDeviation);
        string leakageString = (leakage * 100f).ToString("F2") + "%";


        // Debug.Log(leakageString);
        return leakageString;
    }


    private string LeakageDegree(){
        // int SelectedSmallMapListLength = SmallMapController.GetSelectedSmallMapList().Count;
        string zeroString = "Zero";
        string lowString = "Low";
        string mediumString = "Medium";
        string highString = "High";

        // switch (SelectedSmallMapListLength)
        // int caseInt = permuteIndex;
        switch (permuteIndex)
        {
            case 1:
                if (revealCounter <= 1){return zeroString;}
                if (revealCounter <= 2){return lowString;}
                if (revealCounter <= 3){return mediumString;}
                if (revealCounter > 3){return highString;}
                break;

            case 2:
                if (revealCounter <= 1){return zeroString;}
                if (revealCounter <= 2){return lowString;}
                if (revealCounter <= 5){return mediumString;}
                if (revealCounter > 5){return highString;}
                break;

            case 3:
                if (revealCounter <= 1){return zeroString;}
                if (revealCounter <= 3){return lowString;}
                if (revealCounter <= 7){return mediumString;}
                if (revealCounter > 7){return highString;}
                break;

            case 4:
                if (revealCounter <= 1){return zeroString;}
                if (revealCounter <= 4){return lowString;}
                if (revealCounter <= 11){return mediumString;}
                if (revealTimes > 11){return highString;}
                break;

            case 5:
                if (revealCounter <= 1){return zeroString;}
                if (revealCounter <= 5){return lowString;}
                if (revealCounter <= 20){return mediumString;}
                if (revealTimes > 20){return highString;}
                break;

            case 6:
                return zeroString;
                break;
            

            default:
                return "error";
        }

        return "error";

    }

    // confidence
    public void getConfidence(Transform btn_text){
        TextMeshProUGUI revealtext = revealTimeText.GetComponent<TextMeshProUGUI>();
        revealtext.text = "The number of reveals: " + revealCounter.ToString();

        // 1-(1-1/E)^n, E = 12, n = times
        TextMeshProUGUI myTextMeshPro = btn_text.GetComponent<TextMeshProUGUI>();

        string resultString;
        // get score
        float confidenceScore = 1f - Mathf.Pow(1f-(1f / 12f), revealTimes);
        string percentageString = (confidenceScore * 100f).ToString("F2") + "%";

        string confidenceString = "Confidence: " + percentageString;

        string leakageString = "Leakage: " + LeakageDegree();

        resultString = confidenceString +"\n"+leakageString;

        myTextMeshPro.text = resultString;
        
    }







}
