using UnityEngine;
using Cinemachine;

public class ScreenTransition : MonoBehaviour
{
    // debug only.
    public Rigidbody2D testControl;

    private static CinemachineConfiner confines;
    private static Transform player;

    [SerializeField] private float playerHitboxWidth;
    [SerializeField] private float playerHitboxOriginToGround;
    
    [SerializeField] private PolygonCollider2D[] frameRegions;
    [SerializeField] private uint entranceHeight;

    private int currentFrameIndex = 0;
    private float currentFrameRightBound;

    private void Awake()
    {
        if (confines == null)
        {
            confines = FindObjectOfType<CinemachineConfiner>();
        }
        if (player == null)
        {
            // Replace with code that gets the player class.
            player = testControl.transform;
        }

        UpdateRightBound(frameRegions[currentFrameIndex]);
    }

    private void Update()
    {
        if(player.position.x > currentFrameRightBound)
        {
            currentFrameIndex++;

            if(currentFrameIndex == frameRegions.Length - 1)
            {
                // Last screen in the game.
            }
            else
            {

            }

            // Move to th next screen region
            confines.m_BoundingShape2D = frameRegions[currentFrameIndex];
            Vector2 lowerLeft = GetPolyLowerLeft(frameRegions[currentFrameIndex]);
            lowerLeft.y += entranceHeight;
            lowerLeft.y += playerHitboxOriginToGround;
            player.position = lowerLeft;
            UpdateRightBound(frameRegions[currentFrameIndex]);
        }

        // debug only
        if (Input.GetKeyDown(KeyCode.A))
        {
            testControl.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            testControl.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
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
        currentFrameRightBound = x;
    }
}