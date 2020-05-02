using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace INIParse
{
    using Section = Dictionary<string, string>;

    public class INIParser
    {
        private Dictionary<string, Section> data = new Dictionary<string, Section>();
        private string path;

        public INIParser(string filename, Dictionary<string, Section> default_data = null)
        {
            filename = Application.persistentDataPath + "/" + filename;
            path = filename;

            if (!File.Exists(filename))
            {
                if (default_data != null)
                {
                    data = default_data;
                    flush();
                }
            }
            else
            {
                StreamReader reader = new StreamReader(filename);
                string content = reader.ReadToEnd();
                reader.Close();
                parseINI(content);
            }
        }

        private void parseINI(string content)
        {
            string[] lines = content.Split('\n');
            string current_section = "";
            foreach(string line in lines)
            {
                if (line[0] == '[')
                {
                    current_section = line.Split(new char[2] {'[', ']'},
                        System.StringSplitOptions.RemoveEmptyEntries)[0];
                    data.Add(current_section, new Section());
                }
                else
                {
                    string[] pair = line.Split('=');
                    data[current_section].Add(pair[0], pair[1]);
                }
            }
        }

        public string getString(string section_name, string key, string default_value = "")
        {
            if (data.ContainsKey(section_name))
            {
                if (data[section_name].ContainsKey(key))
                {
                    return data[section_name][key];
                }
            }

            return default_value;
        }

        public void setString(string section_name, string key, string value)
        {
            if (!data.ContainsKey(section_name))
            {
                data.Add(section_name, new Section());
            }

            if (data[section_name].ContainsKey(key))
            {
                data[section_name][key] = value;
            }
            else 
            {
                data[section_name].Add(key, value);
            }
        }

        public void flush()
        {
            string content = "";
            foreach (string section_name in data.Keys)
            {
                content += "[" + section_name + "]\n";
                foreach (string key in data[section_name].Keys)
                {
                    content += key + "=" + data[section_name][key];
                }
            }

            flush(content);
        }

        private void flush(string content)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(content);
            writer.Close();
        }
    }
}