using UnityEngine;
using System.Collections;

public class DeadScript : MonoBehaviour {
	void Start () {
        Destroy(gameObject, 3.0f);
    }
}
