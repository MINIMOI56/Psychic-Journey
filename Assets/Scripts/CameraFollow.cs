using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int size;
    [SerializeField] private float maxCameraHeight;

    [Header("Collisions")]
    private GameObject bottom;
    private GameObject top;

    void Start()
    {
        Camera.main.orthographicSize = size;

        Vector3 topPosition = transform.position + new Vector3(0f, Camera.main.orthographicSize, 0f);
        Vector3 bottomPosition = transform.position - new Vector3(0f, Camera.main.orthographicSize, 0f);

        top = Instantiate(new GameObject(), topPosition, Quaternion.identity);
        bottom = Instantiate(new GameObject(), bottomPosition, Quaternion.identity);

        top.transform.SetParent(transform);
        bottom.transform.SetParent(transform);
    }

    void Update()
    {
        PlayerPosCheck();
    }

    void PlayerPosCheck()
    {
        if (player.transform.position.y > top.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + size * 2, transform.position.z);
        }

        if (player.transform.position.y < bottom.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - size * 2, transform.position.z);
        }
    }
}
