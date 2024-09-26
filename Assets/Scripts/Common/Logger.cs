using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//1. �߰����� ���� ǥ��(ex. Ÿ�� ������)
//2. ��ÿ� ���带 ���� �α� ����
public static class Logger
{
    //��Ƽ�ų� �Ӽ��� ��� : ���Ǻ� ������ �ɺ�
    [Conditional("DEV_VER")]
    public static void Log(string msg)
    {
        //���� �ð��� ��¥�� �ð��� �������� ǥ���� {0} �ְ�, �α� �Ϸ��� �޽����� {1}�� �ִ´�.
        UnityEngine.Debug.LogFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }

    [Conditional("DEV_VER")]
    //���� �α� �Լ�
    public static void LogWarnimg(string msg)
    {
        //���� �ð��� ��¥�� �ð��� �������� ǥ���� {0} �ְ�, �α� �Ϸ��� �޽����� {1}�� �ִ´�.
        UnityEngine.Debug.LogWarningFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }

    //���� �α� �Լ�
    public static void LogError(string msg)
    {
        //���� �ð��� ��¥�� �ð��� �������� ǥ���� {0} �ְ�, �α� �Ϸ��� �޽����� {1}�� �ִ´�.
        UnityEngine.Debug.LogErrorFormat("[{0}] [{1}]", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"), msg);
    }
}
