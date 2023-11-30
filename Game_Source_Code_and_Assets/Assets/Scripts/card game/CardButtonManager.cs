using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CardButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     // reset game
    public void restartGame()
    {
        SceneManager.LoadScene("CardScene");
    }

    //
    public void backToMenu(){
        SceneManager.LoadScene("MainScene");
    }



    // verifier restart
    public void restartGameVerifier()
    {
        SceneManager.LoadScene("CardVerifier");
    }
}
