using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

[Serializable()]
public class RecordPathCSV : MonoBehaviour
{
    // 디렉토리 저장 경로
    static string path;
    private static string folderName = "HCI_study_Project2";

    public int _participantsNumber;
    public static int participantsNumber;
    public int trialNum = 0;

    public Transform _player;
    //public Rigidbody2D rb;

    //public float endTime = 9999.0f;

    //static CsvFileWriter jumpPositionCSV;
    //static List<string> jumpPositionColumns;

    static CsvFileWriter playerMovementCSV;
    static List<string> playerMovementColumns;   
    
    public static bool isEnded = true;

    public static float currentTime = 0f;
    
    // PATH DRAWER
    private static List<Vector3> pathPoints = new List<Vector3>(); // 그림 그릴 점들

    //
    public TMP_Text timerText;
    
    // master file data
    static CsvFileWriter masterCSV;
    static List<string> masterColumns;   
    
    //public string participantName = "default";
    
    public float[] clearTime = new float[5];
    public int clearNum = 1; 

    
    public void CreateCSVFile()
    {
        currentTime = 0;
        
        participantsNumber = _participantsNumber;

        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
               "/" + folderName + "/" + participantsNumber + "번 피험자/";

        // 폴더 유무 확인
        DirectoryInfo di = new DirectoryInfo(path);

        while (di.Exists)    // ex) trial 1 폴더가 이미 있으면 trial 2 폴더를 생성하게끔 설정 (1, 2 존재 -> 3 생성)
        {
            ++trialNum;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
                   "/" + folderName + "/" + participantsNumber + "번 피험자/" + trialNum + "/";
            di = new DirectoryInfo(path);
        }

        // 폴더가 없으면 폴더 생성
        if (!di.Exists)
            di.Create();

        // jumpPositionCSV = new CsvFileWriter(path + "JumpPosition.csv");
        // jumpPositionColumns = new List<string>() { "CurrentTime", "JumpPosition_x", "JumpPosition_y","Scale_X", "Time from drop to jump", "isSuccess" };
        playerMovementCSV = new CsvFileWriter(path + "PlayerMovement"+TaskManager.currentAB +".csv");
        playerMovementColumns = new List<string>() { "CurrentTime", "Position_x", "Position_y", "Scale_X" };
        
        masterCSV = new CsvFileWriter(path + "master"+TaskManager.currentAB +".csv");
        masterColumns = new List<string>() { "section 1", "section 2", "section 3", "section 4", "total time", "average Time" };
        
        // jumpPositionCSV.WriteRow(jumpPositionColumns);
        masterCSV.WriteRow(masterColumns);
        playerMovementCSV.WriteRow(playerMovementColumns);
    }

    // 급조
    // public void writeDeath()
    // {
    //     playerMovementColumns.Clear();
    //     playerMovementColumns.Add("Death Count:");
    //     playerMovementColumns.Add(playerMovementScript.death.ToString());
    //     playerMovementCSV.WriteRow(playerMovementColumns);
    // }
    
    
    public void CloseCSVFile()
    {
        //writeDeath();
        
        //jumpPositionCSV.Dispose();
        playerMovementCSV?.Dispose();
        masterCSV?.Dispose();
    }

    // isEnded가 false면 recording.
    private void Start()
    {
        currentTime = 0;
        clearTime[0] = 0;
        
        participantsNumber = _participantsNumber;

        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
               "/" + folderName + "/" + participantsNumber + "번 피험자/";

        // 폴더 유무 확인
        DirectoryInfo di = new DirectoryInfo(path);

        while (di.Exists)    // ex) trial 1 폴더가 이미 있으면 trial 2 폴더를 생성하게끔 설정 (1, 2 존재 -> 3 생성)
        {
            ++trialNum;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
                   "/" + folderName + "/" + participantsNumber + "번 피험자/" + trialNum + "/";
            di = new DirectoryInfo(path);
        }

        // 폴더가 없으면 폴더 생성
        if (!di.Exists)
            di.Create();
        
        // 시작하면 기록하게.
        CreateCSVFile();
    }

    private void Update()
    {
        if (!isEnded)
        {
            Check_Timer();
            WritingPlayerMovementData();

            timerText.text = "Timer:" + currentTime.ToString("F3") + "s";
        }
    }
    
    public void WritingMasterData()
    {
        // "이름", "구간 1", "구간 2", "구간 3", "구간 4", "총 시간", "평균 시간"
        float clearTimeAvg = 0;
            
        masterColumns.Clear();
     
        // 이름
        //masterColumns.Add(participantName);
        // 구간 시간
        
        for (int i = 1; i < 5; i++)
        {
            masterColumns.Add(clearTime[i].ToString());
            clearTimeAvg += clearTime[i];
        }
        clearTimeAvg /= 4;
        // 총 시간
        masterColumns.Add(currentTime.ToString());
        // 평균 시간
        masterColumns.Add(clearTimeAvg.ToString());

        masterCSV.WriteRow(masterColumns);

        // 파일 닫기
        masterCSV.Dispose();
    }

    public void StartTest()
    {
        Start_Timer();
        CreateCSVFile();
    }

    public void EndTest()
    {
        End_Timer();
        CloseCSVFile();
    }

    // Timer 설정
    private void Check_Timer()
    {
        currentTime += Time.deltaTime;
    }

    public void Start_Timer()
    {
        isEnded = false;
        Debug.Log("Start");
    }

    public void End_Timer()
    {
        if (!isEnded)
        {
            clearTime[clearNum] = currentTime - clearTime[clearNum-1];
            clearNum++;
            Debug.Log("End. Time is " + currentTime);
            isEnded = true;
        }
    }

    void WritingPlayerMovementData()
    {
        // Player의 현재 위치 기록
        Vector3 playerPosition = _player.position;

        playerMovementColumns.Clear();
        playerMovementColumns.Add(currentTime.ToString());
        playerMovementColumns.Add(playerPosition.x.ToString());
        playerMovementColumns.Add(playerPosition.z.ToString());

        if (Input.GetKey(KeyCode.W))
        {
            playerMovementColumns.Add("o");
        }
        else
        {
            playerMovementColumns.Add("");
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerMovementColumns.Add("o");
        }
        else
        {
            playerMovementColumns.Add("");
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerMovementColumns.Add("o");
        }
        else
        {
            playerMovementColumns.Add("");
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerMovementColumns.Add("o");
        }
        else
        {
            playerMovementColumns.Add("");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovementColumns.Add("Running");
        }
        else
        {
            playerMovementColumns.Add("");
        }

        playerMovementCSV.WriteRow(playerMovementColumns);

        // 경로 그리기를 위해 현재 위치 기록
        pathPoints.Add(new Vector2(playerPosition.x, playerPosition.y));
    }
    
    private void OnApplicationQuit()
    {
        // 어플리케이션이 종료될 때 CSV 파일을 닫음
        // 중복!?
        //jumpPositionCSV?.Dispose();
        playerMovementCSV?.Dispose();
    }

    public void OpenFolder()
    {
        // 결과 폴더를 열기 위해 Explorer를 실행
        System.Diagnostics.Process.Start(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/" + folderName + "/" + participantsNumber + "번 피험자/" + trialNum + "/");
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}