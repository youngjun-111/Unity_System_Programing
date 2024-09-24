using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//만들 씬 의 타입을 이넘으로 선언 지금은 3개
public enum SceneType
{
    Title,
    Lobby,
    InGame,
}

//SingletonBehaviour를 상속 받아서 사
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void LoadScene(SceneType sceneType)
    {
        //현재 씬을 로거로 표시
        Logger.Log($"{sceneType} 씬 로딩 중...");
        //만약 일시정지가 있으면 로딩 됐을 때 타임 스케일을 1로 초기화 시켜주고 모든 씬을 로딩
        Time.timeScale = 1f;
        //게임 기획상 타임 스케일이 1이 아닌 경우도 있을 수 있기 때문에
        //씬을 로딩했을 때 타임 스케일을 초기화 해줌.
        SceneManager.LoadScene(sceneType.ToString());
    }

    //해당 씬을 다시 로딩 해줬을 때 실행 되는 함수
    public void ReloadScene()
    {
        //현재 켜져있는 씬의 이름이 로딩 된다는걸 로거로 표시
        Logger.Log($"{SceneManager.GetActiveScene().name} 씬 로딩중...");
        //다시 타임 스케일을 1로 초기화
        Time.timeScale = 1f;
        //현재 켜져있는 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}