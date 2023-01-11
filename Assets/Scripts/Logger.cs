using System;
using System.IO;
using UnityEngine;

public enum LogId {Heatmap, SpeedPercentage, ZoneTakenTime, ItemUsage, PlayerAheadTime, PlayerHit, FieldTypes, Other}
public class Logger : MonoBehaviour
{
    public static Logger Instance;
    private float time = 0;
    private string startTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        startTime = DateTime.Now.ToString("T").Replace(":", ".");
    }

    private void Update()
    {
        if(GameManager.Instance.GameStarted)
            time += Time.deltaTime;
    }

    public void WriteToFile(LogId id, string message)
    {
        try
        {
            if(Directory.Exists(Application.persistentDataPath + "/" + DateTime.Today.ToString("d")) == false)
                Directory.CreateDirectory(Application.persistentDataPath + "/" + DateTime.Today.ToString("d"));
            
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + DateTime.Today.ToString("d") + "/Balance-logs " + startTime + ".txt", true);
            writer.WriteLine(time + "," + id + "," + message);
            writer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
