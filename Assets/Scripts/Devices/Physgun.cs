using System.Collections.Generic;
using UnityEngine;

// https://github.com/cr4yz/UMod/blob/master/Assets/Scripts/Items/PhysGun.cs
public class Physgun : MonoBehaviour
{
    [Header("PhysGun Properties")]
    [SerializeField]
    private float _maxGrabDistance = 40f;
    [SerializeField]
    private float _minGrabDistance = 1f;
    [SerializeField]
    private LineRenderer _pickLine;

    private Rigidbody _grabbedObject;
    private float _pickDistance;
    private Vector3 _pickOffset;
    private Quaternion _rotationOffset;
    private Vector3 _pickTargetPosition;
    private Vector3 _pickForce;
    private IGrabRayContext _pickRayContext;

    public Rigidbody Grabbed => _grabbedObject;
    public float GrabDistance => _pickDistance;

    public Vector3 VelocityOffset { get; set; }

    public bool TryGetGrabPoint(out Vector3 point)
    {
        point = Vector3.zero;   

        if(_grabbedObject == null)
            return false;

        point = _grabbedObject.position + _grabbedObject.transform.TransformVector(_pickOffset);
        return true;
    }

    private void Start()
    {
        if (!_pickLine)
        {
            var obj = new GameObject("PhysGun Pick Line");
            _pickLine = obj.AddComponent<LineRenderer>();
            _pickLine.startWidth = 0.02f;
            _pickLine.endWidth = 0.02f;
            _pickLine.useWorldSpace = true;
            _pickLine.gameObject.SetActive(false);
        }
    }

    public void ChangePickDistance(float dist)
    {
        _pickDistance = Mathf.Clamp(dist, _minGrabDistance, _maxGrabDistance);
    }

    private void LateUpdate()
    {
        if (_grabbedObject)
        {
            var ray = _pickRayContext.GetRay();
            var midpoint = (ray.origin + ray.direction * _pickDistance) * .5f;
            DrawQuadraticBezierCurve(_pickLine, ray.origin, midpoint, _grabbedObject.position + _grabbedObject.transform.TransformVector(_pickOffset));
        }
    }

    private void FixedUpdate()
    {
        if (_grabbedObject != null)
        {
            var ray = _pickRayContext.GetRay();
            _pickTargetPosition = (ray.origin + ray.direction * _pickDistance) - _grabbedObject.transform.TransformVector(_pickOffset);
            var forceDir = _pickTargetPosition - _grabbedObject.position;
            _pickForce = forceDir / Time.fixedDeltaTime * 0.3f / _grabbedObject.mass;
            _grabbedObject.velocity = _pickForce + VelocityOffset;
            _grabbedObject.transform.rotation = transform.rotation * _rotationOffset;

        }
    }

    public void Grab(IGrabRayContext rayContext)
    {
        var ray = rayContext.GetRay();  
        if (Physics.Raycast(ray, out RaycastHit hit, _maxGrabDistance, layerMask: 1 << 0)
            && hit.rigidbody != null
            && !hit.rigidbody.CompareTag("Player"))
        {
            _rotationOffset = Quaternion.Inverse(transform.rotation) * hit.rigidbody.rotation;
            _pickOffset = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
            _pickDistance = hit.distance;
            _grabbedObject = hit.rigidbody;
            _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _grabbedObject.useGravity = false;
            _grabbedObject.freezeRotation = true;
            _grabbedObject.isKinematic = false;
            _pickLine.gameObject.SetActive(true);
            _pickRayContext = rayContext;
            VelocityOffset = Vector3.zero;
        }
    }

    public void Release(bool freeze = false)
    {
        _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _grabbedObject.useGravity = true;
        _grabbedObject.freezeRotation = false;
        _grabbedObject.isKinematic = false;
        _pickLine.gameObject.SetActive(false);

        if (freeze)
        {
            Freeze(_grabbedObject);
        }
        else
        {
            Unfreeze(_grabbedObject);
        }

        _grabbedObject = null;
    }

    private Dictionary<Rigidbody, Rigidbody> _jointSwaps = new Dictionary<Rigidbody, Rigidbody>();
    private void Freeze(Rigidbody rb)
    {
        if (rb.TryGetComponent(out CharacterJoint characterJoint))
        {
            var fixedJointObject = GameObject.Instantiate(rb.gameObject, rb.transform.parent);
            var fixedJoint = fixedJointObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = characterJoint.connectedBody;
            fixedJoint.connectedAnchor = characterJoint.connectedAnchor;
            fixedJoint.massScale = characterJoint.massScale;
            fixedJoint.connectedMassScale = characterJoint.connectedMassScale;
            fixedJoint.GetComponent<Rigidbody>().isKinematic = true;
            _jointSwaps.Add(fixedJoint.GetComponent<Rigidbody>(), rb);

            rb.gameObject.SetActive(false);
        }
        rb.isKinematic = true;
    }

    private void Unfreeze(Rigidbody rb)
    {
        if (_jointSwaps.ContainsKey(rb))
        {
            _jointSwaps[rb].gameObject.SetActive(true);
            _jointSwaps[rb].isKinematic = false;
            _jointSwaps[rb].transform.localPosition = rb.transform.localPosition;
            _jointSwaps[rb].transform.localScale = rb.transform.localScale;
            _jointSwaps[rb].transform.localRotation = rb.transform.localRotation;
            _jointSwaps[rb].GetComponent<CharacterJoint>().connectedAnchor = rb.GetComponent<FixedJoint>().connectedAnchor;
            _jointSwaps[rb].GetComponent<CharacterJoint>().anchor = rb.GetComponent<FixedJoint>().anchor;
            GameObject.Destroy(rb.gameObject);
            _jointSwaps.Remove(rb);
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    // https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    void DrawQuadraticBezierCurve(LineRenderer line, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 20;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < line.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
            t += (1 / (float)line.positionCount);
        }
    }

}

public interface IGrabRayContext
{
    Ray GetRay();
}
