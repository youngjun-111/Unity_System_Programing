using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    //ȭ���� �������� ĵ���� ������Ʈ Ʈ������
    public Transform UICanvasTrs;
    //UI ȭ���� �� UI ĵ���� Ʈ������ ������ ��ġ��������ϱ� ������ �ʿ���.

    //UI ȭ���� ���� �� ��Ȱ��ȭ ��Ų UI ȭ����� ��ġ������ Ʈ������
    public Transform ClosedUITrs;
    //UI ȭ���� �������� �� ���� ��ܿ� �����ִ� UI
    BaseUI m_FrontUI;
    //���� �����ִ�, �� Ȱ��ȭ �Ǿ��ִ� UI ȭ���� ��� �ִ� ����(Ǯ)
    Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    //�����ִ�, �� ��Ȱ��ȭ �Ǿ� �ִ� UI ȭ���� ��� �ִ� ����(Ǯ)
    Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();

    GoodsUI m_GoodsUI;
    protected override void Init()
    {
        base.Init();
        //������Ʈ�� ������ ���� ������Ʈ�� ã�Ƽ� GoodsUI������Ʈ�� ����
        m_GoodsUI = FindObjectOfType<GoodsUI>();
        if (!m_GoodsUI)
        {
            Logger.Log("No stats ui component found");
        }
    }
    //���⸦ ���ϴ� UI ȭ���� ���� �ν��Ͻ��� �������� �Լ�
    //out �Լ����� �Ѱ��� ���̳� ������ ��ȯ�� �� �ֱ� ������
    //�������� ���̳� ������ ��ȯ�ϰ� ���� �� �̷��� out �ŰԺ����� ���
    //�� �Լ��� BaseUI, isAlreadyOpen �ΰ��� ���� ��ȯ
    //�Ŵ����� �ִ� UI�� �������� �����ð� ������ �������� �����ִ� ui�� ��ȯ���ִ� �Լ�
    BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        //���⼭ T�� ���� ������ �ϴ� ȭ�� UI Ŭ���� Ÿ��. �̰��� uiType���� �޾ƿ´�.
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (m_OpenUIPool.ContainsKey(uiType))
        {
            //Ǯ�� �ִ� �ش� ������Ʈ�� BaseUI ������Ʈ�� ui������ ����
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        //�׷��� �ʰ� m_ClosedUIPool�� �����Ѵٸ�
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            //�ش� Ǯ�� �ִ� BaseUI ������Ʈ�� ui������ ����
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType);
        }
        //�Ѵ� �ƴϰ� �ƿ� �ѹ��� ������ ���� ���� �ν��Ͻ����
        else
        {
            //������ ������
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            //�������� �̸��� �ݵ�� UI Ŭ������ �̸��� �����ؾ���
            //�ֳ��ϸ� UIŬ������ �̸����� ��θ� ���� ���ҽ� �������� �ε��ؿ���� ��û�Ѱű� ������
            ui = uiObj.GetComponent<BaseUI>();
        }
        return ui;
    }
    //���� UI�� �����԰ų� ���������� UI�� ���½����ִ� �Լ�
    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);
        //� UIȭ���� ������ �ϴ��� �α׸� �����
        Logger.Log($"{GetType()}::OpenUI({uiType})");
        //�̹� �����ִ��� �� �� �ִ� ����
        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);
        //�ٵ� ���� ui�� ������ �����α� ����ְ� ���� ��Ŵ
        if (!ui)
        {
            Logger.LogError($"{uiType} does not exist");
            return;
        }

        //�����߿� �̹� �����ִٸ� �̰� ���� ���������� ��û�̶�� �Ǵ�
        if (isAlreadyOpen)
        {
            Logger.LogError($"{uiType} is already Open.");
            return;
        }

        //���� ��ȿ�� �˻縦 ����ؼ� ���������� UIȭ���� ���� �� �ִٸ�
        //���� ������ UIȭ���� ���� �����͸� ������ �ش�.

        //childCount ������ �ִ� ���� ������Ʈ ����
        var siblingIdx = UICanvasTrs.childCount - 1;

        //UIȭ�� �ʱ�ȭ
        ui.Init(UICanvasTrs);

        //���̶�Ű ���� ���� SetsiblingIndex : �Ű������� �־ ������ ����
        //siblingIdx�� 0���� �����ϴµ� 0, 1, 2, 3 ...
        //�̷��� ������ 1������ �þ��.
        //�����ϰ��� �ϴ� UIȭ���� �̹� �����Ǿ��ִ� UIȭ��� ����
        //��ܿ� ��ġ���� ����ϱ� ������
        //���� �����ϴ� UICanvasTrs ���� ������Ʈ���� ������ �޾ƿͼ�
        //siblingIdx ������ �Ѱ��ִ� ����.
        //siblingIdx�� 0���� �����ϱ� ������ childCount�� ���ο� UIȭ���� �߰��� ��
        //���� ū sublingIdx���� �Ǳ� ����
        //���� ��� �ڽ��� 2�� -> 0, ������ �����Ǹ� 3���� �Ǵµ� �װ� 1�̵ǰ� �״����� �����Ǹ� 2�� �Ǵ� ������
        //�Ź� ĵ�ٽ� �󿡼� ���� �Ʒ��� ��ġ�ϰ� �ǰ� ȭ�鿡���� ���� �ֻ�ܿ� ����
        ui.transform.SetSiblingIndex(siblingIdx);

        //������Ʈ�� ������ ���ӿ�����Ʈ Ȱ��ȭ
        ui.gameObject.SetActive(true);

        //UI ȭ�鿡 ���̴� UI����� �����͸� ��������
        ui.SetInfo(uiData);
        ui.ShowUI();

        //���� �������ϴ� ȭ�� UI�� ���� ��ܿ� �ִ� UI�� �ɰ��̱� ������ �̷��� ����
        m_FrontUI = ui;

        //m_OpenUIPool�� ������ UI�ν��Ͻ��� �־��ش�.
        m_OpenUIPool[uiType] = ui.gameObject;
    }

    //ȭ���� �ݴ� �Լ�
    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        //� UI�� �ݾ��ִ��� �α׷� ǥ��
        Logger.Log($"CloseUI UI : {uiType}");

        ui.gameObject.SetActive(false);

        //������Ʈ Ǯ���� ����
        m_OpenUIPool.Remove(uiType);

        //Ŭ���� Ǯ���� �߰�
        m_ClosedUIPool[uiType] = ui.gameObject;

        //ClosedUITrs������ ��ġ
        ui.transform.SetParent(ClosedUITrs);

        //�ֻ�� UI�η� �ʱ�ȭ
        m_FrontUI = null;

        //���� �ֻ�ܿ� �ִ� UIȭ�� ������Ʈ�� �����´�.
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);

        //���� UI�� �����Ѵٸ� �� UIȭ�� �ν��Ͻ��� �ֻ�� UI�� ����
        if (lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }

    //Ư�� UIȭ���� �����ִ��� Ȯ���ϰ� �� �����ִ� UIȭ���� �������� �Լ�
    //�̸� �Ű� ���� ���Ŀ� �̸��� �޶� ������ �߻���
    public BaseUI GetActiveUI<T>()
    {
        var uiType = typeof(T);
        //m_OpenUIPool�� Ư�� ȭ�� �ν��Ͻ��� �����Ѵٸ� �� ȭ�� �ν��Ͻ��� ������ �ְ� �׷��� ������ �� ����
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    //UIȭ���� �������� �ϳ��� �ִ��� Ȯ���ϴ� �Լ�
    public bool ExistsOpenUI()
    {
        //m_FrontUI�� null���� �ƴ��� Ȯ���ؼ� �� �Ұ��� ��ȯ
        return m_FrontUI != null;
    }

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    //���� �ֻ�ܿ� �ִ� UIȭ�� �ν��Ͻ��� �ݴ� �Լ�
    public void CloseCurrentFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    //��������� �����ִ� ��� UIȭ���� ������� �Լ�
    public void CloseAllOpenUI()
    {
        while (m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }

    public void EnableGoodsUI(bool value)
    {
        m_GoodsUI.gameObject.SetActive(value);
        if (value)
        {
            //���� �������� �Լ��� �ҷ��ͼ� ��ȭ�� ǥ��
            m_GoodsUI.SetValues();
        }
    }
}
