using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public BgmManager Inst;

    public enum MYPOS { Home, Death, Shop, Dungeon1_1 }
    public MYPOS myState = MYPOS.Home;

    public AudioClip[] myBGM;
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
        myState = MYPOS.Home;
    }

    // Update is called once per frame
    void Update()
    {
        bgmVolume = _bgmVolume * MasterVolume;
        effectVolume = _effectVolume * MasterVolume;
        Mathf.Clamp(_bgmVolume, 0, 1.0f);
        Mathf.Clamp(_effectVolume, 0, 1.0f);

        PlayBGM(myState);
    }

    void PlayBGM(MYPOS t)
    {
        if (myState == t) return;
        myState = t;
        switch ((int)t)
        {
            case 0: //마을
                bgmSpeaker.loop = true;
                bgmSpeaker.clip = HomeBGM[(int)t];
                break;
            case 2: //상점
            case 3: //던전 1_1
                bgmSpeaker.loop = true;
                bgmSpeaker.clip = myBGM[(int)t];
                break;
            case 1: //사망
                bgmSpeaker.loop = false;
                bgmSpeaker.clip = myBGM[(int)t];
                break;
        }
        bgmSpeaker.volume = bgmVolume;
        bgmSpeaker.Play();
    }

}
