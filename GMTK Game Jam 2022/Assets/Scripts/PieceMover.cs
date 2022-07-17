using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceMover : MonoBehaviour
{
    [SerializeField] GameBoardPiece piece;
    [SerializeField] GameBoard board;
    [SerializeField] GameObject dustEffect;
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
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                MoveDir(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                MoveDir(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveDir(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
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
            GameBoardPiece entity = board.GetEntity(nextPosition);
            if (entity == null)
            {
                Instantiate(dustEffect, board.CellToWorld(piece.BoardPosition), Quaternion.identity);

                if (direction == Vector2Int.left)
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
                        Debug.Log(entity.EnemyID);
                        StartCombat(entity.EnemyID);
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
