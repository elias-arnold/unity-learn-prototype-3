using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 _startPos;

    private float backgroundMove;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        backgroundMove = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startPos.x - transform.position.x > backgroundMove)
        {
            transform.position = _startPos;
        }
    }
}
