using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {

    }
    private void Update()
    {
        //�ΰ����� �Ͻ����� �Ǿ����� Ȯ���ؼ� �Ͻ����� ���� ���� ���� ��ǲ�� ó���� �ֵ���
        if (!InGameManager.Instance.IsPaused && !InGameManager.Instance.IsStageCleared)
        {
            HandleInput();
        }
    }

    //ESCŰ�� ������ ��
    void HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //ȿ���� ���
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);
            //BaseUIData(Action) ������
            var uiData = new BaseUIData();
            //UI������
            UIManager.Instance.OpenUI<PauseUI>(uiData);
            //�Ͻ�����
            InGameManager.Instance.PauseGame();
        }
    }

    //���� �Ͻ� ���� ��ư�� ������ ��
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
    //������ PC�� ��� ����ȭ���̳� �ٸ� ���α׷����� ��Ż�ϰų�
    //�������� ��
    //����� ����̽��� ��� ���� �����ų� �ٽ� �÷��� ��
    //MonoBehaviourŬ������ ����� Ŭ������� ȣ��Ǵ� �Լ�
    //�Ű������δ� bool ������ �ް� �ȴ�.
    //ture�� �������� �ٽ� ���ƿԴٴ°�
    //false �� ������ ��Ż�ߴٶ�� ��
    //OS�� �ٸ���� ���̰� ���� �� ����
    //�ȵ���̵��� ��� ������ ó�� �����ص� �װ͵� ���� �÷ȴٶ�� focus�� ture�� �ѹ� ���� ��.
    //ios�� ù ����� ȣ���� �ȵȴٰ� ��(���� ���� ���� ���� Ȯ�� X)
    //�׷��� �ȵ���̵� ���� ��� ���� ó���� ����� �� ���� ����
    private void OnApplicationFocus(bool focus)
    {
        //����Ƽ ���� �Լ��ε� focus�� ������ ���� �ִ� �� ���� �̴�.
        //false�� �������� �ʴٸ��̴�
        if (!focus)
        {
            //�Ͻ� ������ �Ǿ����� �ʾҴٸ�
            if (!InGameManager.Instance.IsPaused && InGameManager.Instance.IsStageCleared)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
            }
        }
    }
}
