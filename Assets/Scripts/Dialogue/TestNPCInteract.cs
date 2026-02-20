using UnityEngine;

public class TestNPCInteract : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            this.gameObject.GetComponent<Dialogue>().InitialiseDialogue();
        }
    }
}
