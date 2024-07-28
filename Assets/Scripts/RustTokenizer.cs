using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.IO;

public class RustTokenizer : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern IntPtr tokenize(string input, string dictPath);

    [DllImport("__Internal")]
    private static extern void free_string(IntPtr str);

    [DllImport("libdl.dylib")]
    private static extern IntPtr dlopen(string fileName, int flags);

    [DllImport("libdl.dylib")]
    private static extern IntPtr dlerror();

    void Start()
    {
        string libraryPath = Path.Combine(Application.dataPath, "Plugins", "librust_tokenizer.dylib");
        Debug.Log("Attempting to load library from: " + libraryPath);

        IntPtr lib = dlopen(libraryPath, 2); // RTLD_NOW = 2
        if (lib == IntPtr.Zero)
        {
            IntPtr errPtr = dlerror();
            string errorMessage = Marshal.PtrToStringAnsi(errPtr);
            Debug.LogError("Failed to load library. Error: " + errorMessage);
            return;
        }

        Debug.Log("Library loaded successfully");
        
        string input = "本とカレーの街神保町へようこそ。";
        string dictPath = Application.dataPath + "/Plugins/ipadic-mecab-2_7_0/system.dic.zst";

        IntPtr resultPtr = tokenize(input, dictPath);
        string result = Marshal.PtrToStringAnsi(resultPtr);
        free_string(resultPtr);

        Debug.Log(result);
    }
}