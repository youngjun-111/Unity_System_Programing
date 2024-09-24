using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//���� �� �� Ÿ���� �̳����� ���� ������ 3��
public enum SceneType
{
    Title,
    Lobby,
    InGame,
}

//SingletonBehaviour�� ��� �޾Ƽ� ��
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void LoadScene(SceneType sceneType)
    {
        //���� ���� �ΰŷ� ǥ��
        Logger.Log($"{sceneType} Scene Loading....");
        //���� �Ͻ������� ������ �ε� ���� �� Ÿ�� �������� 1�� �ʱ�ȭ �����ְ� ��� ���� �ε�
        Time.timeScale = 1f;
        //���� ��ȹ�� Ÿ�� �������� 1�� �ƴ� ��쵵 ���� �� �ֱ� ������
        //���� �ε����� �� Ÿ�� �������� �ʱ�ȭ ����.
        SceneManager.LoadScene(sceneType.ToString());
    }

    //�ش� ���� �ٽ� �ε� ������ �� ���� �Ǵ� �Լ�
    public void ReloadScene()
    {
        //���� �����ִ� ���� �̸��� �ε� �ȴٴ°� �ΰŷ� ǥ��
        Logger.Log($"{SceneManager.GetActiveScene().name} Scene Loading....");
        //�ٽ� Ÿ�� �������� 1�� �ʱ�ȭ
        Time.timeScale = 1f;
        //���� �����ִ� ���� �ٽ� �ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�񵿱�� �ε��ϴ� �Լ� �߰�
    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        //���� �񵿱� ���� �ε� ���̶�� �α׶����
        Logger.Log($"{sceneType} Scene async Loading...");
        //�񵿱� �ε��� �ɶ����� ���� �ð��� �ʱ�ȭ
        Time.timeScale = 1f;
        //�񵿱� �ε����� ��ȯ ����
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}