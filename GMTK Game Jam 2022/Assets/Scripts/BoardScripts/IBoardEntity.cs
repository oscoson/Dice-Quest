using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Player, Enemy, Loot, Exit };

public interface IBoardEntity 
{
    public EntityType GetEntityType();
}
