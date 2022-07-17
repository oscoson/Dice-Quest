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

    public GameBoardPiece GetEntity(Vector2Int position)
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

    public void AddBoardEntity(Vector2Int pos, GameBoardPiece entity)
    {
        boardMap[pos].AddEntity(entity);
    }

    public void Reveal(Vector2Int pos)
    {
        for (int row = -5; row <= 5; row++)
        {
            for (int col = -5; col <= 5; col++)
            {
                var offset = new Vector2Int(col, row);
                if (!HasTile(pos + offset)) continue;
                var tile = boardMap[pos + offset];
                int manhatDist = Mathf.Abs(offset.x) + Mathf.Abs(offset.y);
                tile.ChangeBrightness(Mathf.Clamp(1.2f - 0.2f * manhatDist, 0.0f, 1.0f));
            }
        }
    }

    public void RemoveBoardEntity(Vector2Int pos)
    {
        boardMap[pos].RemoveEntity();
    }

    public void DestroyBoardEntity(Vector2Int pos)
    {
        if(boardMap[pos] != null)
        {
            boardMap[pos].DestroyEntity();
        }
    }
}

