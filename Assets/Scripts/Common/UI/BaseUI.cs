using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseUIData
{
    //�Լ��� ���� �� �ִ� ������� ����
    //������ UIȭ�鿡 ���ؼ��� � ��Ȳ������ A��� ����� ����������ϰ�
    //� ��Ȳ������ B��� ����� ��������� �� ���� ����.
    //�׷��� ������ UIȭ�� Ŭ���� �ȿ��� �̷� OnShow�� OnClose�� �����ϴ� �ͺ���
    //�� ȭ���� ���ڴٰ� UI�Ŵ����� ȣ���� �� � ������ ����� ���� �����ؼ�
    //�Ѱ��ִ� ���� �� �����ϰ� ���ϴ� ��ȹ ������ ������ �� �ִ�.

    //UIȭ���� ������ �� ���ְ� ���� ������ ����
    public Action OnShow;
    //UIȭ���� �����鼭 �����ؾ� �Ǵ� ��� ����
    public Action OnClose;
}

public class BaseUI : MonoBehaviour
{
    //UI ���� �ٶ� ����� �ִϸ��̼� ����
    public Animation m_UIOpenAnim;

    public Action m_OnShow;
    public Action m_OnClose;
    
    public virtual void Init(Transform anchor)
    {
        Logger.Log($"{GetType()}::Init");

        m_OnShow = null;
        m_OnClose = null;
        //anchor : UIĵ���� ������Ʈ�� Ʈ������
        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
        {
            Logger.LogError("UI does not have rectTransform.");
            return;
        }

        //�⺻ ������ ���� �ʱ�ȭ
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;
    }

    public virtual void SetInfo(BaseUIData uiData)
    {
        Logger.Log($"{GetType()}::SetInfo");
        m_OnShow = uiData.OnShow;
        m_OnClose = uiData.OnClose;
    }

    //UI ȭ���� ������ ��� ȭ�鿡 ǥ���� �ִ� �Լ�
    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }

        m_OnShow?.Invoke();//�ΰ˻�� ������
        //? Ű���带 ����� ���� ó��
        //_action?.Invoke(3); //if(action != null)�� ?Ű���带 ����Ͽ� null���� üũ

        m_OnShow = null;//���� �Ŀ��� �η� �ʱ�ȭ
    }

    //UIȭ���� �ݴ� �Լ�
    public virtual void CloseUI(bool isCloseAll = false)
    {
        //isCloseAll : ���� ��ȯ�ϰų� �Ҷ� �����ִ� ȭ����
        //���� �� �ݾ��� �ʿ䰡 ���� ��
        //true ���� �Ѱ��༭ ȭ���� ���� �� �ʿ��� ó������
        //�� �����ϰ� ȭ�鸸 �ݾ��ֱ� ���ؼ� ����� �Լ���
        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        //CloseUI�� �� �ν��Ͻ��� �Ű������� �־���
        //���� �� ȣ�� �ɶ� ��¥ �ݾ���
        UIManager.Instance.CloseUI(this);
    }

    //�ݱ� ��ư�� ������ �� �����ϴ� �Լ�
    //���� ��κ� UI���� �ݱ� ��ư�� �����Ƿ�
    //���⼭ �ƿ� �ݱ� ��ư ����� ����
    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
