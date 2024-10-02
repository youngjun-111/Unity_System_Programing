using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI CurrentChapterNameTxt;
    public RawImage CurrentChapterBg;
    public void Init()
    {
        //(���� ����)EnableGoodsUI �Լ����� ������� ���� ������?
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrChapter();
        //�κ񿡼��� ����� �ϱ⶧���� UIManager���� �ۼ����� ��ȭ Ȱ��ȭ ��Ȱ��ȭ �Լ� ȣ��
        UIManager.Instance.EnableGoodsUI(true);
    }
    //���� ���� ���� é�Ϳ� ���� UI ó���� ���� �Լ�
    public void SetCurrChapter()
    {
        //������ �����Ͱ� ���� ������ ���� �غ���
        //���� �������� ���� ��ȣ�� ���� �ش� é�� �����͸� ��������
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("����");
            return;
        }
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);
        //�ش� �����Ͱ� ���������� �����Ѵٸ�
        //é�͸��� ǥ�� é�� �̹����� �ε��ؼ� ����
        CurrentChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBg/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;
        if(bgTexture != null)
        {
            CurrentChapterBg.texture = bgTexture;
        }
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

    public void OcClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OcClickCurrChapter");
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
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