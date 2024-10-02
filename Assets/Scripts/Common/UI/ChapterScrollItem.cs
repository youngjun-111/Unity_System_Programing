using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gpm.Ui;

//스크롤뷰의 스크롤 아이템에 대해서는 해당 스크롤 아이템에 대한
//전용 데이터 클래스가 필요
public class ChapterScrollItemData : InfiniteScrollData
{
    //챕터 번호
    public int ChapterNo;
}


public class ChapterScrollItem : InfiniteScrollItem
{
    //챕터 오브젝트
    public GameObject CurrChapter;
    //챕터 이미지 컴포넌트
    public RawImage CurrChapterBg;
    //챕터가 해금되지 않았을 때 표시해줄 UI컴포넌트들
    public Image Dim;
    public Image LockIcon;
    public Image Round;
    //아직 존재하지 않는 챕터(이후 업데이트에서 추가 될 챕터)에 대한 UI
    public ParticleSystem ComingSoonFx;
    public TextMeshProUGUI ComingSoonTxt;
    ChapterScrollItemData m_ChapterScrollItemData;

    //UI처리를 해주기 위해 오출되는 UpdateData함수 오버라이드
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //매개 변수로 받은 스크롤 데이터를 받아줌
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("없다;;");
            return;
        }
        //만약 표시해야 할 챕터 넘버가 글로벌 정의에 있는 MAX_CHAPTER의 값
        //즉, 게임 내 존재하는 최대 챕터보다 크다면..
        if(m_ChapterScrollItemData.ChapterNo > GlobalDefine.MAX_CHAPTER)
        {
            //챕터 표시UI는 비활
            //커밍순 UI 활성
            CurrChapter.SetActive(false);
            ComingSoonFx.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        //그렇지 않으면 본격적으로 챕터 UI표시 해줌
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFx.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);

        }
    }
}
