using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModeSettings {

    // Mode types
    public enum Modes { PRACTICE, FFA, VERSUS};

    // Settings
    public static Modes modeType;
    public static int numLives;
    public static int numPlayers;
}