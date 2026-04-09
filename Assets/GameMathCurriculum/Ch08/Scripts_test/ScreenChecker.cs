using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenChecker : MonoBehaviour
{
    [Header("[카메라]")]
    public Camera targetCamera;
    [Header("[오브젝트]")]
    public Transform[] targetObjects;
    [Header("[인디케이터]")]
    public RectTransform[] indicators;
    [Header("[캔버스]")]
    public Canvas canvas;

    [SerializeField] private float edgeOffset = 30f;

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

    }

    
    private void LateUpdate()
    {
        int count = Mathf.Min(targetObjects.Length, indicators.Length);
        for (int i = 0; i < count; i++)
        {
            Transform targetObject = targetObjects[i];
            RectTransform indicator = indicators[i];

            if (targetObject == null || indicator == null )
            {
                continue;
            }

            UpdateIndicator(targetObject, indicator);

        }
    }

    private void UpdateIndicator(Transform targetObject, RectTransform indicator)
    {
        Vector3 screenPos = targetCamera.WorldToScreenPoint(targetObject.position);

        bool isBehind = screenPos.z < 0f;

        bool isInside =
            screenPos.z > 0f &&
            screenPos.x >= 0f && screenPos.x <= Screen.width &&
            screenPos.y >= 0f && screenPos.y <= Screen.height;

        if (isInside)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        indicator.gameObject.SetActive(true);

        if (isBehind )
        { // 화면 중앙 기준으로 반전
            Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
            screenPos.x = center.x - (screenPos.x - center.x);
            screenPos.y = center.y - (screenPos.y - center.y);
        }

        float clampedX = Mathf.Clamp(screenPos.x, edgeOffset, Screen.width - edgeOffset);
        float clampedY = Mathf.Clamp(screenPos.y, edgeOffset, Screen.height - edgeOffset);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, 0f);
        indicator.position = clampedPos;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dir = (screenPos - screenCenter).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        indicator.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
