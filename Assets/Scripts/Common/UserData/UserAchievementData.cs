using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SuperMaxim.Messaging;

[Serializable]
public class UserAchievementProgressData
{
    //���� Ÿ��
    public AchievementType AchievementType;
    //���� �޼� ��ġ
    public int AchievementAmount;
    //���� �޼� ����
    public bool IsAchieved;
    //���� ���� ���� ����
    public bool IsRewardClaimed;
}
//�������� ���� ���� ��Ȳ �����͸� ����Ʈ�� ���·� �÷��̾��������� �����ϰ�
//�ε��� ���̱� ������ ���� �κ��丮 ������ �ε� ����ÿ� �ߴ� �� ó��
//���� Ŭ������ �ϳ� �������
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();
}

//���� ���� ������ ������ �� ���� �޼����� ������ �ٰ���.
//���� ���� �޼��� Ŭ������ �ϳ� ����
public class AchievementProgressMsg
{

}


public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();


    public bool LoadData()
    {
        Logger.Log($"{GetType()}:�ε�");
        bool result = false;
        try
        {
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");
            if (!string.IsNullOrEmpty(achievementProgressDataListJson))
            {
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);
                //���� Ŭ������ ��� �����͸� �ٽ� ���������� Ŭ������ �������α׷��� ������ ����Ʈ ����
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
            Logger.Log($"�ε� ����(" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::���̺�");
        bool result = false;

        try
        {
            //����ȭ �ϱ����� ����Ʈ�� �������� ����Ŭ����
            UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = new UserAchievementProgressDataListWrapper();
            //�� ����Ŭ�������� �������α׷��������͸���Ʈ�� ��������
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //����Ŭ������������ȭ�� �����͸���Ʈ�� ���̽��� ����
            string achievementProgressDataListJson = JsonUtility.ToJson(achievementProgressDataListWrapper);
            //�� ��Ʈ�� ���� �÷��̾��������� ����
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
            Logger.Log($"���̺� ����(" + e.Message + ")");
        }
        return result;
    }

    public void SetDefaultData()
    {
        
    }

    //Ư�� ���� ���� �����͸� ã�Ƽ� ��ȯ�ϴ� �Լ�
    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(item => item.AchievementType == achievementType).FirstOrDefault();
    }
    //���� ������ ó���ϴ� �Լ�(�Ű����� : ���� Ÿ��, ���� ��ġ)
    public void ProgressAchievement(AchievementType achievementType, int achieveAmount)
    {
        //���� �����͸� ��������
        var achievementData = DataTableManager.Instance.GetAchievementsData(achievementType);

        if(achievementData == null)
        {
            Logger.LogError("���� �����Ͱ� ���µ�;;");
            return;
        }
        //�����Ϸ��� ���� Ÿ�Կ� ���� ���� �����͵� ������
        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        //����� ���� �����Ͱ� ���ٸ� ���� ������ �ش�.
        if(userAchievementProgressData == null)
        {
            //���� Ÿ���� �Ű������� ���� ���� Ÿ������ �������ְ�
            //����Ʈ �ڷᱸ���� �߰�
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }
        //���� ���θ� Ȯ���ϰ� �޼��� ���� �ʾ����� ���� ���� ��ġ�� ����
        if (!userAchievementProgressData.IsAchieved)
        {
            //�޼��� ��ġ��ŭ �޼� ��ġ�� ����
            userAchievementProgressData.AchievementAmount += achieveAmount;
            //���� ��ǥ �޼� ��ġ���� �ʰ��ؼ� �޼��ߴٸ� �޼� ��ǥġ�� ����
            if(userAchievementProgressData.AchievementAmount > achievementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achievementData.AchievementGoal;
            }
            //�޼� ��ġ�� ���� �޼� ��ǥ ��ġ�� �����ϸ�
            //������ �޼��ߴٰ� ����
            if(userAchievementProgressData.AchievementAmount == achievementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }
            SaveData();
            //���� ���� ��Ȳ�� ���ŵǾ��ٴ� �޽��� ����
            //�� �޽����� ���� UIȭ�鿡�� ���
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
}
