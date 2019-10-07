using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadTitle()  {
        SceneManager.LoadScene("title");
    }    
    public void loadGame()  {
        SceneManager.LoadScene("scene1");
    }
    public void loadCredits()  {
        SceneManager.LoadScene("credits");
    }    
}
