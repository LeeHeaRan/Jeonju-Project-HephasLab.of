using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AR Part와 3D Part 전환을 관리하는 스크립트/
/// </summary>
public class SideChangeManage : MonoBehaviour
{
    public GameObject outside_AR_PostVal;
    public GameObject sideChangeLoading;
    private float postVal;

    void Start()
    {
        postVal = outside_AR_PostVal.GetComponent<ClickAROutside>().postVal;//초기값 전달
    }

    /// <summary>
    /// 모델링 외부에서 postVal가 임계값을 넘으면 내부로 씬전환을 한다.
    /// </summary>
    void Update()
    {
        postVal = outside_AR_PostVal.GetComponent<ClickAROutside>().postVal;

        //임계값이 넘으면 전환한다.
        if (postVal >= 90) 
        {
            sideChangeLoading.SetActive(true);
        }
    }
}
