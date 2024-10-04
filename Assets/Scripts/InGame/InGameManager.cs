using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; set; }
    //플레이할려고 선택한 챕터
    int m_SelectedChapter;
    //현재 스테이지
    int m_CurrStage;
    //스테이지 프리팹을 로드할 디렉토리 상수를 선언
    const string STAGE_PATH = "Stages/";
    //스테이지 오브젝트의 트랜스폼 변수
    Transform m_StageTrs;
    //백그라운드 스프라이트 랜더러 컴퍼넌트 변수
    SpriteRenderer m_Bg;


    protected override void Init()
    {
        //인게임 매니저는 인게임 씬을 벗어나면 삭제되어야하니까 true로 설정
        m_IsDestroyOnLoad = true;
        base.Init();
        //스테이지 초기화 함수
        InitVariables();
        LoadStage();
        //Color color, float startAlpha, float endAlpha, float duration, float startDelay,
        //bool deactiveOnFinish, Action onFinish = null//액션은 로비 매니저에서 처리해 줬음
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
        Logger.Log($"페이드 아웃");
    }

    void InitVariables()
    {
        Logger.Log($"{GetType()}::초기화");
        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        //스테이지는 1로 초기화 인게임에 진입하면 무조건 첫 번째 스테이지부터 시작해야 하니까
        m_CurrStage = 1;
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();

        if(userPlayData == null)
        {
            Logger.LogError("유저 플레이 데이터가 없음;;");
            return;
        }
        //현재 챕터 값을 대입
        m_SelectedChapter = userPlayData.SelectedChapter;
    }

    //스테이지 로드
    void LoadStage()
    {
        Logger.Log($"{GetType()}::로드 스테이지");
        //현재 챕터와 스테이지를 로그로 찍어주기
        Logger.Log($"챕터 : {m_SelectedChapter} 스테이지 : {m_CurrStage}");
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3 자릿수 맞추기 001 이런식으로
        if(bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(1f, 1f));
        }
        //스테이지 프리팹을 로드하여 인스턴스를 생성 해줌
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;

        stageObj.transform.SetParent(m_StageTrs);
        //스케일 포지션 초기화
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;
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
