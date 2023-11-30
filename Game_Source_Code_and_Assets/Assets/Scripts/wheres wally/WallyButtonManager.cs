using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class WallyButtonManager : MonoBehaviour
{
    public GameObject board;
    public GameObject hole;

    private Vector3 smallScale = new Vector3(3,2,1);
    private Vector3 mediumScale = new Vector3(6,3,1);
    private Vector3 largeScale = new Vector3(9,5,1);

    private Vector3 correctholePosition = new Vector3(-3.67f, 2.75f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartGame(){
        SceneManager.LoadScene("WallyScene");
    }
    public void restartGameVerifier(){
        SceneManager.LoadScene("WallyVerifier");
    }

    public void backToMenu(){
        SceneManager.LoadScene("MainScene");
    }

    public void Smallboard(){
        board.transform.localScale = smallScale;

    }

    public void Mediumboard(){
        board.transform.localScale = mediumScale;
        
    }

    public void Largeboard(){
        board.transform.localScale = largeScale;
        
    }

    // hint button
    public void hint(GameObject userCircle){
        userCircle.transform.localPosition = correctholePosition;
    }

    //save button
    public void saveLocation(GameObject userCircle){
        // change position
        hole.transform.localPosition = userCircle.transform.localPosition;

    }
}
