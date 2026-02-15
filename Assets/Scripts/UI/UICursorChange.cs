using UnityEngine;

public class UICursorChange : MonoBehaviour
{
    public Texture2D cursorEnterTexture;   // Custom cursor for when the mouse enters the slot
    public Texture2D cursorExitTexture;    // Custom cursor for when the mouse exits the slot
    public Vector2 cursorHotspotOffset = Vector2.zero; // Hotspot offset for the cursor

    // Called when the mouse enters the slot
    public void OnCursorEnter()
    {
        if (cursorEnterTexture != null)
        {
            Cursor.SetCursor(cursorEnterTexture, cursorHotspotOffset, CursorMode.Auto);
        }
    }

    // Called when the mouse exits the slot
    public void OnCursorExit()
    {
        if (cursorExitTexture != null)
        {
            Cursor.SetCursor(cursorExitTexture, cursorHotspotOffset, CursorMode.Auto);
        }
        else
        {
            // Reset to default cursor if no custom exit texture
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
