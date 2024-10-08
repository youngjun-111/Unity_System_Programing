using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using SuperMaxim.Messaging;

public class AchievementUI : BaseUI
{
    //��ũ�Ѻ�
    public InfiniteScroll AchievementScrollList;
    
    private void OnEnable()
    {
        //������ ���� �Ǿ��� �� �߻��ϴ� �޽����� ������Ű��
        //������Ų �޽ø��޾��������� �Լ�
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //���� ����� ����
        SetAchievementList();
        //���� ����� ����
        SortAchievementList();
    }

    void SetAchievementList()
    {
        AchievementScrollList.Clear();
        //���� ��� ����
        //���� �����Ϳ� ���� ���� ���� �����͸� ��� ������
        var achievementDataList = DataTableManager.Instance.GetAchievementDataList();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(achievementDataList != null && userAchievementData != null)
        {
            //�����Ͱ� ��� �����̶�� ���� ������ ����� ��ȸ�ϸ鼭
            //���� ������ UI�� �ʿ��� �����͸� ����
            foreach (var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                achievementItemData.AchievementType = achievement.AchievementType;
                //���� ���� ���� ���൥���Ϳ��� �ش� ���� �����Ͱ� �ִ�
                //������ �󸶳� ����Ǿ����� ������ ���ְ�
                //���� �޼� ���ο�
                //���� ���� ���ɿ��ε� ���� ��������.
                var userAchieveData = userAchievementData.GetUserAchievementProgressData(achievement.AchievementType);
                if(userAchieveData != null)
                {
                    achievementItemData.AchieveAmount = userAchieveData.AchievementAmount;
                    achievementItemData.IsAchieved = userAchieveData.IsAchieved;
                    achievementItemData.IsRewardClaimed = userAchieveData.IsRewardClaimed;
                }
                AchievementScrollList.InsertData(achievementItemData);
            }
        }
    }

    void SortAchievementList()
    {
        //��ũ�Ѻ信 ��Ʈ�����͸���Ʈ ȣ���ϰ� �̾ȿ� ���ٽ����� ���� ������ �ۼ�
        //�� ����� a,b���� �� ���� ������ �����͸� �޾ƿ�����
        AchievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;
            //������ ���� �޼������� ������ ���� ���� ������ ���� ������ ����
            var AComp = achievementA.IsAchieved && !achievementA.IsRewardClaimed;
            var BComp = achievementB.IsAchieved && !achievementB.IsRewardClaimed;
            //���������� ���� �ϴٸ� �޼����� ���� ������ �޼��� �������� �� ������ ������ �ְ���.
            //CompareTo ���ؼ� ��ġ�� ������0, (-)�� ��,(+)��
            int compareResult = BComp.CompareTo(AComp);
            if(compareResult == 0)
            {
                compareResult = achievementA.IsAchieved.CompareTo(achievementB.IsAchieved);
                if(compareResult == 0)
                {
                    compareResult = (achievementA.AchievementType).CompareTo(achievementB.AchievementType);
                }
            }
            return compareResult;
        });
    }
    void OnAchievementProgressed(AchievementProgressMsg msg)
    {
        //Ư���� ������ �ʿ� ���� ����� �ٽ� �������ְ� �ٽ� ���Ľ�����
        SetAchievementList();
        SortAchievementList();
    }
}
