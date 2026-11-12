using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static int easyDifficultyHighScore = 0;
    private static int mediumDifficultyHighScore = 0;
    private static int hardDifficultyHighScore = 0;

    // DEDICATED FILE PATH TO SAVE AND LOAD FROM.
    private static readonly string saveFilePath = Application.persistentDataPath + "/savedGame.txt";

    public static int GetCurrentHighScore()
    {
        return DifficultySettings.difficulty switch
        {
            0 => easyDifficultyHighScore,
            1 => mediumDifficultyHighScore,
            2 => hardDifficultyHighScore,
            _ => 0
        };
    }

    public static void SetNewHighScore(int newScore, int difficultyMode)
    {
        switch (difficultyMode)
        {
            case 0:
                easyDifficultyHighScore = newScore;
                break;
            case 1:
                mediumDifficultyHighScore = newScore;
                break;
            case 2:
                hardDifficultyHighScore = newScore;
                break;
        }

        SaveGame();
    }

    public static void GetAllHighScores(ref int easyDifficulty, ref int mediumDifficulty, ref int hardDifficulty)
    {
        easyDifficulty = easyDifficultyHighScore;
        mediumDifficulty = mediumDifficultyHighScore;
        hardDifficulty = hardDifficultyHighScore;
    }

    // TAKES THE DIFFICULTIES AND HIGH SCORES AND PUTS THEM ALL INTO A SIMPLE TEXT FILE.
    public static void SaveGame()
    {
        string saveData = $"0; {easyDifficultyHighScore}, 1; {mediumDifficultyHighScore}, 2; {hardDifficultyHighScore}";
        File.WriteAllText(saveFilePath, saveData);
    }

    public static void OpenSavedGameFileLocation()
    {
        // OPENS THE FILE THAT THE SAVE FILE IS LOCATED IN.
        if (!File.Exists(saveFilePath))
        {
            SaveGame();
        }
        else
        {
            Application.OpenURL(Application.persistentDataPath);
        }
    }

    public static void LoadSavedGame()
    {
        // IF THERE IS A FILE,
        if (File.Exists(saveFilePath))
        {
            // WE TAKE THE THREE SCORES,
            string content = File.ReadAllText(saveFilePath);
            // SPLIT THEM APART,
            string[] parts = content.Split(',');

            foreach (string part in parts)
            {
                // THEN SPLIT THE DIFFICULTY FROM THE SCORE,
                string[] pair = part.Split(';');

                // IF THE PAIR ONLY HAS THE DIFFICULTY AND THE SCORE,
                if (pair.Length == 2)
                {
                    // THEN WE CLEAN UP THE DIFFICULTY AND THE SCORES TEXT SO WE CAN EASILY TAKE THE NUMBER THAT REMAINS AND TURN IT INTO AN INT.
                    int difficulty = int.Parse(pair[0].Trim());
                    int score = int.Parse(pair[1].Trim());

                    // THEN WE ASSIGN THE CORRESPONDING DIFFICULTY WITH THE CORRECT SCORE.
                    switch (difficulty)
                    {
                        case 0:
                            easyDifficultyHighScore = score;
                            break;
                        case 1:
                            mediumDifficultyHighScore = score;
                            break;
                        case 2:
                            hardDifficultyHighScore = score;
                            break;
                    }
                }
            }
        }
    }
}
