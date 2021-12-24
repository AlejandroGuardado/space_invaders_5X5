using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : GameEntity{
    public List<T> entities;

    public Pool(List<T> entities) {
        this.entities = entities;
    }

    public T Spawn(Vector2 position) {
        T entity = null;
        for (int i = 0; i < entities.Count; i++) {
            entity = entities[i];
            if (!entity.Active) {
                entity.Spawn(position);
                break;
            }
            //If not set to null, last entity would be returned even if it didn't spawn
            entity = null;
        }
        return entity;
    }

    public void Clear() {
        for (int i = 0; i < entities.Count; i++) {
            entities[i].Deactivate();
        }
    }
}