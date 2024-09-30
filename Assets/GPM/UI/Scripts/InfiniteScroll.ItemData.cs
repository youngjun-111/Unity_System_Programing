namespace Gpm.Ui
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Events;

    public partial class InfiniteScroll
    {
        public class DataContext
        {
            public DataContext(InfiniteScrollData data, int index)
            {
                this.index = index;
                this.data = data;
            }

            //접근 가능하게 프로퍼티로 변경. 대신 인피니티 스크롤 클래스 내에서만 변경이 가능하게 제한
            public InfiniteScrollData data { get; set; }
            internal int index = -1;

            internal int itemIndex = -1;

            internal float offset = 0;

            internal bool needUpdateItemData = true;

            internal float scrollItemSize = 0;

            internal InfiniteScrollItem itemObject;

            public bool IsNeedUpdateItemData()
            {
                return needUpdateItemData;
            }

            public void UnlinkItem(bool notifyEvent = false)
            {
                if (itemObject != null)
                {
                    itemObject.ClearData(notifyEvent);
                    itemObject = null;
                }

                itemIndex = -1;
            }

            public void UpdateData(InfiniteScrollData data)
            {
                this.data = data;
                needUpdateItemData = true;
            }

            public float GetItemSize()
            {
                return scrollItemSize;
            }

            public void SetItemSize(float value)
            {
                scrollItemSize = value;
            }
        }

        protected List<DataContext> dataList = new List<DataContext>();
        protected int itemCount = 0;

        protected bool needUpdateItemList = true;

        protected int selectDataIndex = -1;
        protected Action<InfiniteScrollData> selectCallback = null;

        public int GetDataIndex(InfiniteScrollData data)
        {
            if (isInitialize == false)
            {
                Initialize();
            }

            return dataList.FindIndex((context) =>
            {
                return context.data.Equals(data);
            });
        }

        public int GetDataCount()
        {
            return dataList.Count;
        }

        public InfiniteScrollData GetData(int index)
        {
            return dataList[index].data;
        }

        public List<InfiniteScrollData> GetDataList()
        {
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            for(int index = 0; index < dataList.Count; index++)
            {
                list.Add(dataList[index].data);
            }
            return list;
        }

        public List<InfiniteScrollData> GetItemList()
        {
            List<InfiniteScrollData> list = new List<InfiniteScrollData>();

            for (int index = 0; index < dataList.Count; index++)
            {
                if(dataList[index].itemIndex != -1)
                {
                    list.Add(dataList[index].data);
                }
            }
            return list;
        }

        public int GetItemCount()
        {
            return itemCount;
        }

        public int GetItemIndex(InfiniteScrollData data)
        {
            var context = GetDataContext(data);
            return context.itemIndex;
        }

        public void AddSelectCallback(Action<InfiniteScrollData> callback)
        {
            if (isInitialize == false)
            {
                Initialize();
            }

            selectCallback += callback;
        }

        public void RemoveSelectCallback(Action<InfiniteScrollData> callback)
        {
            if (isInitialize == false)
            {
                Initialize();
            }

            selectCallback -= callback;
        }

        public void OnChangeActiveItem(int dataIndex, bool active)
        {
            onChangeActiveItem.Invoke(dataIndex, active);
        }

        protected DataContext GetDataContext(InfiniteScrollData data)
        {
            if (isInitialize == false)
            {
                Initialize();
            }

            return dataList.Find((context) =>
            {
                return context.data.Equals(data);
            });
        }

        protected DataContext GetContextFromItem(int itemIndex)
        {
            if (isInitialize == false)
            {
                Initialize();
            }

            if (IsValidItemIndex(itemIndex) == true)
            {
                return GetItem(itemIndex);
            }
            else
            {
                return null;
            }
        }

        protected void AddData(InfiniteScrollData data)
        {
            DataContext addData = new DataContext(data, dataList.Count);
            InitFitContext(addData);

            dataList.Add(addData);

            CheckItemAfterAddData(addData);
        }

        private bool CheckItemAfterAddData(DataContext addData)
        {
            if (onFilter != null &&
                onFilter(addData.data) == true)
            {
                return false;
            }

            int itemIndex = 0;
            if (itemCount > 0)
            {
                for (int dataIndex = addData.index - 1; dataIndex >= 0; dataIndex--)
                {
                    if (dataList[dataIndex].itemIndex != -1)
                    {
                        itemIndex = dataList[dataIndex].itemIndex + 1;
                        break;
                    }
                }
            }

            addData.itemIndex = itemIndex;
            itemCount++;

            for (int dataIndex = addData.index+1; dataIndex < dataList.Count; dataIndex++)
            {
                if (dataList[dataIndex].itemIndex != -1)
                {
                    dataList[dataIndex].itemIndex++;
                }
            }
            
            needReBuildLayout = true;

            return true;
        }

        protected void InsertData(InfiniteScrollData data, int insertIndex)
        {
            if (insertIndex < 0 || insertIndex > dataList.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (insertIndex < dataList.Count)
            {
                DataContext addData = new DataContext(data, insertIndex);
                InitFitContext(addData);
                
                for (int dataIndex = insertIndex; dataIndex < dataList.Count; dataIndex++)
                {
                    dataList[dataIndex].index++;
                }

                dataList.Insert(insertIndex, addData);

                CheckItemAfterAddData(addData);
            }
            else
            {
                AddData(data);
            }
        }

        protected void InitFitContext(DataContext context)
        {
            float size = layout.GetMainSize(defaultItemPrefabSize);
            if (dynamicItemSize == true)
            {
                float ItemSize = context.GetItemSize();
                if (ItemSize != 0)
                {
                    size = ItemSize;
                }
            }

            context.SetItemSize(size);
        }

        protected bool IsValidDataIndex(int index)
        {
            return (index >= 0 && index < dataList.Count) ? true : false;
        }

        protected bool IsValidItemIndex(int index)
        {
            return (index >= 0 && index < itemCount) ? true : false;
        }

        protected void BuildItemList()
        {
            itemCount = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                DataContext context = dataList[i];

                if (onFilter != null &&
                     onFilter(context.data) == true)
                {
                    context.UnlinkItem(false);

                    continue;
                }
                context.itemIndex = itemCount;
                itemCount++;
            }

            needReBuildLayout = true;
        }
        
        private void OnSelectItem(InfiniteScrollData data)
        {
            int dataIndex = GetDataIndex(data);
            if (IsValidDataIndex(dataIndex) == true)
            {
                selectDataIndex = dataIndex;

                if (selectCallback != null)
                {
                    selectCallback(data);
                }
            }
        }
        //Comparison타입은 C#에서 제공하는 일정의 델리게이트
        //이 델리게이트는 두개의 데이터컨텍스트 객체를 비교하는 매서드를 위임받게됨.
        //그래서 comparsion파라미터에 원하는 정렬 로직을 작성해 넘겨주어
        //그 로직대로 스크롤 아이템 목록이 정렬되도록 하겠음.
        //타입이 테이터컨텍스트인 이유는 인피니티스크롤 컴포넌트에서
        //내부적으로 데이터 컨텍스트라는 클래스에 데이터를 갖고있기 때문.
        public void SortDataList(Comparison<DataContext> comparison)
        {
            dataList.Sort(comparison);
            needUpdateItemList = true;
            UpdateShowItem();
        }
    }
}
