using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsUI : MonoBehaviour
{
    //������ ������ ������ ��� ������ ǥ������ �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;

    //���� ��ȭ �����͸� �ҷ��� ������ ��� ������ �������ִ� �Լ�
    public void SetValues()
    {
        //�ϴ� ���� �����͸� ��������
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //�����͸� ���������ų� ������ ����
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
        }else
        {
            GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodData.Gem.ToString("N0");
        }
    }
}
