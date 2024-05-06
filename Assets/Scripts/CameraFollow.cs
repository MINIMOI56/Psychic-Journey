using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [Header("Collisions")]
    [SerializeField] public GameObject Bottom;
    [SerializeField] public GameObject Top;

    void Update()
    {
        playerPosCheck();
    }

    void playerPosCheck()
    {
        if (player.transform.position.y > Top.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 16, transform.position.z);
            Debug.Log("Top:" + Top.transform.position.y);
            Debug.Log("Bottom:" + Bottom.transform.position.y);
        }

        if (player.transform.position.y < Bottom.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 16, transform.position.z);
            Debug.Log("Top:" + Top.transform.position.y);
            Debug.Log("Bottom:" + Bottom.transform.position.y);
        }
    }
}
