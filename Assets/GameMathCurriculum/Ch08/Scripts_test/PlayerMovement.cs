using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float rotateSpeed = 70;


    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(0, 0, v);
        transform.Translate(move * speed * Time.deltaTime);
        transform.Rotate(0f, h * rotateSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
