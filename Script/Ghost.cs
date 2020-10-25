using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Vector3 RotatePosition;
    private Mover mover;

    public void Setup(Transform[,] field)
    {
        mover.Setup(field);
    }

    public Vector3 Move(Vector3 pos)
    {
        transform.position = pos;
        mover.HardDrop();

        return transform.position;
    }

    public Vector3 TurnLeft(Vector3 pos)
    {
        transform.position = pos;
        mover.TurnLeft(RotatePosition);

        return Move(pos);
    }

    public Vector3 TurnRight(Vector3 pos)
    {
        transform.position = pos;
        mover.TurnRight(RotatePosition);

        return Move(pos);
    }

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }
}
