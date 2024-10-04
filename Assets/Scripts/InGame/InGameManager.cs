using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController;

    protected override void Init()
    {
        //인게임 매니저는 인게임 씬을 벗어나면 삭제되어야하니까 true로 설정
        m_IsDestroyOnLoad = true;

        base.Init();
    }
    private void Start()
    {
        //씬 내에서 인게임유아이컨트롤러 스크립트를 가지고 있는 오브젝트를 찾아서 대입함
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("인게임 유아이 컨트롤러가 없음;;");
            return;
        }

        InGameUIController.Init();
    }
}
