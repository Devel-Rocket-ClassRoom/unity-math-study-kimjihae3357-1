using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    private float smoothTime = 0.1f;
    private Vector3 offset = new Vector3(0, 3, -5);
    private Vector3 velocity = Vector3.zero;
    

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.rotation * offset;
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
            );

        Quaternion desiredRotation = target.rotation * Quaternion.Euler(10f, 0, 0);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            desiredRotation,
            Time.deltaTime * 10f
            );
            
    }
}
