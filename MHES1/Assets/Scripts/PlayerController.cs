using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    private float _mouseScrollInput;
    private float _speed = 20;

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(Vector3.up * _verticalInput * Time.deltaTime * _speed);
        transform.Translate(Vector3.right * _horizontalInput * Time.deltaTime * _speed);
        transform.Translate(Vector3.forward * _mouseScrollInput * Time.deltaTime * _speed * 20 * transform.position.y);
    }
}
