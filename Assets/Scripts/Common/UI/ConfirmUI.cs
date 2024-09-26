using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum ConfirmType
{
    //�ܼ��� �˸��� �˾����� Ư�� ����� �Բ� Ȯ�� ��ư�� ��������
    //�̹�ư�� ������ ������ �ϴ� �̳� Ÿ��
    OK,
    //������ � ������ �Ϸ��� ���� �´��� ���� �����
    //�׷��ٸ� Ȯ�� ��ư�� ���� �� ������ �����ϰ�
    //�ƴ϶�� ��� ��ư�� ���� ����ϴ� �˾�
    OK_CANCEL,
}

public class ConfirmUIData : BaseUIData
{
    //�˾� ������ �����ϴ� ����
    public ConfirmType ConfirmType;
    //ȭ�� ���� �� �ؽ�Ʈ
    public string TitleTxt;
    //������ ǥ���� �ؽ�Ʈ
    public string DescTxt;
    //Ȯ�� ��ư�� ������ �ؽ�Ʈ
    public string OkBtnTxt;
    //Ȯ�� ��ư�� ���� �� �ൿ
    public Action OnClickOKBtn;
    //��� ��ư�� ������ �ؽ�Ʈ
    public string CancelBtnTxt;
    //��� ��ư�� ���� �� �ൿ
    public Action OnClickCancelBtn;
}

public class ConfirmUI : BaseUI
{
    //ȭ�� ���� �ؽ�Ʈ ����
    public TextMeshProUGUI TitleTxt = null;
    //���� �ؽ�Ʈ ����
    public TextMeshProUGUI DescTxt = null;
    //Ȯ�� ��ư ����
    public Button OKBtn = null;
    //��� ��ư ����
    public Button CancelBtn = null;
    //Ȯ�� ��ư �ؽ�Ʈ ����
    public TextMeshProUGUI OKBtnTxt = null;
    //��� ��ư �ؽ�Ʈ ����
    public TextMeshProUGUI CancelBtnTxt = null;

    //ȭ���� ���� �Ű������� ���� UIData�� ������ ���� ����
    ConfirmUIData m_ConfirmUIData = null;
    //Ȯ�� ��ư�� ������ �� �׼��� ����
    Action m_OnClickOKBtn = null;
    //��� ��ư�� ������ �� �׼��� ����
    Action m_OnClickCancelBtn = null;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //�Ű������� ���� UI�����͸� ����
        m_ConfirmUIData = uiData as ConfirmUIData;

        TitleTxt.text = m_ConfirmUIData.TitleTxt;
        DescTxt.text = m_ConfirmUIData.DescTxt;
        OKBtnTxt.text = m_ConfirmUIData.OkBtnTxt;
        CancelBtnTxt.text = m_ConfirmUIData.CancelBtnTxt;
        m_OnClickOKBtn = m_ConfirmUIData.OnClickOKBtn;
        m_OnClickCancelBtn = m_ConfirmUIData.OnClickCancelBtn;
        //ok��ư�� cancel��ư�� Ȱ��ȭ
        //ConfirmType�� ok�� ok��ư��, cancel�̸� ok, cancel ��ư �Ѵ� Ȱ��ȭ
        OKBtn.gameObject.SetActive(true);
        CancelBtn.gameObject.SetActive(m_ConfirmUIData.ConfirmType == ConfirmType.OK_CANCEL);
    }

    //Ȯ�� ��ư Ŭ�� �� ó���� ���� �Լ�
    public void OnClickOKBtn()
    {
        //? Ű���� : ���� �ƴϸ� �׼��� ���� �����ִ� Ű����
        m_OnClickOKBtn?.Invoke();
        CloseUI();
    }

    //��� ��ư Ŭ���� ó���� ���� �Լ�
    public void OnClickCancelBtn()
    {
        m_OnClickCancelBtn?.Invoke();
        CloseUI();
    }
}
