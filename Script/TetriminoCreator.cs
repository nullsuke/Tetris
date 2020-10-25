using System.Collections.Generic;
using UnityEngine;

public class TetriminoCreator : MonoBehaviour
{
    [SerializeField] Tetrimino[] tetriminoPrefabs = default;
    private NextRenderer nextRenderer;
    private Queue<int> queue;

    public void NewTetrimino(bool isFirst)
    {
        int i = queue.Dequeue();
        var t = Instantiate(tetriminoPrefabs[i]);

        if (isFirst) t.Initialize();

        t.Falled += (s, e) => NewTetrimino(false);

        int r = UnityEngine.Random.Range(0, 7);
        queue.Enqueue(r);

        nextRenderer.Render(queue);
    }

    private void Awake()
    {
        nextRenderer = GetComponent<NextRenderer>();
        queue = new Queue<int>();

        for (int i = 0; i < 3; i++)
        {
            int r = UnityEngine.Random.Range(0, 7);
            queue.Enqueue(r);
        }
    }
}
