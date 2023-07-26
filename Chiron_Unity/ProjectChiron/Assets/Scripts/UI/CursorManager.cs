using UnityEngine;

namespace UI
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTex; 
        private void Awake()
        {
            Cursor.SetCursor(cursorTex, new Vector2(cursorTex.width/2,cursorTex.height/2) , CursorMode.Auto);
        }
    }
}
