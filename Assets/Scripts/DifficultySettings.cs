public static class DifficultySettings
{
    // 0 == Easy, 1 == Medium, 2 == Hard
    public static int difficulty = 0;

    // EASY MODE SETTINGS
    private static readonly int easyModeLives = 10;
    private static readonly float easyModeGuessTime = 12f;
    private static readonly float easyModeGuessTimeLossPerFail = 1.15f;
    private static readonly int easyModeMaximumGuessValue = 13;

    // MEDIUM MODE SETTINGS
    private static readonly int mediumModeLives = 5;
    private static readonly float mediumModeGuessTime = 8f;
    private static readonly float mediumModeGuessTimeLossPerFail = 1.55f;
    private static readonly int mediumModeMaximumGuessValue = 26;

    // HARD MODE SETTINGS
    private static readonly int hardModeLives = 3;
    private static readonly float hardModeGuessTime = 4f;
    private static readonly float hardModeGuessTimeLossPerFail = 1.28f;
    private static readonly int hardModeMaximumGuessValue = 52;

    public static void GetDifficultyValues(ref int lives, ref float guessTime, ref float guessLossPerFail, ref int maxGuessValue)
    {
        switch (difficulty)
        {
            case 0:
                lives = easyModeLives;
                guessTime = easyModeGuessTime;
                guessLossPerFail = easyModeGuessTimeLossPerFail;
                maxGuessValue = easyModeMaximumGuessValue;
                break;
            case 1:
                lives = mediumModeLives;
                guessTime = mediumModeGuessTime;
                guessLossPerFail = mediumModeGuessTimeLossPerFail;
                maxGuessValue = mediumModeMaximumGuessValue;
                break;
            case 2:
                lives = hardModeLives;
                guessTime = hardModeGuessTime;
                guessLossPerFail = hardModeGuessTimeLossPerFail;
                maxGuessValue = hardModeMaximumGuessValue;
                break;
        }
    }
}
