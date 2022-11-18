using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;
    [SerializeField] float CameraMoveSpeed = 1.1f;

    public Transform myCam = null;

    public Vector2 ZoomRange = new Vector2(5f, 30.0f);
    public float SmoothDistSpeed = 5.0f;
    public float zoomSpeed = 10.0f;
    public LayerMask CrashMask = default;
    float OffsetDist = 0.5f;
    float desireDist;
    float curCamDist;
    void Start()
    {
        desireDist = curCamDist = -myCam.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = characterBody.position;
        LookDistance();
        LookAround();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X") * CameraMoveSpeed, Input.GetAxisRaw("Mouse Y") * CameraMoveSpeed);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1, 80);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    void LookDistance()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > Mathf.Epsilon || Input.GetAxisRaw("Mouse ScrollWheel") < -Mathf.Epsilon)
        {
            desireDist += Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
            desireDist = Mathf.Clamp(desireDist, ZoomRange.x, ZoomRange.y);
        }
        curCamDist = Mathf.Lerp(curCamDist, desireDist, Time.deltaTime * SmoothDistSpeed);

        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = -transform.forward;
        float checkDist = Mathf.Min(curCamDist, desireDist);
        if (Physics.Raycast(ray, out RaycastHit hit, checkDist + OffsetDist + 0.1f, CrashMask))
        {
            curCamDist = Vector3.Distance(transform.position, hit.point + myCam.forward * OffsetDist);
        }

        myCam.transform.localPosition = new Vector3(0, 4f, -curCamDist);
    }
}