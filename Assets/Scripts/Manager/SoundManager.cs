using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour, IEngineComponent
{
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static SoundManager _instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup voiceGroup;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // 배경음악용
    [SerializeField] private AudioSource sfxSource;   // 효과음용
    [SerializeField] private AudioSource voiceSource; // 음성용

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioClip[] voiceClips;

    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1.0f;
    [SerializeField] private float musicVolume = 0.8f;
    [SerializeField] private float sfxVolume = 1.0f;
    [SerializeField] private float voiceVolume = 1.0f;

    [Header("Settings")]
    [SerializeField] private bool isMuted = false;

    // AudioMixer 파라미터 이름들
    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";
    private const string VOICE_VOLUME_PARAM = "VoiceVolume";

    // 사운드 딕셔너리 (이름으로 빠른 접근)
    private Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> voiceDictionary = new Dictionary<string, AudioClip>();

    public IEngineComponent Init()
    {
        InitializeAudioSources();
        InitializeAudioDictionaries();
        ApplyVolumeSettings();
        
        return this;
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void InitializeAudioSources()
    {
        // AudioSource가 없으면 자동으로 생성
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (voiceSource == null)
        {
            GameObject voiceObj = new GameObject("VoiceSource");
            voiceObj.transform.SetParent(transform);
            voiceSource = voiceObj.AddComponent<AudioSource>();
            voiceSource.loop = false;
            voiceSource.playOnAwake = false;
        }

        // AudioMixerGroup 설정
        if (audioMixer != null)
        {
            if (musicGroup != null) musicSource.outputAudioMixerGroup = musicGroup;
            if (sfxGroup != null) sfxSource.outputAudioMixerGroup = sfxGroup;
            if (voiceGroup != null) voiceSource.outputAudioMixerGroup = voiceGroup;
        }
    }

    private void InitializeAudioDictionaries()
    {
        // Music Dictionary 초기화
        musicDictionary.Clear();
        foreach (AudioClip clip in musicClips)
        {
            if (clip != null)
            {
                musicDictionary[clip.name] = clip;
            }
        }

        // SFX Dictionary 초기화
        sfxDictionary.Clear();
        foreach (AudioClip clip in sfxClips)
        {
            if (clip != null)
            {
                sfxDictionary[clip.name] = clip;
            }
        }

        // Voice Dictionary 초기화
        voiceDictionary.Clear();
        foreach (AudioClip clip in voiceClips)
        {
            if (clip != null)
            {
                voiceDictionary[clip.name] = clip;
            }
        }
    }

    private void ApplyVolumeSettings()
    {
        if (audioMixer != null)
        {
            // AudioMixer를 통한 볼륨 제어
            if (isMuted)
            {
                SetMixerVolume(MASTER_VOLUME_PARAM, -80f); // 완전 음소거
            }
            else
            {
                SetMixerVolume(MASTER_VOLUME_PARAM, ConvertToDecibel(masterVolume));
                SetMixerVolume(MUSIC_VOLUME_PARAM, ConvertToDecibel(musicVolume));
                SetMixerVolume(SFX_VOLUME_PARAM, ConvertToDecibel(sfxVolume));
                SetMixerVolume(VOICE_VOLUME_PARAM, ConvertToDecibel(voiceVolume));
            }
        }
        else
        {
            // 기존 방식 (AudioMixer 없을 때)
            if (isMuted)
            {
                musicSource.volume = 0f;
                sfxSource.volume = 0f;
                voiceSource.volume = 0f;
            }
            else
            {
                musicSource.volume = masterVolume * musicVolume;
                sfxSource.volume = masterVolume * sfxVolume;
                voiceSource.volume = masterVolume * voiceVolume;
            }
        }
    }

    // 볼륨을 데시벨로 변환 (AudioMixer용)
    private float ConvertToDecibel(float volume)
    {
        if (volume <= 0f) return -80f; // 완전 음소거
        return Mathf.Log10(volume) * 20f;
    }

    // AudioMixer 파라미터 설정
    private void SetMixerVolume(string parameterName, float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(parameterName, value);
        }
    }

    // AudioMixer 파라미터 가져오기
    private float GetMixerVolume(string parameterName)
    {
        if (audioMixer != null)
        {
            float value;
            if (audioMixer.GetFloat(parameterName, out value))
            {
                return value;
            }
        }
        return 0f;
    }

    // === Music Methods ===
    public void PlayMusic(string musicName)
    {
        if (isMuted || !musicDictionary.ContainsKey(musicName)) return;

        musicSource.clip = musicDictionary[musicName];
        musicSource.Play();
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (isMuted || musicClip == null) return;

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    // === SFX Methods ===
    public void PlaySFX(string sfxName)
    {
        if (isMuted || !sfxDictionary.ContainsKey(sfxName)) return;

        sfxSource.PlayOneShot(sfxDictionary[sfxName]);
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (isMuted || sfxClip == null) return;

        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlaySFX(string sfxName, float volumeScale)
    {
        if (isMuted || !sfxDictionary.ContainsKey(sfxName)) return;

        sfxSource.PlayOneShot(sfxDictionary[sfxName], volumeScale);
    }

    // === Voice Methods ===
    public void PlayVoice(string voiceName)
    {
        if (isMuted || !voiceDictionary.ContainsKey(voiceName)) return;

        voiceSource.clip = voiceDictionary[voiceName];
        voiceSource.Play();
    }

    public void PlayVoice(AudioClip voiceClip)
    {
        if (isMuted || voiceClip == null) return;

        voiceSource.clip = voiceClip;
        voiceSource.Play();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
    }

    // === Volume Control Methods (AudioMixer 지원) ===
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetVoiceVolume(float volume)
    {
        voiceVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    // === AudioMixer 전용 메서드들 ===
    public void SetMixerMasterVolume(float volume)
    {
        if (audioMixer != null)
        {
            SetMixerVolume(MASTER_VOLUME_PARAM, ConvertToDecibel(volume));
        }
    }

    public void SetMixerMusicVolume(float volume)
    {
        if (audioMixer != null)
        {
            SetMixerVolume(MUSIC_VOLUME_PARAM, ConvertToDecibel(volume));
        }
    }

    public void SetMixerSFXVolume(float volume)
    {
        if (audioMixer != null)
        {
            SetMixerVolume(SFX_VOLUME_PARAM, ConvertToDecibel(volume));
        }
    }

    public void SetMixerVoiceVolume(float volume)
    {
        if (audioMixer != null)
        {
            SetMixerVolume(VOICE_VOLUME_PARAM, ConvertToDecibel(volume));
        }
    }

    // === Mute Control ===
    public void SetMute(bool muted)
    {
        isMuted = muted;
        ApplyVolumeSettings();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        ApplyVolumeSettings();
    }

    // === Utility Methods ===
    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }

    public bool IsVoicePlaying()
    {
        return voiceSource.isPlaying;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetVoiceVolume()
    {
        return voiceVolume;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    // === AudioMixer 상태 확인 ===
    public bool IsAudioMixerAvailable()
    {
        return audioMixer != null;
    }

    public AudioMixer GetAudioMixer()
    {
        return audioMixer;
    }
}