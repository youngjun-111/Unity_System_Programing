using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGoodsData : IUserData
{
    //����, int ������ ��� �� �ִ�.
    public long Gem { get; set; }
    //���
    public long Gold { get; set; }

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetlDefaultData");

        Gem = 0;
        Gold = 0;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            //�����ö� ��Ʈ���� long���� ����ȯ�� �����ش�~
            Gem = long.Parse(PlayerPrefs.GetString("Gem"));
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            result = true;
            Logger.Log($"Gem : {Gem} Gold : {Gold}");
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SetlDefaultData");

        bool result = false;
        try
        {
            //�÷��̾��������� ���� ������ �� ���µ� ����ϴ� ��Ʈ������ �������ص�
            PlayerPrefs.SetString("Gem", Gem.ToString());
            PlayerPrefs.SetString("Gold", Gold.ToString());
            PlayerPrefs.Save();
            result = true;

            Logger.Log($"Gem:{Gem}, Gold:{Gold}");
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }


        return result;
    }
}