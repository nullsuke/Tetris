using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
    private static readonly float dropSpeed = 0.9f;
    private Transform[,] field;
    private int width;
    private int height;

    public void Setup(Transform[,] field)
    {
        this.field = field;
        width = field.GetLength(0);
        height = field.GetLength(1);
    }

    public void Fall(float speed)
    {
        transform.position += new Vector3(0, -speed, 0);
    }

    public bool CanMove()
    {
        foreach (Transform c in transform)
        {
            var x = Mathf.RoundToInt(c.transform.position.x);
            //先に四捨五入してから
            var ny = Round4rdDecimalPlace(c.transform.position.y);
            //端数切捨て
            var y = Mathf.FloorToInt(ny);

            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return false;
            }

            if (field[x, y] != null) return false;
        }

        return true;
    }

    public void MoveLeft()
    {
        transform.position += Vector3.left;

        if (!CanMove())
        {
            transform.position -= Vector3.left;
        }
        //もし現状移動できても、上にずらして移動できなかったら
        else if (!CanMoveAboveSpace())
        {
            transform.position -= Vector3.left;
            AdjustToBelow();
            transform.position += Vector3.left;
        }
    }

    public void MoveRight()
    {
        transform.position += Vector3.right;

        if (!CanMove())
        {
            transform.position -= Vector3.right;
        }
        //もし現状移動できても、上にずらして移動できなかったら
        else if (!CanMoveAboveSpace())
        {
            transform.position -= Vector3.right;
            AdjustToBelow();
            transform.position += Vector3.right;
        }
    }

    public void TurnLeft(Vector3 rotatePosition)
    {
        transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), 90);

        if (!CanMove())
        {
            transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), -90);
        }
    }

    public void TurnRight(Vector3 rotatePosition)
    {
        transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), -90);

        if (!CanMove())
        {
            transform.RotateAround(transform.TransformPoint(rotatePosition),
                new Vector3(0, 0, 1), 90);
        }
    }

    public void AdjustToAbove()
    {
        var p = transform.position;
        int y = Mathf.FloorToInt(p.y + 1);

        transform.position = new Vector3(p.x, y);
    }

    public void HardDrop()
    {
        var vec = new Vector3(0, -dropSpeed);

        do
        {
            transform.position += vec;
        } while (CanMove());

        AdjustToAbove();
    }

    //上にずらして移動できるかどうか判定
    private bool CanMoveAboveSpace()
    {
        foreach (Transform c in transform)
        {
            var x = Mathf.RoundToInt(c.transform.position.x);
            var y = Mathf.CeilToInt(c.transform.position.y);

            if (field[x, y] != null) return false;
        }

        return true;
    }

    //下にずらす
    private void AdjustToBelow()
    {
        var p = transform.position;
        int y = Mathf.FloorToInt(p.y);

        transform.position = new Vector3(p.x, y);
    }

    //小数第4位で四捨五入
    private float Round4rdDecimalPlace(float n)
    {
        return (float)(Math.Round(n * 1000, MidpointRounding.AwayFromZero)) / 1000;
    }
}
