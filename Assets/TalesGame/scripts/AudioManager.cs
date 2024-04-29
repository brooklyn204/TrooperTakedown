using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameManager manager;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundEffectsSource;
    [SerializeField] float fadeTime;

    public AudioClip music;
    public AudioClip blast; 
    public AudioClip gasp; 
    public AudioClip grunt;

    private IEnumerator fadeIn;
    // Start is called before the first frame update
    void Start()
    {
        fadeTime = manager.gameStartPause;
        fadeIn = FadeInMusic();
        StartCoroutine(fadeIn);
    }

    void OnDisable()
    {
        StopCoroutine(fadeIn);
    }

    public IEnumerator FadeInMusic()
    {
        musicSource.clip = music;
        musicSource.volume = 0;
        musicSource.Play();
        while (musicSource.volume < 100)
        {
            musicSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    public void BlastSound()
    {
        soundEffectsSource.clip = blast;
        soundEffectsSource.Play();
    }
    
    public void AhsokaDeathSound()
    {
        soundEffectsSource.clip = gasp;
        soundEffectsSource.Play();
    }
    public void CloneDeathSound()
    {
        soundEffectsSource.clip = grunt;
        soundEffectsSource.Play();
    }
}
