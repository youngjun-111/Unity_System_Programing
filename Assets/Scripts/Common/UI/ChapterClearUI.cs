using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterClearUIData : BaseUIData
{
    //어떤 챕터를 클리어했는지
    public int chapter;
    //보상을 받아야 하는지 여부
    //이변수는 이 챕터를 처음으로 클리어해서 보상을 지급해야 하는지
    //아니면 한번 클리어 후 다시 클리어한 것이기 때문에 보상을 지급하지 말아야하는지를 위한 변수
    public bool earnReward;
}

public class ChapterClearUI : BaseUI
{
    //보상 관련 UI요소들의 최상위 오브젝트 변수
    public GameObject Reward;
    //각 리워드 텍스트
    public TextMeshProUGUI GemRewardAmountTxt;
    public TextMeshProUGUI GoldRewardAmountTxt;
    //로비로 돌아가는 버튼
    public Button HomeBtn;
    //클리어 이펙트배열
    public ParticleSystem[] ClearFX;
    //전용 데이터 클래스를 담을 변수
    private ChapterClearUIData m_ChapterClearUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        //매개 변수로 받은 uiData를 ChapterClearUIData로 받아줌
        m_ChapterClearUIData = uiData as ChapterClearUIData;

        if (m_ChapterClearUIData == null)
        {
            Logger.LogError("이건 없는데;;");
            return;
        }
        var chapterData = DataTableManager.Instance.GetChapterData(m_ChapterClearUIData.chapter);

        if(chapterData == null)
        {
            Logger.LogError($"챕터 없음:{m_ChapterClearUIData.chapter}");
            return;
        }

        Reward.SetActive(m_ChapterClearUIData.earnReward);

        if (m_ChapterClearUIData.earnReward)
        {
            GemRewardAmountTxt.text = chapterData.ChapterRewardGem.ToString("N0");
            GoldRewardAmountTxt.text = chapterData.ChapterRewardGold.ToString("N0");
        }

        HomeBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ChapterClearUIData.earnReward ? -250f : 50f, 0f);
        //이펙트 재생
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