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
        LogoAnim.gameObject.SetActive(true);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
