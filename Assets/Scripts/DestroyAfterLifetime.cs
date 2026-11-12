using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    private float lifetime = 10f;

    private void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
