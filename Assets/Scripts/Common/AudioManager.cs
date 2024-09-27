using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BGM
{
    lobby,
    COUNT,
}
public enum SFX
{
    chapter_clear,
    stage_clear,
    ui_button_click,
    COUNT,
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    //����� ��� ������Ʈ�� �������� ������ ����
    public Transform BGMTrs;
    public Transform SFXTrs;
    //����� ������ �ε��� ���
    const string AUDIO_PATH = "Audio";
    //��� BGM ����� ���ҽ��� ������ �����̳�
    Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    //BGM�����ؼ� ���� ����ϰ� �ִ� ����� �ҽ� ������Ʈ
    AudioSource m_CurrBGMSource;
    //��� SFX ����� ���ҽ��� ������ �����̳�
    Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    //�����ϴ� ��� BGM���� ����� ��ȸ �ϸ鼭 ���� ���ӿ�����Ʈ�� �����
    //�� ������Ʈ�� ����� �ҽ� ������Ʈ�� �ٿ��ְ� �ش� ������ ����
    void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = BGMTrs;

            m_BGMPlayer[(BGM)i] = newAudioSource;
        }
    }
    //bgm�� ���� ������ �ۼ�
    void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = SFXTrs;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    //BGM�÷��� �Լ�
    public void PlayBGM(BGM bgm)
    {
        //���� ����ǰ� �ִ� BGM�ҽ��� �ִٸ�
        //����� ���߰� null������ �ʱ�ȭ
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }
        //����ϰ� ���� BGM�� �����ϴ��� Ȯ��
        //�������� �ʴٸ� ������ �߻�
        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Logger.LogError($"Invalid clip name. {bgm}");
            return;
        }
        //�����Ѵٸ� �ش� ������ҽ� ������Ʈ�� ���������ְ�
        //���
        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    //bgm �Ͻ�����
    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }

    //bgm �����
    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }

    //bgm �ƿ� ����
    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }

    //sfx �÷���
    public void PlaySFX(SFX sfx)
    {
        //����ϰ� ���� BGM�� �����ϴ��� Ȯ��
        //�������� �ʴٸ� ������ �߻�
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Logger.LogError($"Invalid clip name. {sfx}");
            return;
        }
        //���
        m_SFXPlayer[sfx].Play();
    }

    public void OnLoadUserData()
    {
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingsData != null)
        {
            if (!userSettingsData.Sound)
            {
                Mute();
            }
        }
    }

    //ȿ������ ª�� �ð��� ����ǰ� �����Ƿ� ���� �Ͻ����� ���� �ʿ���� ��Ʈ�� ������
    //Mute
    public void Mute()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }

    //UnMute
    public void UnMute()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
}
