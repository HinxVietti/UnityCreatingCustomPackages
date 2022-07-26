using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace UnityPackageUtil
{
    public static class EditorTools
    {
        [MenuItem("Tools/UnityPackageUtil/Create Json Test")]
        private static void createJson()
        {
            UnityPackageUtil.Test();
            Debug.Log("done");
        }

        [MenuItem("Tools/UnityPackageUtil/Config File To Json", validate = true)]
        private static bool debugProfileToJsonVer()
        {
            var item = getSelectable();
            if (item)
                return true;
            return false;
        }

        [MenuItem("Tools/UnityPackageUtil/Config File To Json")]
        private static void debugProfileToJson()
        {
            var item = getSelectable();
            if (item)
            {
                string json = UnityPackageUtil.ToJson(item);
                Debug.Log(json);
            }
        }


        private static PackageScriptable getSelectable()
        {
            return Selection.activeObject as PackageScriptable;
        }

    }
}