using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChapterClearUIData : BaseUI
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
    ChapterClearUIData m_ChapterClearUIData;
}
