using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadCard(){
        SceneManager.LoadScene("CardScene");
    }

    public void loadMap(){
        SceneManager.LoadScene("MapScene");
    }

    public void loadBlind(){
        SceneManager.LoadScene("BlindScene");
    }

    public void loadWally(){
        SceneManager.LoadScene("WallyScene");
    }
}
