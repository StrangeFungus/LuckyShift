using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PingPongAlphaEffect : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    private readonly float targetAlpha = 0.5f;
    private readonly float totalDuration = 0.25f;
    private Color originalColor;

    private void Awake()
    {
        // GET THE ORIGINAL COLOR AND SET THE ALPHA VALUE TO ZERO TO START WITH.
        originalColor = uiImage.color;
        uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        StartCoroutine(DoPingPongFade());
    }

    private IEnumerator DoPingPongFade()
    {
        // DIVIDE THE DURATION IN HALF, TO FADE IN AND THEN OUT.
        float halfDuration = totalDuration * 0.5f;

        // FADE IN
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / halfDuration;
            float alpha = Mathf.Lerp(0f, targetAlpha, Mathf.Clamp01(elapsedTime));
            uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // FADE OUT
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / halfDuration;
            float alpha = Mathf.Lerp(targetAlpha, 0f, Mathf.Clamp01(elapsedTime));
            uiImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // DESTROY THE SPAWNED IN EFFECT
        Destroy(gameObject);
    }
}
