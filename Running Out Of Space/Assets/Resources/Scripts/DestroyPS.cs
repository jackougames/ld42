using UnityEngine;

public class DestroyPS : MonoBehaviour {
    
    private ParticleSystem ps;

    private void Awake() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (ps) {
            if (!ps.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}
