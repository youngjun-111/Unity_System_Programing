using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///���� ������ ���� ������ ������ ������ Ŭ����
///���̽� ����ȭ[Serializable]
///�÷��̾������������� ���� �Ǽ� ���ڿ� ���� ������ �� �ֱ� ������
///�� Ŭ������ �ν��Ͻ��� ���ڿ� ������ ��ȯ�� �����ϰ� ����ȭ�� �����ϴٰ� ������ ���ִ� ����.
///����ȭ��? �ν��Ͻ��� byte�� ���ڿ� ������ ��ȯ�ϴ� ���� �ǹ�
/// </summary>

[Serializable]
public class UserItemData
{
    //��ü �����ۿ��� Ư�� �������� �ĺ��ϱ� ���� id
    //unique value
    public long SerialNumber;
    //�ش� �������� �����۵���Ÿ���̺� ���� ItemID
    //���� UI�� ó���� �� �� ������ID�� �����Ͽ� ������ ���̺� �Ŵ�������
    //�ش� �����۵������� �� �����͸� �����ͼ� ������ ���̳� ���ݷ�, ���°� ����
    //������ ǥ�ø� ���� ����
    public int ItemId;

    //���� ������ ������ ������
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}

/// <summary>
///������ ������ ������ ����Ʈ�� ���� ����̽��� �ε��ϰ� �����ϴµ� �� ���� Ŭ����
///�� Ŭ������ �ָ������ �ϸ� �ٷ� �Ʒ��� ���� UserInventoryData��
///�����ϰ� �� ������ ������ ����
///�׸��� �ٷ� �� ������ ������ ������ ������ ����� ����� �ǵ�
///�̷��� ����Ʈ�� �� ��ü �����͸� �÷��̾��������� ������ �� Json��Ʈ������
///��ȯ�� �ؼ� ������ ����
///�׷��� ����Ƽ���� �����ϴ� JsonUtillity��� Json APIŬ������
///����Ʈ�� ������ �� �� ����Ʈ �����̳ʸ� �ٷ�  Json��Ʈ������ ��ȯ�ϴ� ����� �������� ����.
///�� �̷��� ����Ŭ������ �ְ� �� �ȿ� ����Ʈ�� ����� ���·� ��������
///��ȯ�� ����� ���ֱ� ������ �� ���� Ŭ������ ����� ����.
/// </summary>

//wrapper class to parse data yo JSON using JSONUtillity
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

//2. �������� ���ݷ°� ������ ��Ƴ��� ���ο� Ŭ����
public class UserItemStats
{
    public int AttackPower;
    public int Defense;
    //������
    public UserItemStats(int attackPower, int defense)
    {
        AttackPower = attackPower;
        Defense = defense;
    }
}

public class UserInventoryData : IUserData
{

    //1. ���� �� ���� ������ �������� ������
    //2. �����ϰ� �ִ� �������� ������ �����ؾ���.
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAccessoryData { get; set; }

    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    //1. ������ �����ϱ����� Ŭ���� ��ųʸ�
    public Dictionary<long, UserItemStats> EquippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //�⺻������ 12���� �������� ������ �ֵ��� �ϰ���.
        //�������� �ø��� �ѹ���  �ٸ� �����۰� ��ġ�� �ʴ� ������ ���̾�� �Ѵ�.
        //���� �� ������Ʈ���� ������ ��Ģ���� �ø��� �ѹ��� ����
        //������ID�� ���������� �����ۿ� ���� ������ �����ϸ鼭 ������ ���� ���� ������� ����
        //��Ģ�� ������ �� �ƴϹǷ� �ڽ��� ��Ģ���� �ø��� �ѹ��� ����� �ȴ�.
        // InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") �̷��� ���� �ڿ� ��� �ۼ�
        //���� �ð��� ������ ���� �ٿ��� �ø��� �ѹ��� �������ٰ���
        //���ο� ���� ������ �����Ͱ��� ����ȯ�� ���ÿ� IN
        #region �׽�Ʈ
        //_Temp_1
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21002));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31002));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41002));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51002));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61001));
        //InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61002));
        #endregion

        //_Temp_2
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 15001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 14002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 25001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 24002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 35002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 35001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 45002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 45001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 52002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 62001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 62002));
        
        //�⺻������ ����� ���и� ��� �����ϰ� �ش��ϴ� ���Կ� �ش�Ǵ� �������� ����
        //�̰� �ֱ⸸�Ѱ� ���� UI���� �������� �ؾ���
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
        EquippedGlovesData = new UserItemData(InventoryItemDataList[7].SerialNumber, InventoryItemDataList[7].ItemId);
        //6. ������ ó�� �������� ���� ����Ǿ� �ִ� �������� �ε��ϴ� ���� �ƴ϶�
        //����Ʈ�� ȣ���ؼ� ������ �������� ��ųʸ��� �߰� �ǰ�
        SetEquippedItemDic();
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //2)���� ���� ������ �����͸� �ε��ϴ� ������ �ۼ�
            //���� ���� �����͸� �ε�
            //�÷��̾� ���������� ������ͷ� ����Ǿ� �ִ� ���ڿ� �����Ͱ�
            //�ִٸ�(������ �������̶���)

            //����
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemID{EquippedWeaponData.ItemId}");
            }
            //����
            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemID{EquippedShieldData.ItemId}");
            }
            //����
            string chestArmorJson  = PlayerPrefs.GetString("EquippedChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData  = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemID{EquippedChestArmorData.ItemId}");
            }
            //�尩
            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemID{EquippedGlovesData.ItemId}");
            }
            //����
            string bootsJson  = PlayerPrefs.GetString("EquippedBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemID{EquippedBootsData.ItemId}");
            }
            //�Ǽ�����
            string accessoryJson  = PlayerPrefs.GetString("EquippedAccessoryData");
            if (!string.IsNullOrEmpty(accessoryJson))
            {
                EquippedAccessoryData = JsonUtility.FromJson<UserItemData>(accessoryJson);
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemID{EquippedAccessoryData.ItemId}");
            }

            //�̺��丮�����۵��Ÿ ����Ʈ�� ������ ��Ʈ�� ���� �ִ��� Ȯ��
            //���� �����Ͱ� �����Ѵٸ� ���̽���ƿ��Ƽ Ŭ������ �̿��� 
            //������ ���� ����Ŭ������ ����� �����͸� �޾ƿ� InventoryItemDataList : ������ Key ��
            string inventoryItemDataJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataJson))
            {
                //�÷����������� ����� ��� �� �ܰ谡 ��! �ʿ��� �ܰ��� ����Ʈ�����̳ʸ� �������ִ� ��
                UserInventoryItemDataListWrapper itemDataListWrapper =JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataJson);
                //�� ���� Ŭ���� ���� �ִ� �κ��丮�����۵����͸���Ʈ�� �ִµ����͸�
                //�����̺��丮�����۵������� �κ��丮�����۵����� ����Ʈ ������ ����
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;
                Logger.Log("InventoryItemDataList");//�� �ε� �ǰ��ִ��� Ȯ��
                //�ø��� �ѹ��� ������ ���̵� ����
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }
            //5. ���� �κ��丮�� �ε��� �� ��� �ε��� ������ ���� �Լ��� ȣ���ؼ� ����
            SetEquippedItemDic();
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    //������ ������ �������� �����ϴ� �Լ�
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;

        try
        {
            //3���� �ݴ�� ���� ������ �����͸� �����ϴ� ������ �ۼ�
            //������Ͱ� ���̾ ������ �������̾��
            //�ݵ�� ��ȯ�ؼ� ������ �־����

            //����
            string weponJson = JsonUtility.ToJson(EquippedWeaponData);
            PlayerPrefs.SetString("EquippedWeaponData", weponJson);
            if(EquippedWeaponData != null)
            {
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemID{EquippedWeaponData.ItemId}");
            }
            //����
            string shieldJson = JsonUtility.ToJson(EquippedShieldData);
            PlayerPrefs.SetString("EquippedShieldData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemID{EquippedShieldData.ItemId}");
            }
            //����
            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedChestArmorData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemID{EquippedChestArmorData.ItemId}");
            }
            //�尩
            string glovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedGlovesData", glovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemID{EquippedGlovesData.ItemId}");
            }
            //����
            string bootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedBootsData", bootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemID{EquippedBootsData.ItemId}");
            }
            //�Ǽ�����
            string accessoryJson = JsonUtility.ToJson(EquippedAccessoryData);
            PlayerPrefs.SetString("EquippedAccessoryData", accessoryJson);
            if (EquippedAccessoryData != null)
            {
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemID{EquippedAccessoryData.ItemId}");
            }

            //���忡 �ʿ��� ���� Ŭ���� �ν��Ͻ��� ����
            //�� �ν��Ͻ� �ȿ� �ִ� ������ �����͸���Ʈ�� ������ ���� ������ 
            //�κ��丮 ������ ������ ��� �ִ� �κ��丮 ������ ������ ����Ʈ�� ����
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //�� �����͸� JsonUtillityŬ������ �̿��ؼ� ��Ʈ������ ��ȯ
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //�� ��Ʈ�� ���� �÷��̾��������� ����
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);
            Logger.Log("InventoryItemDataList");
            foreach (var item in InventoryItemDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
            }

            PlayerPrefs.Save();
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }

        return result;
    }

    //3. �� ���� ���� �����͸� Ȯ���ؼ� ������ �������� �ִٸ� �ʿ��� ������ �����ؼ�
    //��ųʸ��� �߰�
    public void SetEquippedItemDic()
    {
        //����
        if(EquippedWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedWeaponData.ItemId);
            if(itemData != null)
            {
                EquippedItemDic.Add(EquippedWeaponData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //����
        if (EquippedShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedShieldData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedShieldData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //��
        if (EquippedChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedChestArmorData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedChestArmorData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //�尩
        if (EquippedGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedGlovesData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedGlovesData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //�Ź�
        if (EquippedBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedBootsData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedBootsData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //�Ǽ�
        if (EquippedAccessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedAccessoryData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedAccessoryData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
    }

    //4. Ư�� �������� �����Ǿ� �ִ��� ���θ� Ȯ���ϴ� �Լ�
    public bool IsEquipped(long serialNumber)
    {
        return EquippedItemDic.ContainsKey(serialNumber);
    }

    //������ ���� ó������ �Լ�
    public void EquipItem(long serialNumber, int itemId)
    {
        //���������̺��� �ش� �����ۿ� ���� �����͸� �����´�.
        var itemData = DataTableManager.Instance.GetItemData(itemId);
        //������ ����
        if(itemData == null)
        {
            Logger.LogError($"Item data does not exist. ItemId:{itemId}");
            return;
        }
        //������ ���� ����
        var itemTpye = (ItemType)(itemId / 10000);
        //������ ������ ���� �б�
        switch (itemTpye)
        {
            case ItemType.Weapon:
                if(EquippedWeaponData != null)
                {
                    EquippedItemDic.Remove(EquippedWeaponData.SerialNumber);
                    EquippedWeaponData = null;
                }
                EquippedWeaponData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Shield:
                if (EquippedShieldData != null)
                {
                    EquippedItemDic.Remove(EquippedShieldData.SerialNumber);
                    EquippedShieldData = null;
                }
                EquippedShieldData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.ChestArmor:
                if (EquippedChestArmorData != null)
                {
                    EquippedItemDic.Remove(EquippedChestArmorData.SerialNumber);
                    EquippedChestArmorData = null;
                }
                EquippedChestArmorData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Gloves:
                if (EquippedGlovesData != null)
                {
                    EquippedItemDic.Remove(EquippedGlovesData.SerialNumber);
                    EquippedGlovesData = null;
                }
                EquippedGlovesData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Boots:
                if (EquippedBootsData != null)
                {
                    EquippedItemDic.Remove(EquippedBootsData.SerialNumber);
                    EquippedBootsData = null;
                }
                EquippedBootsData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Accessory:
                if (EquippedAccessoryData != null)
                {
                    EquippedItemDic.Remove(EquippedAccessoryData.SerialNumber);
                    EquippedAccessoryData = null;
                }
                EquippedAccessoryData = new UserItemData(serialNumber, itemId);
                break;
            default:
                break;
        }
        EquippedItemDic.Add(serialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
    }

    //������ Ż�� �Լ�
    public void UnequipItem(long serialNumber, int itemId)
    {
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                EquippedWeaponData = null;
                break;
            case ItemType.Shield:
                EquippedShieldData = null;
                break;
            case ItemType.ChestArmor:
                EquippedChestArmorData = null;
                break;
            case ItemType.Gloves:
                EquippedGlovesData = null;
                break;
            case ItemType.Boots:
                EquippedBootsData = null;
                break;
            case ItemType.Accessory:
                EquippedAccessoryData = null;
                break;
            default:
                break;
        }
        //��ųʸ����� ����
        EquippedItemDic.Remove(serialNumber);
    }

    //������ ���� ������ ������ ������ ������ ���ϴ� �Լ�
    public UserItemStats GetUserTotalItemStats()
    {
        var totalAttackPower = 0;
        var totalDefense = 0;

        foreach (var item in EquippedItemDic)
        {
            totalAttackPower += item.Value.AttackPower;
            totalDefense += item.Value.Defense;
        }
        return new UserItemStats(totalAttackPower, totalDefense);
    }
}