using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class EquipmentUIData : BaseUIData
{
    public long SerialNuber;
    public int ItemId;
    public bool IsEquipped;
}
public class EquipmentUI : BaseUI
{
    //아이템 등급 이미지
    public Image ItemGradeBg;
    //아이템 이미지
    public Image ItemIcon;
    //아이템 등급 텍스트
    public TextMeshProUGUI ItemGradeTxt;
    //아이템 이름 텍스트
    public TextMeshProUGUI ItemNameTxt;
    //아이템 스텟 텍스트( 공격력, 방어력)
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;

    EquipmentUIData m_EquipmentUIData;


    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        m_EquipmentUIData = uiData as EquipmentUIData;
        if(m_EquipmentUIData == null)
        {
            //인벤토리랑 동일하게 없으면 리턴
            Logger.Log("m_EquipmentUIData is invalid.");
            return;
        }
        //상제 정보를 표시해주기 위해선 아이템 데이터 테이블에서
        //해당 아이템 정보를 가지고 와야함.
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);

        if(itemData == null)
        {
            Logger.LogError($"Item data is invaild. ItemId:{m_EquipmentUIData.ItemId}");
            return;
        }

        //누른 아이템의 등급 이미지와 등급 텍스트, 이름, 장착 버튼, 능력치 표시해주기
        //등급 추출
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        //추출한 아이템 등급 정보로 아이템 등급 이미지 로드
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        //이미지가 잘 로드 되어있으면 아이템 그레이드이미지 컴포넌트에 세팅
        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //아이템 등급을 텍스트로 표시
        ItemGradeTxt.text = itemGrade.ToString();
        var hexColor = string.Empty;
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }
        //컬러값을 변수로 지정해주고
        Color color;
        //컬러 값을 Html 컬러 문자열로 바꿔주는 문법(Ex : "#1AB3FF");
        if(ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        //아이템 아이콘 리소스를 로드해서 세팅
        //이부분은 인벤토리 UI슬롯 때와 처리가 동일
        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if(itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f,1f));
        }
        ItemNameTxt.text = itemData.ItemName;
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}";
        DefenseAmountTxt.text = $"+{itemData.Defense}";
    }
}
