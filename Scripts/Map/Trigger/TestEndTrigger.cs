using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class TestEndTrigger : MonoBehaviour
{
    public RecordPathCSV recordPathCsv;
    public StarterAssetsInputs starterAssetsInputs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            recordPathCsv.EndTest();
            TaskManager.LoadToSurveyScene();
            starterAssetsInputs.SetCursorState(false);
        }
    }
}
