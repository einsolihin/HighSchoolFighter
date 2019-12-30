using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    float currentTimer;
    public GameObject GameManager;
    Text txt;
    // Use this for initialization
    void Start () {
        txt = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        currentTimer = GameManager.GetComponent<ComboSystem>().getTimer();
        txt.text = currentTimer.ToString("F2");
    }

    public IEnumerator TimeExtend()
    {
        txt.color = Color.green;
        yield return new WaitForSeconds(2f);
        txt.color = Color.white;
    }
}
