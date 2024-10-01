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

        //_Temp_2
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 13001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 13002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 14001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 14002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 15002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 15001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 25002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 25001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 32002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 42001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 42002));
        
        //�⺻������ ����� ���и� ��� �����ϰ� �ش��ϴ� ���Կ� �ش�Ǵ� �������� ����
        //�̰� �ֱ⸸�Ѱ� ���� UI���� �������� �ؾ���
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
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
}

