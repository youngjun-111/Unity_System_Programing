using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gpm.Ui;
public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList;
    //스크롤 뷰에서 현재 선택 중인 챕터(정확히 말하면 선택한 것은 아니지만 화면 정중에 있는 챕터)
    //대한 정보를 표시해줄 UI요소들의 최상위 오브젝트 변수 선언
    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt;
    public Button SelectBtn;

    int SelectedChapter;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("없네?");
            return;
        }
        SelectedChapter = userPlayData.SelectedChapter;
        //현재 선택한 챕터에 대한 UI처리를 해주는 함수
        SetSeletedChapter();
        //챕터 목록 스크롤뷰를 셋팅하는 함수
        SetChapterScrollLost();
        //인피니티 스크롤에 있는 MoveTo함수
        //첫번째 매개변수 : 인덱스 번호 -1 두번째 매개변수 : 이동 위치
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //스크롤이 끝난 후 가장 가까운 아이템으로 자동 이동
        //자동 이동 끝난 후에 처리를 위한 OnSnap에 람다로 원하는 처리
        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if(chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }

    //현재 선택한 챕터에 대한 UI처리를 해주는 함수
    void SetSeletedChapter()
    {
        //게임 내에 추가된 챕터에 해당하면 선택한 챕터 UI요소들을 활성화
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //챕터데이터테이블에서 해당 챕터에 대한 데이터를 가져와서 챕터 명도 표시
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        //반대로 아직 게임 내에 추가 되지 않은 챕터라면 선택한 챕터 요소의 요소들을 비활성화
        else
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }

    //챕터 목록 스크롤뷰를 셋팅하는 함수
    void SetChapterScrollLost()
    {
        //먼저 스크롤뷰 내부에 이미 존재하는 아이템이 있다면 삭제
        ChapterScrollList.Clear();
        //1번 인덱스부터 최고스테이지+1 까지 순회하면서 아이템을 하나씩 추가
        //최고스테이지 + 1 까지 포함시켜주는것은 챕터스크롤뷰 마지막에커밍 순이라는 아이템을 만들어 주기 위함
        for (int i = 1; i <= GlobalDefine.MAX_CHAPTER+1; i++) 
        {
            var chapterItemData = new ChapterScrollItemData();
            chapterItemData.ChapterNo = i; //+1
            ChapterScrollList.InsertData(chapterItemData);
        }
    }

    void OnSnap(int selectedChapter)
    {

    }
}
