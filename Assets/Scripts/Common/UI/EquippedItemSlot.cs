using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Text;



public class EquippedItemSlot : MonoBehaviour
{
    //������ �������� ������ + ǥ���� ������
    public Image AddIcon;
    //������ �������� ���� �� ǥ������ ������ ������
    public Image EquippedItemIcon;
    //�����Ѿ����� �����͸� ������ ����
    UserItemData m_EquippedItemData;

    //�������� �������� ���
    public void SetItem(UserItemData userItemData)
    {
        m_EquippedItemData = userItemData;

        //������ �������� ������ +�������� ��Ȱ��ȭ
        AddIcon.gameObject.SetActive(false);
        //������ �������� ���� ���� �������� ������ ������ ������ ǥ��
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

    //�������� �������
    public void ClearItem()
    {
        m_EquippedItemData = null;

        AddIcon.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    //�Ϲ� �κ��丮 ������ ���԰� ����������
    //�� ���� ������ ���Ե� Ŭ���ϸ� ������ ��UI�� �����ִ� ó��
    public void OnClickEquippedItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNuber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
