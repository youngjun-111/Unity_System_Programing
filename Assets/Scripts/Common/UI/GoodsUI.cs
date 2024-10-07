using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperMaxim.Messaging;
using System;

//��ȭ ������ �߻��� �޽��� Ŭ����
//��� ���� �޼���
public class GoldUpdateMsg
{
    //��ȭ�� ������ ������ ������ ������ ���θ� ��Ÿ���� ����
    public bool isAdd;
}
//���� ���� �޼���
public class GemUpdateMsg
{
    public bool isAdd;
}


public class GoodsUI : MonoBehaviour
{
    //������ ������ ������ ��� ������ ǥ������ �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;
    //��� ������ ��ġ�� �˱� ���� ��� ������ �� ������ ���� ����
    public Image GoldIcon;
    public Image GemIcon;
    //��� ���� ������ �ڷ�ƾ�� ���ؼ� ������ ����
    //���� ���� �ڷ�ƾ�� ������ �� �ִ� �ڷ�ƾ ���� ����
    Coroutine m_GoldIncreaseCo;
    Coroutine m_GemIncreaseCo;
    //�̺����� �����ϴ� ������ ���� ��ȭ ȹ���� ������ ������ ��û�Ǿ� �̹� ȹ�� ������
    //���� ���ε� ���ο� ȹ�� ���� ��û ó��
    //���� ȹ�� ������ ����ϰ� ���ο� ȹ�� ����� ����� ����
    const float GOODS_INCRASE_DURATION = 0.5f;

    //��� ���
    //���� �ΰ��� �޼����� �߻��Ǿ��� �� �� GoodsUI�� �����ڰ� �Ǿ�� �ϴ�
    //�� Ŭ������ Ȱ��ȭ �ɶ�
    //�� Ŭ������ ��ȭ ���� �޽��� �����ڷ� ���
    //���⿡ �� ������ �ν��Ͻ��� Ȱ��ȭ �Ǿ� ���� ����
    //��ȭ ���� �޽����� �޾� ó���ϱ� ���ϱ� ����
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

    //���� ��ȭ �����͸� �ҷ��� ������ ��� ������ �������ִ� �Լ�
    public void SetValues()
    {
        //�ϴ� ���� �����͸� ��������
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //�����͸� ���������ų� ������ ����
        if (userGoodData == null)
        {
            Logger.LogError("���� �����Ͱ� ����;;");
        }
        else
        {
            GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodData.Gem.ToString("N0");
        }
    }

    #region ��� ����
    //���� gold ��ȭ�� ���� �Ǿ��� �� ������ �Լ� �ۼ� ȹ�� ����
    //�ŰԺ����� �޼����� �ް�
    //�Լ� ���� GoldUI�ν��Ͻ����� GoldUpdate�޽����� �޾��� ��
    //�� �Լ��� �ڵ� ����
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
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);
        var elapedTime = 0f;
        //0~ ���� ���۵ǰ� ���� text
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        //������ �����Ǿ� ǥ�õǾ���� ��� ��ġ
        var destValue = userGoodsData.Gold;
        while(elapedTime < GOODS_INCRASE_DURATION)
        {
            //�������� ��� �ð��� ���� ���� �ð����� ������ ����ؼ� ���� ǥ���ؾ��� �ؽ�Ʈ ���� ����
            var currValue = Mathf.Lerp(currTextValue, destValue, elapedTime / GOODS_INCRASE_DURATION);
            //������ ��ġ�� UI�ؽ�Ʈ ������Ʈ�� ǥ��
            GoldAmountTxt.text = currValue.ToString("N0");
            //��� �ð��� ����
            elapedTime += Time.deltaTime;

            //���� ȣ��� ���� ���
            yield return null;
        }
        //���ö󰡴� ������ ������ ���� ��ġ�� �ؽ�Ʈ ������Ʈ�� ǥ��
        GoldAmountTxt.text = destValue.ToString("N0");
    }
    #endregion

    #region �� ����
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
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
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
        //������ �����Ǿ� ǥ�õǾ���� �� ��ġ
        var destValue = userGoodsData.Gem;
        while (elapedTime < GOODS_INCRASE_DURATION)
        {
            //�������� ��� �ð��� ���� ���� �ð����� ������ ����ؼ� ���� ǥ���ؾ��� �ؽ�Ʈ ���� ����
            var currValue = Mathf.Lerp(currTextValue, destValue, elapedTime / GOODS_INCRASE_DURATION);
            //������ ��ġ�� UI�ؽ�Ʈ ������Ʈ�� ǥ��
            GemAmountTxt.text = currValue.ToString("N0");
            //��� �ð��� ����
            elapedTime += Time.deltaTime;
            //���� ȣ��� ���� ���
            yield return null;
        }
        //���ö󰡴� ������ ������ ���� ��ġ�� �ؽ�Ʈ ������Ʈ�� ǥ��
        GemAmountTxt.text = destValue.ToString("N0");
    }
    #endregion
}