using UnityEngine;

public class DragObject : MonoBehaviour
{
    public bool isReturning;
    public Vector3 originalPosition;
    public float timeReturn = 2f;
    private Vector3 startPostion;

    private Terrain terrain;
    private float timer;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
    }
    public void DragStart()
    {
        isReturning = false;
        timer = 0f;
        startPostion = Vector3.zero;
        originalPosition = transform.position;
    }

    public void Return()
    {
        timer = 0f;
        isReturning = true;
        startPostion = transform.position;
    }

    public void DragEnd()
    {
        isReturning=false;
        timer = 0f;
        originalPosition = Vector3.zero;
        startPostion = Vector3.zero;
    }

    private void Update()
    {
        if (isReturning)
        {
            timer += Time.deltaTime / timeReturn;
            Vector3 newPos = Vector3.Lerp(startPostion, originalPosition, timer);
            newPos.y = terrain.SampleHeight(newPos);
            transform.position = newPos;

            if (timer > 1f)
            {
                isReturning = false;
                transform.position = originalPosition;
                timer = 0f;
            }
        }
    }
}
