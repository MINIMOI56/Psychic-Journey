using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int size;
    [SerializeField] private float maxCameraHeight;

    [Header("Collisions")]
    private GameObject bottom;
    private GameObject top;
    private GameObject left;
    private GameObject right;

    void Start()
    {
        Camera.main.orthographicSize = size;

        Vector3 topPosition = transform.position + Vector3.up * Camera.main.orthographicSize;
        Vector3 bottomPosition = transform.position - Vector3.up * Camera.main.orthographicSize;
        Vector3 leftPosition = transform.position - Vector3.right * Camera.main.orthographicSize * Camera.main.aspect;
        Vector3 rightPosition = transform.position + Vector3.right * Camera.main.orthographicSize * Camera.main.aspect;

        top = CreateEmptyGameObject(topPosition);
        bottom = CreateEmptyGameObject(bottomPosition);
        left = CreateEmptyGameObject(leftPosition);
        right = CreateEmptyGameObject(rightPosition);
    }

    // Create an empty game object at a given position
    GameObject CreateEmptyGameObject(Vector3 position)
    {
        GameObject emptyGameObject = new GameObject();
        emptyGameObject.transform.position = position;
        emptyGameObject.transform.SetParent(transform);
        return emptyGameObject;
    }

    void Update()
    {
        PlayerPosCheck();
    }

    // Check if the player is out of the camera bounds, if so, move the camera to the next screen
    void PlayerPosCheck()
    {
        Vector3 playerPos = player.transform.position;

        if (playerPos.y > top.transform.position.y)
        {
            transform.position += Vector3.up * size * 2;
        }

        if (playerPos.y < bottom.transform.position.y)
        {
            transform.position -= Vector3.up * size * 2;
        }

        if (playerPos.x < left.transform.position.x)
        {
            transform.position -= Vector3.right * size * 2 * Camera.main.aspect;
        }

        if (playerPos.x > right.transform.position.x)
        {
            transform.position += Vector3.right * size * 2 * Camera.main.aspect;
        }
    }
}
