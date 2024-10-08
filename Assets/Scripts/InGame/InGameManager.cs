using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; set; }

    //���� Ŭ���� ����
    public bool IsStageCleared { get; set; }
    //�Ͻ����� ������ ������Ƽ
    public bool IsPaused { get; set; }
    //�÷����ҷ��� ������ é��
    int m_SelectedChapter;
    //���� ��������
    int m_CurrStage;
    //�������� �������� �ε��� ���丮 ����� ����
    const string STAGE_PATH = "Stages/";
    //�������� ������Ʈ�� Ʈ������ ����
    Transform m_StageTrs;
    //��׶��� ��������Ʈ ������ ���۳�Ʈ ����
    SpriteRenderer m_Bg;
    ChapterData m_CurrChapterData;
    //���� �������� ������Ʈ �ν��Ͻ��� ������ ���� ���� ������Ʈ ����
    GameObject m_LoadedStage;

    //private Coin[] m_Coins;

    private void Start()
    {
        //�� ������ �ΰ�����������Ʈ�ѷ� ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� ã�Ƽ� ������
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("�ΰ��� ������ ��Ʈ�ѷ��� ����;;");
            return;
        }

        InGameUIController.Init();
        //_TEMP
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
        }
    }

    private void Update()
    {
        CheckStageClear();
    }


    protected override void Init()
    {
        //�ΰ��� �Ŵ����� �ΰ��� ���� ����� �����Ǿ���ϴϱ� true�� ����
        m_IsDestroyOnLoad = true;
        base.Init();
        //�������� �ʱ�ȭ �Լ�
        InitVariables();
        //������ LoadStage�Լ��� �ѹ��� ó���� �κ��� ����� ȣ���ϴ� �κ� �и�
        LoadBg();
        LoadStage();
        //Color color, float startAlpha, float endAlpha, float duration, float startDelay,
        //bool deactiveOnFinish, Action onFinish = null//�׼��� �κ� �Ŵ������� ó���� ����
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
        Logger.Log($"���̵� �ƿ�");
    }

    //Ŭ��� üũ����
    void CheckStageClear()
    {
        if (IsStageCleared)
        {
            return;
        }

        /// <summary>
        /// Ŭ���� ������ ���⼭ �߰��ϸ� ��
        /// </summary>
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();
        }
    }

    //������ Ŭ���� ó�� �Լ�
    void ClearStage()
    {
        Logger.Log($"{GetType()}:�������� Ŭ����");
        IsStageCleared = true;

        StartCoroutine(ShowStageClearCo());
    }

    IEnumerator ShowStageClearCo()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData);

        yield return new WaitForSeconds(1f);
        //���������� Ŭ�����ߴٸ� Ŭ���� ���� ����ϰ� Ŭ���� UI �����ϰ�
        //1�ʵڿ� Ŭ���� UI�� �ٽ� �ݰ� ���罺�������� +1 ���ְ� �ٽ� ���������� �ε�����
        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if (stageClearUI)
        {
            stageClearUI.CloseUI();
        }
        
        if (IsAllClear())
        {
            ClearChapter();
        }
        else
        {
            IsStageCleared = false;
            m_CurrStage++;
            LoadStage();
        }
    }

    void ClearChapter()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear);
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();

        if(userPlayData == null)
        {
            return;
        }

        var uiData = new ChapterClearUIData();
        uiData.chapter = m_SelectedChapter;
        //�������� ���δ� ����é�Ͱ� ���� �÷��̵������� MaxClearedChapter�� ���� ū���� ��
        uiData.earnReward = m_SelectedChapter > userPlayData.MaxClearedChapter;
        //ChapterClearUI����
        UIManager.Instance.OpenUI<ChapterClearUI>(uiData);

        if(m_SelectedChapter > userPlayData.MaxClearedChapter)
        {
            //é�� + 1
            userPlayData.MaxClearedChapter++;
            userPlayData.SelectedChapter = userPlayData.MaxClearedChapter + 1;
            //������ é�͵� ��� �ر��� ���� é�ͷ� ����
            //�̴� �κ�� ������ �� �ر��� é�Ͱ� ������ é�ͷ� ���õǵ��� �ϱ� ����
            userPlayData.SaveData();
        }
        //é�� Ŭ���� ���� ó��
        //���� ���� �����͸� ��������
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            //���� ������ é�Ϳ� ���� ����ġ������ �б⸦ �༭
            //é�� 1,2,3�� ���� ���� ���� ó���� ���ְ���
            //é�� Ŭ���� ������ �ܼ��ϰ� 1�� �޼� ��ġ�� �Ѱ��ְ���
            //�ش� é���� Ÿ���� ������������ �� Ŭ���� Ÿ�Կ���
            //csv���� �ȿ��ִ� achievement_goal�� �����ִ� �޼� ���ڸ� ����
            switch (m_SelectedChapter)
            {
                case 1:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
                    break;
                case 2:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter2, 1);
                    break;
                case 3:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
                    break;
                default:
                    break;
            }
        }
    }

    //���� ���������� é�͵������� ��Ż�������� ������ ���� ���� ��
    private bool IsAllClear()
    {
        return m_CurrStage == m_CurrChapterData.TotalStage;
    }

    void InitVariables()
    {
        Logger.Log($"{GetType()}::�ʱ�ȭ");
        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        //���������� 1�� �ʱ�ȭ �ΰ��ӿ� �����ϸ� ������ ù ��° ������������ �����ؾ� �ϴϱ�
        m_CurrStage = 19;
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();

        if(userPlayData == null)
        {
            Logger.LogError("���� �÷��� �����Ͱ� ����;;");
            return;
        }
        //���� é�� ���� ����
        m_SelectedChapter = userPlayData.SelectedChapter;

        //���� ������ é���� é�� �����͸� ���� m_CurrChapterData ������ ����
        m_CurrChapterData = DataTableManager.Instance.GetChapterData(m_SelectedChapter);

        if(m_CurrChapterData == null)
        {
            Logger.LogError($"����;;{m_CurrChapterData}");
            return;
        }
    }

    //�������� �ε�
    void LoadStage()
    {
        Logger.Log($"{GetType()}::�ε� ��������");
        //���� é�Ϳ� ���������� �α׷� ����ֱ�
        Logger.Log($"é�� : {m_SelectedChapter} �������� : {m_CurrStage}");
        //���� �ε�� �������� ������Ʈ�� �ִٸ� ����
        if (m_LoadedStage)
        {
            Destroy(m_LoadedStage);
        }
        //�������� �������� �ε��Ͽ� �ν��Ͻ��� ���� ����
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;

        stageObj.transform.SetParent(m_StageTrs);
        //������ ������ �ʱ�ȭ
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        //���Ӱ� �ε��� �������� ������Ʈ�� ����
        m_LoadedStage = stageObj;
    }

    //����� �ҷ��� �Լ�
    void LoadBg()
    {
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3 �ڸ��� ���߱� 001 �̷�������
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(1f, 1f));
        }
    }

    public void PauseGame()
    {
        IsPaused = true;
        //������ ������ �ΰ����� �Ͻ������� ó���ϴ� �ڵ�
        //GameManager.Instance.Paused = true;
        //LevelManager.Intance.ToggleCharacterPause();
        Time.timeScale = 0f;
        //Ÿ�� �������� 0 ���� �ٲٸ� �׷� ���� ������ �޴� �͵��� ���Ƽ�
        //�Ͻ������� �� �� ���� ������������ ������ � ������ �ϵ��� �����ϴµ� ���� ������ ����� ��
        //�׷��ٰ� Ÿ�ӽ����Ͽ� ������ ���� �ʵ��� Ÿ�ӽ������� ��ȸ�ؼ�
        //���� �������� �����ϰ� �Ǹ� ���� �ð��� ����� �Ҹ��ϰ� �� �� �����Ƿ� �����ϸ�
        //Ÿ�ӽ������� 1�� �����ϸ� �Ͻ������� ó���ϴ� ���� ����
        //� �����̳Ŀ� ���� �ܼ��� ������ ��ǲ�� ���ų� ���� ���� Ÿ�̸Ӹ� �����ص�
        //ó���� ����� �� �� �ֱ� ������ ��ȹ������ �� ����ؼ� �ִ��� �������� �ʰ� �Ͻ����� ó��
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}