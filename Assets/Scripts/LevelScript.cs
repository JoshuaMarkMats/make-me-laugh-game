using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelScript : MonoBehaviour
{
    public UnityEvent bossKillEvent = new();
    public UnityEvent clearEnemiesEvent = new();

    public bool IsGameWon = false;

    [Space()]

    [SerializeField]
    private float gameOverDuration = 5f;
    [SerializeField]
    private float gameWinDuration = 5f;
    [SerializeField]
    private CinemachineVirtualCamera childVirtualCamera;
    [SerializeField]
    private Child child;

    [Space()]
    [SerializeField]
    private string gameplayTheme = "GameplayTheme";
    [SerializeField]
    private string winTheme = "WinTheme";
    [SerializeField]
    private string loseTheme = "LoseTheme";

    private void Start()
    {
        AudioManager.Instance.Play(gameplayTheme);
    }

    public void GameOver()
    {
        AudioManager.Instance.Stop(gameplayTheme);
        AudioManager.Instance.Play(loseTheme);
        child.Kill();
        bossKillEvent.RemoveAllListeners();
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        childVirtualCamera.Priority = 20;
        yield return new WaitForSeconds(gameOverDuration);
        GameManager.Instance.game_overEvent.Invoke();
    }

    public void GameWin()
    {
        AudioManager.Instance.Stop(gameplayTheme);
        AudioManager.Instance.Play(winTheme);
        child.MakeHappy();
        IsGameWon = true;
        clearEnemiesEvent.Invoke();
        clearEnemiesEvent.RemoveAllListeners();
        StartCoroutine(GameWinSequence());
    }

    private IEnumerator GameWinSequence()
    {
        childVirtualCamera.Priority = 20;
        yield return new WaitForSeconds(gameWinDuration);
        GameManager.Instance.game_winEvent.Invoke();
    }
}
