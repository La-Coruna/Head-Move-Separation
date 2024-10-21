using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStartTrigger : MonoBehaviour
{
    public RecordPathCSV recordPathCsv;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            recordPathCsv.Start_Timer();
        }
    }
}
