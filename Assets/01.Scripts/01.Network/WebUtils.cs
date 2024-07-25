using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebUtils
{
    /// <summary>
    /// 해당 URL이 접속 가능한지 확인 , 마지막 매개변수를 통해 추가로 다운로드 권한이 있으면 다운로드 진행
    /// </summary>
    public static async System.Threading.Tasks.Task<bool> ValidURL(string url,  bool checkDownload = false)
    {
        using UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Head(url);
        try
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await System.Threading.Tasks.Task.Yield();
            }

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                // HTTP 응답 코드가 200-299 범위인지 확인합니다
                int responseCode = (int)request.responseCode;
                if (responseCode >= 200 && responseCode < 300)
                {
                    if (checkDownload)
                    {
                        // GET 요청을 시도하여 실제 다운로드 권한을 확인
                        using UnityWebRequest getRequest = UnityWebRequest.Get(url);
                        var getOperation = getRequest.SendWebRequest();

                        while (!getOperation.isDone)
                        {
                            await Task.Yield();
                        }

                        if (getRequest.result == UnityWebRequest.Result.Success)
                        {
                            // 다운로드 성공
                            Message.Log("Download successful.");
                            return true;
                        }
                        else
                        {
                            // 다운로드 실패
                            Message.LogWarning($"Download failed: {getRequest.error}");
                            return false;
                        }
                    }
                    else
                    {
                        Message.Log($"{request.result} _ {responseCode}");
                        return true;
                    }
                }
                else
                {
                    Message.LogWarning($"URL is not accessible (HTTP Status Code: {responseCode}): {url}");
                    return false;
                }
            }
            else
            {
                Message.LogWarning($"URL is not valid: {url} - Error: {request.error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Message.LogError($"Exception occurred while validating URL: {url} - Exception: {ex.Message}");
            return false;
        }
    }


}
