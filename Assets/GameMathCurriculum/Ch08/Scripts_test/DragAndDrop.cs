using UnityEditor.PackageManager;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Camera camera;
    public LayerMask ground;
    public LayerMask dragObject;
    public LayerMask dropZone;

    private bool isDraging = false;
    private DragObject dragginObject;

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, dragObject))
            {
                isDraging = true;
                dragginObject = hitInfo.collider.GetComponent<DragObject>();
                dragginObject.DragStart();
            }
        }

        else if (Input. GetMouseButtonUp(0))
        {
            if  (isDraging)
            {
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, dropZone))
                {
                    dragginObject.DragEnd();
                }
                else
                {
                    dragginObject.Return();
                }
                
                isDraging = false;
                dragginObject = null;
            }
        }

        else if (isDraging)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, ground))
            {
                dragginObject.transform.position = hitInfo.point;
            }
        }
       



    }
}
