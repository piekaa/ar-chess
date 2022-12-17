using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;



public class Win32
{
    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);
 
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);
 
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
 
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}


public class Move : MonoBehaviour
{

    [SerializeField]
    private float speed = 1;
    
    [SerializeField]
    private float rotationSpeed = 1;

    private float defaultX;
    private float defaultY;

    private bool lockMouse = true;
    
    void Update()
    {
        
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("q"))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("e"))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }
        
        if (Input.GetKey("escape"))
        {
            EditorApplication.isPlaying = false;
        }


        if (Input.GetKeyDown("o"))
        {
            lockMouse = !lockMouse;
        }

        if (lockMouse)
        {
            if (Time.frameCount > 10)
            {

                var dx = Input.mousePosition.x - defaultX;
                transform.Rotate(Vector3.up, dx * Time.deltaTime * rotationSpeed);
            
                var dy = Input.mousePosition.y - defaultY;
                transform.Rotate(Vector3.right, -dy * Time.deltaTime * rotationSpeed);

                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(euler.x, euler.y, 0);
            }
            Win32.SetCursorPos(500, 500);

            if (Time.frameCount == 5)
            {
                defaultX = Input.mousePosition.x;
                defaultY = Input.mousePosition.y;
            }
        }

    }
}
