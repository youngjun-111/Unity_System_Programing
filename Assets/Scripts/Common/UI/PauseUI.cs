using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���⼭�� SetInfo�Լ��� �������̵� ���� �͵� ����.
//�Ͻ����� ���� ��ư�� ������ �� ������ �Լ���
//Ȩ ��ư�� ������ �� �κ������ ���ư����� ó���� �Լ��� �ۼ�
//�Ͻ����� �����̴� �Ͻ����� ��ư�� �����ų�
//������ ȭ���� ����� �� �� ���� �����̰� �������ϴ¹�?
public class PauseUI : BaseUI
{
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();
        CloseUI();
    }

    public void OnClickHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);
        CloseUI();
    }
}
