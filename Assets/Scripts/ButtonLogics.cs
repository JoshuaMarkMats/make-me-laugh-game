using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogics : MonoBehaviour
{
    public GameObject howToPlay;

    public GameObject page1;
    public GameObject page2;

    [SerializeField]
    private TextMeshProUGUI currentPageText;

    private int activePage = 1;
    

    public void StartGame()
    {
        Debug.Log("Game Started!!");
        SceneManager.LoadScene("SampleScene");

    }

    public void HowToPlay()
    {
        howToPlay.SetActive(true);
        Debug.Log("Opening How To Play Instructions!!");

    }

    public void ExitGame()
    {
        Debug.Log("Game Closed!");
        Application.Quit();

    }
    public void ReturnMain()
    {
        Debug.Log("Back To Main");
        SceneManager.LoadScene("MainMenu");


    }

    public void CloseHowToPlay()
    {
        howToPlay.SetActive(false);
    }

    public void NextPage()
    {
        if (activePage == 1)
        {
            page1.SetActive(false);
            page2.SetActive(true);

            activePage = 2;
        }
        else
        {
            page1.SetActive(true);
            page2.SetActive(false);

            activePage = 1;
        }

        currentPageText.text = $"Page {activePage} of 2";
    }
}
