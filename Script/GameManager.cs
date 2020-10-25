using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] TetriminoCreator tetriminoCreator = default;
    [SerializeField] PlayData playData = default;
    [SerializeField] GameObject gameOverPanel = default;
    [SerializeField] GameObject clearPanel = default;

    public void RenderPlayData(int score, int level, int deletedLines)
    {
        playData.Render(score, level, deletedLines);
    }

    public void Over()
    {
        gameOverPanel.SetActive(true);
    }

    public void Clear()
    {
        clearPanel.SetActive(true);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        tetriminoCreator.NewTetrimino(true);
    }
}