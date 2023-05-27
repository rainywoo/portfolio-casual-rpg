using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Inst;

    public enum MYPOS { Default, Home, Death, Shop, Dungeon }
    public MYPOS myState = MYPOS.Home;

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
        bgmSpeaker.clip = myBGM;
        bgmSpeaker.Play();
    }

    // Update is called once per frame
    void Update()
    {
        bgmVolume = _bgmVolume * MasterVolume;
        effectVolume = _effectVolume * MasterVolume;
        Mathf.Clamp(_bgmVolume, 0, 1.0f);
        Mathf.Clamp(_effectVolume, 0, 1.0f);
    }

    public void ChangeStateBGM(MYPOS t)
    {
        if (myState == t) return;
        myState = t;
        switch (t)
        {
            case MYPOS.Home: //마을
            case MYPOS.Dungeon:
                bgmSpeaker.loop = true;
                bgmSpeaker.clip = myBGM;
                break;
            case MYPOS.Shop: //상점
                bgmSpeaker.loop = true;
                bgmSpeaker.clip = shopBgm;
                break;
            case MYPOS.Death: //사망
                bgmSpeaker.loop = false;
                bgmSpeaker.clip = deathBgm;
                break;
        }
        bgmSpeaker.volume = bgmVolume;
        bgmSpeaker.Play();
    }

}
