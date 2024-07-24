using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebUtils
{
    public static async System.Threading.Tasks.Task<bool> ValidURL(string url)
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
                    Debug.Log($"{request.result} _ {responseCode}");
                    return true;
                }
                else
                {
                    Debug.LogWarning($"URL is not accessible (HTTP Status Code: {responseCode}): {url}");
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
    public static async Task<bool> CanDownload(string url)
    {
        try
        {
            // HEAD 요청을 보내서 URL의 접근 권한을 확인
            using (UnityWebRequest headRequest = UnityWebRequest.Head(url))
            {
                var headOperation = headRequest.SendWebRequest();

                while (!headOperation.isDone)
                {
                    await Task.Yield();
                }

                if (headRequest.result == UnityWebRequest.Result.Success)
                {
                    int responseCode = (int)headRequest.responseCode;
                    Debug.Log($"HEAD Request Response Code: {responseCode}");

                    // 상태 코드가 200-299 범위에 있는지 확인
                    if (responseCode >= 200 && responseCode < 300)
                    {
                        // GET 요청을 시도하여 실제 다운로드 권한을 확인
                        using (UnityWebRequest getRequest = UnityWebRequest.Get(url))
                        {
                            var getOperation = getRequest.SendWebRequest();

                            while (!getOperation.isDone)
                            {
                                await Task.Yield();
                            }

                            if (getRequest.result == UnityWebRequest.Result.Success)
                            {
                                // 다운로드 성공
                                Debug.Log("Download successful.");
                                return true;
                            }
                            else
                            {
                                // 다운로드 실패
                                Debug.LogWarning($"Download failed: {getRequest.error}");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // HEAD 요청에서 비정상적인 상태 코드 반환
                        Debug.LogWarning($"URL is not downloadable (HTTP Status Code: {responseCode}): {url}");
                        return false;
                    }
                }
                else
                {
                    // HEAD 요청 실패
                    Debug.LogWarning($"HEAD request failed: {headRequest.error}");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception occurred while checking download permission: {url} - Exception: {ex.Message}");
            return false;
        }
    }
}
