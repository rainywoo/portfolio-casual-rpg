using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Inst;

    public AudioClip myBGM;
    public AudioClip deathBgm;
    public AudioClip shopBgm;
    public AudioClip[] HomeBGM;

    public float MasterVolume = 1.0f;
    float bgmVolume = 1.0f;
    public float _bgmVolume = 1.0f;
    float effectVolume = 1.0f;
    public float _effectVolume = 1.0f;

    AudioSource _bgmSpeaker = null;
    public AudioSource bgmSpeaker
    {
        get
        {
            if (_bgmSpeaker == null)
            {
                _bgmSpeaker = Camera.main.GetComponent<AudioSource>();
            }
            return _bgmSpeaker;
        }
    }

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        PlayMusic(myBGM);
    }

    // Update is called once per frame
    void Update()
    {
        //bgmVolume = _bgmVolume * MasterVolume;
        //effectVolume = _effectVolume * MasterVolume;
        Mathf.Clamp(_bgmVolume, 0, 1.0f);
        Mathf.Clamp(_effectVolume, 0, 1.0f);
    }

    public void PlayMusic(AudioClip _clip, bool _true = true)
    {
        bgmSpeaker.clip = _clip;
        bgmSpeaker.loop = _true;
          
        //bgmSpeaker.volume = bgmVolume;
        bgmSpeaker.Play();
    }
}
