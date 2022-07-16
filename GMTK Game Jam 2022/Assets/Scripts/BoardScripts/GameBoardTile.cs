using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardTile : MonoBehaviour
{
    public IBoardEntity Entity { get; protected set; }

    public void AddEntity(IBoardEntity entity)
    {
        Debug.Assert(Entity == null, $"Can't add entity to occupied tile!");
        this.Entity = entity;
    }

    public IBoardEntity RemoveEntity()
    {
        Debug.Assert(Entity != null, $"No entity in tile!");
        IBoardEntity e = Entity;
        Entity = null;
        return e;
    }

    public bool IsOccupied()
    {
        return Entity != null;
    }
}