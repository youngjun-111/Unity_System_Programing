using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserPlayData : IUserData
{
    public int MaxClearedChapter { get; set; }
    //���� ������ �������� é�ʹ� ���� �÷��̾��������� ������ ������
    //���ӿ� �����ؼ� �����͸� �ε��� �� ������ �÷��� ������ �ְ� é�ͷ� �ڵ����� �������ְ�
    //���� �����߿��� �� ������ �����ϵ��� �ϰ���
    //not saved to playerprefs
    public int SelectedChapter { get; set; } = 1;

    //�ε� ó�� �Լ�
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //���� �Ǿ� �ִ� ���� �ε�
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            //������ �÷��� ������ ���� ���� é�ͷ� ���� �������� é�͸� ����
            SelectedChapter = MaxClearedChapter + 1;
            result = true;
            Logger.Log($"MxClearedChapter:{MaxClearedChapter}");
        }
        catch(Exception e)
        {
            Logger.Log($"Load failed.(" + e.Message + ")");
        }
        return result;
    }

    //���� �Լ�
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("MaxClearedChapter", MaxClearedChapter);
            PlayerPrefs.Save();

            result = true;

            Logger.Log($"MaxClearedChapter:{MaxClearedChapter}");
        }
        catch ( Exception e)
        {
            Logger.Log($"Save failed(" + e.Message + ")");
        }

        return result;
    }

    public void SetDefaultData()
    {
        //�ϴ� �α� ����
        Logger.Log($"{GetType()}::SetDefaultData");
        //�ʱ� ���� ��������
        MaxClearedChapter = 4;
        SelectedChapter = 1;
    }
}
