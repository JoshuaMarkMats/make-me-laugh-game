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
    private float gameOverDuration = 3f;
    [SerializeField]
    private float gameWinDuration = 3f;
    [SerializeField]
    private CinemachineVirtualCamera childVirtualCamera;

    public void GameOver()
    {
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
