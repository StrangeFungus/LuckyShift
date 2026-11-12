using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI easyDifficultyHighScoreValueTMP;
    [SerializeField] private TextMeshProUGUI mediumDifficultyHighScoreValueTMP;
    [SerializeField] private TextMeshProUGUI hardDifficultyHighScoreValueTMP;

    [SerializeField] private TextMeshProUGUI creditsTMP;
    private static readonly string[] credits = new string[]
        {
            "All assets were drawn by myself 'Strange Fungus Video Game Studios' in Aseprite.",
            "This is my first full video game I've ever made!",
            "I used Googles AI to ask questions about the Shader Graph and how to use it properly.\n It was a lot of fun starting to learn about that!",
            "I used Googles AI to come up with some simple ideas for a game I could make.",
            "Full code files can be found on GitHub at: ",
            "No AI was used to generate code files.",
        };

    private void OnEnable()
    {
        SaveLoadManager.LoadSavedGame();
        int easyDifficultyHighScore = 0;
        int mediumDifficultyHighScore = 0;
        int hardDifficultyHighScore = 0;
        SaveLoadManager.GetAllHighScores(ref easyDifficultyHighScore, ref mediumDifficultyHighScore, ref hardDifficultyHighScore);
        easyDifficultyHighScoreValueTMP.SetText($"{easyDifficultyHighScore}");
        mediumDifficultyHighScoreValueTMP.SetText($"{mediumDifficultyHighScore}");
        hardDifficultyHighScoreValueTMP.SetText($"{hardDifficultyHighScore}");
        StartCoroutine(CycleThroughCredits());
    }

    private IEnumerator CycleThroughCredits()
    {
        int currentCredit = 0;
        int totalCredits = credits.Length - 1;
        while (true)
        {
            creditsTMP.SetText(credits[currentCredit]);

            currentCredit++;
            if ( currentCredit > totalCredits )
            {
                currentCredit = 0;
            }

            yield return new WaitForSeconds(5f);
        }
    }

    public void StartEasyMode()
    {
        DifficultySettings.difficulty = 0;
        SceneManager.LoadScene(1);
    }

    public void StartMediumMode()
    {
        DifficultySettings.difficulty = 1;
        SceneManager.LoadScene(1);
    }

    public void StartHardMode()
    {
        DifficultySettings.difficulty = 2;
        SceneManager.LoadScene(1);
    }

    public void OpenSaveFileLocation()
    {
        SaveLoadManager.OpenSavedGameFileLocation();
    }
}
