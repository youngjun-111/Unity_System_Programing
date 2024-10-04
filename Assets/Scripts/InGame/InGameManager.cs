using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController;

    protected override void Init()
    {
        //�ΰ��� �Ŵ����� �ΰ��� ���� ����� �����Ǿ���ϴϱ� true�� ����
        m_IsDestroyOnLoad = true;

        base.Init();
    }
    private void Start()
    {
        //�� ������ �ΰ�����������Ʈ�ѷ� ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� ã�Ƽ� ������
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("�ΰ��� ������ ��Ʈ�ѷ��� ����;;");
            return;
        }

        InGameUIController.Init();
    }
}
