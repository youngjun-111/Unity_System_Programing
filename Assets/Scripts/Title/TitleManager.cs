using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    //로고
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    //타이틀
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgressTxt;

    AsyncOperation m_AsyncOperation;
    private void Awake()
    {
        //처음 시작할때 로고 애니메이션을 재생시켜주기위해 로고애니메이션을 활성화 시켜줌
        LogoAnim.gameObject.SetActive(true);
        //로고애니메이션 재생이 끝난 후 타이틀을 활성화 해줘야 하기 때문에 타이틀 씬이 시작되었을 땐 타이틀은 일단 꺼놔줌
        Title.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(LoadGameCo());
    }

    IEnumerator LoadGameCo()
    {
        //이 코루틴 함수는 게임의 로딩을 처음 시작하는 중요한 함수이기 때문에
        //로그를 찍음.
        //GetType() : 클래스 명을 출력
        //타이틀 매니저에서 호출하는 로드게임 코루틴이라는 함수 확인
        Logger.Log($"{GetType()}::LoadGameCo");
        //로로 애니메이션 재생시켜주면서
        LogoAnim.Play();
        //애니메이션클립의 길만큼 대기 후에
        yield return new WaitForSeconds(LogoAnim.clip.length);
        //애니메이션 꺼주고
        LogoAnim.gameObject.SetActive(false);
        //타이틀 화면 꺼줬던걸 애니메이션 재생이 끝난 후 켜주기
        Title.SetActive(true);
    }
}
