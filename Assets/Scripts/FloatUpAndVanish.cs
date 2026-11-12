using TMPro;
using UnityEngine;

public class FloatUpAndVanish : MonoBehaviour
{
    private float lifeCycleTime = 3f;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private Color originalColor;

    private void Start()
    {
        originalColor = textMeshProUGUI.color;
    }

    private void Update()
    {
        float newYPos = transform.position.y + Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newYPos);

        if (lifeCycleTime > 0 )
        {
            lifeCycleTime -= Time.deltaTime;
            if (lifeCycleTime < 1f)
            {
                textMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, lifeCycleTime);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
