using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public Vector3 cameraOffset;
    public float smoothing = 5f;

    Hero target;
    bool isTargetFocused;

    public int screenBoundary;
    public float scrollSpeed;

    int screenWidth;
    int screenHeight;

    float top, bottom, left, right;
    int groundLayerMask;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        groundLayerMask = LayerMask.GetMask("Ground");
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask);
        top = hit.point.z - transform.position.z;
        //left = hit.point.x - transform.position.x;
        ray = Camera.main.ViewportPointToRay(new Vector3(1, 0, 0));
        Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask);
        bottom = hit.point.z - transform.position.z;
        right = hit.point.x - transform.position.x;
        left = -right;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (target != null)
            {
                isTargetFocused = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isTargetFocused)
        {
            Vector3 targetCamPos = new Vector3(target.GetXPos(), 0.8f, target.GetZPos()) + cameraOffset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            CheckMapEdge();
        }

        if (CursorOnTheEdge())
        {
            ScrollCamera();
            CheckMapEdge();
            isTargetFocused = false;
        }
    }

    bool CursorOnTheEdge()
    {
        if (Input.mousePosition.x < screenBoundary || Input.mousePosition.x >= screenWidth - screenBoundary ||
            Input.mousePosition.y < screenBoundary || Input.mousePosition.y >= screenHeight - screenBoundary)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ScrollCamera()
    {
        if (Input.mousePosition.x < screenBoundary)
        {
            transform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.mousePosition.x >= screenWidth - screenBoundary)
        {
            transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.mousePosition.y < screenBoundary)
        {
            transform.position += new Vector3(0, 0, -scrollSpeed * Time.deltaTime);
        }
        else if (Input.mousePosition.y >= screenHeight - screenBoundary)
        {
            transform.position += new Vector3(0, 0, scrollSpeed * Time.deltaTime);
        }
    }

    public void SetCameraPos(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, 0, pos.y) + cameraOffset;
        isTargetFocused = false;

        CheckMapEdge();
    }

    public void SetPlayerToCamera(Hero target)
    {
        this.target = target;
        if (target != null)
        {
            isTargetFocused = true;
        }
        else
        {
            isTargetFocused = false;
        }
    }

    public void UnSetPlayerFromCamera()
    {
        target = null;
        isTargetFocused = false;
    }

    void CheckMapEdge()
    {
        if(transform.position.z + top > 55.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 55.0f - top);
        }

        if (transform.position.z + bottom < -55.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -55.0f - bottom);
        }

        if (transform.position.x + right > 55.0f)
        {
            transform.position = new Vector3(55.0f - right, transform.position.y, transform.position.z);
        }

        if (transform.position.x + left < -55.0f)
        {
            transform.position = new Vector3(-55.0f - left, transform.position.y, transform.position.z);
        }
    }
}