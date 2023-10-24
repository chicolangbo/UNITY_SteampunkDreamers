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
        SceneManager.LoadScene("PlayScene 1");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu Scene 1");
    }
    public void Upgrade()
    {
        SceneManager.LoadScene("Upgrade Scene 1");
    }
    public void Option()
    {
        SceneManager.LoadScene("Option Scene 1");
    }
    public void CloseGame()
    {
        Application.Quit();
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
