using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

class ResourceUtil {

    public static string[] GetPrefabPaths(string folderName) {
        var path = Application.dataPath + "/Resources/" + folderName;
	    var r = Directory.GetFiles(path).Select(s => CleanPath(s, folderName)).Distinct().ToArray();
#if DEBUG
        foreach (string s in r) {
            Debug.Log(s);
        }
#endif
        return r;
    }

    private static string CleanPath(string path, string folderName) {
        path = new Uri(path).AbsolutePath;
        var start = path.IndexOf(folderName);
        var end = path.IndexOf(@".prefab");
        path = path.Substring(start, end - start);
        return Uri.UnescapeDataString(path);
    }
}
