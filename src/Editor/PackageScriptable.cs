using UnityEngine;



namespace UnityPackageUtil
{
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
}