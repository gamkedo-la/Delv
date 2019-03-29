using UnityEngine;

public enum ControllerKind {
    Keyboard,
    XInput,
    DualShock,
    AI
}

// GameConfig is a scriptable object to store config settings.
[CreateAssetMenu(fileName = "gameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject {
    public bool soloPlay = true;
    public bool enableScreenShake = true;
    public bool showDamageNumbers = true;
    public int maxParticles = 4;
    public ControllerKind p1ControllerKind = ControllerKind.Keyboard;
    public ControllerKind p2ControllerKind = ControllerKind.AI;
    public float masterVolume = .5f;
    public float sfxVolume = .5f;
    public float musicVolume = .5f;
}
