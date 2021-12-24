using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFXPool : MonoBehaviour{
    public ParticleSystem test;
    public ParticleSystem[] particles;

    public void Spawn(Vector2 position) {
        for (int i = 0; i < particles.Length; i++) {
            ParticleSystem particle = particles[i];
            if (!particle.isEmitting) {
                particle.transform.position = position;
                particle.Emit(1);
            }
        }
    }
}