using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; set; }

    //게임 클리어 여부
    public bool IsStageCleared { get; set; }
    //일시정지 여부의 프로퍼티
    public bool IsPaused { get; set; }
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

    //현재 스테이지 오브젝트 인스턴스를 가지고 있을 게임 오브젝트 변수
    GameObject m_LoadedStage;
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

    private void Update()
    {
        CheckStageClear();
    }


    protected override void Init()
    {
        //인게임 매니저는 인게임 씬을 벗어나면 삭제되어야하니까 true로 설정
        m_IsDestroyOnLoad = true;
        base.Init();
        //스테이지 초기화 함수
        InitVariables();
        //기존에 LoadStage함수에 한번에 처리한 부분을 배경을 호출하는 부분 분리
        LoadBg();
        LoadStage();
        //Color color, float startAlpha, float endAlpha, float duration, float startDelay,
        //bool deactiveOnFinish, Action onFinish = null//액션은 로비 매니저에서 처리해 줬음
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
        Logger.Log($"페이드 아웃");
    }

    //클리어를 체크해줌
    void CheckStageClear()
    {
        if (IsStageCleared)
        {
            return;
        }

        /// <summary>
        /// 클리어 조건은 여기서 추가하면 됨
        /// </summary>
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();
        }
    }

    //실제로 클리어 처리 함수
    void ClearStage()
    {
        Logger.Log($"{GetType()}:스테이지 클리어");
        IsStageCleared = true;

        StartCoroutine(ShowStageClearCo());
    }

    IEnumerator ShowStageClearCo()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData);

        yield return new WaitForSeconds(1f);
        //스테이지를 클리어했다면 클리어 사운드 재생하고 클리어 UI 오픈하고
        //1초뒤에 클리어 UI를 다시 닫고 현재스테이지를 +1 해주고 다시 스테이지를 로드해줌
        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if (stageClearUI)
        {
            stageClearUI.CloseUI();
        }
        IsStageCleared = false;
        m_CurrStage++;
        LoadStage();
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
        //현재 로드된 스테이지 오브젝트가 있다면 삭제
        if (m_LoadedStage)
        {
            Destroy(m_LoadedStage);
        }
        //스테이지 프리팹을 로드하여 인스턴스를 생성 해줌
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;

        stageObj.transform.SetParent(m_StageTrs);
        //스케일 포지션 초기화
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        //새롭게 로드한 스테이지 오브젝트를 대입
        m_LoadedStage = stageObj;
    }

    //배경을 불러올 함수
    void LoadBg()
    {
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3 자릿수 맞추기 001 이런식으로
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(1f, 1f));
        }
    }

    public void PauseGame()
    {
        IsPaused = true;
        //앞으로 구현할 인게임의 일시정지를 처리하는 코드
        //GameManager.Instance.Paused = true;
        //LevelManager.Intance.ToggleCharacterPause();
        Time.timeScale = 0f;
        //타임 스케일을 0 으로 바꾸면 그로 인해 영향을 받는 것들이 많아서
        //일시정지를 한 후 게임 컨텐츠적으로 유저가 어떤 행위를 하도록 구현하는데 많은 제약이 생기게 됨
        //그렇다고 타임스케일에 영향을 받지 않도록 타임스케일을 우회해서
        //게임 컨텐츠를 개발하게 되면 많은 시간과 비용을 소모하게 될 수 있으므로 웬만하면
        //타임스케일을 1로 유지하면 일시정지를 처리하는 것을 권장
        //어떤 게임이냐에 따라서 단순히 유저의 인풋만 막거나 게임 내의 타이머만 제어해도
        //처리가 충분히 될 수 있기 때문에 기획적으로 잘 고민해서 최대한 복잡하지 않게 일시정지 처리
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}