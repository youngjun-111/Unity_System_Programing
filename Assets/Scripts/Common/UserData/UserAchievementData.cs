using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SuperMaxim.Messaging;

[Serializable]
public class UserAchievementProgressData
{
    //업적 타입
    public AchievementType AchievementType;
    //업적 달성 수치
    public int AchievementAmount;
    //업적 달성 여부
    public bool IsAchieved;
    //업적 보상 수령 여부
    public bool IsRewardClaimed;
}
//여러개의 업적 진행 상황 데이터를 리스트의 현태로 플레이어프랩스에 저장하고
//로드할 것이기 때문에 유저 인벤토리 데이터 로드 저장시에 했던 것 처럼
//래퍼 클래스를 하나 만들어줌
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();
}

//업적 진행 사항을 갱신할 때 마다 메세지를 발행해 줄것임.
//발행 해줄 메세지 클래스도 하나 선언
public class AchievementProgressMsg
{

}


public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();


    public bool LoadData()
    {
        Logger.Log($"{GetType()}:로드");
        bool result = false;
        try
        {
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");
            if (!string.IsNullOrEmpty(achievementProgressDataListJson))
            {
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);
                //래퍼 클래스에 담긴 데이터를 다시 업적데이터 클래스의 업적프로그래스 데이터 리스트 대입
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;
                Logger.Log("AchievementProgressDataList");
                foreach (var item in AchievementProgressDataList)
                {
                    Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount{item.AchievementAmount} IsAchieved{item.IsAchieved} IsRewardClaimed{item.IsRewardClaimed}");
                }
            }
            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"로드 실패(" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::세이브");
        bool result = false;

        try
        {
            //직렬화 하기위해 리스트를 랩핑해준 랩퍼클래스
            UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = new UserAchievementProgressDataListWrapper();
            //그 랩퍼클래스에서 업적프로그래서데이터리스트를 가져오고
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //랩퍼클래스에서직렬화한 데이터리스트를 제이슨에 저장
            string achievementProgressDataListJson = JsonUtility.ToJson(achievementProgressDataListWrapper);
            //이 스트링 값을 플레이어프랩스에 저장
            PlayerPrefs.SetString("AchievementProgressDataList", achievementProgressDataListJson);

            Logger.Log("AchievementProgressDataList");
            foreach (var item in AchievementProgressDataList)
            {
                Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount{item.AchievementAmount} IsAchieved{item.IsAchieved} IsRewardClaimed{item.IsRewardClaimed}");
            }
            PlayerPrefs.Save();
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log($"세이브 실패(" + e.Message + ")");
        }
        return result;
    }

    public void SetDefaultData()
    {
        
    }

    //특정 업적 진행 데이터를 찾아서 반환하는 함수
    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(item => item.AchievementType == achievementType).FirstOrDefault();
    }
    //업적 진행을 처리하는 함수(매개변수 : 업적 타입, 업적 수치)
    public void ProgressAchievement(AchievementType achievementType, int achieveAmount)
    {
        //업적 데이터를 가져오고
        var achievementData = DataTableManager.Instance.GetAchievementsData(achievementType);

        if(achievementData == null)
        {
            Logger.LogError("업적 데이터가 없는데;;");
            return;
        }
        //갱신하려는 업적 타입에 대한 진행 데이터도 가져옴
        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        //저장된 진행 데이터가 없다면 새로 생성해 준다.
        if(userAchievementProgressData == null)
        {
            //업적 타입을 매개변수로 받은 업적 타입으로 대입해주고
            //리스트 자료구조에 추가
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }
        //달정 여부를 확인하고 달성이 되지 않았으면 업적 진행 수치를 갱신
        if (!userAchievementProgressData.IsAchieved)
        {
            //달성한 수치만큼 달성 수치를 증가
            userAchievementProgressData.AchievementAmount += achieveAmount;
            //만약 목표 달성 수치보다 초과해서 달성했다면 달성 목표치로 대입
            if(userAchievementProgressData.AchievementAmount > achievementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achievementData.AchievementGoal;
            }
            //달성 수치가 업적 달성 목표 수치와 동일하면
            //업적을 달성했다고 저장
            if(userAchievementProgressData.AchievementAmount == achievementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }
            SaveData();
            //업적 진행 상황이 갱신되엇다는 메시지 발행
            //이 메시지는 업적 UI화면에서 사용
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
}
