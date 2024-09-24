using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    //�ΰ�
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    //Ÿ��Ʋ
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgressTxt;

    AsyncOperation m_AsyncOperation;
    private void Awake()
    {
        //ó�� �����Ҷ� �ΰ� �ִϸ��̼��� ��������ֱ����� �ΰ�ִϸ��̼��� Ȱ��ȭ ������
        LogoAnim.gameObject.SetActive(true);
        //�ΰ�ִϸ��̼� ����� ���� �� Ÿ��Ʋ�� Ȱ��ȭ ����� �ϱ� ������ Ÿ��Ʋ ���� ���۵Ǿ��� �� Ÿ��Ʋ�� �ϴ� ������
        Title.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(LoadGameCo());
    }

    IEnumerator LoadGameCo()
    {
        //�� �ڷ�ƾ �Լ��� ������ �ε��� ó�� �����ϴ� �߿��� �Լ��̱� ������
        //�α׸� ����.
        //GetType() : Ŭ���� ���� ���
        //Ÿ��Ʋ �Ŵ������� ȣ���ϴ� �ε���� �ڷ�ƾ�̶�� �Լ� Ȯ��
        Logger.Log($"{GetType()}::LoadGameCo");
        //�η� �ִϸ��̼� ��������ָ鼭
        LogoAnim.Play();
        //�ִϸ��̼�Ŭ���� �游ŭ ��� �Ŀ�
        yield return new WaitForSeconds(LogoAnim.clip.length);
        //�ִϸ��̼� ���ְ�
        LogoAnim.gameObject.SetActive(false);
        //Ÿ��Ʋ ȭ�� ������� �ִϸ��̼� ����� ���� �� ���ֱ�
        Title.SetActive(true);
    }
}
