using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    //속도
    public float MoveSpeed = 5f;
    //이동해야할 위치
    Vector3 m_DestPosition;
    //이 오브젝트의 트랜스폼을 담을 변수
    Transform m_Transform;
    //렉트트랜스폼을 담을 변수
    RectTransform m_RectTransform;
    public void SetMove(int idx, Vector3 destPositon)
    {
        m_Transform = transform;
        m_RectTransform = GetComponent<RectTransform>();
        m_DestPosition = new Vector3(destPositon.x, destPositon.y);

        StartCoroutine(MoveCo(idx));
    }

    IEnumerator MoveCo(int idx)
    {
        //이동하기 전에 대기할 시간을 인덱스 값에 따라 계산
        //이동할 재화 인스턴스 들이 동시에 이동하지 않고 일정 간격을 가지고 차례대로 이동하게
        yield return new WaitForSeconds(0.1f +0.08f * idx);
        //이 오브젝트가 목적 위치까지 갔는지 매 프레임 확인
        //목적 위치에 도달하지 않았다면 매 프레임 이동시켜 줌
        while(m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, MoveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            //z값은 0 으로 보정
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f);

            yield return null;
        }
        //목표 위치에 도달하면 삭제
        Destroy(gameObject);
    }
}
