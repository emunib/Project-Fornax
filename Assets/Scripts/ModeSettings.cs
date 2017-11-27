using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModeSettings {

    // Mode types
    public enum Modes { PRACTICE, FFA, VERSUS};

    // Settings
    public static Modes modeType = ModeSettings.Modes.FFA; // Default
    public static int numLives = 3; // Default
    public static int numPlayers;
}