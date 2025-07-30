using System.Linq;
using UnityEngine;

public class LastHumanCameraTrigger : MonoBehaviour
{
    public float zoomTargetSize = 10f;
    public float zoomSpeed = 2f;
    private CameraController cameraController;
    private bool zooming = false;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        var humans = FindObjectsOfType<HumanIdentity>().Where(h => h.IsAlive).ToList();

        if (humans.Count == 1 && !zooming)
        {
            zooming = true;
            Transform target = humans[0].transform;
            StartCoroutine(ZoomToTarget(target));
        }
    }

    System.Collections.IEnumerator ZoomToTarget(Transform target)
    {
        cameraController.enabled = false; // 暂时关闭手动控制

        while (Camera.main.orthographicSize > zoomTargetSize)
        {
            // 缩放
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomTargetSize, Time.deltaTime * zoomSpeed);

            // 移动摄像机中心点
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, Time.deltaTime * zoomSpeed);

            yield return null;
        }
    }
}
