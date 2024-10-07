using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    //�ӵ�
    public float MoveSpeed = 5f;
    //�̵��ؾ��� ��ġ
    Vector3 m_DestPosition;
    //�� ������Ʈ�� Ʈ�������� ���� ����
    Transform m_Transform;
    //��ƮƮ�������� ���� ����
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
        //�̵��ϱ� ���� ����� �ð��� �ε��� ���� ���� ���
        //�̵��� ��ȭ �ν��Ͻ� ���� ���ÿ� �̵����� �ʰ� ���� ������ ������ ���ʴ�� �̵��ϰ�
        yield return new WaitForSeconds(0.1f +0.08f * idx);
        //�� ������Ʈ�� ���� ��ġ���� ������ �� ������ Ȯ��
        //���� ��ġ�� �������� �ʾҴٸ� �� ������ �̵����� ��
        while(m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, MoveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            //z���� 0 ���� ����
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f);

            yield return null;
        }
        //��ǥ ��ġ�� �����ϸ� ����
        Destroy(gameObject);
    }
}
