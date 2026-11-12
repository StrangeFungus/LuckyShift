using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private Image gameOverScreenImage;

    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private Button restartGameButton;
    [SerializeField] private TextMeshProUGUI restartGameButtonText;

    [SerializeField] private Button easyDifficultyButton;
    [SerializeField] private TextMeshProUGUI easyDifficultyButtonText;

    [SerializeField] private Button mediumDifficultyButton;
    [SerializeField] private TextMeshProUGUI mediumDifficultyButtonText;

    [SerializeField] private Button hardDifficultyButton;
    [SerializeField] private TextMeshProUGUI hardDifficultyButtonText;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI mainMenuButtonText;

    private void Start()
    {
        SetUIAlpha(0f);
        restartGameButton.interactable = false;
        easyDifficultyButton.interactable = false;
        mediumDifficultyButton.interactable = false;
        hardDifficultyButton.interactable = false;
        mainMenuButton.interactable = false;
    }

    public void ActivateGameOverScreen()
    {
        SetUIAlpha(0f);

        easyDifficultyButton.gameObject.SetActive(true);
        mediumDifficultyButton.gameObject.SetActive(true);
        hardDifficultyButton.gameObject.SetActive(true);
        switch (DifficultySettings.difficulty)
        {
            case 0:
                easyDifficultyButton.gameObject.SetActive(false);
                break;
            case 1:
                mediumDifficultyButton.gameObject.SetActive(false);
                break;
            case 2:
                hardDifficultyButton.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(5f);

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

        restartGameButton.interactable = true;
        easyDifficultyButton.interactable = true;
        mediumDifficultyButton.interactable = true;
        hardDifficultyButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    private void SetUIAlpha(float alphaValue) // MUST BE 0 to 1
    {
        Color colorWithAlpha = new Color(1f, 1f, 1f, alphaValue);
        gameOverScreenImage.color = colorWithAlpha;
        gameOverText.color = colorWithAlpha;
        restartGameButton.image.color = colorWithAlpha;
        restartGameButtonText.color = colorWithAlpha;
        easyDifficultyButton.image.color = colorWithAlpha;
        easyDifficultyButtonText.color = colorWithAlpha;
        mediumDifficultyButton.image.color = colorWithAlpha;
        mediumDifficultyButtonText.color = colorWithAlpha;
        hardDifficultyButton.image.color = colorWithAlpha;
        hardDifficultyButtonText.color = colorWithAlpha;
        mainMenuButton.image.color = colorWithAlpha;
        mainMenuButtonText.color = colorWithAlpha;
    }
}
