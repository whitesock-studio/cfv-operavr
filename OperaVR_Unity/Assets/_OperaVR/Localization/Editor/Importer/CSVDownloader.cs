using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
public static class CSVDownloader
{
    private static string URL(string sheetId, int gid) => 
        $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=csv&id={sheetId}&gid={gid}";

    internal static IEnumerator DownloadData(System.Action<string> onCompleted, string sheetId, int gid = 0)
    {
        yield return new WaitForEndOfFrame();

        var downloadData = "";
        using var webRequest = UnityWebRequest.Get(URL(sheetId, gid));

        Debug.Log("Starting Download...");

        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("...Download Error: " + webRequest.error);
            yield break;
        }

        downloadData = webRequest.downloadHandler.text;
        Debug.Log("...Downloaded correctly");

        onCompleted?.Invoke(downloadData);
    }
}
#endif