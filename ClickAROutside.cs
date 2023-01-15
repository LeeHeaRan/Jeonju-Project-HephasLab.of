using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// AR Part
/// 생성된 AR Object를 회전하고 클릭하는 기능을 관리하는 스크립트
/// </summary>
public class ClickAROutside : MonoBehaviour
{
    public Text t1, t2, t3; //디버그형
    public Camera cam;
    public SetAROutside s_SetAR;
    public GameObject arObj;
    public GameObject arOutside, vrInside;

    public Camera camMain;
    public bool isStop;

    //Navi
    //public GameObject camPos, startPos, endPos;
    //public GameObject camLookPos1, lookPos3, lookPos4;
    public ORNavigation navi;

    public GameObject camPos, endPos;
    public GameObject camLookPos1, lookPos2;

    //Post
    public GameObject post;
    public Volume v;
    public Bloom b;
    public Vignette vg;
    public double fromV;
    public double toV;

    public float postVal;


    //길이조절
    public GameObject pivotObj;
    public GameObject pivot;


    //터치인지 드래그 인지 확인.
    Vector2 dirV, startV, moveV;


    //Drag
    public GameObject tempObj;
    public float accV; //누적값
    public Vector2 pos;

    public float minV = 1f;
    public float maxV = 30f;
    public bool isDrag = false; //드래그 상태를 확인한다.
    public bool isHit = false;
    public float velocity;

    public Vector3 initPos; //포지션을 고정하기 위한 초기값.

    //클릭 시간에 따라 들어갈지 말지 결정.
    public float timeV = 0f;

    //초기화 함을 알려주는 함수.
    public bool isInit = false;

    //클릭 오브젝트를 가져온다.
    public GameObject clickObj;

    GameObject OutsideAll;


    void Start()
    {
        v = post.GetComponent<Volume>();
        v.profile.TryGet(out b);
        v.profile.TryGet(out vg);

        camMain.gameObject.SetActive(false);
        isStop = false;
        postVal = 0f;

        velocity = 8f;
    }

    void Update()
    {

        timeV += Time.deltaTime;
        /*t2.text = s_SetAR.createAR.transform.Find("Pivot").gameObject.transform.rotation.ToString();
        t2.text = s_SetAR.createAR.transform.rotation.ToString() + "\n" + cam.gameObject.transform.rotation.ToString(); //11.12
        t2.text = camMain.transform.forward.ToString();
        t1.text = accV + isDrag.ToString() + "||" + pos.x;
        t1.text = s_SetAR.createAR.transform.position +"\n"+ cam.transform.position + "\n" + camMain.transform.position;*/
        if (!isStop)
        {
            camMain.fieldOfView = cam.fieldOfView;
            camMain.transform.position = cam.transform.position;
            camMain.transform.rotation = cam.transform.rotation;

            OutsideAll = s_SetAR.createAR.transform.Find("OutsideALL").gameObject;
            pivot=OutsideAll.transform.Find("Pivot").gameObject;

            if(clickObj != null)
            {
                 clickObj.transform.rotation = new Quaternion(cam.gameObject.transform.rotation.x, cam.gameObject.transform.rotation.y, 0f, cam.gameObject.transform.rotation.w);
            }
        }

        b.intensity.value = postVal;

        //AR오브젝트가 생성되면 tempObj에 객체를 넣어준다.
        if (s_SetAR.createAR != null)
        {
            tempObj = s_SetAR.createAR;
            initPos = tempObj.transform.position;
            clickObj = s_SetAR.createAR.transform.Find("RotClick").gameObject;

           /* t2.text = clickObj.transform.position + "\n" + clickObj.transform.rotation;
            t2.text = clickObj.transform.rotation.ToString() + "\n" + clickObj.transform.position.ToString();
            t2.text = clickObj.name;  RotClick으로 나옴.*/

        }

        tempObj.transform.position = initPos;

        if (!isDrag) //드래그가 끝난 상태. 손을 뗀 상태이다.
        {
            //서서히 멈추도록 한다.
            OutsideAll.transform.Rotate(new Vector3(0f, (-pos.x * velocity * Time.deltaTime), 0f));
        }

        RaycastHit hit;

        if (Input.touchCount > 0)
        {
            s_SetAR.RotGuideUI.SetActive(false);

            t1.text = "click";
            Touch to = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(to.position);


            if (to.phase == TouchPhase.Began) //한번실행
            {
                startV = to.position;
                timeV = 0f;
            }

            if (to.phase == TouchPhase.Moved) //움직이면 계속 실행.
            {
                moveV = to.position;
                dirV = startV - moveV; //거리벡터.
                accV += Mathf.Abs(dirV.magnitude); //값을 누적한다. 절대값으로.
            }

            if (to.phase == TouchPhase.Ended) //한번실행
            {
                if (accV < 100f && timeV < 1f) //클릭일 때.   클릭하는 시간이 1초 미만일때만 클릭으로 간주한다.
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        s_SetAR.reLoadUI.SetActive(false); //클릭이 발생하면 초기화 버튼을 사용할 수 없다.

                        if (s_SetAR.isCreateAR && hit.collider.gameObject.tag == "hitObject") //AR오브젝트가 생성되고. hit된 오브젝트가 hitObject일때. 
                        {
                            t1.text = "hit Object!";
                            isHit = true;//드래그가 끝남. hit이 되었기에 네비가 실행 되는동안 회전하지 못하게 한다.
                            arObj = hit.collider.gameObject;

                            
                            arObj.gameObject.layer = 5;
                            foreach (Transform child in arObj.GetComponentsInChildren<Transform>())
                            {
                                child.gameObject.layer = 5;
                            }

                            isStop = true;
                            camMain.gameObject.SetActive(true);

                            float rotTime = (Mathf.Abs(arObj.transform.rotation.y) * 5f);
                            iTween.ValueTo(transform.gameObject, iTween.Hash("from", OutsideAll.transform.rotation.y, "to", -camMain.gameObject.transform.rotation.y, "onupdate", "RotObject", "oncomplete", "endRot", "time", rotTime)); //회전할 값들을 관리.
                        }
                    }
                }
                else 
                {
                    accV = 0f;//초기화 해주어야 스르륵 멈추는게 작동한다.
                    isDrag = false;
                    float timeV = (Mathf.Abs(dirV.x) * 0.001f) * 5;
                    iTween.ValueTo(transform.gameObject, iTween.Hash("from", pos.x, "to", 0f, "onupdate", "DrageStop", "time", timeV)); //값을 떨어트린다. 거리에 다라 회전을 서서히 멈춘다.
                }
            }

            if (accV > 200f) //드래그 일때. 누적된 값이 임계값을 넘으면 드래그가 시작된다.
            {
                isDrag = true; //드래그 중이다.

                pos = Input.GetTouch(0).deltaPosition;
                if (Mathf.Abs(pos.x) < minV)
                {
                    pos.x = .0f;
                }

                if (Mathf.Abs(pos.x) > maxV)
                {
                    pos.x = (pos.x < 0) ? -maxV : maxV;
                }

                //만약 ARObject가 hit되어 있다면 회전하지 않게 한다. 실행하지 않는다.
                if (!isHit)
                {
                    OutsideAll.transform.Rotate(new Vector3(0f, (-pos.x * velocity * Time.deltaTime), 0f)); //손이 닿은 채로 회전

                }
            }
        }
    }

    public void RotObject(float rotY) //들어오는 회전.
    {
        OutsideAll.transform.rotation = new Quaternion(OutsideAll.transform.rotation.x, rotY, OutsideAll.transform.rotation.x, OutsideAll.transform.rotation.w);
    }

    public void endRot()
    {
        camPos.transform.position = camMain.gameObject.transform.position; //카메라의 위치를 pos01에 넣는다.
        camPos.transform.rotation = camMain.gameObject.transform.rotation;
        camLookPos1.transform.position = camMain.transform.Find("CamLookPoint1").transform.position;
        camLookPos1.transform.rotation = camMain.gameObject.transform.rotation;
        pivotObj = OutsideAll.transform.Find("Pivot").gameObject;
        endPos.transform.position = pivotObj.transform.Find("EndPoint").gameObject.transform.position;
        lookPos2.transform.position = pivotObj.transform.Find("LookPoint4").gameObject.transform.position;
        navi.Play();
    }
    public void InDoor()
    {
        
            iTween.MoveAdd(camMain.gameObject, iTween.Hash("z", 0.17f, "time", 3f, "easetype", iTween.EaseType.easeOutCirc));
            fromV = 0f;
            toV = 100f;
            iTween.ValueTo(transform.gameObject, iTween.Hash("from", fromV, "to", toV, "onupdate", "PostUP", "oncomplete", "GoInside", "time", 1.5f)); //밝아지는게 끝나면 GoInside함수를 호출한다.
    }

    public void PostUP(float upPost)
    {
        
             postVal = upPost;
        
    }

    public void DrageStop(float val)
    {
        pos.x = val;
    }
    public void GoInside()
    {
            s_SetAR.InitAR1(); //실행된다. outside를 초기화한다.
            InitAR2();
            arOutside.SetActive(false); //밖이 꺼짐.
            vrInside.SetActive(true); //안이 켜짐.
    }
    public void InitAR2() //초기화 한다.
    {
        iTween.Stop();
        Destroy(arObj);

        postVal = 0f;
        b.intensity.value = 0f;

        camMain.gameObject.SetActive(false);
        isStop = false;

        initPos = Vector3.zero;

        isHit = false;
        isDrag = false;
        timeV = 0f;
    }

}

