using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperMaxim.Messaging;
using System;

//재화 변동시 발생할 메시지 클래스
//골드 변동 메세지
public class GoldUpdateMsg
{
    //재화가 증가한 것인지 감소한 것인지 여부를 나타내는 변수
    public bool isAdd;
}
//보석 변동 메세지
public class GemUpdateMsg
{
    public bool isAdd;
}


public class GoodsUI : MonoBehaviour
{
    //유저가 보유한 보석과 골드 수량을 표시해줄 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;
    //골드 아이템 위치를 알기 위해 골드 아이콘 잼 아이콘 변수 선언
    public Image GoldIcon;
    public Image GemIcon;
    //골드 증가 연출을 코루틴을 통해서 실행할 예정
    //실행 중인 코루틴을 참조할 수 있는 코루틴 변수 선언
    Coroutine m_GoldIncreaseCo;
    Coroutine m_GemIncreaseCo;
    //이변수를 선언하는 이유는 만약 재화 획득이 빠르게 여러번 요청되어 이미 획득 연출이
    //진행 중인데 새로운 획득 연출 요청 처리
    //기존 획득 연출을 취소하고 새로운 획득 연출로 덮어쓰기 위함
    const float GOODS_INCRASE_DURATION = 0.5f;

    //사용 방법
    //위에 두가지 메세지가 발생되었을 때 이 GoodsUI가 구독자가 되어야 하니
    //이 클레스가 활성화 될때
    //이 클래스를 재화 변동 메시지 구독자로 등록
    //여기에 한 이유는 인스턴스가 활성화 되어 있을 때만
    //재화 변동 메시지를 받아 처리하길 원하기 때문
    private void OnEnable()
    {
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);
    }

    //유저 재화 데이터를 불러와 보석과 골드 수량을 세팅해주는 함수
    public void SetValues()
    {
        //일단 유저 데이터를 가져오고
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //데이터를 못가져오거나 없으면 오류
        if (userGoodData == null)
        {
            Logger.LogError("굿즈 데이터가 없음;;");
        }
        else
        {
            GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodData.Gem.ToString("N0");
        }
    }

    #region 골드 연출
    //먼저 gold 재화가 변경 되었을 시 실행할 함수 작성 획득 연출
    //매게변수로 메세지를 받고
    //함수 선언 GoldUI인스턴스에서 GoldUpdate메시지를 받았을 때
    //이 함수가 자동 실행
    void OnUpdateGold(GoldUpdateMsg goldUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);
        if (goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null)
            {
                StopCoroutine(m_GoldIncreaseCo);
            }
            m_GoldIncreaseCo = StartCoroutine(IncreaseGoldCo());
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }

    IEnumerator IncreaseGoldCo()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);
        var elapedTime = 0f;
        //0~ 부터 시작되게 해줄 text
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        //실제로 증가되어 표시되어야할 골드 수치
        var destValue = userGoodsData.Gold;
        while(elapedTime < GOODS_INCRASE_DURATION)
        {
            //매프레임 경과 시간에 따라 연출 시간과의 비율을 계상해서 현재 표시해야할 텍스트 값을 산출
            var currValue = Mathf.Lerp(currTextValue, destValue, elapedTime / GOODS_INCRASE_DURATION);
            //산출한 수치를 UI텍스트 컴포넌트에 표시
            GoldAmountTxt.text = currValue.ToString("N0");
            //경과 시간을 증가
            elapedTime += Time.deltaTime;

            //다음 호출시 까지 대기
            yield return null;
        }
        //돈올라가는 연출이 끝나면 도달 수치를 텍스트 컴포넌트에 표시
        GoldAmountTxt.text = destValue.ToString("N0");
    }
    #endregion

    #region 잼 연출
    void OnUpdateGem(GemUpdateMsg gemUpdateMsg)
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get);
        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCo != null)
            {
                StopCoroutine(m_GemIncreaseCo);
            }
            m_GemIncreaseCo = StartCoroutine(IncreaseGemCo());
        }
        else
        {
            GemAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }

    IEnumerator IncreaseGemCo()
    {
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            gemObj.GetComponent<GoodsMove>().SetMove(i, GemIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);
        var elapedTime = 0f;
        var currTextValue = Convert.ToInt64(GemAmountTxt.text.Replace(",", ""));
        //실제로 증가되어 표시되어야할 잼 수치
        var destValue = userGoodsData.Gem;
        while (elapedTime < GOODS_INCRASE_DURATION)
        {
            //매프레임 경과 시간에 따라 연출 시간과의 비율을 계산해서 현재 표시해야할 텍스트 값을 산출
            var currValue = Mathf.Lerp(currTextValue, destValue, elapedTime / GOODS_INCRASE_DURATION);
            //산출한 수치를 UI텍스트 컴포넌트에 표시
            GemAmountTxt.text = currValue.ToString("N0");
            //경과 시간을 증가
            elapedTime += Time.deltaTime;
            //다음 호출시 까지 대기
            yield return null;
        }
        //돈올라가는 연출이 끝나면 도달 수치를 텍스트 컴포넌트에 표시
        GemAmountTxt.text = destValue.ToString("N0");
    }
    #endregion
}