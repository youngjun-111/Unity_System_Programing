using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gpm.Ui;
using TMPro;
using SuperMaxim.Messaging;

public class AchievementItemData : InfiniteScrollData
{
    //업적 타입
    public AchievementType AchievementType;
    //업적 달성 수치
    public int AchieveAmount;
    //업적 달성 여부
    public bool IsAchieved;
    //보상 수령 여부
    public bool IsRewardClaimed;
}
public class AchievementItem : InfiniteScrollItem
{
    //달성한 업적의 배경
    public GameObject AchievedBg;
    //달성하지 못한 업적의 배경
    public GameObject UnAchievedBg;
    //업적 이름 텍스트
    public TextMeshProUGUI AchievementNameTxt;
    //진행 슬라이더
    public Slider AchievementProgressSlider;
    //진행 표시 텍스트
    public TextMeshProUGUI AchievementProgressTxt;
    //업적 달성 시 수령하게 될 보상 이미지
    public Image RewardIcon;
    //수령 하게될 보상의 수량을 표시할 텍스트
    public TextMeshProUGUI RewardAmountTxt;
    //보상 버튼
    public Button ClaimBtn;
    //보상 버튼 이미지
    public Image ClaimBtnImg;
    //보상 수령 텍스트
    public TextMeshProUGUI ClaimBtnTxt;
    //업적 아이템 전용 데이터를 담을 변수
    AchievementItemData m_AchievementItemData;

    //인피니티 스크롤 오버라이드 함수
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        //매개변수인 scrollData의 전용데이터를 받아옴
        m_AchievementItemData = scrollData as AchievementItemData;
        //없으면 리턴
        if(m_AchievementItemData == null)
        {
            Logger.LogError("업적아이템이 없음;");
            return;
        }

        //해당 업적에 대한 데이터를 테이블데이터 매니저에서 가져옴
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        //없으면 리턴
        if(achievementData == null)
        {
            Logger.LogError("해당 데이터를 테이블에서 가져올수 없음");
            return;
        }
        //필요한 데이터를 모두 가져왔으면 본격적으로 UI요소를 세팅해 봄
        //업적의 달성 여부에 따라 그에 맞는 백그라운드 이미지 컴포넌트를 활성화
        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        //보상이미지를 보상 타입에 따라 세팅
        var rewardTextureName = string.Empty;

        switch (achievementData.AchievementRewardType)
        {
            case GlobalDefine.RewardType.Gold:
                rewardTextureName = "IconGolds";
                break;
            case GlobalDefine.RewardType.Gem:
                rewardTextureName = "IconGems";
                break;
            default:
                break;
        }

        //이미지 로드
        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}");
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }
        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.gray;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.gray;
    }

    //보상 받기 버튼
    public void OnClickClaimBtn()
    {
        //보상을이미 받았고 조건이 충족이 안되었다면
        if(!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }

        //보상 지급 처리를 하기 위해 필요한 데이터를 가져옴
        //유저 업적 진행 데이터를 가져옴
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("업적데이터가 없음");
            return;
        }

        //데이터 테이블에서 업적 데이터도 가져옴
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("업적테이블이 없음");
            return;
        }

        //현재 업적 타입에 맞는 유저 진행 데이터를 가져옴
        var userAchieveData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);

        //필요한 데이터를 모두 가져왔으니 업적 보상 지급처리
        if(userAchieveData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                //유저 업적 진행 데이터에서 보상 수령여부를 트루로
                userAchieveData.IsRewardClaimed = true;
                //받은 보상을 저장
                userAchievementData.SaveData();
                //현재 UI전용 데이터에도 보상 수령 여부를 트루로 갱신
                m_AchievementItemData.IsRewardClaimed = true;
            }

            //업적 보상 타입에 따라 보상을 지금
            switch (achievementData.AchievementRewardType)
            {
                case GlobalDefine.RewardType.Gold:
                    userGoodsData.Gold += achievementData.AchievementRewardAmount;
                    //골드 수령 메세지 발행
                    var goldUpdateMsg = new GoldUpdateMsg();
                    goldUpdateMsg.isAdd = true;
                    Messenger.Default.Publish(goldUpdateMsg);
                    //업적 중에 골드를 획득하는 업적이 있음
                    //그렇기 때문에 그 업적에 대해서 업적 진행 처리를 해 주 어야함
                    //(매개변수 : 업적 타입, 획득한 골드 수량
                    userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                    break;
                case GlobalDefine.RewardType.Gem:
                    userGoodsData.Gem += achievementData.AchievementRewardAmount;
                    //골드 수령 메세지 발행
                    var gemUpdateMsg = new GemUpdateMsg();
                    gemUpdateMsg.isAdd = true;
                    Messenger.Default.Publish(gemUpdateMsg);
                    //업적 중에 골드를 획득하는 업적이 있음
                    //그렇기 때문에 그 업적에 대해서 업적 진행 처리를 해 주 어야함
                    //(매개변수 : 업적 타입, 획득한 보석 수량)
                    break;
                default:
                    break;
            }
            userGoodsData.SaveData();
        }
    }
}
