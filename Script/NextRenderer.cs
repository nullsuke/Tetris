using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRenderer : MonoBehaviour
{
    [SerializeField] Image[] nextImages = default;
    [SerializeField] Sprite[] tetriminoSprites = default;

    public void Render(Queue<int> queue)
    {
        for (int i = 0; i < 3; i++)
        {
            int n = queue.ToArray()[i];
            nextImages[i].sprite = tetriminoSprites[n];
        }
    }
}
