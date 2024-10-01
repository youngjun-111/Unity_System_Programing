using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using TMPro;

//1. 먼저 어떤 조건에 의해 정렬을 할것인지를 나타내는 이넘값을 선언
public enum InventorySortType
{
    //등급
    ItemGrade,
    //종류
    ItemType,
}

//이벤토리 UI는 유저 인벤토리데이터에서 UI세팅에 필요한 데이터를 가져올 것임
//전용 UI데이터 클래스는 만들지 않음.(UI호출 시 그냥 Base UI데이터를 사용)
public class InventoryUI : BaseUI
{
    //1. 먼저 각 장착 슬롯 컴포넌트 변수를 선언
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;

    //스크롤 뷰를 처리해줄 인피니티 스크롤 변수
    public InfiniteScroll InventoryScrollList;
    //현재 어떤 조건으로 정렬되어 있는지 표시해줄 텍스트
    public TextMeshProUGUI SortBtnTxt;
    //공격력과 방어력의 총합을 표시하는 텍스트 컴포넌트
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;

    //현재 정렬 방식을 갖고 있는 변수 선언 초기 값은 등급으로 설정
    InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        //유저의 모든 스텟을 셋
        SetUserStats();
        //장착된 아이템 셋
        SetEquippedItems();
        //인벤토리 처음 셋
        SetInventory();
        //인벤토리 처음 정렬
        SortInventory();
    }

    #region 유저 스텟 초기, 장착, 탈착
    void SetUserStats()
    {
        //유저 인벤토리 가져오고
        var userInvetoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInvetoryData == null)
        {
            Logger.Log("UserInventory does is not exist.");
            return;
        }
        var userTotalItemStats = userInvetoryData.GetUserTotalItemStats();
        AttackPowerAmountTxt.text = $"+{userTotalItemStats.AttackPower.ToString("N0")}";
        DefenseAmountTxt.text = $"+{userTotalItemStats.Defense.ToString("N0")}";
    }
    #endregion

    #region 인벤토리 갱신 정렬
    void SetInventory()
    {
        //UI화면을 재활용하기 때문에 새롭게 세팅할 때마다 삭제처리를 해주지 않으면
        //기존에 생성된 아이템들이 그대로 남아 있게됨. 그래서 인피니티스크롤 안에있는 Clear함수 를 호출
        InventoryScrollList.Clear();
        //유저가 보유한 아이템을 스크롤 뷰에 만들자
        //우선 유저 인벤토리 데이터를 유저 데이터매니저에서 가져옴
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
     
        if(userInventoryData != null)
        {
            //순회하며 각 아이템에 대해서 아이템 슬롯 인스턴스를 만들어준다.
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                //스크롤 뷰에 아이템 데이터를 하나씩 추가해 줄 때
                //만약 장착된 아이템이라면 예외 처리를 해주겠음.
                if (userInventoryData.IsEquipped(itemData.SerialNumber))
                {
                    continue;
                }
                //실제로 슬롯에 있는 데이터를 실제 아이템 데이터를 넣어주기
                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    //인벤토리 정렬 함수
    void SortInventory()
    {
        //타입에 따라 분기 시킴
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortBtnTxt.text = "GRADE";
                //위에서 인피니티스크롤에 만든 숏데이터리스트 함수를 호출하면서
                //원하는 등급별 정렬 조건 로직을 람다 형식으로 작성해서 넘겨줌
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b데이터를 인벤토리아이템슬롯데이터로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //아이템 ID의 두번째 자릿수가 아이템의 등급을 나타내기 때문에 그 등급 값을 가져와서 비교
                    //여기서 itemB등급을 기준으로 비교한 것은 그래야 내림차순으로 정렬이됨.
                    //높은 등급에서 낮은 등급으로 정렬되기를 원하기 때문에 내림차순으로 
                    //CompareTo : 앞, 뒤 또는 동일한지를 나타내는 정수를 반환
                    //0보다 작음 이 인스턴스가 밸류 앞에 오는 경우 이런식임.
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    //결과 값이 0 이라면 즉, 등급이 같다면 같은 등급내에서는
                    //종류별로 다시 정렬을 해줘야함
                    //종류별 정렬을 아이템 ID에서 등급을 나타내는 두번째 자릿수만을 제외한 나머지 값으로 비교
                    if (compareResult == 0)
                    {
                        var itemAIdStr = itemA.ItemId.ToString();
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                        var itemBIdStr = itemB.ItemId.ToString();
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);
                        compareResult = itemAComp.CompareTo(itemBComp);
                    }
                    return compareResult;
                });
                break;
            case InventorySortType.ItemType:
                SortBtnTxt.text = "TYPE";
                //위에서 인피니티스크롤에 만든 숏데이터리스트 함수를 호출하면서
                //원하는 등급별 정렬 조건 로직을 람다 형식으로 작성해서 넘겨줌
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b데이터를 인벤토리아이템슬롯데이터로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item type
                    var itemAIdStr = itemA.ItemId.ToString();
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    int compareResult = itemAComp.CompareTo(itemBComp);

                    if (compareResult == 0)
                    {
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }
                    return compareResult;
                });
                break;
            default:
                break;
        }
    }
    //인벤토리 UI정렬 버튼을 눌렀을 때 현재 정렬 조건을
    //다른 정렬 조건으로 변경하고 그값에 따라 다시 정렬해주는 기능
    public void OnClickSortBtn()
    {
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_InventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }
        //인벤토리 정렬 갱신
        SortInventory();
    }
    #endregion

    #region 아이템 탈부착
    //2. 장착된 아이템에 대한 UI처리를 담당할 함수 작성
    void SetEquippedItems()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
            return;
        }
        //널이 아니면 SetItem 실행 날이면 ClearItem 실행

        //무기
        if(userInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
        }
        //방패
        if (userInventoryData.EquippedShieldData != null)
        {
            ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
        }
        else
        {
            ShieldSlot.ClearItem();
        }
        //갑옷
        if (userInventoryData.EquippedChestArmorData != null)
        {
            ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
        }
        else
        {
            ChestArmorSlot.ClearItem();
        }
        //장갑
        if (userInventoryData.EquippedGlovesData != null)
        {
            GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
        }
        else
        {
            GlovesSlot.ClearItem();
        }
        //신발
        if (userInventoryData.EquippedBootsData != null)
        {
            BootsSlot.SetItem(userInventoryData.EquippedBootsData);
        }
        else
        {
            BootsSlot.ClearItem();
        }
        //악세
        if (userInventoryData.EquippedAccessoryData != null)
        {
            AccessorySlot.SetItem(userInventoryData.EquippedAccessoryData);
        }
        else
        {
            AccessorySlot.ClearItem();
        }
    }

    //아이템 장착을 한 후 UI처리에 대한 함수를 먼저 작성
    public void OnEquipItem(int itemId)
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
            return;
        }
        //종류에 따른 분기 처리
        var itemType = (ItemType)(itemId / 10000);

        switch (itemType)
        {
            case ItemType.Weapon:
                WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
                break;
            case ItemType.Shield:
                ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
                break;
            case ItemType.Gloves:
                GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
                break;
            case ItemType.Boots:
                BootsSlot.SetItem(userInventoryData.EquippedBootsData);
                break;
            case ItemType.Accessory:
                AccessorySlot.SetItem(userInventoryData.EquippedAccessoryData);
                break;
        }
        //스텟 적용
        SetUserStats();
        //인벤토리 갱신
        SetInventory();
        //인벤토리 정렬 갱신
        SortInventory();
    }

    //탈착 후 UI 처리에 대한 함수
    public void OnUnequipItem(int itemId)
    {
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                WeaponSlot.ClearItem();
                break;
            case ItemType.Shield:
                ShieldSlot.ClearItem();
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.ClearItem();
                break;
            case ItemType.Gloves:
                GlovesSlot.ClearItem();
                break;
            case ItemType.Boots:
                BootsSlot.ClearItem();
                break;
            case ItemType.Accessory:
                AccessorySlot.ClearItem();
                break;
            default:
                break;
        }
        //스텟 적용
        SetUserStats();
        //인벤토리 갱신
        SetInventory();
        //인벤토리 정렬 갱신
        SortInventory();
    }
    #endregion
}