using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisiblityController : MonoBehaviour
{
    public bool alsoChild = false;

    void Start()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.enabled = false;
        if(alsoChild)
            SetChildrenTransparency(transform);
    }

    void SetChildrenTransparency(Transform parent)
    {
        // 부모의 자식들을 반복하며 처리
        foreach (Transform child in parent)
        {
            // Renderer 컴포넌트가 있는지 확인
            Renderer child_renderer = child.GetComponent<Renderer>();
            
            if (child_renderer != null)
            {
                child_renderer.enabled = false;
            }

            // 자식의 자식들도 처리 (재귀 호출)
            SetChildrenTransparency(child);
        }
    }
}
