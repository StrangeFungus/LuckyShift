using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private Image gameOverScreenImage;

    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private Image restartGameButtonImage;
    [SerializeField] private TextMeshProUGUI restartGameButtonText;

    [SerializeField] private Image easyDifficultyButtonImage;
    [SerializeField] private TextMeshProUGUI easyDifficultyButtonText;

    [SerializeField] private Image mediumDifficultyButtonImage;
    [SerializeField] private TextMeshProUGUI mediumDifficultyButtonText;

    [SerializeField] private Image hardDifficultyButtonImage;
    [SerializeField] private TextMeshProUGUI hardDifficultyButtonText;

    [SerializeField] private Image mainMenuButtonImage;
    [SerializeField] private TextMeshProUGUI mainMenuButtonText;

    private void Start()
    {
        SetUIAlpha(0f);
    }

    public void ActivateGameOverScreen()
    {
        SetUIAlpha(0f);

        easyDifficultyButtonImage.gameObject.SetActive(true);
        mediumDifficultyButtonImage.gameObject.SetActive(true);
        hardDifficultyButtonImage.gameObject.SetActive(true);
        switch (DifficultySettings.difficulty)
        {
            case 0:
                easyDifficultyButtonImage.gameObject.SetActive(false);
                break;
            case 1:
                mediumDifficultyButtonImage.gameObject.SetActive(false);
                break;
            case 2:
                hardDifficultyButtonImage.gameObject.SetActive(false);
                break;
        }

        gameObject.SetActive(true);

        StartCoroutine(FadeInGameOverUI());
    }

    public void DeactivateGameOverScreen()
    {
        gameObject.SetActive(false);
        SetUIAlpha(0f);
    }

    private IEnumerator FadeInGameOverUI()
    {
        // FADE IN
        float totalDuration = 0.25f;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / totalDuration;
            float alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp01(elapsedTime));
            SetUIAlpha(alpha);
            yield return null;
        }
    }

    private void SetUIAlpha(float alphaValue) // MUST BE 0 to 1
    {
        Color colorWithAlpha = new Color(1f, 1f, 1f, alphaValue);
        gameOverScreenImage.color = colorWithAlpha;
        gameOverText.color = colorWithAlpha;
        restartGameButtonImage.color = colorWithAlpha;
        restartGameButtonText.color = colorWithAlpha;
        easyDifficultyButtonImage.color = colorWithAlpha;
        easyDifficultyButtonText.color = colorWithAlpha;
        mediumDifficultyButtonImage.color = colorWithAlpha;
        mediumDifficultyButtonText.color = colorWithAlpha;
        hardDifficultyButtonImage.color = colorWithAlpha;
        hardDifficultyButtonText.color = colorWithAlpha;
        mainMenuButtonImage.color = colorWithAlpha;
        mainMenuButtonText.color = colorWithAlpha;
    }
}
