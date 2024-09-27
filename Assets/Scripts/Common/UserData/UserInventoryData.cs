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

public class UserInventoryData : IUserData
{
    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //�⺻������ 12���� �������� ������ �ֵ��� �ϰ���.
        //�������� �ø��� �ѹ���  �ٸ� �����۰� ��ġ�� �ʴ� ������ ���̾�� �Ѵ�.
        //���� �� ������Ʈ���� ������ ��Ģ���� �ø��� �ѹ��� ����
        //������ID�� ���������� �����ۿ� ���� ������ �����ϸ鼭 ������ ���� ���� ������� ����
        //��Ģ�� ������ �� �ƴϹǷ� �ڽ��� ��Ģ���� �ø��� �ѹ��� ����� �ȴ�.
        // InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") �̷��� �� �ڿ� ��� �ۼ�
        //���� �ð��� ������ ���� �ٿ��� �ø��� �ѹ��� �������ٰ���
        //���ο� ���� ������ �����Ͱ��� ����ȯ�� ���ÿ� IN
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61002));
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
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