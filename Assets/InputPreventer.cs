using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPreventer : MonoBehaviour
{
    void Update()
    {
        // 모든 입력을 무시하려면 아래와 같이 사용합니다.
        Input.ResetInputAxes();
    }
}
