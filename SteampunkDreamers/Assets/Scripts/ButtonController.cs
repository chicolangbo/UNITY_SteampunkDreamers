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



    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
