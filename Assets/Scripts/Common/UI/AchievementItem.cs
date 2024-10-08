using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gpm.Ui;
using TMPro;
using SuperMaxim.Messaging;

public class AchievementItemData : InfiniteScrollData
{
    //���� Ÿ��
    public AchievementType AchievementType;
    //���� �޼� ��ġ
    public int AchieveAmount;
    //���� �޼� ����
    public bool IsAchieved;
    //���� ���� ����
    public bool IsRewardClaimed;
}
public class AchievementItem : InfiniteScrollItem
{
    //�޼��� ������ ���
    public GameObject AchievedBg;
    //�޼����� ���� ������ ���
    public GameObject UnAchievedBg;
    //���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI AchievementNameTxt;
    //���� �����̴�
    public Slider AchievementProgressSlider;
    //���� ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI AchievementProgressTxt;
    //���� �޼� �� �����ϰ� �� ���� �̹���
    public Image RewardIcon;
    //���� �ϰԵ� ������ ������ ǥ���� �ؽ�Ʈ
    public TextMeshProUGUI RewardAmountTxt;
    //���� ��ư
    public Button ClaimBtn;
    //���� ��ư �̹���
    public Image ClaimBtnImg;
    //���� ���� �ؽ�Ʈ
    public TextMeshProUGUI ClaimBtnTxt;
    //���� ������ ���� �����͸� ���� ����
    AchievementItemData m_AchievementItemData;

    //���Ǵ�Ƽ ��ũ�� �������̵� �Լ�
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        //�Ű������� scrollData�� ���뵥���͸� �޾ƿ�
        m_AchievementItemData = scrollData as AchievementItemData;
        //������ ����
        if(m_AchievementItemData == null)
        {
            Logger.LogError("������������ ����;");
            return;
        }

        //�ش� ������ ���� �����͸� ���̺����� �Ŵ������� ������
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        //������ ����
        if(achievementData == null)
        {
            Logger.LogError("�ش� �����͸� ���̺��� �����ü� ����");
            return;
        }
        //�ʿ��� �����͸� ��� ���������� ���������� UI��Ҹ� ������ ��
        //������ �޼� ���ο� ���� �׿� �´� ��׶��� �̹��� ������Ʈ�� Ȱ��ȭ
        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        //�����̹����� ���� Ÿ�Կ� ���� ����
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

        //�̹��� �ε�
        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}");
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }
        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.gray;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.gray;
    }

    //���� �ޱ� ��ư
    public void OnClickClaimBtn()
    {
        //�������̹� �޾Ұ� ������ ������ �ȵǾ��ٸ�
        if(!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }

        //���� ���� ó���� �ϱ� ���� �ʿ��� �����͸� ������
        //���� ���� ���� �����͸� ������
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("���������Ͱ� ����");
            return;
        }

        //������ ���̺��� ���� �����͵� ������
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("�������̺��� ����");
            return;
        }

        //���� ���� Ÿ�Կ� �´� ���� ���� �����͸� ������
        var userAchieveData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);

        //�ʿ��� �����͸� ��� ���������� ���� ���� ����ó��
        if(userAchieveData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                //���� ���� ���� �����Ϳ��� ���� ���ɿ��θ� Ʈ���
                userAchieveData.IsRewardClaimed = true;
                //���� ������ ����
                userAchievementData.SaveData();
                //���� UI���� �����Ϳ��� ���� ���� ���θ� Ʈ��� ����
                m_AchievementItemData.IsRewardClaimed = true;
            }

            //���� ���� Ÿ�Կ� ���� ������ ����
            switch (achievementData.AchievementRewardType)
            {
                case GlobalDefine.RewardType.Gold:
                    userGoodsData.Gold += achievementData.AchievementRewardAmount;
                    //��� ���� �޼��� ����
                    var goldUpdateMsg = new GoldUpdateMsg();
                    goldUpdateMsg.isAdd = true;
                    Messenger.Default.Publish(goldUpdateMsg);
                    //���� �߿� ��带 ȹ���ϴ� ������ ����
                    //�׷��� ������ �� ������ ���ؼ� ���� ���� ó���� �� �� �����
                    //(�Ű����� : ���� Ÿ��, ȹ���� ��� ����
                    userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                    break;
                case GlobalDefine.RewardType.Gem:
                    userGoodsData.Gem += achievementData.AchievementRewardAmount;
                    //��� ���� �޼��� ����
                    var gemUpdateMsg = new GemUpdateMsg();
                    gemUpdateMsg.isAdd = true;
                    Messenger.Default.Publish(gemUpdateMsg);
                    //���� �߿� ��带 ȹ���ϴ� ������ ����
                    //�׷��� ������ �� ������ ���ؼ� ���� ���� ó���� �� �� �����
                    //(�Ű����� : ���� Ÿ��, ȹ���� ���� ����)
                    break;
                default:
                    break;
            }
            userGoodsData.SaveData();
        }
    }
}
