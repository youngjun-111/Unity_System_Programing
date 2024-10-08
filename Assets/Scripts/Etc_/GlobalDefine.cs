using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalDefine
{
    //현재까지 게임 내 존재하는 최대 챕터라는 뜻으로 스테틱 클래스 생성
    public const int MAX_CHAPTER = 4;

    //업적 보상 타입
    public enum RewardType
    {
        Gold,
        Gem,
    }
}
