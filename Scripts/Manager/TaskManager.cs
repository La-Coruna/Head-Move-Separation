using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    //static public string name;
    static public bool isBA = false;
    static public int trial = 0;
    static public string currentAB = "X";

    static private string linkA = "https://forms.gle/yuS4Gs1TgtymwApX6";
    static private string linkB = "https://forms.gle/z79emZtAfYR36jRj7";
    static private string linkAB = "https://forms.gle/s6tKHViNY397cs8b9";

    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static TaskManager _instance;

    // 인스턴스에 접근하기 위한 프로퍼티
    public static TaskManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(TaskManager)) as TaskManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleIsBA()
    {
        isBA = !isBA;
    }

    static public void NextScene()
    {
        if (trial == 0)
        {
            if (isBA)
                LoadTaskPractice_B();
            else
                LoadTaskPractice_A();

            trial++;
        }
        else if (trial == 1)
        {
            if (isBA)
                LoadTaskB();
            else
                LoadTaskA();
            trial++;
        }
        else if (trial == 2)
        {
            if (isBA)
                LoadTaskPractice_A();
            else
                LoadTaskPractice_B();

            trial++;
        }
        else if (trial == 3)
        {
            if (isBA)
                LoadTaskA();
            else
                LoadTaskB();
            trial++;
        }
        else if (trial == 4)
        {
            Application.OpenURL(linkAB);
            SceneManager.LoadScene("Task_End");
        }
    }


    static public void LoadTaskA()
    {
        SceneManager.LoadScene("TaskA");
        currentAB = "A";
    }

    static public void LoadTaskB()
    {
        SceneManager.LoadScene("TaskB");
        currentAB = "B";
    }

    static public void LoadTaskPractice_A()
    {
        SceneManager.LoadScene("Task_Practice_A");
        currentAB = "XA";
    }

    static public void LoadTaskPractice_B()
    {
        SceneManager.LoadScene("Task_Practice_B");
        currentAB = "XB";
    }

    static public void EndOneTask()
    {
        if (currentAB == "XA" || currentAB == "XB")
            NextScene();
        else
            LoadToSurveyScene();
    }
    static public void LoadToSurveyScene()
    {
        SceneManager.LoadScene("Task_Survey");
    }


    static public void GoToSurvey()
    {
        if (currentAB == "A")
            Application.OpenURL(linkA);
        else if (currentAB == "B")
            Application.OpenURL(linkB);
    }

    static public void EndGame()
    {
        Application.Quit();
    }
}