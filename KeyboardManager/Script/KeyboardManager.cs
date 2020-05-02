using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INIParse;

[System.Serializable]
public struct KeySetting
{
    string key_name;
    KeyCode code;
}

public class KeyboardManager : MonoBehaviour
{
    public bool editable = false;
    public GameObject item_object;
    public KeySetting[] settings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
