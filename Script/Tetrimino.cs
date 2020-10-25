using System;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    [SerializeField] Ghost ghostPrefab = default;
    public event EventHandler<EventArgs> Falled;
    public Vector3 RotatePosition;
    private static GameManager gameManager;
    private static readonly Vector3 startPosition = new Vector3(3, 20);
    private static readonly float defaultSpeed = 0.05f;
    private static readonly float maxSpeed = 0.5f;
    private static readonly int delayLimit = 50;
    private static readonly int maxLevel = 10;
    private static readonly int nextLine = 10;
    private static float levelSpeed = defaultSpeed;
    private static float speed = levelSpeed;
    private static int score = 0;
    private static int level = 1;
    private static int deletedLines = 0;
    private Field field;
    private Mover mover;
    private Ghost ghost;
    private Vector3 dropPoint;
    private bool isDelaying;
    private int delayCount;

    public void Initialize()
    {
        levelSpeed = defaultSpeed;
        speed = levelSpeed;
        score = 0;
        level = 1;
        deletedLines = 0;

        field.Clear();
    }

    protected virtual void OnFalled()
    {
        Falled?.Invoke(this, EventArgs.Empty);
    }

    private void Awake()
    {
        transform.position = startPosition;

        field = GetComponent<Field>();

        mover = GetComponent<Mover>();
        mover.Setup(field.RawField);
        
        delayCount = delayLimit;
        isDelaying = false;

        ghost = Instantiate(ghostPrefab);
        ghost.Setup(field.RawField);
        dropPoint = ghost.Move(transform.position);

        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        CheckInput();
        mover.Fall(speed);

        if (!mover.CanMove())
        {
            isDelaying = true;
            delayCount--;

            mover.AdjustToAbove();

            if (delayCount < 0)
            {
                enabled = false;

                if (transform.position.y >= startPosition.y)
                {
                    gameManager.Over();
                    return;
                }

                field.Fix();
                int cnt = field.DeleteLines();

                if (cnt > 0) UpdatePlayData(cnt);

                Destroy(ghost.gameObject);

                if (level >= maxLevel)
                {
                    gameManager.Clear();
                    return;
                }

                OnFalled();
            }
        }
        else
        {
            isDelaying = false;
        }
    }

    private void CheckInput()
    {
        KeyInputReceiver.Update();

        if (KeyInputReceiver.GetKeyLongDown(KeyCode.LeftArrow))
        {
            mover.MoveLeft();
            dropPoint = ghost.Move(transform.position);
        }
        else if (KeyInputReceiver.GetKeyLongDown(KeyCode.RightArrow))
        {
            mover.MoveRight();
            dropPoint = ghost.Move(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isDelaying) delayCount = -1;
            else speed = maxSpeed;

            dropPoint = ghost.Move(transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            speed = levelSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isDelaying) transform.position = dropPoint;

            delayCount = -1;
            dropPoint = ghost.Move(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            mover.TurnLeft(RotatePosition);
            dropPoint = ghost.TurnLeft(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            mover.TurnRight(RotatePosition);
            dropPoint = ghost.TurnRight(transform.position);
        }
    }

    private void UpdatePlayData(int cnt)
    {
        int[] rate = { 0, 40, 100, 300, 1200 };
        deletedLines += cnt;
        score += rate[cnt];

        level = Mathf.Min(maxLevel, deletedLines / nextLine + 1);
        levelSpeed = defaultSpeed * level;
        speed = levelSpeed;

        gameManager.RenderPlayData(score, level, deletedLines);
    }
}
