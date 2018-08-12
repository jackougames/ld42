using UnityEngine;

public class Stars : MonoBehaviour {

    public Color colorMin;
    public Color colorMax;

    private SpriteRenderer[] stars;

    private void Awake() {
        stars = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start() {
        foreach(SpriteRenderer star in stars) {
            float h, s, v, h1, s1, v1;
            Color.RGBToHSV(colorMin, out h, out s, out v);
            Color.RGBToHSV(colorMax, out h1, out s1, out v1);

            star.sortingLayerName = "BG";
            star.color = Random.ColorHSV(h, h1, s, s1, v, v1);
        }
    }

}
