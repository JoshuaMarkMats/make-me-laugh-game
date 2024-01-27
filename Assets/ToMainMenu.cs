using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    public int seconds = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(1);
    }
}
