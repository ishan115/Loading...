using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ScreenTransition : MonoBehaviour
{
    private const float leftBoundaryWidth = 2;

    [SerializeField] private CinemachineConfiner confines;

    [SerializeField] private Transform player;

    [SerializeField] private float playerHitboxWidth;
    [SerializeField] private float playerHitboxOriginToGround;
    
    [SerializeField] private PolygonCollider2D[] frameRegions;
    [SerializeField] private uint entranceHeight;

    [SerializeField] private RawImage fadeImage;

    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private AnimationClip fadeClip;
 
    private int currentFrameIndex = 0;
    private float currentFrameRightBound;

    private void Awake()
    {
        UpdateRightBound(frameRegions[currentFrameIndex]);
        GenerateNewLeftBoundary(frameRegions[currentFrameIndex]);
    }

    private void Update()
    {
        if(player.position.x > currentFrameRightBound)
        {
            currentFrameIndex++;

            // Move to the next screen region
            confines.m_BoundingShape2D = frameRegions[currentFrameIndex];
            Vector2 lowerLeft = GetPolyLowerLeft(frameRegions[currentFrameIndex]);
            lowerLeft.y += entranceHeight;
            lowerLeft.y += playerHitboxOriginToGround;
            player.position = lowerLeft;
            UpdateRightBound(frameRegions[currentFrameIndex]);
            GenerateNewLeftBoundary(frameRegions[currentFrameIndex]);
        }
    }

    private Vector2 GetPolyLowerLeft(PolygonCollider2D collider)
    {
        float x = Mathf.Infinity;
        float y = Mathf.Infinity;

        foreach(Vector2 point in collider.points)
        {
            if(point.x < x) { x = point.x; }
            if(point.y < y) { y = point.y; }
        }

        Transform parent = collider.gameObject.transform;
        return new Vector2(x + parent.position.x, y + parent.position.y);
    }
    private void UpdateRightBound(PolygonCollider2D collider)
    {
        float x = Mathf.NegativeInfinity;
        foreach (Vector2 point in collider.points)
        {
            if (point.x > x) { x = point.x; }
        }
        currentFrameRightBound = x + collider.gameObject.transform.position.x;
    }
    private Transform GenerateNewLeftBoundary(PolygonCollider2D collider)
    {
        GameObject newBoundary = new GameObject();
        BoxCollider2D newCollider = newBoundary.AddComponent<BoxCollider2D>();

        newCollider.size = new Vector2(leftBoundaryWidth, collider.bounds.extents.y * 2);
        Vector2 colliderCenter = GetPolyLowerLeft(collider) + Vector2.up * collider.bounds.extents.y;
        colliderCenter.x -= 0.5f * leftBoundaryWidth;
        newBoundary.transform.position = colliderCenter;
        return newBoundary.transform;
    }
}