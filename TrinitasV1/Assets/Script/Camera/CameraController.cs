using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float moveSpeed = 0.5f;
    public float minZoom = 10f;
    public float maxZoom = 50f;
    public Rect worldBounds = new Rect(-200, -200, 400, 400);

    private Vector3 dragOrigin;
    public bool isLocked = false;
    private Transform zoomTarget;
    public float zoomInSize = 15f;
    public float zoomOutSize = 50f;
    public float zoomLerpSpeed = 1f;

    private bool isZoomingOut = false;

    void Update()
    {
        if (isLocked && zoomTarget != null)
        {
            Vector3 targetPos = new Vector3(zoomTarget.position.x, zoomTarget.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * zoomLerpSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomInSize, Time.deltaTime * zoomLerpSpeed);
        }
        else if (isZoomingOut)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomOutSize, Time.deltaTime * zoomLerpSpeed);
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * zoomLerpSpeed);

            if (Mathf.Abs(Camera.main.orthographicSize - zoomOutSize) < 0.1f && Vector3.Distance(transform.position, new Vector3(0, 0, transform.position.z)) < 0.1f)
            {
                Camera.main.orthographicSize = zoomOutSize;
                transform.position = new Vector3(0, 0, transform.position.z);
                isZoomingOut = false;
            }
        }
        else
        {
            HandleZoom();
            HandleDrag();
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = dragOrigin - currentPos;
            Camera.main.transform.position = ClampToBounds(Camera.main.transform.position + move);
        }
    }

    Vector3 ClampToBounds(Vector3 targetPos)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float minX = worldBounds.xMin + camWidth;
        float maxX = worldBounds.xMax - camWidth;
        float minY = worldBounds.yMin + camHeight;
        float maxY = worldBounds.yMax - camHeight;

        return new Vector3(
            Mathf.Clamp(targetPos.x, minX, maxX),
            Mathf.Clamp(targetPos.y, minY, maxY),
            Camera.main.transform.position.z
        );
    }

    public void ZoomToTarget(Transform target)
    {
        zoomTarget = target;
        isLocked = true;
    }

    public void UnlockCamera()
    {
        isLocked = false;
        zoomTarget = null;
    }

    public void TriggerZoomOut()
    {
        isLocked = false;
        zoomTarget = null;
        isZoomingOut = true;
    }

    // ✅ 新增：平滑缩放到全图（协程方式）
    public IEnumerator SmoothZoomOut(float targetSize, float speed)
    {
        isLocked = false;
        zoomTarget = null;

        float duration = 2f;  // 可以调节
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        float startSize = Camera.main.orthographicSize;

        Vector3 endPos = new Vector3(0, 0, transform.position.z);
        float endSize = targetSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Camera.main.orthographicSize = Mathf.Lerp(startSize, endSize, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        // 确保最终状态完全到位
        Camera.main.orthographicSize = endSize;
        transform.position = endPos;

        Debug.Log("✅ SmoothZoomOut finished");
    }

}
