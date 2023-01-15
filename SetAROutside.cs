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
/// ARPlane�� �ν��ϰ� Ŭ���� ���� AR Object�� �����ϴ� ��ũ��Ʈ.
/// ARPlane�� ARObject�� �����ϴ� ��ũ��Ʈ.
/// </summary>
public class SetAROutside : MonoBehaviour
{
    //hit�� �߻��� ���� ARObject�� �����Ѵ�.
    public ARRaycastManager m_RaycastManager;
    public List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private int count; //ARObject ���� ������ ����. 
    public bool isCreateAR; //ARObject�� �����Ǿ����� Ȯ���ϴ� ����.
    public GameObject createAR; //������ ARObject�� ����.

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
        //�ʱ�ȭ�Ѵ�.
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
        //��ġ�� �߻��ϸ� hit�� �� �ڸ��� AR������Ʈ�� �����ȴ�.
        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (m_RaycastManager.Raycast(touchPos, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;

                if (count <= 0) //�ϳ��� ������Ʈ�� �����ϱ� ���� ī��Ʈ
                {
                    createAR = Instantiate(m_RaycastManager.raycastPrefab, hitPose.position, new Quaternion(hitPose.rotation.x, hitPose.rotation.y - 0.3f, hitPose.rotation.z, hitPose.rotation.w)); //������Ʈ ����.
                    //RotGuideUI.SetActive(true);
                    //RecoGuideUI.SetActive(false);
                    isPlaneLook = false; //SetAllPlaneActive�� �� �Ű�����.
                    Invoke("CreateAR", 1f); //�ִϰ� ����Ǵ� ������ isCreate �� false�����Ѵ�.
                    SetAllPlaneActive(isPlaneLook); //AR Plane�� ������ �ʰ� �ϴ� �Լ�.

                }
                count++;
                startAni.Play("Base Layer"); //�������� �ִϸ޴ϼ� ����.
                //t1.text = count.ToString();
            }
        }

        var Pcount = planeManager.trackables.count; //�ٴ��ν����� ����� �����Ǿ��� Ȯ���ϱ� ���� ����.

        if(Pcount > 0 && createAR == null)
        {
            arGuideUI.SetActive(true); //�ٴ��� �ν� �Ǿ���.
            RecoGuideUI.SetActive(false);
        }
        else
        {
            arGuideUI.SetActive(false); //�ٴ��ν��� �ȵǾ���.
            if(createAR == null) //ar��ü�� �� ����������.
            {
                RecoGuideUI.SetActive(true);
            }
        }
    }

    //ARObject�� �����Ǿ����� üũ�ϴ� �Լ�. clickAROutside.cs���� ����Ѵ�.
    public void CreateAR()
    {
        isCreateAR = true;
        //arGuideUI.SetActive(false);
        reLoadUI.SetActive(true);
        RotGuideUI.SetActive(true);
        planeManager.requestedDetectionMode = PlaneDetectionMode.None;
       // SetAllPlaneActive(isPlaneLook); //AR Plane�� ������ �ʰ� �ϴ� �Լ�.
    }

        //AR����� ��� ������ Object�� �ʱ�ȭ�ϴ� �Լ�. ��ư�� ����.
        public void InitAR1() //�ʱ�ȭ�Ѵ�.
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

    //ARPlane�� �Ⱥ��̵��� �ϴ� �Լ�.
    public void SetAllPlaneActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
        {
            arP = plane; //�νĵǾ����� Ȯ���ϱ� ���� ����.
            arPG = plane.gameObject;
            plane.gameObject.SetActive(value);
        }
    }

}
   

