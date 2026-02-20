using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform panel;
    Rect lastSafeArea = new Rect(0, 0, 0, 0);
    Rect safeArea;
    public static bool isSafeArea = false;
    private void Awake()
    {
        panel = GetComponent<RectTransform>();
#if !UNITY_WEBGL
        Refresh();
#endif
    }

    void Refresh()
    {
        safeArea = GetSafeArea();
        if (safeArea != lastSafeArea)
        {
            isSafeArea = true;
            ApplySafeArea(safeArea);
        }
    }

    void Update()
    {
#if !UNITY_WEBGL
        Refresh();
#endif
    }

    Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    void ApplySafeArea(Rect rect)
    {
        lastSafeArea = rect;

        Vector2 anchorMin = rect.position;
        Vector2 anchorMax = rect.position + rect.size;


        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;


        anchorMin.y /= Screen.height;
        anchorMax.y /= Screen.height;

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
    }

}