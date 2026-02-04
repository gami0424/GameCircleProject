using UnityEngine;
using UnityEngine.InputSystem;

public class Demo_Demo : MonoBehaviour
{
    void Update()
    {
        // Keyboard.current が null でないかチェック（念のため）
        if (Keyboard.current != null)
        {
            Keyboard keyborad = Keyboard.current;
            // wasPressedThisFrame が旧 GetKeyDown に相当します
            if (keyborad.wKey.wasPressedThisFrame)
            {
                Debug.Log("Wキーが押された（新システム方式）");
            }

        }
    }
}
