using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    public enum MusicPlaylistIndex
    {
        MainMenu,
        EasyDifficulty,
        MediumDifficulty,
        HardDifficulty,
        GameOverScreen
    }

    [SerializeField] private AudioSource musicPlayerSource;
    private MusicPlaylistIndex musicIndex = MusicPlaylistIndex.MainMenu;
    [SerializeField] private List<AudioClip> mainMenuMusic;

    [SerializeField] private List<AudioClip> easyDifficultyMusic;
    [SerializeField] private List<AudioClip> mediumDifficultyMusic;
    [SerializeField] private List<AudioClip> hardDifficultyMusic;

    [SerializeField] private List<AudioClip> gameOverMenuMusic;

    private int currentTrackIndex = 0;
    private int musicTrackIndex = 0;

    private float musicTransitionTime = 0.25f;
    public bool isMusicPaused = false;

    private Coroutine WaitAndPlayNextSongCoroutineTracker = null;
    private Coroutine PlayNextSongCoroutineTracker = null;
    private Coroutine PlayNewPlaylistCoroutineTracker = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartMusic(musicIndex);
    }

    // This will select a track, start playing it and wait for the next track before it triggers the rest of the sequence.
    public void StartMusic(MusicPlaylistIndex index)
    {
        musicIndex = index;
        currentTrackIndex = 0;

        musicTrackIndex = musicIndex switch
        {
            MusicPlaylistIndex.MainMenu => mainMenuMusic.Count,
            MusicPlaylistIndex.EasyDifficulty => easyDifficultyMusic.Count,
            MusicPlaylistIndex.MediumDifficulty => mediumDifficultyMusic.Count,
            MusicPlaylistIndex.HardDifficulty => hardDifficultyMusic.Count,
            MusicPlaylistIndex.GameOverScreen => gameOverMenuMusic.Count,
            _ => mainMenuMusic.Count,
        };

        musicPlayerSource.volume = 0.22f;

        musicPlayerSource.clip = musicIndex switch
        {
            MusicPlaylistIndex.MainMenu => mainMenuMusic[currentTrackIndex],
            MusicPlaylistIndex.EasyDifficulty => easyDifficultyMusic[currentTrackIndex],
            MusicPlaylistIndex.MediumDifficulty => mediumDifficultyMusic[currentTrackIndex],
            MusicPlaylistIndex.HardDifficulty => hardDifficultyMusic[currentTrackIndex],
            MusicPlaylistIndex.GameOverScreen => gameOverMenuMusic[currentTrackIndex],
            _ => mainMenuMusic[currentTrackIndex],
        };

        if (isMusicPaused) { return; }
        else
        {
            musicPlayerSource.Play();
        }

        if (WaitAndPlayNextSongCoroutineTracker != null)
        {
            StopCoroutine(WaitAndPlayNextSongCoroutineTracker);
            WaitAndPlayNextSongCoroutineTracker = null;
        }

        WaitAndPlayNextSongCoroutineTracker ??= StartCoroutine(WaitAndPlayNextSongCoroutine());

        if (PlayNextSongCoroutineTracker != null)
        {
            StopCoroutine(PlayNextSongCoroutineTracker);
            PlayNextSongCoroutineTracker = null;
        }
    }

    private IEnumerator WaitAndPlayNextSongCoroutine()
    {
        if (isMusicPaused) { yield return null; }
        else
        {
            float clipLength = musicPlayerSource.clip.length;
            yield return new WaitForSeconds(clipLength);

            if (PlayNextSongCoroutineTracker != null)
            {
                StopCoroutine(PlayNextSongCoroutineTracker);
                PlayNextSongCoroutineTracker = null;
            }

            PlayNextSongCoroutineTracker ??= StartCoroutine(PlayNextSongCoroutine());
        }
    }

    private IEnumerator PlayNextSongCoroutine()
    {
        currentTrackIndex++;
        if (currentTrackIndex > musicTrackIndex - 1)
        {
            currentTrackIndex = 0;
        }

        float currentFadeTime = 0;
        float startingVolume = musicPlayerSource.volume;
        while (currentFadeTime < musicTransitionTime)
        {
            if (isMusicPaused) { yield break; }
            currentFadeTime += Time.deltaTime;
            float fadeTimer = currentFadeTime / musicTransitionTime;
            musicPlayerSource.volume = Mathf.Lerp(startingVolume, 0f, fadeTimer);
            yield return null;
        }

        musicPlayerSource.Stop();

        StartMusic(musicIndex);
    }

    public void StartNewPlaylist(MusicPlaylistIndex index)
    {
        if (PlayNewPlaylistCoroutineTracker != null)
        {
            StopCoroutine(PlayNewPlaylistCoroutineTracker);
            PlayNewPlaylistCoroutineTracker = null;
        }

        PlayNewPlaylistCoroutineTracker ??= StartCoroutine(PlayNewPlaylistCoroutine(index));
    }

    private IEnumerator PlayNewPlaylistCoroutine(MusicPlaylistIndex index)
    {
        float currentFadeTime = 0;
        float startingVolume = musicPlayerSource.volume;
        while (currentFadeTime < musicTransitionTime)
        {
            if (isMusicPaused) { yield break; }
            currentFadeTime += Time.deltaTime;
            float fadeTimer = currentFadeTime / musicTransitionTime;
            musicPlayerSource.volume = Mathf.Lerp(startingVolume, 0f, fadeTimer);
            yield return null;
        }

        musicPlayerSource.Stop();

        StartMusic(index);
    }
}
