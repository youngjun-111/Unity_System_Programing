using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    //�κ� ��Ʈ�ѷ��� ������Ƽ�� ����
    public LobbyUIController LobbyUIController { get; set; }
    protected override void Init()
    {
        //�κ� �Ŵ����� �ٸ� ������ ��ȯ �� �� ������ �� ����.
        m_IsDestroyOnLoad = true;
        base.Init();
    }

    private void Start()
    {
        //FindObjectofType�� ���� �����ϴ� Ÿ���� ã�� ���� ���� ã���ν��Ͻ��� �Ѱ���
        //�κ���������Ʈ�ѷ��� �κ������ �ϳ��� �����Ұ���
        LobbyUIController = FindObjectOfType<LobbyUIController>();
        //�����ϰ� ������ �� �̴ϱ� ���̶�� �ַ��ڵ� ���
        if (!LobbyUIController)
        {
            Logger.LogError("LobbyUIController does not exist");
            return;
        }

        LobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }
}
