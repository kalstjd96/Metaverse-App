using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.IO;
using System;
using Newtonsoft.Json;

public class TaxGPTApi : MonoBehaviour
{
    public string questionCode = "세금이 뭐야?";

    private void Start()
    {
        SendRequest();
    }

    public void SendRequest()
    {
        string url = "http://192.168.10.123:7000/tax-gpt/ask";
        url += "?questionCode=" + UnityWebRequest.EscapeURL(questionCode);
        url += "&streaming=false";

        StartCoroutine(GetRequest(url));
    }

    

    #region 텍스트 형식
    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string responseJson = webRequest.downloadHandler.text;
                Debug.Log("Response JSON: " + responseJson);

                byte[] responseData = Encoding.UTF8.GetBytes(responseJson);
                string jsonData = Encoding.UTF8.GetString(responseData);
                Debug.Log("jsonData : " + jsonData);

                JSONObject jsonObject = new JSONObject(responseJson);
                string resultMsg = jsonObject.GetField("resultMsg").str;
                Debug.Log("Result Message: " + resultMsg);

                byte[] unicodeBytes = Encoding.Unicode.GetBytes(responseJson);
                string unicodeStringResult = Encoding.Unicode.GetString(unicodeBytes);
                Debug.Log("Unicode Result: " + unicodeStringResult);

                //string unicodeString = "\\uc548\\ub155\\ud558\\uc2e0\\ub2e4";
                string decodedString = System.Text.RegularExpressions.Regex.Unescape(responseJson);
                Debug.Log("Decoded Result: " + decodedString);

                // utf-8 인코딩
                //byte[] bytesForEncoding = Encoding.UTF8.GetBytes(responseJson);
                //string encodedString = Convert.ToBase64String(bytesForEncoding);

                // 문자열을 메모리 스트림으로 변환
                //byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                //MemoryStream memoryStream = new MemoryStream(responseBytes);

                //// 메모리 스트림에서 한 문자씩 읽어서 출력
                //StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8);
                //while (!reader.EndOfStream)
                //{
                //    char c = (char)reader.Read();
                //    Debug.Log("Character: " + c);
                //}

                // utf-8 디코딩
                //byte[] decodedBytes = Convert.FromBase64String(encodedString);
                //string decodedString = Encoding.UTF8.GetString(decodedBytes);
                //Debug.Log("decodedString Message: " + decodedString);
            }

           
        }
    }
    #endregion
}
