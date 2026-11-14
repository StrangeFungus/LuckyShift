using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasParent;

    // LIVES RELATED
    [SerializeField] private TextMeshProUGUI livesRemainingTMP;
    private static int livesRemaining = 3;
    [SerializeField] private GameObject lifeLostEffect;

    // TIME REMAINING RELATED
    [SerializeField] private Image timeRemainingBGBar;
    [SerializeField] private Image timeRemainingFGBar;
    [SerializeField] private TextMeshProUGUI timeRemainingTMP;
    private float timerDefault = 5f;
    private float timeRemaining;
    private float timeLostPerFail = 0f;

    // CURRENT LUCKY NUMBER RELATED
    [SerializeField] private TextMeshProUGUI playersCurrentLuckyNumberTMP;
    private int playersCurrentLuckyNumber;

    // SCORE RELATED
    [SerializeField] private TextMeshProUGUI highScoreTMP;
    [SerializeField] private TextMeshProUGUI currentScoreTMP;
    private int currentScore = 0;

    // CURRENT QUESTION RELATED
    [SerializeField] private TextMeshProUGUI currentQuestionTMP;
    private int minimumQuestionValue = 1;
    private int maximumQuestionValue = 13;
    [SerializeField] private TextMeshProUGUI numberRangeTMP;
    private int currentNumber = 0;
    private int nextNumber = 0;
    [SerializeField] private GameObject correctAnswerEffect;

    // GUESS BUTTONS RELATED
    [SerializeField] private Button lowerButton;
    [SerializeField] private Button higherButton;

    // BACKGROUND RELATED
    [SerializeField] private Material backgroundMaterial;
    [SerializeField] private ParticleSystem backgroundParticleSystem;

    // GAME PAUSED
    private static bool GamePaused = false;
    [SerializeField] private Button toggleExitToMainMenuOptionButton;
    [SerializeField] private GameObject exitToMainMenuOption;

    // GAME OVER
    public static bool GameOver = false;
    [SerializeField] private GameOverScreenController gameOverScreenController;
    private BackgroundMusicManager.MusicPlaylistIndex previousMusicIndex = BackgroundMusicManager.MusicPlaylistIndex.MainMenu;

    // BONUS POINTS AND BUFF
    [SerializeField] private GameObject bonusBuffIcon;
    private bool isBonusBuffActive = false;
    private Coroutine bonusBuffCoroutine;
    private float bonusBuffDuration = 5f;
    [SerializeField] private TextMeshProUGUI bonusPointsUITextPrefab;
    [SerializeField] private GameObject bonusPointsParticlesPrefab;

    [SerializeField] private GameObject lifeProtectionBuffIcon;
    private bool isLifeProtectionActive = false;

    private void Start()
    {
        ResetGameData();
    }

    private void Update()
    {
        if (GameOver || GamePaused) { return; }

        timeRemaining -= Time.deltaTime;
        timeRemainingTMP.SetText($"{timeRemaining:F1}s / {timerDefault:F1}s");
        float percentOfTimeLeft = timeRemaining / timerDefault;
        timeRemainingFGBar.fillAmount = percentOfTimeLeft;

        if (timeRemaining <= 0f)
        {
            BadGuess();
        }
    }

    public void ToggleExitMenu()
    {
        if (GamePaused)
        {
            GamePaused = false;
            exitToMainMenuOption.SetActive(false);
        }
        else
        {
            GamePaused = true;
            exitToMainMenuOption.SetActive(true);
        }
    }

    private void PickNewLuckyNumber()
    {
        playersCurrentLuckyNumber = Random.Range(minimumQuestionValue, maximumQuestionValue + 1);
        playersCurrentLuckyNumberTMP.SetText($"Lucky Number: {playersCurrentLuckyNumber}");
    }

    public void GuessLower()
    {
        if (nextNumber <= currentNumber)
        {
            GoodGuess();
        }
        else
        {
            BadGuess();
        }
    }

    public void GuessHigher()
    {
        if (nextNumber >= currentNumber)
        {
            GoodGuess();
        }
        else
        {
            BadGuess();
        }
    }

    private void GoodGuess()
    {
        currentScore++;
        StartNextRound();
        Instantiate(correctAnswerEffect, canvasParent.transform);

        if (nextNumber == playersCurrentLuckyNumber)
        {
            // Give 3 bonus points for correct activation.
            currentScore += 3;
            Instantiate(bonusPointsParticlesPrefab, canvasParent.transform);

            // Activate the bonus points splash text and particles.
            GameObject newBonusText = Instantiate(bonusPointsUITextPrefab.gameObject, canvasParent.transform);
            if (newBonusText.TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                textMeshProUGUI.SetText($"Bonus Points! +3");
            }

            // Activate a short buff that gives bonus points (1 instead of the 3 above) for each correct guess.
            isBonusBuffActive = true;
            bonusBuffDuration = 5f;
            bonusBuffIcon.SetActive(true);
            bonusBuffCoroutine ??= StartCoroutine(BonusBuffCoroutine());

            // Buff protects a life lost until the end of the buff.
            isLifeProtectionActive = true;
            lifeProtectionBuffIcon.SetActive(true);
            return;
        }

        if (isBonusBuffActive)
        {
            // Give 1 bonus points for correct activation.
            currentScore += 1;
            Instantiate(bonusPointsParticlesPrefab, canvasParent.transform);

            // Activate the bonus points splash text and particles.
            GameObject newBonusText = Instantiate(bonusPointsUITextPrefab.gameObject, canvasParent.transform);
            if (newBonusText.TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
            {
                textMeshProUGUI.SetText($"Bonus Points! +1");
            }
        }

        UpdateScores();
    }

    private IEnumerator BonusBuffCoroutine()
    {
        while (isBonusBuffActive)
        {
            if (bonusBuffDuration > 0)
            {
                bonusBuffDuration -= Time.deltaTime;
            }
            else
            {
                isBonusBuffActive = false;
                isLifeProtectionActive = false;
                bonusBuffIcon.SetActive(false);
                lifeProtectionBuffIcon.SetActive(false);
            }

            yield return null;
        }
        // The buff lasts 5 seconds and only the life protection effect ends early if it gets used.
        // We need to track the buff so if needed we can restart the coroutine for it. Or just reset the buff if still active.
    }

    private void BadGuess()
    {
        timerDefault -= timeLostPerFail;
        currentScore = 0;

        if (!isLifeProtectionActive)
        {
            livesRemaining--;
            livesRemainingTMP.SetText($"Lives: {livesRemaining}");
            Instantiate(lifeLostEffect, canvasParent.transform);
        }
        else
        {
            isLifeProtectionActive = false;
            lifeProtectionBuffIcon.SetActive(false);
        }

        if (livesRemaining == 0)
        {
            GameOver = true;
            lowerButton.interactable = false;
            higherButton.interactable = false;
            toggleExitToMainMenuOptionButton.interactable = false;
            gameOverScreenController.ActivateGameOverScreen();
            BackgroundMusicManager.Instance.StartNewPlaylist(BackgroundMusicManager.MusicPlaylistIndex.GameOverScreen);
            return;
        }

        StartNextRound();
    }

    private void UpdateScores()
    {
        currentScoreTMP.SetText($"{currentScore}");
        float highScoreToCompare = SaveLoadManager.GetCurrentHighScore();
        if (currentScore > highScoreToCompare)
        {
            SaveLoadManager.SetNewHighScore(currentScore, DifficultySettings.difficulty);
        }

        highScoreTMP.SetText($"{highScoreToCompare}");
    }

    private void StartNextRound()
    {
        timeRemaining = timerDefault;
        PickNewLuckyNumber();
        currentNumber = nextNumber;
        nextNumber = Random.Range(minimumQuestionValue, maximumQuestionValue + 1);
        currentQuestionTMP.SetText($"Is the next number higher or lower than...?;\n{currentNumber}");
        UpdateScores();
    }

    public void RestartGame()
    {
        BackgroundMusicManager.Instance.StartNewPlaylist(previousMusicIndex);
        ResetGameData();
    }

    public void ResetGameData()
    {
        DifficultySettings.GetDifficultyValues(ref livesRemaining, ref timerDefault, ref timeLostPerFail, ref maximumQuestionValue);
        PickNewLuckyNumber();
        livesRemainingTMP.SetText($"Lives: {livesRemaining}");
        numberRangeTMP.SetText($"(Number Range: {minimumQuestionValue} to {maximumQuestionValue})");
        timeRemaining = timerDefault;
        currentNumber = Random.Range(minimumQuestionValue, maximumQuestionValue + 1);
        nextNumber = Random.Range(minimumQuestionValue, maximumQuestionValue + 1);
        currentQuestionTMP.SetText($"Is the next number, higher or lower than...?;\n{currentNumber}");
        currentScore = 0;
        highScoreTMP.SetText($"{SaveLoadManager.GetCurrentHighScore()}");
        lowerButton.interactable = true;
        higherButton.interactable = true;
        toggleExitToMainMenuOptionButton.interactable = true;
        GamePaused = false;
        GameOver = false;
        gameOverScreenController.DeactivateGameOverScreen();
        UpdateScores();
    }

    public void ChangeToEasyDifficulty()
    {
        DifficultySettings.difficulty = 0;
        previousMusicIndex = BackgroundMusicManager.MusicPlaylistIndex.EasyDifficulty;
        BackgroundMusicManager.Instance.StartNewPlaylist(BackgroundMusicManager.MusicPlaylistIndex.EasyDifficulty);
        ResetGameData();
    }

    public void ChangeToMediumDifficulty()
    {
        DifficultySettings.difficulty = 1;
        previousMusicIndex = BackgroundMusicManager.MusicPlaylistIndex.MediumDifficulty;
        BackgroundMusicManager.Instance.StartNewPlaylist(BackgroundMusicManager.MusicPlaylistIndex.MediumDifficulty);
        ResetGameData();
    }

    public void ChangeToHardDifficulty()
    {
        DifficultySettings.difficulty = 2;
        previousMusicIndex = BackgroundMusicManager.MusicPlaylistIndex.HardDifficulty;
        BackgroundMusicManager.Instance.StartNewPlaylist(BackgroundMusicManager.MusicPlaylistIndex.HardDifficulty);
        ResetGameData();
    }

    public void ReturnToMainMenu()
    {
        previousMusicIndex = BackgroundMusicManager.MusicPlaylistIndex.MainMenu;
        BackgroundMusicManager.Instance.StartNewPlaylist(BackgroundMusicManager.MusicPlaylistIndex.MainMenu);
        SceneManager.LoadScene(0);
    }
}
