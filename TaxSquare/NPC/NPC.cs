using SCM.Platform.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCM.Platform.InteractableObject
{
    public class NPC : Interactable
    {
        public NPCChatUI chatUI;



        // 상속 받은 Interaction 함수 구현
        public override void Interaction()
        {
            chatUI.OpenNPCChat();
        }
    }
}
