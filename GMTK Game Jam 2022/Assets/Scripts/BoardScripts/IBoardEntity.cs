using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Player, Enemy, Boss, Loot, Exit, FinalExit };

public interface IBoardEntity 
{
    public EntityType GetEntityType();
}
