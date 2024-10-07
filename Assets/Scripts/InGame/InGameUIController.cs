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
        //인게임이 일시정지 되었는지 확인해서 일시정지 되지 않을 때만 인풋을 처리해 주도록
        if (!InGameManager.Instance.IsPaused && !InGameManager.Instance.IsStageCleared)
        {
            HandleInput();
        }
    }

    //ESC키를 눌렀을 때
    void HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //효과음 재생
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);
            //BaseUIData(Action) 생성자
            var uiData = new BaseUIData();
            //UI열어줌
            UIManager.Instance.OpenUI<PauseUI>(uiData);
            //일시정지
            InGameManager.Instance.PauseGame();
        }
    }

    //실제 일시 정지 버튼을 눌렀을 때
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
    //유저가 PC의 경우 바탕화면이나 다른 프로그램으로 이탈하거나
    //복귀했을 때
    //모바일 디바이스의 경우 앱을 내리거나 다시 올렸을 때
    //MonoBehaviour클래스를 상속한 클래스라면 호출되는 함수
    //매개변수로는 bool 변수를 받게 된다.
    //ture면 게임으로 다시 돌아왔다는걸
    //false 면 게임을 이탈했다라는 뜻
    //OS가 다를경우 차이가 있을 수 있음
    //안드로이드의 경우 게임을 처음 실행해도 그것도 앱을 올렸다라고 focus가 ture로 한번 실행 됨.
    //ios는 첫 실행시 호출이 안된다고 함(변경 됐을 수도 있음 확실 X)
    //그래서 안드로이드 같은 경우 예외 처리를 해줘야 할 수도 있음
    private void OnApplicationFocus(bool focus)
    {
        //유니티 제공 함수인데 focus는 게임을 보고 있는 불 변수 이다.
        //false면 보고있지 않다면이다
        if (!focus)
        {
            //일시 정지가 되어있지 않았다면
            if (!InGameManager.Instance.IsPaused && InGameManager.Instance.IsStageCleared)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
            }
        }
    }
}
