using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AR Part�� 3D Part ��ȯ�� �����ϴ� ��ũ��Ʈ/
/// </summary>
public class SideChangeManage : MonoBehaviour
{
    public GameObject outside_AR_PostVal;
    public GameObject sideChangeLoading;
    private float postVal;

    void Start()
    {
        postVal = outside_AR_PostVal.GetComponent<ClickAROutside>().postVal;//�ʱⰪ ����
    }

    /// <summary>
    /// �𵨸� �ܺο��� postVal�� �Ӱ谪�� ������ ���η� ����ȯ�� �Ѵ�.
    /// </summary>
    void Update()
    {
        postVal = outside_AR_PostVal.GetComponent<ClickAROutside>().postVal;

        //�Ӱ谪�� ������ ��ȯ�Ѵ�.
        if (postVal >= 90) 
        {
            sideChangeLoading.SetActive(true);
        }
    }
}
