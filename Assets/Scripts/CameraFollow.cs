using UnityEngine;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float smoothSpeed = 5f;

    [Header("카메라 화면이 벗어나지 않아야 할 영역")]
    public Rect visibleBounds = new Rect(-10, -5, 20, 10); // (x, y, width, height)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        Vector3 desiredPosition = target.position + (Vector3)offset;
        desiredPosition.z = transform.position.z;

        // 카메라가 보여주는 영역이 visibleBounds를 벗어나지 않도록 제한
        float minX = visibleBounds.xMin + horzExtent;
        float maxX = visibleBounds.xMax - horzExtent;
        float minY = visibleBounds.yMin + vertExtent;
        float maxY = visibleBounds.yMax - vertExtent;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(visibleBounds.center.x, visibleBounds.center.y, 0f);
        Vector3 size = new Vector3(visibleBounds.width, visibleBounds.height, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}
