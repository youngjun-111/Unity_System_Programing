using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    //������ ���̺� ���ϵ��� ����ִ� ��θ� ��Ʈ�� ������ ����
    const string DATA_PATH = "DataTable";
    //é�� ������ ���̺� ���ϸ��� ���� ��Ʈ�� ����
    const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    //��� é�� �����͸� ������ ���ִ� �����̳� ��, �ڷᱸ���� ����
    List<ChapterData> ChapterDataTable = new List<ChapterData>();

    //�̱��� ���� �������̵�
    protected override void Init()
    {
        base.Init();
        LoadChapterDataTable();
    }

    void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
        //var Ÿ�� : ������ Ÿ���� �Ű澲�� �ʾƵ� �˾Ƽ� ������ �ִ� ���� �Ǵ��ؼ�
        //Ÿ���� �ν��ϴ� ������ Ÿ��.
        //���������θ� ��� ����
        //�ݵ�� ����� ���ÿ� �ʱ�ȭ(�׷��� ���������� ������Ÿ���� �������� ����)
        //Ÿ���� ������ ��� ���������� ����Ҷ��� var����ص� ������.

        //���̺��� ��ȸ�ϸ鼭 �� �����͸�
        //ChapterData�ν��Ͻ��� ����
        //ChapterDataTable �����̳ʿ� �־���
        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                //������Ʈ Ÿ���̶� ������ ��ü�� ���� 32��Ʈ ��ȣ �ִ� ������ ��ȯ
                //Convert.ToInt32�� ����Ͽ� 32��Ʈ�� ��Ʈ�� ����ȯ �����ִ� ����
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                TotalStage = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"])
            };
            ChapterDataTable.Add(chapterData);
        }
    }

    //�̷��� �ε��� ChapterDataTable���� ã���� �ϴ� ChapterData�� �������� �Լ�
    public ChapterData GetChapterData(int chapterNo)
    {
        //Ư�� é�� �ѹ��� é�� ������ ���̺��� �˻��ؼ�
        //�� é�� �ѹ��� �ش��ϴ� �����͸� ��ȯ�ϴ� �Լ�
        //��ũ��� -> ��ũ : �˻��� ������ �� �� �����ϰ� ���ִ� ����̴�.
        //���� ��ũ�� ������� �ʴ´ٸ�
        /*
        foreach (var item in ChapterDataTable)
        {
            if(item.ChapterNo == chapterNo)
            {
                return item;
            }
        }
        return null;
        */
        //Linq
        //.Where ���ǽ��� true�� ��Ҹ� ���͸����ش�.
        //FirstOrDefault() �Լ� : �Ű������� ������ ��� �÷����� ù ��° ��Ҹ� ��ȯ �մϴ�.
        //�־��� ������ �����ϴ� ���������� ù ��° ��Ҹ� �˻��ϴ� LINQ�� �ż���
        //new[] {"A", "B", "C"}.FirstOrDefault();
        //���//
        //A

        //�� �����̳� �ȿ� �ִ� ������ �߿��� é�� �ѹ��� �Ű����� é�� �ѹ� ���� ���� �� ����
        //�� ���ǿ� �����ϴ� ù ������Ʈ�� �����ϰų� �ƴϸ� �� ���ǿ� �´� ������Ʈ�� �������� ���� ��������
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
    }
}

//é�� �������� �� ���� ������ �� �ֵ��� �������ϴ� Ŭ����
public class ChapterData
{
    public int ChapterNo;
    public int TotalStage;
    public int ChapterRewardGem;
    public int ChapterRewardGold;
}