using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardPiece : MonoBehaviour, IBoardEntity
{
    [SerializeField] GameBoard board;
    [SerializeField] EntityType entityType;
    bool isMoving = false;
    private Vector2Int boardPosition;
    [SerializeField] int enemyID;
    public int EnemyID { get => enemyID; }
    public Vector2Int BoardPosition { get => boardPosition; }

    // Start is called before the first frame update
    void Start()
    {
        boardPosition = board.WorldToCell(transform.position);
        transform.position = board.CellToWorld(boardPosition);
        board.AddBoardEntity(boardPosition, this);
        if(entityType == EntityType.Player)board.Reveal(boardPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EntityType GetEntityType()
    {
        return entityType;
    }

    public void MoveLeft()
    {
        board.RemoveBoardEntity(boardPosition);
        boardPosition.x--;
        transform.position = board.CellToWorld(boardPosition);
        board.AddBoardEntity(boardPosition, this);
    }

    public void MoveRight()
    {
        board.RemoveBoardEntity(boardPosition);
        boardPosition.x++;
        transform.position = board.CellToWorld(boardPosition);
        board.AddBoardEntity(boardPosition, this);
    }

    public void MoveUp()
    {
        board.RemoveBoardEntity(boardPosition);
        boardPosition.y++;
        transform.position = board.CellToWorld(boardPosition);
        board.AddBoardEntity(boardPosition, this);
    }

    public void MoveDown()
    {
        board.RemoveBoardEntity(boardPosition);
        boardPosition.y--;
        transform.position = board.CellToWorld(boardPosition);
        board.AddBoardEntity(boardPosition, this);
    }
}
