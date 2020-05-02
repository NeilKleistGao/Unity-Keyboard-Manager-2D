using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardReciever : MonoBehaviour
{
    private string key_name, key_value, last_value = "";
    private Text self;
    private bool editable = false;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (last_value != "")
            {
                setKey(last_value);
                last_value = "";
            }

            checkClick(Input.mousePosition);
        }

        if (editable)
        {
            KeyCode code = fetchKeyCode();
            if (code != KeyCode.None)
            {
                last_value = "";
                setKey(code.ToString());
                editable = false;
            }
        }
    }

    public void setKeys(string name, string value)
    {
        key_name = name;
        key_value = value;
        flush();
    }

    public void setKey(string value)
    {
        key_value = value;
        flush();
    }

    private void flush()
    {
        self.text = key_name + ": " + key_value;
    }

    private void checkClick(Vector2 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == this.gameObject)
            {
                Debug.Log(this.gameObject.name);
                editable = true;
                last_value = key_value;
                setKey("");
                return;
            }
        }

        editable = false;
    }

    private KeyCode fetchKeyCode()
    {
        int length = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < Mathf.Min(length, 323); i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }
}
