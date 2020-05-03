using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INIParse;
using UnityEngine.Events;

public class KeyboardManager : MonoBehaviour
{
    public string keyboard_filename = "keyboard.ini";
    public bool editable = false;
    public GameObject item_object;
    public Transform parent;
    public UnityEvent<string> key_down_event = null, key_event = null, key_up_event = null;
    private INIParse.INIParser parser = null;
    private Dictionary<string, Dictionary<string, string>> settings_dict =
            new Dictionary<string, Dictionary<string, string>>();
    
    // Start is called before the first frame update
    void Start()
    {
        createParser();
        if (editable)
        {
            foreach (string key in settings_dict["keys"].Keys)
            {
                GameObject item = GameObject.Instantiate(item_object, parent);
                KeyboardReciever reciever = item.GetComponent<KeyboardReciever>();
                reciever.parent = this;
                reciever.setKeys(key, parser.getString("keys", key));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!editable)
        {
            foreach (string key in settings_dict["keys"].Keys)
            {
                KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), parser.getString("keys", key));
                if (Input.GetKeyDown(code))
                {
                    if (key_down_event != null)
                    {
                        key_down_event.Invoke(key);
                    }
                    Debug.Log(key);
                }
                if (Input.GetKey(code) && key_event != null)
                {
                    key_event.Invoke(key);
                }
                if (Input.GetKeyUp(code) && key_up_event != null)
                {
                    key_up_event.Invoke(key);
                }
            }
        }
    }

    public void modify(string key, KeyCode value) 
    {
        settings_dict["keys"][key] = value.ToString();
        parser.setString("keys", key, value.ToString());
    }

    void createParser()
    {
        settings_dict.Add("keys", new Dictionary<string, string>());

        settings_dict["keys"].Add("up", KeyCode.W.ToString());
        settings_dict["keys"].Add("down", KeyCode.S.ToString());
        settings_dict["keys"].Add("left", KeyCode.A.ToString());
        settings_dict["keys"].Add("right", KeyCode.D.ToString());
        settings_dict["keys"].Add("fire", KeyCode.Space.ToString());

        parser = new INIParser(keyboard_filename, settings_dict);
    }

    private void OnDestroy()
    {
        parser.flush();
    }
}
