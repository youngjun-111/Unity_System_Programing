using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IUserData ������ ���������� ǥ���ؼ� �������̽� ������ ��������
//�ڵ����� �������̽� �Լ� ����
public class UserSettingsData : IUserData
{
    //���� on / off ����
    public bool Sound { get; set; }

    public void SetDefaultData()
    {
        //GetType()�� ȣ���� Ŭ������ ����ϰ� �Լ����� �״�� ���
        Logger.Log($"{GetType()}::SetDefaultData");

        Sound = true;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
        try
        {
            Sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
            result = true;

            Logger.Log($"Sound:{Sound}");
        }
        catch (System.Exception e)
        {
            Logger.Log($"Load failed (" + e.Message + ")");
        }

        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::Save Data");

        bool result = false;
        try
        {
            //���尡 Ʈ��� 1, �޽��� 0
            PlayerPrefs.SetInt("Sound", Sound ? 1 : 0);
            PlayerPrefs.Save();
            result = true;
            Logger.Log($"Sound : {Sound}");
        }
        catch(System.Exception e)
        {
            Logger.Log("Save failed(" + e.Message + ")");
        }

        return result;
    }
}
