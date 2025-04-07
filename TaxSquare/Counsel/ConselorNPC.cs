using SCM.Platform.Account;
using SCM.Platform.Common;
using SCM.Platform.InteractableObject;
using SCM.Platform.SpaceCommon;
using SCM.Platform.UI;
using SCM.Service.TaxSquare.Common;
using SCM.Service.TaxSquare.Data;
using UnityEngine;

namespace SCM.Service.TaxSquare.Consultation
{
    public class ConselorNPC : Interactable
    {
        public GameObject ChatPanel;
        public GameObject InforPanel;
        private ChatSocketClient ChatSocketClient;
        public TaxCityCustomizablePortal taxCityCustomizablePortal;

        public override void Interaction()
        {
            if (GlobalData.isGuestMode)
            {
                GuestChecker.ShowGuestModeNeedRegist();
                return;
            }
            else
            {
                ChatPanel.SetActive(true);
            }
        }

        public void ConsultationCheck()
        {
            UIPopup.Instance.OpenHeavyPopup(new PopupInfo()
            {
                title = "알림",
                content = "상담 신청을 진행하시겠습니까? \n" +
                     "해당 공간을 벗어나면 자동으로 상담 신청이 종료됩니다.",
                okTitle = "확인",
                cancelTitle = "취소",
                onOkPressed = () => InforPanel.SetActive(true)
            });
        }

        public void InitiateStandBy()
        {
            taxCityCustomizablePortal.isConselor = true;
            UIPopup.Instance.OpenNoticePopup(new PopupInfo()
            {
                title = "알림",
                content = "상담 신청이 완료되었습니다. 상담사와 매칭이 되었을 때 알림이 갑니다.",
                okTitle = "확인"
            });
            string nickname = GlobalData.isGuestMode ? PhotonContainer.Manager.PlayerNickname : AccountManager.Instance.UserData.nickname;
            
            ChatSocketClient = new (AccountManager.Instance.UserData.sn, nickname);
            ChatSocketClient.Connect();

            ChatSocketClient.OnMessageReceived += HandleConnectionRequest;
            ChatSocketClient.OnDisConnected += Disconnect;
            ChatSocketClient.OnConMessageReceived += ReceiveMessage;
        }

        private void ReceiveMessage(string message)
        {
            SocketClient socketClient = ChatPanel.GetComponent<SocketClient>();
            socketClient.RecevieMessageToServer(message);
        }

        private void HandleConnectionRequest()
        {
            UIPopup.Instance.OpenHeavyPopup(new PopupInfo()
            {
                title = "상담 요청",
                content = "상담사가 상담을 요청했습니다. 수락하시겠습니까?",
                okTitle = "수락",
                cancelTitle = "거절",
                onOkPressed = () => ApproveCounseling(),
                onCancelPressed = () => RejectCounseling()
            });
        }

        private void ApproveCounseling()
        {
            ChatSocketClient.SendConnectionResponse(1);
            ChatPanel.SetActive(true);
            Screen.orientation = ScreenOrientation.Portrait;
        }

        private void RejectCounseling()
        {
            taxCityCustomizablePortal.isConselor = false;
            ChatSocketClient.SendConnectionResponse(0);
            ChatSocketClient.OnMessageReceived -= HandleConnectionRequest;
            ChatSocketClient.Disconnect();
            UIPopup.Instance.OpenNoticePopup(new PopupInfo()
            {
                title = "알림",
                content = "상담을 거절했습니다.",
                okTitle = "확인"
            });
        }

        public void Disconnect()
        {
            ChatSocketClient.Disconnect();
            ChatPanel.SetActive(false);
            UIPopup.Instance.OpenNoticePopup(new PopupInfo()
            {
                title = "알림",
                content = "상담이 종료되었습니다.",
                okTitle = "확인",
                onOkPressed = () => taxCityCustomizablePortal.isConselor = false,
            });
        }

        public void SendMessageToServer(string message)
        {
            ChatSocketClient.SendMessageToServer(message);
        }

        public void OnDestroy()
        {
            Disconnect();
        }
    }

}

