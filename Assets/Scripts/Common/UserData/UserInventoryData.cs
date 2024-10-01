using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///먼저 보유한 개별 아이템 정보를 저장할 클래스
///제이슨 직렬화[Serializable]
///플레이어프렙스에서는 정수 실수 문자열 값만 저장할 수 있기 때문에
///이 클래스의 인스턴스가 문자열 값으로 변환이 가능하게 직렬화가 가능하다고 선언을 해주는 것임.
///직렬화란? 인스턴스를 byte나 문자열 값으로 변환하는 것을 의미
/// </summary>

[Serializable]
public class UserItemData
{
    //전체 아이템에서 특정 아이템을 식별하기 위한 id
    //unique value
    public long SerialNumber;
    //해당 아이템의 아이템데이타테이블 상의 ItemID
    //이후 UI를 처리할 때 이 아이템ID를 참고하여 데이터 테이블 매니저에서
    //해당 아이템데이터의 상세 데이터를 가져와서 아이템 명이나 공격력, 방어력과 같은
    //스탯을 표시를 해줄 예정
    public int ItemId;

    //유저 아이템 데이터 생성자
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}

/// <summary>
///유저가 보유한 아이템 리스트를 로컬 디바이스에 로드하고 저장하는데 쓸 래퍼 클래스
///이 클래스를 왜만드느냐 하면 바로 아래에 만들 UserInventoryData에
///동일하게 이 변수를 선안할 예정
///그리고 바로 그 변수에 유저가 보유한 아이템 목록이 저장될 건데
///이렇게 리스트로 된 객체 데이터를 플레이어프랩스로 저장할 때 Json스트링으로
///변환을 해서 저장할 것임
///그런데 유니티에서 제공하는 JsonUtillity라는 Json API클래스가
///리스트를 저장할 때 이 리스트 컨테이너를 바로  Json스트링으로 변환하는 기능을 제공하지 않음.
///꼭 이렇게 래퍼클래스가 있고 그 안에 리스트가 선언된 형태로 만들어줘야
///변환을 제대로 해주기 때문에 이 래퍼 클래스를 만드는 거임.
/// </summary>

//wrapper class to parse data yo JSON using JSONUtillity
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}
//2. 아이템의 공격력과 방어력을 모아놓은 새로운 클래스
public class UserItemStats
{
    public int AttackPower;
    public int Defense;
    //생성자
    public UserItemStats(int attackPower, int defense)
    {
        AttackPower = attackPower;
        Defense = defense;
    }
}

public class UserInventoryData : IUserData
{

    //1. 먼저 각 장착 슬롯의 변수들을 선언함
    //2. 장착하고 있는 아이템의 정보를 저장해야함.
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAccessoryData { get; set; }

    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    //1. 스텟을 적용하기위한 클래스 딕셔너리
    public Dictionary<long, UserItemStats> EquippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //기본적으로 12개의 아이템을 지급해 주도록 하겠음.
        //아이템의 시리얼 넘버는  다른 아이템과 겹치지 않는 고유의 값이어야 한다.
        //실제 각 프로젝트마다 고유의 법칙으로 시리얼 넘버를 만듬
        //아이템ID와 마찬가지로 아이템에 대한 정보를 내포하면서 고유한 값을 갖는 방식으로 만듬
        //규칙이 정해진 게 아니므로 자신의 규칙으로 시리얼 넘버를 만들면 된다.
        // InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 이렇게 쓰고 뒤에 등급 작성
        //현재 시간에 랜덤한 수를 붙여서 시리얼 넘버를 생성해줄거임
        //새로운 유저 아이템 데이터가를 생성환과 동시에 IN
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
        
        //기본적으로 무기와 방패를 들고 시작하게 해당하는 슬롯에 해당되는 아이템이 들어가게
        //이건 넣기만한것 이제 UI에서 보여지게 해야함
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
        EquippedGlovesData = new UserItemData(InventoryItemDataList[7].SerialNumber, InventoryItemDataList[7].ItemId);
        //6. 게임을 처음 실행했을 때는 저장되어 있는 아이템을 로딩하는 것이 아니라
        //디폴트로 호출해서 장착된 아이템이 딕셔너리에 추가 되게
        SetEquippedItemDic();
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //2)이제 장착 아이템 데이터를 로드하는 로직을 작성
            //무기 슬롯 데이터를 로드
            //플레이어 프랩스에서 장비데이터로 저장되어 있는 문자열 데이터가
            //있다면(장착한 아이템이란뜻)

            //무기
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemID{EquippedWeaponData.ItemId}");
            }
            //방패
            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemID{EquippedShieldData.ItemId}");
            }
            //갑옷
            string chestArmorJson  = PlayerPrefs.GetString("EquippedChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData  = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemID{EquippedChestArmorData.ItemId}");
            }
            //장갑
            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemID{EquippedGlovesData.ItemId}");
            }
            //부츠
            string bootsJson  = PlayerPrefs.GetString("EquippedBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemID{EquippedBootsData.ItemId}");
            }
            //악세서리
            string accessoryJson  = PlayerPrefs.GetString("EquippedAccessoryData");
            if (!string.IsNullOrEmpty(accessoryJson))
            {
                EquippedAccessoryData = JsonUtility.FromJson<UserItemData>(accessoryJson);
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemID{EquippedAccessoryData.ItemId}");
            }

            //이벤토리아이템디어타 리스트로 저정된 스트링 값이 있는지 확인
            //만약 데이터가 존재한다면 제이슨유틸리티 클래스를 이용해 
            //위에서 만든 래퍼클래스로 저장된 데이터를 받아옴 InventoryItemDataList : 저장한 Key 값
            string inventoryItemDataJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataJson))
            {
                //플레이프레스를 사용할 경우 이 단계가 꼭! 필요한 단계임 리스트컨테이너를 랩핑해주는 것
                UserInventoryItemDataListWrapper itemDataListWrapper =JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataJson);
                //그 래퍼 클래스 내에 있는 인벤토리아이템데이터리스트에 있는데이터를
                //유저이벤토리아이템데이터의 인벤토리아이템데이터 리스트 변수에 대입
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;
                Logger.Log("InventoryItemDataList");//잘 로드 되고있는지 확인
                //시리얼 넘버와 아이템 아이디도 찍어보자
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }
            //5. 유저 인벤토리를 로드할 시 모든 로딩이 끝나고 나면 함수를 호출해서 담음
            SetEquippedItemDic();
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    //유저가 보유한 아이템을 저장하는 함수
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;

        try
        {
            //3이제 반대로 장착 아이템 데이터를 저장하는 로직을 작성
            //장비데이터가 널이어도 장착한 아이템이없어도
            //반드시 반환해서 저장해 주어야함

            //무기
            string weponJson = JsonUtility.ToJson(EquippedWeaponData);
            PlayerPrefs.SetString("EquippedWeaponData", weponJson);
            if(EquippedWeaponData != null)
            {
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemID{EquippedWeaponData.ItemId}");
            }
            //방패
            string shieldJson = JsonUtility.ToJson(EquippedShieldData);
            PlayerPrefs.SetString("EquippedShieldData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemID{EquippedShieldData.ItemId}");
            }
            //갑옷
            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedChestArmorData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemID{EquippedChestArmorData.ItemId}");
            }
            //장갑
            string glovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedGlovesData", glovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemID{EquippedGlovesData.ItemId}");
            }
            //부츠
            string bootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedBootsData", bootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemID{EquippedBootsData.ItemId}");
            }
            //악세서리
            string accessoryJson = JsonUtility.ToJson(EquippedAccessoryData);
            PlayerPrefs.SetString("EquippedAccessoryData", accessoryJson);
            if (EquippedAccessoryData != null)
            {
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemID{EquippedAccessoryData.ItemId}");
            }

            //저장에 필요한 랩퍼 클래스 인스턴스를 생성
            //그 인스턴스 안에 있는 아이템 데이터리스트에 유저가 현재 보유한 
            //인벤토리 아이템 정보를 담고 있는 인벤토리 아이템 데이터 리스트를 대입
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //이 데이터를 JsonUtillity클래스를 이용해서 스트링으로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //이 스트링 값을 플레이어프렙스에 저장
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

    //3. 각 장착 슬롯 데이터를 확인해서 장착된 아이템이 있다면 필요한 전보를 가공해서
    //딕셔너리에 추가
    public void SetEquippedItemDic()
    {
        //무기
        if(EquippedWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedWeaponData.ItemId);
            if(itemData != null)
            {
                EquippedItemDic.Add(EquippedWeaponData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //방패
        if (EquippedShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedShieldData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedShieldData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //방어구
        if (EquippedChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedChestArmorData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedChestArmorData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //장갑
        if (EquippedGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedGlovesData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedGlovesData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //신발
        if (EquippedBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedBootsData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedBootsData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        //악세
        if (EquippedAccessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedAccessoryData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedAccessoryData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
    }

    //4. 특정 아이템이 장착되어 있는지 여부를 확인하는 함수
    public bool IsEquipped(long serialNumber)
    {
        return EquippedItemDic.ContainsKey(serialNumber);
    }

    //아이템 장착 처리해줄 함수
    public void EquipItem(long serialNumber, int itemId)
    {
        //데이터테이블에서 해당 아이템에 대한 데이터를 가져온다.
        var itemData = DataTableManager.Instance.GetItemData(itemId);
        //없으면 에러
        if(itemData == null)
        {
            Logger.LogError($"Item data does not exist. ItemId:{itemId}");
            return;
        }
        //아이템 종류 추출
        var itemTpye = (ItemType)(itemId / 10000);
        //아이템 종류에 따라 분기
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

    //아이템 탈착 함수
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
        //딕셔너리에서 삭제
        EquippedItemDic.Remove(serialNumber);
    }

    //유저가 현재 장착한 아이템 스탯의 총합을 구하는 함수
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