using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboText : MonoBehaviour {

    public GameObject GameManager;
    Text txt;
    Vector3 OriginalPos;
    // Use this for initialization
    void Start () {
        txt = GetComponent<Text>();
        OriginalPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        txt.text = "Combo : " + GameManager.GetComponent<ComboSystem>().getCombo().ToString();
        StartCoroutine(ShakeEffect());
	}

    public IEnumerator ShakeEffect()
    {
        float newPosX = transform.position.x + Random.Range(-5, 5);
        float newPosY = transform.position.y + Random.Range(-5, 5);
        transform.position = new Vector2(newPosX, newPosY);
        yield return new WaitForSeconds(Time.deltaTime);
        transform.position = OriginalPos;
    }
}
