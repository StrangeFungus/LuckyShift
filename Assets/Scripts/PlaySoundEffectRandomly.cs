using System.Collections;
using UnityEngine;

public class PlaySoundEffectRandomly : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private float timeToWait = 3f;
    private float minTimeToWait = 1f;
    private float maxTimeToWait = 10f;

    private void Start()
    {
        StartCoroutine(PlaySoundsRandomly());
    }

    private IEnumerator PlaySoundsRandomly()
    {
        timeToWait = Random.Range(minTimeToWait, maxTimeToWait);

        while (true)
        {
            yield return new WaitForSeconds(timeToWait);

            timeToWait = Random.Range(minTimeToWait, maxTimeToWait);

            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
