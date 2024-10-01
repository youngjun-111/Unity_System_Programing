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
    //버튼 텍스트
    public TextMeshProUGUI EquipBtnTxt;

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
        if (gradeBgTexture != null)
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
        //트루면 탈착, 펄스면 장착
        EquipBtnTxt.text = m_EquipmentUIData.IsEquipped ? "Unequip" : "Equip";
    }
    //탈착 장착 버튼을 눌렀을 때 호출 할 함수
    public void OnClickEquipBtn()
    {
        //유저인벤토리데이터를 가저옴
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //널이면 애러
        if(userInventoryData == null)
        {
            Logger.Log("UserInventoryData does not exist.");
            return;
        }

        //장착 중이면 탈착 가능하게
        if (m_EquipmentUIData.IsEquipped)
        {
            userInventoryData.UnequipItem(m_EquipmentUIData.SerialNuber, m_EquipmentUIData.ItemId);
        }

        //탈착 했을 때 장착이 다시 가능하게
        else
        {
            userInventoryData.EquipItem(m_EquipmentUIData.SerialNuber, m_EquipmentUIData.ItemId);
        }

        //유저 인벤토리 데이터에 변경점을 저장
        userInventoryData.SaveData();
        //아이템 장착 또는 탈착 했을 때는 인벤토리UI를 갱신도 해줘야함
        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        //열려 있으면
        if(inventoryUI != null)
        {
            //장착 여부에 따른 UI처리 함수를 호출해 주겠음.
            if (m_EquipmentUIData.IsEquipped)
            {
                inventoryUI.OnUnequipItem(m_EquipmentUIData.ItemId);
            }
            else
            {
                inventoryUI.OnEquipItem(m_EquipmentUIData.ItemId);
            }
        }
        //장착하면 장착하고 UI는 닫아줌
        CloseUI();
    }
}
