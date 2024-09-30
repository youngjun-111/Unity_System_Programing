using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Text;



public class EquippedItemSlot : MonoBehaviour
{
    //장착된 아이템이 없으면 + 표시의 아이콘
    public Image AddIcon;
    //장착된 아이템이 있을 때 표시해줄 아이템 아이콘
    public Image EquippedItemIcon;
    //장착한아이템 데이터를 저장할 변수
    UserItemData m_EquippedItemData;

    //아이템을 장착했을 경우
    public void SetItem(UserItemData userItemData)
    {
        m_EquippedItemData = userItemData;

        //장착된 아이템이 있으면 +아이콘은 비활성화
        AddIcon.gameObject.SetActive(false);
        //장착된 아이템이 있을 때는 선언해준 장착된 아이템 아이콘 표시
        EquippedItemIcon.gameObject.SetActive(true);

        StringBuilder sb = new StringBuilder(m_EquippedItemData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if (itemIconTexture != null)
        {
            EquippedItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    //아이템이 없을경우
    public void ClearItem()
    {
        m_EquippedItemData = null;

        AddIcon.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    //일반 인벤토리 아이템 슬롯과 마찬가지로
    //이 장착 아이템 슬롯도 클릭하면 아이템 상세UI를 열어주는 처리
    public void OnClickEquippedItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNuber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
