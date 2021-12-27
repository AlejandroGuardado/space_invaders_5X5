using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFXPool : MonoBehaviour{
    public ParticleSystem[] particles;
    public int numberParticlePerEmit;

    public void Spawn(Vector2 position) {
        for (int i = 0; i < particles.Length; i++) {
            ParticleSystem particle = particles[i];
            if (!particle.isPlaying) {
                particle.transform.position = position;
                particle.Emit(numberParticlePerEmit);
                break;
            }
        }
    }
}