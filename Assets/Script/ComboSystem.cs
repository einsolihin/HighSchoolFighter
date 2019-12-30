using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboSystem : MonoBehaviour {
    float currentCombo = 0;
    float MaxCombo = 30;
    float CurrentTimer = 180;
    float TotalCombo = 0;
    public GameObject ComboText;
    public GameObject TimerTxt;
    public GameObject ComboScore;
    public GameObject IncSec;

	// Use this for initialization
	void Start () {
        ComboText.SetActive(false);
        ComboScore.GetComponent<Text>().text = "Total Combo: " + TotalCombo.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentTimer > 0)
            CurrentTimer -= Time.deltaTime;
        else
            gameObject.GetComponent<UI>().EndGame();

        if(currentCombo >= MaxCombo)
        {
            currentCombo = 0;
            MaxCombo += 10;
            CurrentTimer += 5;
            StartCoroutine(TimerTxt.GetComponent<Timer>().TimeExtend());
            /*Instantiate(IncSec);
            StartCoroutine(ShortDisplay());*/
        }

        if (currentCombo>20)
            ComboText.SetActive(true);
        else
            ComboText.SetActive(false);
    }

    public float getTimer()
    {
        return CurrentTimer;
    }

    public float getCombo()
    {
        return currentCombo;
    }

    public void ComboAdd()
    {
        currentCombo++;
        TotalCombo++;
        ComboScore.GetComponent<Text>().text = "Total Combo: " + TotalCombo.ToString();
    }

    /*IEnumerator ShortDisplay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(IncSec);
    }*/
}
