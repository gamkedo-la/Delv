using UnityEngine;
using UnityEngine.UI;

public static class UxUtil {
    public static Canvas GetCanvas() {
        // canvas should always be tagged
        var canvasGo = GameObject.FindWithTag("canvas");
        if (canvasGo != null) {
            return canvasGo.GetComponent<Canvas>();
        }
        return null;
    }
}
