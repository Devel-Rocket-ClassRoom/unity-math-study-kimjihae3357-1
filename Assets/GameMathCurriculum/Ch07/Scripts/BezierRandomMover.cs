using System.Collections.Generic;
using UnityEngine;

public class BezierRandomMover : MonoBehaviour
{
    [Header("[생성할 프리팹]")]
    [SerializeField] private GameObject spherePrefab;

    [Header("[시작점 / 끝점]")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("[생성 설정]")]
    [SerializeField] private int spawnCount = 5;

    [Header("[랜덤 제어점 오프셋 범위]")]
    [SerializeField] private Vector3 controlPointOffsetMin = new Vector3(-5f, -5f, -5f);
    [SerializeField] private Vector3 controlPointOffsetMax = new Vector3(5f, 5f, 5f);

    [Header("[랜덤 이동 시간 범위]")]
    [SerializeField] private Vector2 moveDurationRange = new Vector2(1.5f, 4f);

    private class MovingObject
    {
        public Transform transform;
        public Renderer renderer;

        public TrailRenderer trailRenderer;

        public Vector3 p0, p1, p2, p3, p4;

        public float duration; // 총 이동 시간
        public float elapsedTime; // 현재까지 경과한 시간
    }

    private readonly List<MovingObject> movingObjects = new List<MovingObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObjects();
        }

        UpdateMovingObjects();
    }

    private void SpawnObjects()
    {
        if (spherePrefab == null || startPoint == null || endPoint == null)
        {
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject obj = Instantiate(spherePrefab, startPoint.position, Quaternion.identity);

            MovingObject newObject = new MovingObject();
            newObject.transform = obj.transform;
            newObject.renderer = obj.GetComponent<Renderer>();
            newObject.trailRenderer = obj.GetComponent<TrailRenderer>();

            newObject.p0 = startPoint.position;
            newObject.p1 = newObject.p0 + GetRandomControlOffset();
            newObject.p2 = Vector3.Lerp(newObject.p0, endPoint.position, 0.5f) + GetRandomControlOffset();
            newObject.p3 = endPoint.position + GetRandomControlOffset();
            newObject.p4 = endPoint.position;

            newObject.duration = Random.Range(moveDurationRange.x, moveDurationRange.y);
            newObject.elapsedTime = 0f;

            SetRandomColor(newObject.renderer, newObject.trailRenderer);
            movingObjects.Add(newObject);
        }

            
    }

    private void UpdateMovingObjects()
    {
        for (int i = movingObjects.Count - 1; i >= 0; i--)
        {
            MovingObject obj = movingObjects[i];

            if (obj == null || obj.transform == null)
            {
                movingObjects.RemoveAt(i);
                continue;
            }

            obj.elapsedTime += Time.deltaTime;

            float t = obj.elapsedTime / obj.duration;
            t = Mathf.Clamp01(t);

            Vector3 newPosition = CalculateQuarticBezierPoint(t, obj.p0, obj.p1, obj.p2, obj.p3, obj.p4);
            obj.transform.position = newPosition;

            Vector3 dir = CalculateQuarticBezierTangent(t, obj.p0, obj.p1, obj.p2, obj.p3, obj.p4);
            if (dir != Vector3.zero)
            {
                obj.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }


            if (t >= 1f)
            {
                Destroy(obj.transform.gameObject);
                movingObjects.RemoveAt(i);
            }
        }
    }

    private Vector3 CalculateQuarticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float oneMinusT = 1f - t;

        return
            oneMinusT * oneMinusT * oneMinusT * oneMinusT * p0 +
            4f * oneMinusT * oneMinusT * oneMinusT * t * p1 +
            6f * oneMinusT * oneMinusT * t * t * p2 +
            4f * oneMinusT * t * t * t * p3 +
            t * t * t * t * p4;
    }

    private Vector3 CalculateQuarticBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float oneMinusT = 1f - t;

        return
            4f * oneMinusT * oneMinusT * oneMinusT * (p1 - p0) +
            12f * oneMinusT * oneMinusT * t * (p2 - p1) +
            12f * oneMinusT * t * t * (p3 - p2) +
            4f * t * t * t * (p4 - p3);
    }

    private Vector3 GetRandomControlOffset()
    {
        float x = Random.Range(controlPointOffsetMin.x, controlPointOffsetMax.x);
        float y = Random.Range(controlPointOffsetMin.y, controlPointOffsetMax.y);
        float z = Random.Range(controlPointOffsetMin.z, controlPointOffsetMax.z);

        return new Vector3(x, y, z);
    }

    private void SetRandomColor(Renderer targetRenderer, TrailRenderer targetTrailRenderer)
    {
        if (targetRenderer == null) return;

        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 0.8f, 0.8f, 1f);

        targetRenderer.material.color = randomColor;

        if (targetTrailRenderer != null)
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(randomColor, 0f);
            colorKeys[1] = new GradientColorKey(randomColor, 1f);

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f); // 꼬리 앞
            alphaKeys[1] = new GradientAlphaKey(0f, 1f); // 꼬리 끝

            gradient.SetKeys(colorKeys, alphaKeys);

            targetTrailRenderer.colorGradient = gradient;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (startPoint != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(startPoint.position, 0.2f);
        }

        if (endPoint != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(endPoint.position, 0.2f);
        }
    }
}