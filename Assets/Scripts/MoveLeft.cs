using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 20;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver)
        {
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed * 2);
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
    }
}