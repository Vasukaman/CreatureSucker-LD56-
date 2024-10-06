using UnityEngine;

public class Creature
{
    private Vector3 position;
    private Vector3 movementDirection;
    private float movementSpeed;

    public Creature(Vector3 startPosition, float speed)
    {
        position = startPosition;
        movementSpeed = speed;
        SetRandomDirection();
    }

    public void UpdateMovement()
    {
        position += movementDirection * movementSpeed * Time.deltaTime;
        if (Random.Range(0, 100) < 5)
        {
            SetRandomDirection();
        }
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    private void SetRandomDirection()
    {
        movementDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;
    }
}
