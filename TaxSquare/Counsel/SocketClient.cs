using SCM.Platform.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCM.Service.TaxSquare.Consultation
{
    public class SocketClient : ManagedUIComponent<SocketClient>
    {
        public TMP_InputField chatInput;
        public Button sendButton;
        public Button closeButton;
        public Transform chatScrollViewTrans;
        public ScrollRect chatScrollRect;
        public GameObject agentsMessagePrefab;
        public GameObject clientMessagePrefab;
        public ConselorNPC ConselorNPC;


        protected override ManagedUIProperties UIProperties => ManagedUIProperties.LOCK_KEYBOARD;

        private void Awake()
        {
            InitializeUIListeners();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void InitializeUIListeners()
        {
            sendButton.onClick.AddListener(SendMessageToServer);
            closeButton.onClick.AddListener(CloseCanvas);
            chatInput.onSubmit.AddListener(_ => SendMessageToServer());
        }

        public void CloseCanvas()
        {
            gameObject.SetActive(false);
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        private void CreateChatBubble(GameObject messagePrefab, string message)
        {
            var chatObj = Instantiate(messagePrefab, chatScrollViewTrans);
            var chatItem = chatObj.GetComponent<SendChatItem>();
            if (chatItem != null)
            {
                chatItem.SetItem(message);
            }
        }

        private void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatScrollViewTrans.GetComponent<RectTransform>());
            chatScrollRect.verticalNormalizedPosition = 0f;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ConselorNPC.Disconnect();
            foreach (Transform chatItem in chatScrollViewTrans)
            {
                Destroy(chatItem.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            ConselorNPC.Disconnect();
        }

        private void SendMessageToServer()
        {
            if (string.IsNullOrEmpty(chatInput.text))
                return;

            var message = chatInput.text;
            CreateChatBubble(clientMessagePrefab, message);
            ScrollToBottom();

            ConselorNPC.SendMessageToServer(message); 

            chatInput.text = string.Empty;
        }

        public void RecevieMessageToServer(string message)
        {
            CreateChatBubble(agentsMessagePrefab, message);
            ScrollToBottom();
        }
    }

}

