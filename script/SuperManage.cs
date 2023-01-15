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
/// AR Part �� 3D Part�� ��ȯ���� ����Ǵ� ������ �����ϴ� ��ũ��Ʈ.
/// </summary>
public class SuperManage : MonoBehaviour
{
    //AR Part�� �ֻ��� ��ü�� �޾ƿ´�.
    public GameObject outside_Super;

    //3D Part�� �ֻ��� ��ü�� �޾ƿ´�.
    public GameObject inside_Super;
    
    public Material insideSkyBox; //���� skybox�� ����
    public Product_UI_Manage s_product_ui_manager; //3D Part�� ��ǰUI�� ����
    public UniversalRenderPipelineAsset ass; //������������ ������ ����
   
    //�ʱ�ȭ
    void Start()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        outside_Super.SetActive(true);  //AR Part ��ü�� Ȱ��ȭ �Ѵ�.
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
    /// AR Part�� 3D Part�� �ֻ��� ��ü�� Ȱ��ȭ �Ǿ��ִ��� üũ�Ͽ� ������ �����Ѵ�.
    /// </summary>
    public void ChangeProejct()
    {
        //AR Part�� Ȱ��ȭ �Ǿ����� ���
        if (outside_Super.activeSelf)
        {
            if (Screen.orientation.Equals(ScreenOrientation.AutoRotation))
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }

        //3D Part�� Ȱ��ȭ �Ǿ����� ���
        if (inside_Super.activeSelf)
        {
            RenderSettings.skybox = insideSkyBox; //skybox material�� ����
            
            ass.shadowDistance = 60.0f; //shadowDistance�� ����

            if (!Screen.orientation.Equals(ScreenOrientation.AutoRotation) && !s_product_ui_manager.product_UI_Detail.activeSelf)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;
            }
        }
    }

    /// <summary>
    /// AR Part�� ���� Back��ư�� ��������.
    /// </summary>
    public void BackOutside() 
    {
        outside_Super.SetActive(true); 
        inside_Super.SetActive(false);
    }
}
