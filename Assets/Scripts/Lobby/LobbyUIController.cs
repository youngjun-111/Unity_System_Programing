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
        //(주의 사항)EnableGoodsUI 함수명이 어느순간 부터 꼬엿네?
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrChapter();
        //로비에서는 켜줘야 하기때문에 UIManager에서 작성해준 재화 활성화 비활성화 함수 호출
        UIManager.Instance.EnableGoodsUI(true);
    }
    //현재 선택 중인 챕터에 대한 UI 처리를 해줄 함수
    public void SetCurrChapter()
    {
        //가져올 데이터가 뭐가 있을지 생각 해보면
        //현재 선택중인 팹터 번호를 가진 해당 챕터 데이터를 가져오고
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("없다");
            return;
        }
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);
        //해당 데이터가 정상적으로 존재한다면
        //챕터명을 표시 챕터 이미지도 로드해서 세팅
        CurrentChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBG/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;
        if(bgTexture != null)
        {
            CurrentChapterBg.texture = bgTexture;
        }
    }

    //로비UI프리팹을 생성시켜주고, 뭐 특정 행동 했을 때 
    //로비씬에서 설정 버튼을 누르면 SettingUI(프리팹)가 열리는 함수
    //설정 버튼을 눌렀을때 처리해줄 것들
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        //로그
        Logger.Log($"{GetType()}::OnClickSettingBtn");
        //데이터 인스턴스를 베이스유아이데이터클래스로 만들어줌
        var uiData = new BaseUIData();
        //UI매니저를 통해 인벤토리를 열게함
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OcClickCurrChapter");
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }

    public void OnClickStartBtn()
    {
        Logger.Log($"{GetType()}::스타트 버튼 누름");
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        AudioManager.Instance.StopBGM();
        //이건이제 로비 매니저에서 구현해서 관리해줄 것임.
        //왜?? 다른건 전부다 UI컨트롤러에서 해줬으면서.. 왜 스타트버튼만 Manager에서 하는건지?
        //다른건 PopupUI 라면 이 버튼은 씬전환이기 때문에
        LobbyManager.Instance.StartInGame();
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