using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCM.Platform.Common.UI
{
    [Serializable]
    /// <summary>
    /// NPC 대화창에 대한 설정 값. Chatting은 여러 개의 Chat Item을 가지고 있을 수 있다.
    /// </summary>
    public class ChatItemOption
    {
        // TODO : 사용자 권한 Enum 추가
        /// <summary>
        /// 해당 옵션의 이름.
        /// </summary>
        [Tooltip("해당 옵션의 이름")]
        public string optionName;
        /// <summary>
        /// 해당 대화를 볼 수 있는 유저의 역할.
        /// </summary>
        [Tooltip("해당 채팅을 볼 수 있는 유저의 역할.")]
        /// <summary>
        /// 해당 채팅을 볼 수 있는 유저의 역할.
        /// </summary>
        public USER_ROLE[] accessibleRole;
        public USER_ROLE[] notAccessibleRole;
        
        [Tooltip("해당 아이템이 버튼을 활성화 가능한지 여부.")]
        /// <summary>
        /// 해당 아이템이 버튼을 활성화 가능한지 여부
        /// </summary>
        public bool isInteractable = false;
        /// <summary>
        /// 클릭 시 실행되는 이벤트.
        /// </summary>
        [Tooltip("클릭 시 실행되는 이벤트.")]
        public UnityEvent onClick;
    }

    public class NPCButtonUIItem : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
        public Button npcButton;
        public int index;



        public void SetItem(ChatItemOption _optionInfo, int index = 0)
        {
            buttonText.text = _optionInfo.optionName;

            // 사용자가 버튼 클릭 시 실행되어야 하는 함수 설정
            this.index = index;
            npcButton.onClick.AddListener(() =>
            {
                transform.root.GetComponent<NPCChatUI>().onButtonInItemClicked(index);
            });
            npcButton.interactable = _optionInfo.isInteractable;
            npcButton.onClick.AddListener(_optionInfo.onClick.Invoke);
        }
    }
}
