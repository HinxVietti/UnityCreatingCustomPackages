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

    [CreateAssetMenu(menuName = "Package Config", fileName = "package.conf.asset")]
    internal class PackageScriptable : ScriptableObject
    {
        [Header("required properties"), Tooltip(@"The officially registered package name. This name must conform to the Unity Package Manager naming convention, which uses reverse domain name notation. For more information about the naming convention, see Naming your package.

Note: This is a unique identifier, not the user - friendly name that appears in the list view in the Package Manager window.")]

        new public string name = "com.[company-name].[package-name]";
        [Tooltip(@"The package version number (MAJOR.MINOR.PATCH).

For example, “3.2.1” indicates that this is the 3rd major release, the 2nd minor release, and the first patch.

This value must respect Semantic Versioning.For more information, see Versioning.")]
        public string version = "1.0.0";

        [Header("recomended properties"), Tooltip(@"A brief description of the package. This is the text that appears in the details view of the Package Manager window. This field supports UTF–8 character codes. This means that you can use special formatting character codes, such as line breaks (\n) and bullets (\u25AA).")]
        public string description;
        [Tooltip(@"A user-friendly name to appear in the Unity Editor (for example, in the Project Browser, the Package Manager window, etc.).

For example, Unity Timeline, ProBuilder, In App Purchasing.")]
        public string displayName;
        [Tooltip(@"Indicates the lowest Unity version the package is compatible with. If omitted, the Package Manager considers the package compatible with all Unity versions.

The expected format is “<MAJOR>.<MINOR>” (for example, 2018.3). To point to a specific patch, use the unityRelease property as well.

Note: A package that isn’t compatible with Unity doesn’t appear in the Package Manager window.")]
        public string unity; // 2018.3 

        [Header("Optional properties"), Tooltip(@"The author of the package.

This object contains one required field, name, and two optional fields, email and url.")]
        public Author author = new Author();
        [Tooltip("Custom location for this package’s changelog specified as a URL. \nNote: When the Package Manager can’t reach the URL location(for example, if there is a network issue), it does the following:\n\n-If the package is installed, the Package Manager opens a file browser displaying the CHANGELOG.md file in the package cache.\n\n- If the package isn’t installed, the Package Manager displays a warning that an offline changelog isn’t available.")]
        public string changelogUrl;
        [Tooltip(@"A map of package dependencies. Keys are package names, and values are specific versions. They indicate other packages that this package depends on.

Note: The Package Manager doesn’t support range syntax, only SemVer versions.")]
        public Dependencies dependencies = new Dependencies();
        [Tooltip(@"Custom location for this package’s documentation specified as a URL.
Note: When the Package Manager can’t reach the URL location(for example, if there is a network issue), it does the following:

-If the package is installed, the Package Manager opens a file browser displaying the Documentation~folder in the package cache.
- If the package isn’t installed, the Package Manager displays a warning that offline documentation isn’t available.")]
        public string documentationUrl;
        [Tooltip("Package Manager hides most packages automatically (the implicit value is “true”), but you can set this property to “false” to make sure that your package and its assets are always visible.")]
        public bool hideInEditor;
        [Tooltip("An array of keywords used by the Package Manager search APIs. This helps users find relevant packages.")]
        public string[] keywords;
        [Tooltip(@"Identifier for an OSS license using the SPDX identifier format, or a string such as “See LICENSE.md file”.

Note: If you omit this property in your package manifest, your package must contain a LICENSE.md file.")]
        public string license;
        [Tooltip("Custom location for this package’s license information specified as a URL. For example:\n\"licensesUrl\": \"https://example.com/licensing.html\"" + @"Note: When the Package Manager can’t reach the URL location(for example, if there is a network issue), it does the following:

-If the package is installed, it opens a file browser displaying the LICENSE.md file in the package cache.
- If the package isn’t installed, the Package Manager displays a warning that offline license information isn’t available.")]
        public string licensesUrl;
        [Tooltip(@"List of samples included in the package. Each sample contains a display name, a description, and the path to the sample folder starting at the Samples~ folder itself:
")]
        public Sample[] samples;
        [Tooltip(@"A constant that provides additional information to the Package Manager.

Reserved for internal use.")]
        public string type;
        [Tooltip(@"Part of a Unity version indicating the specific release of Unity that the package is compatible with. You can use this property when an updated package requires changes made during the Unity alpha/beta development cycle. This might be the case if the package needs newly introduced APIs, or uses existing APIs that have changed in a non-backward-compatible way without API Updater rules.

The expected format is “< UPDATE >< RELEASE >” (for example, 0b4).

Note: If you omit the recommended unity property, this property has no effect.

A package that isn’t compatible with Unity doesn’t appear in the Package Manager window.")]
        public string unityRelease; // 0b5


        public Package Package
        {
            get
            {
                return new Package
                {
                    name = name,
                    version = version,

                    description = description,
                    displayName = displayName,
                    unity = unity,

                    author = string.IsNullOrEmpty(author?.name) ? null : author,
                    changelogUrl = changelogUrl,
                    dependencies = dependencies.Count > 0 ? dependencies : null,
                    documentationUrl = documentationUrl,
                    hideInEditor = hideInEditor,
                    keywords = keywords,
                    license = license,
                    licensesUrl = licensesUrl,
                    samples = samples,
                    type = type,
                    unityRelease = unityRelease
                };
            }
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
