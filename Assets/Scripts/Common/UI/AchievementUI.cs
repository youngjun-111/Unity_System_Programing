using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using SuperMaxim.Messaging;

public class AchievementUI : BaseUI
{
    //스크롤뷰
    public InfiniteScroll AchievementScrollList;
    
    private void OnEnable()
    {
        //업적이 진행 되었을 때 발생하는 메시지를 구독시키고
        //구독시킨 메시를받았을때실할 함수
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //업적 목록을 세팅
        SetAchievementList();
        //업적 목록을 정렬
        SortAchievementList();
    }

    void SetAchievementList()
    {
        AchievementScrollList.Clear();
        //업적 목록 세팅
        //업적 데이터와 유저 업적 진행 데이터를 모두 가져옴
        var achievementDataList = DataTableManager.Instance.GetAchievementDataList();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(achievementDataList != null && userAchievementData != null)
        {
            //데이터가 모두 정상이라면 업적 데이터 목록을 순회하면서
            //업적 아이템 UI에 필요한 데이터를 생성
            foreach (var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                achievementItemData.AchievementType = achievement.AchievementType;
                //만약 유저 업적 진행데이터에도 해당 업적 데이터가 있다
                //업적이 얼마나 진행되었는지 세팅을 해주고
                //업적 달성 여부와
                //업적 보상 수령여부도 값을 대입해줌.
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
        //스크롤뷰에 소트데이터리스트 호출하고 이안에 람다식으로 정렬 로직을 작성
        //비교 대상인 a,b에서 각 업적 아이템 데이터를 받아오겠음
        AchievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;
            //업적을 먼저 달성했지만 보상을 받지 않은 업적을 제일 상위로 정렬
            var AComp = achievementA.IsAchieved && !achievementA.IsRewardClaimed;
            var BComp = achievementB.IsAchieved && !achievementB.IsRewardClaimed;
            //만약조건이 동일 하다면 달성하지 못한 업적을 달성한 업적보다 더 상위에 정렬해 주겠음.
            //CompareTo 비교해서 위치가 같으면0, (-)면 앞,(+)뒤
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
        //특별한 로직은 필요 없고 목록을 다시 생성해주고 다시 정렬시켜줌
        SetAchievementList();
        SortAchievementList();
    }
}
