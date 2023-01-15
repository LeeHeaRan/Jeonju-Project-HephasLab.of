using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// AR Part 와 3D Part의 전환에서 변경되는 설정을 관리하는 스크립트.
/// </summary>
public class SuperManage : MonoBehaviour
{
    //AR Part의 최상위 객체를 받아온다.
    public GameObject outside_Super;

    //3D Part의 최상위 객체를 받아온다.
    public GameObject inside_Super;
    
    public Material insideSkyBox; //씬의 skybox를 변경
    public Product_UI_Manage s_product_ui_manager; //3D Part의 상품UI를 관리
    public UniversalRenderPipelineAsset ass; //렌터파이프의 설정을 변경
   
    //초기화
    void Start()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        outside_Super.SetActive(true);  //AR Part 객체를 활성화 한다.
        inside_Super.SetActive(false);

        ass.shadowDistance = 2.0f;
    }

    void Update()
    {
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        ChangeProejct();

      
    }

    /// <summary>
    /// update
    /// AR Part와 3D Part의 최상위 객체가 활성화 되어있는지 체크하여 설정을 변경한다.
    /// </summary>
    public void ChangeProejct()
    {
        //AR Part가 활성화 되어있을 경우
        if (outside_Super.activeSelf)
        {
            if (Screen.orientation.Equals(ScreenOrientation.AutoRotation))
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }

        //3D Part가 활성화 되어있을 경우
        if (inside_Super.activeSelf)
        {
            RenderSettings.skybox = insideSkyBox; //skybox material을 변경
            
            ass.shadowDistance = 60.0f; //shadowDistance를 변경

            if (!Screen.orientation.Equals(ScreenOrientation.AutoRotation) && !s_product_ui_manager.product_UI_Detail.activeSelf)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;
            }
        }
    }

    /// <summary>
    /// AR Part로 가는 Back버튼을 눌렀을때.
    /// </summary>
    public void BackOutside() 
    {
        outside_Super.SetActive(true); 
        inside_Super.SetActive(false);
    }
}
