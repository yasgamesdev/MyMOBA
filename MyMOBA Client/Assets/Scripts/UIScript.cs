using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    PlayerControl player = null;
    int groundLayerMask;
    int enemyLayerMask;
    GameObject recall;
    Slider recallSlider;
    Slider qSlider, wSlider;
    const float qCD = 4.0f;
    const float wCD = 10.0f;
    float qTimer, wTimer;
    bool qActive = false;
    bool wActive = false;
    GameObject arrow, circle;
    //minimap
    Rect minimap_rect;
    CameraScript camera;

    void Start () {
        groundLayerMask = LayerMask.GetMask("Ground");
        recall = GameObject.Find("Recall");
        recallSlider = recall.transform.GetComponentInChildren<Slider>();
        recall.SetActive(false);
        qSlider = GameObject.Find("QSkill").transform.GetComponentInChildren<Slider>();
        wSlider = GameObject.Find("WSkill").transform.GetComponentInChildren<Slider>();
        arrow = GameObject.Find("Arrow");
        arrow.SetActive(false);
        circle = GameObject.Find("Circle");
        circle.SetActive(false);

        RectTransform temp = (RectTransform)transform.FindChild("MiniMap");
        minimap_rect = new Rect(Screen.width - temp.rect.width + temp.anchoredPosition.x, temp.anchoredPosition.y, temp.rect.width, temp.rect.height);
        camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }

    public void SetPlayer(PlayerControl player)
    {
        this.player = player;
        enemyLayerMask = player.GetEnemyLayerMask();
    }

    public void UnSetPlayer()
    {
        this.player = null;
        SetQSkillActive(false);
        SetWSkillActive(false);
    }

    void Update () {
        qTimer -= Time.deltaTime;
        wTimer -= Time.deltaTime;
        if(qTimer < 0)
        {
            qTimer = 0;
        }
        if(wTimer < 0)
        {
            wTimer = 0;
        }
        qSlider.value = (qCD - qTimer) / qCD;
        wSlider.value = (wCD - wTimer) / wCD;

	    if(player != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftMouseButtonDown();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RightMouseButtonDown();
            }
            if(Input.GetKeyDown(KeyCode.B))
            {
                player.Recall();
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                if (!qActive && qTimer == 0 && player.GetMP() >= 10)
                {
                    SetQSkillActive(true);
                }
                else
                {
                    SetQSkillActive(false);
                }
                SetWSkillActive(false);
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                if (!wActive && wTimer == 0 && player.GetMP() >= 20)
                {
                    SetWSkillActive(true);
                }
                else
                {
                    SetWSkillActive(false);
                }
                SetQSkillActive(false);
            }
            SetGroundTexture();
        }

        if (Input.GetMouseButton(0))
        {
            if (minimap_rect.Contains(Input.mousePosition))
            {
                camera.SetCameraPos(GetRelativeMiniMapPos(Input.mousePosition));
            }
        }
	}

    Vector2 GetRelativeMiniMapPos(Vector2 pos)
    {
        return new Vector2(
            (pos.x - minimap_rect.x - (minimap_rect.width / 2.0f)) / (minimap_rect.width / (100.0f)),
            (pos.y - minimap_rect.y - (minimap_rect.height / 2.0f)) / (minimap_rect.height / (100.0f)));
    }

    void RightMouseButtonDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask))
        {
            player.Attack(hit.collider.transform);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            player.Move(new Vector2(hit.point.x, hit.point.z));
        }
    }

    public void SetRecallActive(bool value)
    {
        recall.SetActive(value);
    }

    public void SetRecallSlider(float time)
    {
        recallSlider.value = 5.0f - time;
    }

    void SetQSkillActive(bool value)
    {
        arrow.SetActive(value);
        qActive = value;
    }

    void SetWSkillActive(bool value)
    {
        circle.SetActive(value);
        wActive = value;
    }

    void SetGroundTexture()
    {
        if (qActive)
        {
            arrow.transform.position = player.transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                arrow.transform.LookAt(new Vector3(hit.point.x, arrow.transform.position.y, hit.point.z));
                arrow.transform.rotation *= Quaternion.Euler(90, 0, 0);
            }
        }
        else if (wActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                Vector3 groundPos = new Vector3(hit.point.x, 0, hit.point.z);
                Vector3 v = groundPos - player.transform.position;
                if (v.magnitude > 6)
                {
                    v.Normalize();
                    v *= 6.0f;
                    groundPos = player.transform.position + v;
                }
                groundPos.y = 0.01f;
                circle.transform.position = groundPos;
            }
        }
    }

    void LeftMouseButtonDown()
    {
        if (qActive)
        {
            //fire
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                Vector2 dir = new Vector2(hit.point.x, hit.point.z) - new Vector2(player.transform.position.x, player.transform.position.z);
                dir.Normalize();
                GameObject.Find("Network").GetComponent<NetworkScript>().SendFireball(new Vector2(player.transform.position.x, player.transform.position.z), dir);
                SetQSkillActive(false);
                qTimer = qCD;
            }
        }
        else if (wActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                Vector3 groundPos = new Vector3(hit.point.x, 0, hit.point.z);
                Vector3 v = groundPos - player.transform.position;
                if (v.magnitude > 6)
                {
                    v.Normalize();
                    v *= 6.0f;
                    groundPos = player.transform.position + v;
                }
                groundPos.y = 0.01f;
                GameObject.Find("Network").GetComponent<NetworkScript>().SendFireground(new Vector2(groundPos.x, groundPos.z));
                SetWSkillActive(false);
                wTimer = wCD;
            }
        }
    }
}
