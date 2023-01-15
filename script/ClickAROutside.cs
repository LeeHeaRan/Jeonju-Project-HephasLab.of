using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// AR Part
/// ������ AR Object�� ȸ���ϰ� Ŭ���ϴ� ����� �����ϴ� ��ũ��Ʈ
/// </summary>
public class ClickAROutside : MonoBehaviour
{
    public Text t1, t2, t3; //�������
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


    //��������
    public GameObject pivotObj;
    public GameObject pivot;


    //��ġ���� �巡�� ���� Ȯ��.
    Vector2 dirV, startV, moveV;


    //Drag
    public GameObject tempObj;
    public float accV; //������
    public Vector2 pos;

    public float minV = 1f;
    public float maxV = 30f;
    public bool isDrag = false; //�巡�� ���¸� Ȯ���Ѵ�.
    public bool isHit = false;
    public float velocity;

    public Vector3 initPos; //�������� �����ϱ� ���� �ʱⰪ.

    //Ŭ�� �ð��� ���� ���� ���� ����.
    public float timeV = 0f;

    //�ʱ�ȭ ���� �˷��ִ� �Լ�.
    public bool isInit = false;

    //Ŭ�� ������Ʈ�� �����´�.
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

        //AR������Ʈ�� �����Ǹ� tempObj�� ��ü�� �־��ش�.
        if (s_SetAR.createAR != null)
        {
            tempObj = s_SetAR.createAR;
            initPos = tempObj.transform.position;
            clickObj = s_SetAR.createAR.transform.Find("RotClick").gameObject;

           /* t2.text = clickObj.transform.position + "\n" + clickObj.transform.rotation;
            t2.text = clickObj.transform.rotation.ToString() + "\n" + clickObj.transform.position.ToString();
            t2.text = clickObj.name;  RotClick���� ����.*/

        }

        tempObj.transform.position = initPos;

        if (!isDrag) //�巡�װ� ���� ����. ���� �� �����̴�.
        {
            //������ ���ߵ��� �Ѵ�.
            OutsideAll.transform.Rotate(new Vector3(0f, (-pos.x * velocity * Time.deltaTime), 0f));
        }

        RaycastHit hit;

        if (Input.touchCount > 0)
        {
            s_SetAR.RotGuideUI.SetActive(false);

            t1.text = "click";
            Touch to = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(to.position);


            if (to.phase == TouchPhase.Began) //�ѹ�����
            {
                startV = to.position;
                timeV = 0f;
            }

            if (to.phase == TouchPhase.Moved) //�����̸� ��� ����.
            {
                moveV = to.position;
                dirV = startV - moveV; //�Ÿ�����.
                accV += Mathf.Abs(dirV.magnitude); //���� �����Ѵ�. ���밪����.
            }

            if (to.phase == TouchPhase.Ended) //�ѹ�����
            {
                if (accV < 100f && timeV < 1f) //Ŭ���� ��.   Ŭ���ϴ� �ð��� 1�� �̸��϶��� Ŭ������ �����Ѵ�.
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        s_SetAR.reLoadUI.SetActive(false); //Ŭ���� �߻��ϸ� �ʱ�ȭ ��ư�� ����� �� ����.

                        if (s_SetAR.isCreateAR && hit.collider.gameObject.tag == "hitObject") //AR������Ʈ�� �����ǰ�. hit�� ������Ʈ�� hitObject�϶�. 
                        {
                            t1.text = "hit Object!";
                            isHit = true;//�巡�װ� ����. hit�� �Ǿ��⿡ �׺� ���� �Ǵµ��� ȸ������ ���ϰ� �Ѵ�.
                            arObj = hit.collider.gameObject;

                            
                            arObj.gameObject.layer = 5;
                            foreach (Transform child in arObj.GetComponentsInChildren<Transform>())
                            {
                                child.gameObject.layer = 5;
                            }

                            isStop = true;
                            camMain.gameObject.SetActive(true);

                            float rotTime = (Mathf.Abs(arObj.transform.rotation.y) * 5f);
                            iTween.ValueTo(transform.gameObject, iTween.Hash("from", OutsideAll.transform.rotation.y, "to", -camMain.gameObject.transform.rotation.y, "onupdate", "RotObject", "oncomplete", "endRot", "time", rotTime)); //ȸ���� ������ ����.
                        }
                    }
                }
                else 
                {
                    accV = 0f;//�ʱ�ȭ ���־�� ������ ���ߴ°� �۵��Ѵ�.
                    isDrag = false;
                    float timeV = (Mathf.Abs(dirV.x) * 0.001f) * 5;
                    iTween.ValueTo(transform.gameObject, iTween.Hash("from", pos.x, "to", 0f, "onupdate", "DrageStop", "time", timeV)); //���� ����Ʈ����. �Ÿ��� �ٶ� ȸ���� ������ �����.
                }
            }

            if (accV > 200f) //�巡�� �϶�. ������ ���� �Ӱ谪�� ������ �巡�װ� ���۵ȴ�.
            {
                isDrag = true; //�巡�� ���̴�.

                pos = Input.GetTouch(0).deltaPosition;
                if (Mathf.Abs(pos.x) < minV)
                {
                    pos.x = .0f;
                }

                if (Mathf.Abs(pos.x) > maxV)
                {
                    pos.x = (pos.x < 0) ? -maxV : maxV;
                }

                //���� ARObject�� hit�Ǿ� �ִٸ� ȸ������ �ʰ� �Ѵ�. �������� �ʴ´�.
                if (!isHit)
                {
                    OutsideAll.transform.Rotate(new Vector3(0f, (-pos.x * velocity * Time.deltaTime), 0f)); //���� ���� ä�� ȸ��

                }
            }
        }
    }

    public void RotObject(float rotY) //������ ȸ��.
    {
        OutsideAll.transform.rotation = new Quaternion(OutsideAll.transform.rotation.x, rotY, OutsideAll.transform.rotation.x, OutsideAll.transform.rotation.w);
    }

    public void endRot()
    {
        camPos.transform.position = camMain.gameObject.transform.position; //ī�޶��� ��ġ�� pos01�� �ִ´�.
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
            iTween.ValueTo(transform.gameObject, iTween.Hash("from", fromV, "to", toV, "onupdate", "PostUP", "oncomplete", "GoInside", "time", 1.5f)); //������°� ������ GoInside�Լ��� ȣ���Ѵ�.
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
            s_SetAR.InitAR1(); //����ȴ�. outside�� �ʱ�ȭ�Ѵ�.
            InitAR2();
            arOutside.SetActive(false); //���� ����.
            vrInside.SetActive(true); //���� ����.
    }
    public void InitAR2() //�ʱ�ȭ �Ѵ�.
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

