using UnityEngine;
using UnityEngine.UI;

public class DisplayPointsAtScreenPosition : MonoBehaviour {

    public RectTransform canvas;
    public Text UITextObject;

    public void DisplayPoints(string text)
    {
        Text t = Instantiate(UITextObject);
        t.transform.SetParent(canvas.transform, false);
        t.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        t.text = text;
        Destroy(t, 1.0f);
    }
}
