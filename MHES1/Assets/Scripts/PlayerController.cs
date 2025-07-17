using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed = 20;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(Vector3.up * verticalInput * Time.deltaTime * _speed);
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * _speed);
        transform.Translate(Vector3.forward * mouseScrollInput * Time.deltaTime * _speed * 30);
    }
}
