using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Player, Enemy, Loot };

public interface IBoardEntity 
{
    public EntityType GetEntityType();
}
