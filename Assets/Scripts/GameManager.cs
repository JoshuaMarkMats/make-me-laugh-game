using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the game manager

    public static GameManager Instance { get; private set; }

    public UnityEvent game_overEvent = new();
    public UnityEvent game_winEvent = new();

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //game_overEvent.AddListener(CodeBlockManager.Instance.EraseData);
        game_winEvent.AddListener(winNow);
        game_overEvent.AddListener(loseNow);


    }

    public void winNow()
    {
        SceneManager.LoadScene("WinScene");


    }

    public void loseNow()
    {
        SceneManager.LoadScene("LoseScene");


    }

}