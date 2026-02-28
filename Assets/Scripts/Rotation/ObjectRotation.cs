using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    private bool rotateAllowed;

    private Camera camera;

    [SerializeField] private float speed;

    [SerializeField] private bool inverted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotateAllowed)
            return;

        Vector2 MouseDelta = new Vector2(); // needs to change to get mouse input


        MouseDelta *= speed * Time.deltaTime;

        transform.Rotate(Vector3.up * (inverted ? 1 : -1), MouseDelta.x, Space.World);
        transform.Rotate(Vector3.right * (inverted ? -1 : 1), MouseDelta.y, Space.World);
    }
}
