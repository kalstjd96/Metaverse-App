#if !BESTHTTP_DISABLE_SOCKETIO
using System;
using System.Net;
using BestHTTP.SocketIO;
using UnityEngine;

namespace SCM.Service.TaxSquare.Consultation
{
    public class SocketIOManager : MonoBehaviour
    {
        private SocketManager socketManager;
        private Socket socket;

        private Uri serverUri;
        private string nsp = "/";

        public Action OnResponseReceived;

        private void Awake()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            Debug.Log("모든 SSL 인증 True임");
        }

        public void ConnectToServer(string serverUri)
        {
            this.serverUri = new Uri(serverUri);

            if (socketManager == null)
            {
                SocketOptions options = new SocketOptions
                {
                    ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket,
                    AdditionalQueryParams = new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>
                    {
                        { "nickNm", "tttkim333" },
                        { "role", "1" },
                        { "prfImgAddr", "https://smj080741-smj080741.ktcdn.co.kr/rnd/e03aeacd-3d61-4a17-af4e-9dc7daf2bded.png" },
                        { "acntSn", "7777" },
                    }
                };

                socketManager = new SocketManager(this.serverUri, options);

                Debug.Log("socketManager : " + socketManager.Options.ConnectWith);
                
                socketManager.Socket.On("connect", OnConnected);
                socketManager.Socket.On("disconnect", OnDisconnected);
                socketManager.Socket.On("error", OnError);

                socketManager.Socket.On("response", OnResponseReceivedFromServer);
                socketManager.Open();
            }
        }

        private void OnConnected(Socket socket, Packet packet, object[] args)
        {
            Debug.Log("Connected");
        }

        private void OnDisconnected(Socket socket, Packet packet, object[] args)
        {
            Debug.Log("Disconnected");
        }

        private void OnResponseReceivedFromServer(Socket socket, Packet packet, object[] args)
        {
            if (args.Length > 0 && args[0] is bool response)
            {
                Debug.Log("Response: " + response);
                OnResponseReceived?.Invoke();
            }
        }

        private void OnError(Socket socket, Packet packet, object[] args)
        {
            Debug.LogError("Error : " + args[0]);
        }

        public void EmitMessage(string eventName, object message)
        {
            if (socket != null && socketManager.State == SocketManager.States.Open)
            {
                socket.Emit(eventName, message);
                Debug.Log("메시지 테스트!: " + eventName);
            }
            else
            {
                Debug.LogWarning("not connected to server.");
            }
        }

        public void CloseConnection()
        {
            if (socketManager != null && socketManager.State != SocketManager.States.Closed)
            {
                socketManager.Close();
                Debug.Log("Connection closed");
            }
        }

        private void OnDestroy()
        {
            CloseConnection();
        }
    }
}
#endif
