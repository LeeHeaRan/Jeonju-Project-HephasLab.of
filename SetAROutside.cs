using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARPlaneManager))]

/// <summary>
/// AR Part
/// ARPlane을 인식하고 클릭한 곳에 AR Object를 생성하는 스크립트.
/// ARPlane과 ARObject를 관리하는 스크립트.
/// </summary>
public class SetAROutside : MonoBehaviour
{
    //hit이 발생한 곳에 ARObject를 생성한다.
    public ARRaycastManager m_RaycastManager;
    public List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private int count; //ARObject 생성 갯수를 저장. 
    public bool isCreateAR; //ARObject가 생성되었는지 확인하는 변수.
    public GameObject createAR; //생성된 ARObject를 저장.

    //AR Plane.
    private ARPlaneManager planeManager;
    private bool isPlaneLook;

    //Debug Text.
    public Text t1, t2;

    //Init.
    public ARSession session;

    //Animation
    public Animator startAni;

    //Guide UI
    ARPlane arP;
    public GameObject arPG;
    public GameObject arGuideUI;

    //Refresg UI
    public GameObject reLoadUI;

    //Guide UI
    public GameObject RotGuideUI, RecoGuideUI; 

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }
    private void Start()
    {
        //초기화한다.
        count = 0;
        isCreateAR = false;
        isPlaneLook = true;
        arGuideUI.SetActive(false);
        reLoadUI.SetActive(false);
        RotGuideUI.SetActive(false);
        RecoGuideUI.SetActive(false);
     //   ARSession.stateChanged += ARSession_stateChanged;
    }

    void Update()
    {
        //터치가 발생하면 hit이 된 자리에 AR오브젝트가 생성된다.
        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (m_RaycastManager.Raycast(touchPos, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;

                if (count <= 0) //하나의 오브젝트만 생성하기 위한 카운트
                {
                    createAR = Instantiate(m_RaycastManager.raycastPrefab, hitPose.position, new Quaternion(hitPose.rotation.x, hitPose.rotation.y - 0.3f, hitPose.rotation.z, hitPose.rotation.w)); //오브젝트 생성.
                    //RotGuideUI.SetActive(true);
                    //RecoGuideUI.SetActive(false);
                    isPlaneLook = false; //SetAllPlaneActive에 들어갈 매개변수.
                    Invoke("CreateAR", 1f); //애니가 실행되는 동안은 isCreate 가 false여야한다.
                    SetAllPlaneActive(isPlaneLook); //AR Plane을 보이지 않게 하는 함수.

                }
                count++;
                startAni.Play("Base Layer"); //떨어지는 애니메니션 실행.
                //t1.text = count.ToString();
            }
        }

        var Pcount = planeManager.trackables.count; //바닥인식으로 평면일 생성되었는 확인하기 위한 변수.

        if(Pcount > 0 && createAR == null)
        {
            arGuideUI.SetActive(true); //바닥이 인식 되었음.
            RecoGuideUI.SetActive(false);
        }
        else
        {
            arGuideUI.SetActive(false); //바닥인식이 안되었음.
            if(createAR == null) //ar객체가 안 생겼을때만.
            {
                RecoGuideUI.SetActive(true);
            }
        }
    }

    //ARObject가 생성되었는지 체크하는 함수. clickAROutside.cs에서 사용한다.
    public void CreateAR()
    {
        isCreateAR = true;
        //arGuideUI.SetActive(false);
        reLoadUI.SetActive(true);
        RotGuideUI.SetActive(true);
        planeManager.requestedDetectionMode = PlaneDetectionMode.None;
       // SetAllPlaneActive(isPlaneLook); //AR Plane을 보이지 않게 하는 함수.
    }

        //AR모드의 모든 변수와 Object를 초기화하는 함수. 버튼에 들어간다.
        public void InitAR1() //초기화한다.
    {
        session.Reset();
        Destroy(createAR);
        count = 0;
        isCreateAR = false;
        //isPlaneLook = true;
        arGuideUI.SetActive(false);
        reLoadUI.SetActive(false);

        RotGuideUI.SetActive(false);
        RecoGuideUI.SetActive(false);

        planeManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
    }

    //ARPlane을 안보이도록 하는 함수.
    public void SetAllPlaneActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
        {
            arP = plane; //인식되었는지 확인하기 위한 변수.
            arPG = plane.gameObject;
            plane.gameObject.SetActive(value);
        }
    }

}
   

