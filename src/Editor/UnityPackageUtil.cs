using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace UnityPackageUtil
{
    public class UnityPackageUtil
    {
        public static string packageFileName = "package.json";

        internal static string ToJson(PackageScriptable package)
        {
            return ToJson(package.Package);
        }
        public static string ToJson(Package package)
        {
            string json = JsonMapper.ToJson(package);
            var table = JsonMapper.ToObject(json);
            return ClearTable(table).ToJson();
        }

        /// <summary>
        /// clear empty properties
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static JsonData ClearTable(JsonData table)
        {
            if (table == null)
                return null;

            JsonData copy = new JsonData();
            if (table.IsArray == false)
            {
                var keys = table.Keys;
                foreach (var key in keys)
                {
                    var value = table[key];
                    if (value == null)
                        continue;
                    if (value.IsString && string.IsNullOrEmpty((string)value))
                        continue;
                    if (value.IsObject)
                    {
                        var ctable = ClearTable(value);
                        if (ctable != null && ctable.Count > 0)
                            copy[key] = (ctable);
                        continue;
                    }
                    copy[key] = value;
                }
                if (copy.Count == 0)
                    return null;
                return copy;
            }
            else
            {
                //array
                List<JsonData> copies = new List<JsonData>();

                int length = table.Count;
                for (int i = 0; i < length; i++)
                {
                    var jd = table[i];
                    if (jd.IsString && string.IsNullOrEmpty((string)jd))
                        continue;

                    if (jd.IsObject || jd.IsArray)
                    {
                        var ctable = ClearTable(jd);
                        if (table != null)
                            copies.Add(table);
                        continue;
                    }

                    var t = jd.GetJsonType();
                    switch (t)
                    {
                        case JsonType.Int:
                            copies.Add((int)jd);
                            break;
                        case JsonType.Long:
                            copies.Add((long)jd);
                            break;
                        case JsonType.Double:
                            copies.Add((double)jd);
                            break;
                        case JsonType.Boolean:
                            copies.Add(bool.Parse((string)jd));
                            break;
                        default:
                            break;
                    }
                }
                if (copies.Count > 0)
                    return new JsonData(copies.ToArray());
                return null;
            }

        }

        public static void Test()
        {
            var package = new Package
            {
                name = "com.hinxcor.unity.UnityPackageUtil".ToLower(),
                version = "0.0.1",
                description = "Custom Package Tools",
                displayName = "Custom Package (Hinx)",

                author = new Author { name = "hinx", email = "hinxvietti@gmail.com" },

                license = "license.md"
            };

            string json = package.ToString();
            Debug.Log(json);
        }

    }

    [System.Serializable]
    public class Package
    {
        //required properties
        /// <summary>
        ///  Naming your package
        ///  There are two names for a package: the official name you register the package with; and the user-facing display name that users can see in the Editor.
        ///  The display name should be brief but provide some indication of what the package contains. Otherwise, the Unity Package Manager imposes no restrictions on the display name.
        ///  The official name must conform to the Unity Package Manager naming convention, which uses reverse domain name notation. The name must:
        ///  Start with <domain-name-extension>.<company-name> (for example, com.example or net.example), even if your company or website name begins with a digit.
        ///  Be no more than 50 characters if you want it to be visible in the Editor.If the package name does not need to appear in the Editor, the Unity Package Manager imposes a limit of 214 characters or less.
        ///  Contain only lowercase letters, digits, hyphens(-), underscores(_), and periods(.)
        ///  To indicate nested namespaces, suffix the namespace with an additional period.For example, “com.unity.2d.animation” and “com.unity.2d.ik”.
        ///  For example, “com.unity.2d.animation” and “com.unity.2d.ik” are the names of two Unity 2D packages, but a custom package developer at https://example.net might create a package named “net.example.physics”. Use your own company name in your package names. Do not use the “unity” prefix in your own package names.
        ///  Note: These naming restrictions apply only to the package names themselves and do not need to match the namespace in your code.For example, you could use Project3dBase as a namespace in a package called net.example.3d.base.
        /// </summary>
        public string name { get; set; }
        public string version { get; set; }

        //recomended properties
        public string description { get; set; }
        public string displayName { get; set; }
        public string unity { get; set; } // 2018.3 

        //Optional properties
        public Author author { get; set; }
        public string changelogUrl { get; set; }
        public Dependencies dependencies { get; set; }
        public string documentationUrl { get; set; }
        public bool hideInEditor { get; set; }
        public string[] keywords { get; set; }
        public string license { get; set; }
        public string licensesUrl { get; set; }
        public Sample[] samples { get; set; }
        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        public string type { get; set; }
        public string unityRelease { get; set; } // 0b5


        /// <summary>
        /// empty constructor for litjson or other serializor
        /// </summary>
        public Package() { }

        public override string ToString()
        {
            return UnityPackageUtil.ToJson(this);
        }

    }

    [System.Serializable]
    public class Sample
    {
        [SerializeField] private string _displayName;
        [SerializeField] private string _description;
        [SerializeField] private string _path;

        public string displayName { get => _displayName; set => _displayName = value; }
        public string description { get => _description; set => _description = value; }
        public string path { get => _path; set => _path = value; }
    }

    [System.Serializable]
    public class Dependencies : Dictionary<string, string> { }

    [System.Serializable]
    public class Author
    {
        [SerializeField] private string _name; //require
        [SerializeField] private string _email;
        [SerializeField] private string _url;

        public string name { get => _name; set => _name = value; } //require
        public string email { get => _email; set => _email = value; }
        public string url { get => _url; set => _url = value; }
    }
}
