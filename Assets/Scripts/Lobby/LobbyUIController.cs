using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void Init()
    {
        //�κ񿡼��� ����� �ϱ⶧���� UIManager���� �ۼ����� ��ȭ Ȱ��ȭ ��Ȱ��ȭ �Լ� ȣ��
        UIManager.Instance.EnableGoodsUI(true);
    }

    //�κ�UI�������� ���������ְ�, �� Ư�� �ൿ ���� �� 
    //�κ������ ���� ��ư�� ������ SettingUI(������)�� ������ �Լ�
    //���� ��ư�� �������� ó������ �͵�
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        //�α�
        Logger.Log($"{GetType()}::OnClickSettingBtn");
        //������ �ν��Ͻ��� ���̽������̵�����Ŭ������ �������
        var uiData = new BaseUIData();
        //UI�Ŵ����� ���� �κ��丮�� ������
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
    private void Update()
    {
        MandleInput();
    }

    private void MandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            if (frontUI != null)
            {
                frontUI.CloseUI();
            }
            else
            {
                var uiData = new ConfirmUIData();
                uiData.ConfirmType = ConfirmType.OK_CANCEL;
                uiData.TitleTxt = "Quit";
                uiData.DescTxt = "Do you want to quit game?";
                uiData.OkBtnTxt = "Quit";
                uiData.CancelBtnTxt = "Cancel";
                uiData.OnClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
    }
}