using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button startButton;
    public Button howToButton;
    public Button exitButton;
    public Button returnToMain;

    // Singleton instance of the game manager
    public static GameManager Instance { get; private set; }

    private const string MAIN_MENU_SCENE = "StrangeFarm";

    public enum GameState
    {
        MAIN_MENU,
        PLAYING,
        PAUSED,
        GAME_WIN,
        GAME_OVER,
        TRANSITION
    }

    public GameState gameState = GameState.MAIN_MENU;

    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject pausemenu;
    //public SpeechBox speechBox;

    public UnityEvent gameOverEvent = new();
    private bool gameFrozen;

    public bool GameFrozen { 
        get { return gameFrozen; } 
        set
        {
            if (value == true)
                FreezeGame();
            else
                UnfreezeGame();
        }
    }

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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;

        gameOverEvent.AddListener(GameOver);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
            OnPause();
    }

    void GameOver()
    {
        gameState = GameState.GAME_OVER;
        gameOverMenu.SetActive(true);
        FreezeGame();        
    }

    public void RestartLevel()
    {
        UnfreezeGame();
        //AudioManager.Instance.StopAll();
        gameOverMenu.SetActive(false);
        pausemenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
    /*  UnfreezeGame();
        //AudioManager.Instance.StopAll();
        gameOverMenu.SetActive(false);
        pausemenu.SetActive(false);
        gameState = GameState.MAIN_MENU;
        SceneManager.LoadScene(MAIN_MENU_SCENE);*/

        Debug.Log("Main Menu Open!");

    }

    /*public void GameWin()
    {
        gameState = GameState.GAME_WIN;
        UIManager.Instance.hideTimer();
        FreezeGame();
        UIManager.Instance.GameWin();
    }*/

    //toggle pause
    public void OnPause()
    {
        if (gameState != GameState.PLAYING && gameState != GameState.PAUSED)
            return;

        if (!gameFrozen)
        {
            pausemenu.SetActive(true); 
            FreezeGame();
            gameState = GameState.PAUSED;
        }
        else
        {
            pausemenu.SetActive(false);
            UnfreezeGame();
            gameState = GameState.PLAYING;
        }
    }

    public void FreezeGame()
    {
        gameFrozen = true;
        Time.timeScale = 0f;
    }

    public void UnfreezeGame()
    {
        gameFrozen = false;
        Time.timeScale = 1f;
    }

    public void SetFps(int fps)
    { 
        Application.targetFrameRate = fps;
    }

    //start gameplay from  main menu
    public void StartGame() {
        Debug.Log("Game Started");
    
    }

    //open how to play UI
    public void HowToPlay()
    {
        Debug.Log("How to Play Opened!");
    
    }

    //close game application from main menu
    public void ExitGame() {
        Debug.Log("Game Exited!");
        Application.Quit();
    
    
    }

 
}
