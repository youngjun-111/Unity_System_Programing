using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserPlayData : IUserData
{
    public int MaxClearedChapter { get; set; }
    //현재 유저가 선택중인 챕터는 따로 플레이어프랩스에 저장은 안해줌
    //게임에 진입해서 데이터를 로드할 때 유저가 플레이 가능한 최고 챕터로 자동으로 설정해주고
    //게임 진행중에만 이 변수를 관리하도록 하겠음
    //not saved to playerprefs
    public int SelectedChapter { get; set; } = 1;

    //로드 처리 함수
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //저장 되어 있는 값을 로드
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            //유저가 플레이 가능한 제일 높은 챕터로 현재 선택중인 챕터를 설정
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

    //저장 함수
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
        //일단 로그 띄우고
        Logger.Log($"{GetType()}::SetDefaultData");
        //초기 값을 설정해줌
        MaxClearedChapter = 4;
        SelectedChapter = 1;
    }
}
