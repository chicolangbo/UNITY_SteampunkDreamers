using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    Button button;
    public void GameStart()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu Scene");
    }
    public void Upgrade()
    {
        SceneManager.LoadScene("Upgrade Scene");
    }




    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
