using Cinemachine;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform _character;
    private float offsetX = 0f;
    private float offsetZ = 0f;

    public void Init(Transform character)
    {
        _character = character;
        transform.position = new Vector3(1.59f, 13.84f, -10.84f);

        GetComponent<CinemachineVirtualCamera>().LookAt = _character.transform;
        offsetX = transform.position.x - _character.position.x;
        offsetZ = transform.position.z - _character.position.z;
    }

    void Update()
    {
        if (_character != null)
        {
            transform.position = new Vector3(_character.transform.position.x + offsetX, transform.position.y, _character.transform.position.z + offsetZ);
        }
    }
}
