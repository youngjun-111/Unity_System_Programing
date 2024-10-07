using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterClearUIData : BaseUIData
{
    //� é�͸� Ŭ�����ߴ���
    public int chapter;
    //������ �޾ƾ� �ϴ��� ����
    //�̺����� �� é�͸� ó������ Ŭ�����ؼ� ������ �����ؾ� �ϴ���
    //�ƴϸ� �ѹ� Ŭ���� �� �ٽ� Ŭ������ ���̱� ������ ������ �������� ���ƾ��ϴ����� ���� ����
    public bool earnReward;
}

public class ChapterClearUI : BaseUI
{
    //���� ���� UI��ҵ��� �ֻ��� ������Ʈ ����
    public GameObject Reward;
    //�� ������ �ؽ�Ʈ
    public TextMeshProUGUI GemRewardAmountTxt;
    public TextMeshProUGUI GoldRewardAmountTxt;
    //�κ�� ���ư��� ��ư
    public Button HomeBtn;
    //Ŭ���� ����Ʈ�迭
    public ParticleSystem[] ClearFX;
    //���� ������ Ŭ������ ���� ����
    private ChapterClearUIData m_ChapterClearUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        //�Ű� ������ ���� uiData�� ChapterClearUIData�� �޾���
        m_ChapterClearUIData = uiData as ChapterClearUIData;

        if (m_ChapterClearUIData == null)
        {
            Logger.LogError("�̰� ���µ�;;");
            return;
        }
        var chapterData = DataTableManager.Instance.GetChapterData(m_ChapterClearUIData.chapter);

        if(chapterData == null)
        {
            Logger.LogError($"é�� ����:{m_ChapterClearUIData.chapter}");
            return;
        }

        Reward.SetActive(m_ChapterClearUIData.earnReward);

        if (m_ChapterClearUIData.earnReward)
        {
            GemRewardAmountTxt.text = chapterData.ChapterRewardGem.ToString("N0");
            GoldRewardAmountTxt.text = chapterData.ChapterRewardGold.ToString("N0");
        }

        HomeBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ChapterClearUIData.earnReward ? -250f : 50f, 0f);
        //����Ʈ ���
        for (int i = 0; i < ClearFX.Length; i++)
        {
            ClearFX[i].Play();
        }
    }

    public void OnClickHomeBtn()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);
        CloseUI();
    }
}