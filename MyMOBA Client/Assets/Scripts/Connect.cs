using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Connect : MonoBehaviour {
    InputField host, port;

	// Use this for initialization
	void Start () {
        host = GameObject.Find("Host").GetComponent<InputField>();
        host.text = "localhost";
        port = GameObject.Find("Port").GetComponent<InputField>();
        port.text = "15123";
	}
	
	public void ConnectButtonClick()
    {
        string hostText = host.text;
        int portText = int.Parse(port.text);

        ConnectConfig.host = hostText;
        ConnectConfig.port = portText;

        SceneManager.LoadScene("main");
    }
}
