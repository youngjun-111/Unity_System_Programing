using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using Gpm.Ui;//Gpm.UI���ִ� Ŭ������ ����ؼ� �������� using


//�κ��丮�����۽��� �ν��Ͻ� ������ ���� �ʿ��� ������ Ŭ�����̴�.
//���Ǵ�Ƽ��ũ�ѵ����͸� ��ӹ������� using Gpm.UI �� �ʿ��ϰ�
//���� ������ ���Ǵ�Ƽ ��ũ��������Ʈ�� ����Ͽ� ��ũ�� �������� �����ϱ� ���ؼ� �̴�.
public class InventoryItemSlotData : InfiniteScrollData
{
    //�ʿ��� �����ʹ� ���� �����۰� �����ϰ� �ø���ѹ��� ���̵� �̴�.
    public long SerialNumber;
    public int ItemId;
}

public class InventoryItemSlot : InfiniteScrollItem
{
    //��޿� ���� �̹����� ó������ ��׶����̹���
    public Image ItemGradeBg;
    //�������� ���������� ���� ������ �̹����� ó������ �������̹���
    public Image ItemIcon;
    //�������� Ŭ������ ������ �����ؼ� ���(�翬�Ѱ�)
    InventoryItemSlotData m_InventoryIteSlotData;
    //������ Gpm.UI���ִ� ���Ǵ�Ƽ��ũ�ѿ��� ���� �ʿ����� �����ϰ�
    //���� ���⼱ �κ��丮 �����ۿ� �ִ� UpdataData�� �������̵� ���ش�
    //���� UI�� ���� ó���� �ϰ� ��.
    //���Ǵ�Ƽ��ũ�ѵ����͸� �Ű������� �޾ƿͼ� UI�� �������� �Լ�
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //��ũ�� �����͸� �κ��丮 ������ ���� �����ͷ� ��ȯ�ؼ� �޴´�.
        m_InventoryIteSlotData = scrollData as InventoryItemSlotData;
        //null �̸� �α׿���
        if(m_InventoryIteSlotData == null)
        {
            Logger.LogError("�κ� ������ ����;;");
            return;
        }
        //������ ��޿� ���� ��׶����̹� ó��
        //(�̼����̸�) ������ ID���� ��� ���ڸ� ���� ����, �̰��� (ItemGrade)�̳� ������ ��ȯ�ؼ� �޾ƿ´�.
        var itemGrade = (ItemGrade)((m_InventoryIteSlotData.ItemId / 1000) % 10);//11001
        //�̷��� �޾ƿ� �̳Ѱ��� �״�� �̹��� ������ ���
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //nullüũ �̻��� ������ �����۱׷��̵� ��׶��� �̹��� ������Ʈ�� �ش� �ؽ�ó�� ��������
        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //�Ϲݵ���� ������ ID�� �̹����� ����� �ξ���. ������ ID�� ��ް��� 1�� ġȯ�غ�����
        StringBuilder sb = new StringBuilder(m_InventoryIteSlotData.ItemId.ToString());
        //�ι�° �ڸ��� ������ 1�� �־���
        sb[1] = '1';
        //�װ� �ٽ� ���ڿ��� ��ȯ
        var itemIconName = sb.ToString();
        //�̷��� ������ �̹��� ���� �ϼ� �Ǿ���
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        //null�˻� ���ְ� �̻��� ������ ������ ��� �̹����� ���������� �������̹��� ������Ʈ�� �ؽ�ó�� ����
        if(itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    //Slot�� Ŭ�������� ������ �Լ�
    public void OnClickIventoryItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNuber = m_InventoryIteSlotData.SerialNumber;
        uiData.ItemId = m_InventoryIteSlotData.ItemId;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
