using UnityEngine;
using UnityEngine.InputSystem;

public class Demo_Demo : MonoBehaviour
{
    void Update()
    {
        // Keyboard.current が null でないかチェック（念のため）
        if (Keyboard.current != null)
        {
            // wasPressedThisFrame が旧 GetKeyDown に相当します
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("スペースキーが押された（新システム方式）");
            }
        }
    }
}
