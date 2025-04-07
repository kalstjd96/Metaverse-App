using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace SCM.Service.TaxSquare.Consultation
{
    using Manager = SocketManager;

    public class ChatSocketClient
    {
        private Manager manager;
        private bool isConnected = false;
        private int acntSn;
        private string userNickName;
        public event Action OnConnected;
        public event Action OnErrorReceived;
        public event Action OnMessageReceived;
        public event Action<string> OnConMessageReceived;
        public event Action OnDisConnected;

        string serverURL = "http://61.38.144.222:3050/tax-consultation/";

        public ChatSocketClient(int accountSerialNumber, string accountNickname)
        {
            userNickName = accountNickname;
            var additionalQueryParams = new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>
            {
                { "nickNm", accountNickname},
                { "role", "2" },
                { "prfImgAddr", "https://smj080741-smj080741.ktcdn.co.kr/rnd/e03aeacd-3d61-4a17-af4e-9dc7daf2bded.png"},
                { "acntSn", accountSerialNumber.ToString() }
            };

            var options = new SocketOptions
            {
                ConnectWith = BestHTTP.SocketIO3.Transports.TransportTypes.WebSocket,
                AdditionalQueryParams = additionalQueryParams
            };

            manager = new Manager(new Uri(serverURL), options);

            InitializeEvents();
        }

        public void InitializeEvents()
        {
            manager.Socket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnect);
            manager.Socket.On<ConnectResponse>(SocketIOEventTypes.Error, OnError);
            manager.Socket.On<string>("connectionClosed", Disconnect);

            manager.Socket.On<MessageDto>("consultationMessage", data =>
            {
                try
                {
                    if (data.from.ToString().Equals(userNickName))
                        return;

                    string message = data.body.data;
                    OnConMessageReceived?.Invoke(message);
                }
                catch (JsonException ex)
                {
                    Debug.LogError("JSON 파싱 오류: " + ex.Message);
                }


            });

            manager.Socket.On<ConnectionRequestDto>("connectionRequest", data =>
            {
                try
                {
                    acntSn = data.Body.Counselor.AcntSn;
                }
                catch (JsonException ex)
                {
                    Debug.LogError("JSON 파싱 오류: " + ex.Message);
                }

                OnMessageReceivedHandler();
            });
        }


        public void SendConnectionResponse(int acceptYn)
        {
            var connectionResponse = new ResponseDto
            {
                acceptYn = acceptYn,
                counselorAcntSn = acntSn
            };

            manager.Socket.Emit("connectionResponse", connectionResponse);
        }



        public void Connect()
        {
            if (manager.Socket.IsOpen)
            {
                Debug.Log("Socket is already connected.");
                return;
            }

            manager.Socket.On(SocketIOEventTypes.Connect, OnSocketConnected);

            ((ISocket)manager.Socket).Open();

        }

        private void OnSocketConnected()
        {
            manager.Socket.Emit("standby");
        }

        public void SendMessageToServer(string message)
        {
            if (manager.Socket.IsOpen)
            {
                var connectionMes = new SendMsgDto
                {
                    message = message
                };
                manager.Socket.Emit("consultationMessage", connectionMes);
                Debug.Log($"Message sent to server: {connectionMes}");
            }
            else
            {
                OnErrorReceived?.Invoke();
                Debug.LogWarning("WebSocket is not open. Cannot send message.");
            }
        }


        private void OnConnect(ConnectResponse response)
        {
            isConnected = true;
            OnConnected?.Invoke();
        }

        private void OnError(ConnectResponse errorResponse)
        {
            if (errorResponse != null)
            {
                Debug.LogError($"Connection Error: {errorResponse}");
                Debug.LogError($"Error Code: {errorResponse.sid}");
            }
            else
            {
                Debug.LogError("Connection Error: No response details available.");
            }
        }


        private void OnMessageReceivedHandler()
        {
            OnMessageReceived?.Invoke();
        }

        public void Disconnect(string message = null)
        {
            if (isConnected)
            {
                manager.Socket.Emit("connectionClosed");
                manager.Socket.Disconnect();
                isConnected = false;

                OnDisConnected?.Invoke();
            }
        }
    }
}
