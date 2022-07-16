using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    [SerializeField] GameObject boardTile;
    Dictionary<Vector2Int, GameBoardTile> boardMap;
    Tilemap tilemap;


    private void Awake() {
        boardMap = new Dictionary<Vector2Int, GameBoardTile>();
        tilemap = GetComponent<Tilemap>();

        foreach (var bound in tilemap.cellBounds.allPositionsWithin)
        {
            tilemap.GetComponent<TilemapRenderer>().enabled = false;
            if (tilemap.HasTile(bound))
            {
                GameObject tileObj = Instantiate(boardTile, CellToWorld((Vector2Int) bound), Quaternion.identity);
                tileObj.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(bound);
                tileObj.transform.rotation = tilemap.GetTransformMatrix(bound).rotation;
                boardMap[(Vector2Int)bound] = tileObj.GetComponent<GameBoardTile>();
            }
        }
    }

    public bool IsEmpty(Vector2Int position)
    {
        if (!HasTile(position)) return false;
        return boardMap[position].Entity?.GetEntityType() == null;
    }

    public bool HasTile(Vector2Int position)
    {
        return boardMap.ContainsKey(position);
    }

    public IBoardEntity GetEntity(Vector2Int position)
    {
        return boardMap[position].Entity;
    }

    public Vector2Int WorldToCell(Vector2 position)
    {
        return (Vector2Int)tilemap.WorldToCell((Vector3)position);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        if (boardMap.ContainsKey(cell)) return boardMap[cell].transform.position;
        return (Vector2)tilemap.CellToWorld((Vector3Int)cell) + (Vector2)tilemap.cellSize / 2;
    }

    public void AddBoardEntity(Vector2Int pos, IBoardEntity entity)
    {
        boardMap[pos].AddEntity(entity);
    }

    public void RemoveBoardEntity(Vector2Int pos)
    {
        boardMap[pos].RemoveEntity();
    }
}

