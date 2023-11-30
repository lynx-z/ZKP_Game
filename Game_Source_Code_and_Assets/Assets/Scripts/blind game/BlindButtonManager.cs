using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BlindButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restartGame(){
        SceneManager.LoadScene("BlindScene");
    }

    public void restartGameVerifier(){
        SceneManager.LoadScene("BlindVerifier");
    }

    public void backToMenu(){
        SceneManager.LoadScene("MainScene");
    }



}
