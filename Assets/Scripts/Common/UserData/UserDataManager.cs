using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    //����� ���� ������ ���� ����
    public bool ExistsSavedData { get; set; }
    //��� ���� ������ �ν��Ͻ��� �����ϴ� �����̳�
    //��� UserDataŬ������ IUserData �������̽��� �����ϱ� ������
    //IUserData Ÿ������ �����̳ʸ� �����ϸ� ��� ���� ������ Ŭ������ �� �����̳ʿ� ������ �� ����
    public List<IUserData> UserDataList { get; set; } = new List<IUserData>();

    protected override void Init()
    {
        //��Ŭ���ν��Ͻ� ó���� ���� �Լ����� ���� �Ǳ� ������ ���࿩��.
        base.Init();
        //��� ���� �����͸� UserDataList�� �߰�
        //���� ������ - ����â
        UserDataList.Add(new UserSettingsData());
        //���� ������ - ��ȭ
        UserDataList.Add(new UserGoodsData());
        //�κ��丮 ������ - ������
        UserDataList.Add(new UserInventoryData());
    }
    //��� ���������͸� �⺻ ������ �ʱ�ȭ �ϴ� �Լ�
    public void SetDefaultUserData() 
    {
        for (int i = 0; i < UserDataList.Count; i++)
        {
            //����Ʈ �ȿ� ���� ���� ���� ������
            UserDataList[i].SetDefaultData();
        }
    }

    //��� ���� ������ Ŭ������ LoadData�Լ��� ȣ�����ִ� �Լ�
    public void LoadUserData()
    {
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;
        //���� ����� �����Ͱ� �����Ѵٸ�
        if (ExistsSavedData)
        {
            //��� ���������� Ŭ������ LoadData�� ȣ��
            for (int i = 0; i < UserDataList.Count; i++)
            {
                UserDataList[i].LoadData();
            }
        }
    }

    //��� ���������� Ŭ������ SaveData�Լ��� ȣ���ؼ� ��� ���� �����͸� �����ϴ� �Լ�
    public void SaveUserData()
    {
        bool hasSaveError = false;

        for (int i = 0; i < UserDataList.Count; i++)
        {
            //���̺갡 ���������� �̷�� ������ Ȯ�����ִ� �� ���� ����
            bool isSaveSuccess = UserDataList[i].SaveData();
            //���� ���� ������ ���ٸ�
            if (!isSaveSuccess)
            {
                hasSaveError = true;
            }
        }
        //�̷��� �Ǹ� ������ ���������� �� ��, ��� ���̺� ������ ������ ��
        //�ϳ��� ������ �߻��� ����������Ŭ������ �ִٸ� hasSaveError = true�� �� ����.
        //���̺꿡���� �ϳ��� �߻����� �ʾҴٸ�(���̺갡 ���������� �̷�� ����)
        if (!hasSaveError)
        {
            ExistsSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
        }
    }
    //���⼭ T ������ƮŸ���� ã���� �ϴ� UserData�� Ŭ���� Ÿ�� (class, IUserData)
    public T GetUserData<T>() where T : class, IUserData
    {
        //Ÿ���� T�� �� �߿� ù��° �ν��Ͻ��� �����ϰų� ������ null����
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}