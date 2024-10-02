using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gpm.Ui;
public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList;
    //��ũ�� �信�� ���� ���� ���� é��(��Ȯ�� ���ϸ� ������ ���� �ƴ����� ȭ�� ���߿� �ִ� é��)
    //���� ������ ǥ������ UI��ҵ��� �ֻ��� ������Ʈ ���� ����
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
            Logger.LogError("����?");
            return;
        }
        SelectedChapter = userPlayData.SelectedChapter;
        //���� ������ é�Ϳ� ���� UIó���� ���ִ� �Լ�
        SetSeletedChapter();
        //é�� ��� ��ũ�Ѻ並 �����ϴ� �Լ�
        SetChapterScrollLost();
        //���Ǵ�Ƽ ��ũ�ѿ� �ִ� MoveTo�Լ�
        //ù��° �Ű����� : �ε��� ��ȣ -1 �ι�° �Ű����� : �̵� ��ġ
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //��ũ���� ���� �� ���� ����� ���������� �ڵ� �̵�
        //�ڵ� �̵� ���� �Ŀ� ó���� ���� OnSnap�� ���ٷ� ���ϴ� ó��
        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if(chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }

    //���� ������ é�Ϳ� ���� UIó���� ���ִ� �Լ�
    void SetSeletedChapter()
    {
        //���� ���� �߰��� é�Ϳ� �ش��ϸ� ������ é�� UI��ҵ��� Ȱ��ȭ
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //é�͵��������̺��� �ش� é�Ϳ� ���� �����͸� �����ͼ� é�� �� ǥ��
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        //�ݴ�� ���� ���� ���� �߰� ���� ���� é�Ͷ�� ������ é�� ����� ��ҵ��� ��Ȱ��ȭ
        else
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }

    //é�� ��� ��ũ�Ѻ並 �����ϴ� �Լ�
    void SetChapterScrollLost()
    {
        //���� ��ũ�Ѻ� ���ο� �̹� �����ϴ� �������� �ִٸ� ����
        ChapterScrollList.Clear();
        //1�� �ε������� �ְ�������+1 ���� ��ȸ�ϸ鼭 �������� �ϳ��� �߰�
        //�ְ������� + 1 ���� ���Խ����ִ°��� é�ͽ�ũ�Ѻ� ��������Ŀ�� ���̶�� �������� ����� �ֱ� ����
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
