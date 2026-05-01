using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class WebServer : MonoBehaviour
{
    private string siteImagePath = string.Empty;
    private bool isRunning;
    private string folderPath = "\\Sites";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public bool returnRunning()
    {
        return isRunning;
    }

    public void deleteDefault()
    {
        siteImagePath = string.Empty;
        isRunning = false;
    }

    public string getSite()
    {
        if (isRunning)
            return siteImagePath;
        return null;
    }

    public void toggleActive()
    {
        if (isRunning)
        {
            isRunning = false;
        }
        else
            isRunning = true;
    }

    public void LoadImagePathFromLocal(string filePath)
    {
        siteImagePath = filePath;
    }

    public void LoadImageFromLocal(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }
        Debug.LogWarning($"{filePath}");
        byte[] fileBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileBytes);
        //siteImage.texture = texture;
    }


    public List<string> getAllPNGPaths()
    {
        string path = Application.persistentDataPath + folderPath;
        if (!Directory.Exists(path)) return null;

        List<string> pngFiles = Directory.GetFiles(path, "*.png").ToList<string>();
        return pngFiles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
