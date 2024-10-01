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
    //������ ��� �̹���
    public Image ItemGradeBg;
    //������ �̹���
    public Image ItemIcon;
    //������ ��� �ؽ�Ʈ
    public TextMeshProUGUI ItemGradeTxt;
    //������ �̸� �ؽ�Ʈ
    public TextMeshProUGUI ItemNameTxt;
    //������ ���� �ؽ�Ʈ( ���ݷ�, ����)
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;
    //��ư �ؽ�Ʈ
    public TextMeshProUGUI EquipBtnTxt;

    EquipmentUIData m_EquipmentUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        m_EquipmentUIData = uiData as EquipmentUIData;
        if(m_EquipmentUIData == null)
        {
            //�κ��丮�� �����ϰ� ������ ����
            Logger.Log("m_EquipmentUIData is invalid.");
            return;
        }
        //���� ������ ǥ�����ֱ� ���ؼ� ������ ������ ���̺���
        //�ش� ������ ������ ������ �;���.
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);

        if(itemData == null)
        {
            Logger.LogError($"Item data is invaild. ItemId:{m_EquipmentUIData.ItemId}");
            return;
        }

        //���� �������� ��� �̹����� ��� �ؽ�Ʈ, �̸�, ���� ��ư, �ɷ�ġ ǥ�����ֱ�
        //��� ����
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        //������ ������ ��� ������ ������ ��� �̹��� �ε�
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        //�̹����� �� �ε� �Ǿ������� ������ �׷��̵��̹��� ������Ʈ�� ����
        if (gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //������ ����� �ؽ�Ʈ�� ǥ��
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
        //�÷����� ������ �������ְ�
        Color color;
        //�÷� ���� Html �÷� ���ڿ��� �ٲ��ִ� ����(Ex : "#1AB3FF");
        if(ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        //������ ������ ���ҽ��� �ε��ؼ� ����
        //�̺κ��� �κ��丮 UI���� ���� ó���� ����
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
        //Ʈ��� Ż��, �޽��� ����
        EquipBtnTxt.text = m_EquipmentUIData.IsEquipped ? "Unequip" : "Equip";
    }
    //Ż�� ���� ��ư�� ������ �� ȣ�� �� �Լ�
    public void OnClickEquipBtn()
    {
        //�����κ��丮�����͸� ������
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //���̸� �ַ�
        if(userInventoryData == null)
        {
            Logger.Log("UserInventoryData does not exist.");
            return;
        }

        //���� ���̸� Ż�� �����ϰ�
        if (m_EquipmentUIData.IsEquipped)
        {
            userInventoryData.UnequipItem(m_EquipmentUIData.SerialNuber, m_EquipmentUIData.ItemId);
        }

        //Ż�� ���� �� ������ �ٽ� �����ϰ�
        else
        {
            userInventoryData.EquipItem(m_EquipmentUIData.SerialNuber, m_EquipmentUIData.ItemId);
        }

        //���� �κ��丮 �����Ϳ� �������� ����
        userInventoryData.SaveData();
        //������ ���� �Ǵ� Ż�� ���� ���� �κ��丮UI�� ���ŵ� �������
        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        //���� ������
        if(inventoryUI != null)
        {
            //���� ���ο� ���� UIó�� �Լ��� ȣ���� �ְ���.
            if (m_EquipmentUIData.IsEquipped)
            {
                inventoryUI.OnUnequipItem(m_EquipmentUIData.ItemId);
            }
            else
            {
                inventoryUI.OnEquipItem(m_EquipmentUIData.ItemId);
            }
        }
        //�����ϸ� �����ϰ� UI�� �ݾ���
        CloseUI();
    }
}
