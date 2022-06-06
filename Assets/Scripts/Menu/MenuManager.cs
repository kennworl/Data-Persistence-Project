using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public GameObject playerName;
    public GameObject hiScoreDisplay;

    public void Start()
    {
        playerName.GetComponent<TMP_InputField>().text = "";

        hiScoreDisplay.GetComponent<TMP_Text>().text = HiScoreManager.Instance.GetAllHiScores();
    }

    // When the start button is pressed, if the name field is not empty then load the main scene
    public void StartNew()
    {
        if (playerName.GetComponent<TMP_InputField>().text != "")
        {
            HiScoreManager.Instance.playerName = playerName.GetComponent<TMP_InputField>().text;
            SceneManager.LoadScene(1);
        }
    }

    // when the quit button is pressed, save the high scores and quit the game completely.
    public void Exit()
    {
        HiScoreManager.Instance.SaveHiScores();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
