using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceMover : MonoBehaviour
{
    [SerializeField] GameBoardPiece piece;
    [SerializeField] GameBoard board;
    // Start is called before the first frame update

    Vector2Int nextPosition;
    bool inCombat = false;
    void Start()
    {
        CombatManager.Instance.OnCombatEnd += ReturnToOverworld;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveDir(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveDir(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveDir(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDir(Vector2Int.down);
            }
        }
    }

    public void MoveDir(Vector2Int direction)
    {
        Debug.Assert(Mathf.Abs(direction.x) + Mathf.Abs(direction.y) == 1);
        nextPosition = piece.BoardPosition + direction;
        if (board.HasTile(nextPosition))
        {
            IBoardEntity entity = board.GetEntity(nextPosition);
            if (entity == null)
            {
                if(direction == Vector2Int.left)
                {
                    piece.MoveLeft();
                } else if(direction == Vector2Int.right)
                {
                    piece.MoveRight();
                } else if(direction == Vector2Int.up)
                {
                    piece.MoveUp();
                } else if(direction == Vector2Int.down)
                {
                    piece.MoveDown();
                }
                board.Reveal(piece.BoardPosition);
            }
            else
            {
                switch (entity.GetEntityType())
                {
                    case EntityType.Enemy:
                        GameBoardPiece enemy = entity as GameBoardPiece;

                        StartCombat(enemy.EnemyID);
                        break;
                    case EntityType.Exit:
                        ExitLevel();
                        break;
                }
            }
        }
    }

    public void StartCombat(int index)
    {

        inCombat = true;
        CombatManager.Instance.StartCombat(index);
    }

    public void ReturnToOverworld()
    {

        inCombat = false;
        board.DestroyBoardEntity(nextPosition);
    }

    public void ExitLevel()
    {
        CombatManager.Instance.OnCombatEnd -= ReturnToOverworld;

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        buildIndex++;
        buildIndex %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(buildIndex);
    }
}
