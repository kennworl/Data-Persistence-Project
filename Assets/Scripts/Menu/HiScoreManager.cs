using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HiScoreManager : MonoBehaviour
{
    public static HiScoreManager Instance;

    public string playerName;

    [System.Serializable]
    public struct HiScore
    {
        public int score;
        public string playerName;

        // struct for the High score entry
        public HiScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    // method for the list sort
    private static int CompareHiScores(HiScore x, HiScore y)
    {
        if (x.score == y.score)
            return 0;
        else
        {
            if (x.score > y.score)
                return -1;
            else
                return 1;
        }
    }

    public List<HiScore> hiScoreList;
    private int highestScore;
    private string highestScorePlayerName;

    // Singleton handler
    private void Awake()
    {
        //if we already have a singleton instance then 
        if (Instance != null)
        {
            //destroy this instance and return
            Destroy(gameObject);
            return;
        }

        //Otherwise if we don't have any instances, create one and set to Don't Destroy temp scene.
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHiScores();
    }

    // the json serialisable list object
    [System.Serializable]
    class SaveData
    {
        public List<HiScore> hiScoreList;
    }

    // Add in a new HiScore and then sort the list in descending order, and trim the list to 10 entries
    public void AddHiScore(int pScore, string pName)
    {
        HiScore hiScore = new HiScore(pScore, pName);
        hiScoreList.Add(hiScore);
        hiScoreList.Sort(CompareHiScores);
        if (hiScoreList.Count > 10)
        {
            hiScoreList.RemoveRange(10, hiScoreList.Count - 10);
        }
    }

    // returns the top element of the high scores (or a string saying there are no high scores)
    public string GetBestScore()
    {
        if (hiScoreList.Count == 0) return "No HighScore";

        return "High Score : " + hiScoreList[0].playerName + " : " + hiScoreList[0].score;
    }

    // Getter for the highest score as an integer
    public int GetBestScoreInt()
    {
        if (hiScoreList.Count == 0) return 0;

        return hiScoreList[0].score;
    }

    // create a big text string with CRs for up to 10 high score entries.
    public string GetAllHiScores()
    {
        if (hiScoreList.Count == 0) return "No HighScores";

        string scores = "";

        for (int i = 0; i < hiScoreList.Count; i++)
        {
            scores += "High Score : " + hiScoreList[i].playerName + " : " + hiScoreList[i].score + "\n";
        }

        return scores;
    }

    // Save HiScores from memory into a json file
    public void SaveHiScores()
    {
        SaveData data = new SaveData();
        data.hiScoreList = hiScoreList;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/hiScoreSavefile.json", json);
    }

    // Load HiScores into memory from the json file or create a new list if that doesn't exist.
    public void LoadHiScores()
    {
        string path = Application.persistentDataPath + "/hiScoreSavefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            hiScoreList = data.hiScoreList;
        }

        if (hiScoreList == null)
            hiScoreList = new List<HiScore>();
    }
}
