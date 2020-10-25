using System;
using UnityEngine;

public class Field : MonoBehaviour
{
    private static readonly int width = 10;
    private static readonly int height = 24;
    private static readonly Transform[,] field = new Transform[width, height];
    
    public Transform[,] RawField { get => field; }

    public void Fix()
    {
        foreach (Transform c in transform)
        {
            int x = Mathf.RoundToInt(c.transform.position.x);
            int y = Mathf.RoundToInt(c.transform.position.y);

            if (field[x, y] != null)
            {
                Debug.Log(c.transform.position.x + ", " + c.transform.position.y);
                throw new Exception("block overlap");
            }

            field[x, y] = c;
        }
    }

    public int DeleteLines()
    {
        int cnt = 0;

        for (int y = height - 1; y >= 0; y--)
        {
            if (HasLine(y))
            {
                DeleteLine(y);
                DownRow(y);
                cnt++;
            }
        }

        return cnt;
    }

    private bool HasLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (field[x, y] == null) return false;
        }

        return true;
    }

    private void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(field[x, y].gameObject);
            field[x, y] = null;
        }
    }

    public void DownRow(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (field[x, y] != null)
                {
                    field[x, y - 1] = field[x, y];
                    field[x, y] = null;
                    field[x, y - 1].transform.position += Vector3.down;
                }
            }
        }
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field[x, y] = null;
            }
        }
    }
}
