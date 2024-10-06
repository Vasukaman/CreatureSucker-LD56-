using UnityEngine;

public class StayInFurnace : MonoBehaviour
{
    private Collider furnaceCollider;

    public void SetFurnaceTrigger(Collider furnace)
    {
        furnaceCollider = furnace;
    }

    void Update()
    {
        if (furnaceCollider != null)
        {
            // Check if the creature is outside the furnace area
            if (!furnaceCollider.bounds.Contains(transform.position))
            {
                // Snap back to the nearest point inside the furnace bounds
                Vector3 nearestPoint = furnaceCollider.ClosestPoint(transform.position);
                transform.position = nearestPoint;
            }
        }
    }
}
