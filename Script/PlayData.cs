using UnityEngine;
using UnityEngine.UI;

public class PlayData : MonoBehaviour
{
    [SerializeField] Text ScoreText = default;
    [SerializeField] Text LevelText = default;
    [SerializeField] Text LineText = default;

    public void  Render(int score,int level,int deletedLines)
    {
        ScoreText.text = score.ToString();
        LevelText.text = level.ToString();
        LineText.text = deletedLines.ToString();
    }
}
