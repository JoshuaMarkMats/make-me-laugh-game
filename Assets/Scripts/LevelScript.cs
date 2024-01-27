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

    public void GameOver()
    {
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
