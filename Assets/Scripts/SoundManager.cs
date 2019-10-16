using UnityEngine;
using System.Collections;
//using MoreMountains.NiceVibrations;
//using DG.Tweening;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public float musicVolume = 0.3F;
    public AudioSource[] _MusicSources;

    bool _isMain = false;

    void Awake()
    {
        if (instance != null && !_isMain)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            _isMain = true;
            DontDestroyOnLoad(gameObject);
        }
    }
    /*
    public void PlayMusic (int number)
    {
        for(int i = 0; i < _MusicSources.Length; i++)
        {
            if (i == number)
            {
                _MusicSources[i].volume = 0;
                _MusicSources[i].Play();
                _MusicSources[i].DOFade(musicVolume, 1);
            }
            else
            {
                _MusicSources[i].DOFade(0, 1).OnComplete(() => { _MusicSources[i].Stop(); });
            }
        }
    }
    */

    public void PlaySound (AudioClip audioClip, float delay = 0)
    {
        StartCoroutine(Playing(audioClip, delay)); 
    }

    IEnumerator Playing (AudioClip audioClip, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        var go = new GameObject().AddComponent<AudioSource>();
        DontDestroyOnLoad(go);
        var aS = go.GetComponent<AudioSource>();
        aS.clip = audioClip;
        aS.Play();

        Destroy(go, audioClip.length * 1.1f);
    }

    /*
    public void _KeepPlaying (int SourceNumber, int ClipNumber)
    {
        if (SourceNumber < _AudioSources.Length && ClipNumber < _AudioClips.Length)
        {
            if (!_AudioSources[SourceNumber].isPlaying)
            {
                _AudioSources[SourceNumber].clip = _AudioClips[ClipNumber];
                _AudioSources[SourceNumber].Play();
            }
        }
    }
    */
}
