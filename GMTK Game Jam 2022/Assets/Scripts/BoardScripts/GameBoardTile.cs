using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardTile : MonoBehaviour
{
    private float Brightness;

    public GameBoardPiece Entity { get; protected set; }

    Color oldColor;

    private void Start()
    {
        ChangeBrightness(0.0f);
        UpdateColor();
    }

    public void AddEntity(GameBoardPiece entity)
    {
        Debug.Assert(Entity == null, $"Can't add entity to occupied tile!");
        this.Entity = entity;
        Entity.transform.SetParent(transform);

        Color c = entity.GetComponent<SpriteRenderer>().color;
        oldColor = c;

        UpdateColor();
    }

    private void UpdateColor()
    {
        Color tilec = GetComponent<SpriteRenderer>().color;
        tilec.a = Brightness;
        GetComponent<SpriteRenderer>().color = tilec;

        if(Entity!= null)
        {
            Color c = Entity.GetComponent<SpriteRenderer>().color;
            c.a = Brightness;
            Entity.GetComponent<SpriteRenderer>().color = c;
        }
    }

    public void ChangeBrightness(float b)
    {
        Brightness = Mathf.Max(Brightness, Mathf.Min(1.0f, b));
        UpdateColor();
    }

    public GameBoardPiece RemoveEntity()
    {
        Debug.Assert(Entity != null, $"No entity in tile!");
        transform.DetachChildren();
        GameBoardPiece e = Entity;
        e.GetComponent<SpriteRenderer>().color = oldColor;
        Entity = null;
        return e;
    }

    public void DestroyEntity()
    {
        Debug.Log("destroy");
        Destroy(RemoveEntity().gameObject);
    }

    public bool IsOccupied()
    {
        return Entity != null;
    }
}
