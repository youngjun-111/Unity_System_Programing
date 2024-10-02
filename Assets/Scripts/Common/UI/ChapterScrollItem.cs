using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gpm.Ui;

//��ũ�Ѻ��� ��ũ�� �����ۿ� ���ؼ��� �ش� ��ũ�� �����ۿ� ����
//���� ������ Ŭ������ �ʿ�
public class ChapterScrollItemData : InfiniteScrollData
{
    //é�� ��ȣ
    public int ChapterNo;
}


public class ChapterScrollItem : InfiniteScrollItem
{
    //é�� ������Ʈ
    public GameObject CurrChapter;
    //é�� �̹��� ������Ʈ
    public RawImage CurrChapterBg;
    //é�Ͱ� �رݵ��� �ʾ��� �� ǥ������ UI������Ʈ��
    public Image Dim;
    public Image LockIcon;
    public Image Round;
    //���� �������� �ʴ� é��(���� ������Ʈ���� �߰� �� é��)�� ���� UI
    public ParticleSystem ComingSoonFx;
    public TextMeshProUGUI ComingSoonTxt;
    ChapterScrollItemData m_ChapterScrollItemData;

    //UIó���� ���ֱ� ���� ����Ǵ� UpdateData�Լ� �������̵�
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //�Ű� ������ ���� ��ũ�� �����͸� �޾���
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("����;;");
            return;
        }
        //���� ǥ���ؾ� �� é�� �ѹ��� �۷ι� ���ǿ� �ִ� MAX_CHAPTER�� ��
        //��, ���� �� �����ϴ� �ִ� é�ͺ��� ũ�ٸ�..
        if(m_ChapterScrollItemData.ChapterNo > GlobalDefine.MAX_CHAPTER)
        {
            //é�� ǥ��UI�� ��Ȱ
            //Ŀ�ּ� UI Ȱ��
            CurrChapter.SetActive(false);
            ComingSoonFx.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        //�׷��� ������ ���������� é�� UIǥ�� ����
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFx.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);
            //�����÷��̵����͸� �����;���
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if(userPlayData != null)
            {
                //���� �ִ�� Ŭ������ é�Ϳ� ���Ͽ� é���� �ر� ���θ� �Ǵ�
                //��, Ŭ������ é�ͺ��� ������ é�Ͱ� Ŭ��� false = �ر� true = ��
                var isLocked = m_ChapterScrollItemData.ChapterNo > userPlayData.MaxClearedChapter + 1;
                //�׸��� �ر� ���ο� ���� �̹��� ������Ʈ���� ó������

                //���(�ణ ���� ȿ�� ����)�� �رݵ��� �ʾ����� Ȱ��ȭ
                Dim.gameObject.SetActive(isLocked);
                //��� �����ܵ� �رݵ��� �ʾ����� Ȱ��ȭ
                LockIcon.gameObject.SetActive(isLocked);
                //�׵θ� �̹����� �رݵǾ����� ��� �ƴϸ� ��Ӱ�
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }
            //�ش� é�� �ѹ��� �´� ��� �̹����� �ε�
            var bgTexture = Resources.Load($"ChapterBg/Background_{m_ChapterScrollItemData.ChapterNo.ToString("D3")}") as Texture2D;
            if(bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }
}
