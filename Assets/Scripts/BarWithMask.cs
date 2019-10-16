using UnityEngine;

public class BarWithMask : MonoBehaviour {

	public RectTransform visualBar;
	public Vector3 _zeroPos;
	Vector3 fullPos = Vector3.zero;

	private float progress = 0;
    private float lastTime;
    private float Progress {
		get {
			return progress;
		}
		set {
			if (value > 0 && value < 100) {
				SetBarProgress (value);
            } else {
				if (value >= 100) {
					SetBarProgress (100);
                }
				if (value <= 0) {
					SetBarProgress (0);
				}
			}
		}
	}

    public void SetBarProgress (float progress) {
		visualBar.localPosition = Vector3.Lerp (_zeroPos, fullPos, progress);
		this.progress = progress;
	}
}
