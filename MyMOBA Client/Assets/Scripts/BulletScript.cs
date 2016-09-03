using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    Vector3 startPos, endPos;
    Vector3 dir;
    float time;

    public void Initialize(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;

        transform.position = startPos;

        dir = endPos - startPos;
        transform.LookAt(endPos);
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 0.25f)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 curPos = startPos + (time) * dir * 4;
            transform.position = curPos;
        }
    }
}
