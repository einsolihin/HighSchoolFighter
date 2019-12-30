using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    float CameraPosX;
    float CameraPosY;
    public float rangeX = 12.33f;
    public float rangeY = 2.46f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player.transform.position.x < rangeX && player.transform.position.x > -rangeX)
            CameraPosX = player.transform.position.x;
        else if (player.transform.position.x > rangeX)
            CameraPosX = rangeX;
        else
            CameraPosX = -rangeX;

        if (player.transform.position.y < rangeY && player.transform.position.y > -rangeY)
        {
            CameraPosY = player.transform.position.y;
        }
        else if (player.transform.position.y > rangeY)
            CameraPosY = rangeY;
        else
            CameraPosY = -rangeY;

        transform.position = new Vector3(CameraPosX, CameraPosY, -10f);
    }
}
