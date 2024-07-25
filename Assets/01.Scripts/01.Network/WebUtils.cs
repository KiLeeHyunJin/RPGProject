using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebUtils
{
    /// <summary>
    /// �ش� URL�� ���� �������� Ȯ�� , ������ �Ű������� ���� �߰��� �ٿ�ε� ������ ������ �ٿ�ε� ����
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
                // HTTP ���� �ڵ尡 200-299 �������� Ȯ���մϴ�
                int responseCode = (int)request.responseCode;
                if (responseCode >= 200 && responseCode < 300)
                {
                    if (checkDownload)
                    {
                        // GET ��û�� �õ��Ͽ� ���� �ٿ�ε� ������ Ȯ��
                        using UnityWebRequest getRequest = UnityWebRequest.Get(url);
                        var getOperation = getRequest.SendWebRequest();

                        while (!getOperation.isDone)
                        {
                            await Task.Yield();
                        }

                        if (getRequest.result == UnityWebRequest.Result.Success)
                        {
                            // �ٿ�ε� ����
                            Message.Log("Download successful.");
                            return true;
                        }
                        else
                        {
                            // �ٿ�ε� ����
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
