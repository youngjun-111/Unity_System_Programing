using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//여기서는 SetInfo함수도 오버라이드 해줄 것도 없음.
//일시정지 해제 버튼을 눌렀을 때 실행할 함수와
//홈 버튼을 눌렀을 때 로비신으로 돌아가도록 처리할 함수를 작성
//일시정지 유아이는 일시정지 버튼을 누르거나
//유저가 화면을 벗어났을 때 이 퍼즈 유아이가 열리게하는법?
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
