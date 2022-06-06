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

        public HiScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

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

    [System.Serializable]
    class SaveData
    {
        public List<HiScore> hiScoreList;
    }

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

    public string GetBestScore()
    {
        if (hiScoreList.Count == 0) return "No HighScore";

        return "High Score : " + hiScoreList[0].playerName + " : " + hiScoreList[0].score;
    }

    public int GetBestScoreInt()
    {
        if (hiScoreList.Count == 0) return 0;

        return hiScoreList[0].score;
    }

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


    public void SaveHiScores()
    {
        SaveData data = new SaveData();
        data.hiScoreList = hiScoreList;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/hiScoreSavefile.json", json);
    }

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
