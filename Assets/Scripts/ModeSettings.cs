using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModeSettings {

    // Mode types
    public enum Modes { PRACTICE, FFA, VERSUS};

    // Settings
    public static Modes modeType;
    public static int numLives = 5; // Default
    public static int numPlayers;
}