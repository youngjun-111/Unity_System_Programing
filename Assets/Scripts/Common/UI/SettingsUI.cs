using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SettingsUI : BaseUI
{
    //���� ������ ǥ������ �ؽ�Ʈ ���۳�Ʈ
    public TextMeshProUGUI GameVersionTxt;
    //���尡 On �϶� Ȱ��ȭ ���� UI ������Ʈ
    public GameObject SoundOnToggle;
    //���尡 Off�϶� Ȱ��ȭ ���� UI ������Ʈ
    public GameObject SoundOffToggle;
    //������å ���
    //�ؽ�Ʈ�� ������ �ش� ������Ʈ ��ũ�� �̵�
    const string PRIVACY_POLICY_URL = "https : // www.naver.com/";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //���� ���� ǥ�� �Լ�
        SetGameVersion();
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();

        if(userSettingsData != null)
        {
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //���� ���� ǥ�� �Լ�
    void SetGameVersion()
    {
        GameVersionTxt.text = $"Version : {Application.version}";
    }

    void SetSoundSetting(bool sound)
    {
        SoundOnToggle.SetActive(sound);
        SoundOffToggle.SetActive(!sound);
    }

    //���������� ������ �� ���� ������ off�� �ٲ��ִ� �Լ�
    public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");
        //UIŬ�� �� ���� �÷���
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        //���� ������ ������
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        //��ȿ�� �˻�
        if(userSettingsData != null)
        {
            userSettingsData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //off�� �������� On���� �ٲ��ִ� �Լ�
    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOffToggle");
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingsData != null)
        {
            userSettingsData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.UnMute();
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    public void OnClickPrivacyPolicyURL()
    {
        Logger.Log($"{GetType()}::OnClickPrivacyPolicyURL");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        Application.OpenURL(PRIVACY_POLICY_URL);
    }
}
