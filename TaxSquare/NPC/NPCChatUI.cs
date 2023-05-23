
using SCM.Platform.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SCM.Platform.Common.UI
{
    public class NPCChatUI : ManagedUIComponent<NPCChatUI>
    {
        public GameObject npcCanvas;
        public NPCManageUI npcManageUI;
        [SerializeField] private bool use3DModel = false;

        [Foldout("UI/NPC")]
        public Image npcImage;
        public TextMeshProUGUI npcNameText;

        [Foldout("Chat")]
        public RectTransform chatItemParentTransform;

        [Header("Chatting Item")]
        public GameObject npcChatItem;
        public GameObject userChatItem;
        public ScrollRect chatScrollRect;

        [HideInInspector]
        public ChatItemOption returnOption;
        /// <summary>
        /// NPC Chat에 나타날 chat item을 보관하는 변수.
        /// </summary>
        public List<(GameObject chatItemPrefab, string text, ChatItemOption[] option)> container = 
            new List<(GameObject chatItemPrefab, string text, ChatItemOption[] option)>();

        [HideInInspector]
        public Action<int> onButtonInItemClicked;
        
        [HideInInspector]
        public static readonly string introduceComment = "안녕하세요. 무엇을 도와드릴까요?";
        private int idx;

        protected override ManagedUIProperties UIProperties => ManagedUIProperties.LOCK_KEYBOARD;

        protected override void OnEnable()
        {
            base.OnEnable();
            returnOption = new ChatItemOption() {
                accessibleRole = USER_ROLE_REPRESENTATION.ALL,
                optionName = "돌아가기",
                isInteractable = true,
                onClick = new UnityEvent()
            };
            returnOption.onClick.AddListener(ShowMenus);
            onButtonInItemClicked += GetIndex;
        }

        protected override void OnDisable()
        {
            ClearChat();
            base.OnDisable();
        }

        public void ClearChat()
        {
            for (int i = chatItemParentTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(chatItemParentTransform.GetChild(i).gameObject);
            }
        }

        public void CloseNPCCanvas()
        {
            npcCanvas.SetActive(false);

            SoundManager.Instance.PlaySE("슥-");
        }

        /// <summary>
        /// NPC 대화 열기(설정)
        /// </summary>
        public void OpenNPCChat()
        {
            ClearChat();
            npcCanvas.SetActive(true);

            // NPC 정보, 카테고리 등의 다양한 내용을 받아와서 설정 진행
            if (!use3DModel)
            {
                npcImage.sprite = npcManageUI.thumbnailImage;
            }

            
            npcNameText.text = npcManageUI.npcName;

            container.Add((npcChatItem, "안녕하세요. 무엇을 도와드릴까요?", npcManageUI.optionInfoArray));
            ShowContainer();
        }

        /// <summary>
        /// <see cref="NPCManageUI"/>에 정의된 <see cref="NPCManageUI.optionInfoArray"/>의 option들을 호출하여 
        /// 초기 UI에 해당하는 NPC측 메뉴를 생성하는 함수.
        /// </summary>
        public void ShowMenus()
        {
            ClearChat();
            
            container.Add((npcChatItem, "안녕하세요. 무엇을 도와드릴까요?", npcManageUI.optionInfoArray));
            ShowContainer();
        }

        /// <summary>
        /// NPC의 Chat에 설정된 아이템들을 보여주는 함수. <see cref="container"/>에 지정된 chat item들을 인스턴스화 한다.
        /// </summary>
        public void ShowContainer()
        {
            ClearChat();
            foreach (var item in container)
            {
                GameObject chatItem = Instantiate(item.chatItemPrefab, chatItemParentTransform);
                if (chatItem.TryGetComponent<IChatUIItem>(out IChatUIItem chatUIItem))
                {
                    chatUIItem.SetItem(item.text, item.option);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(chatItem.GetComponent<RectTransform>());
                    StartCoroutine(SetChatPosition());                    
                }
            }
            container.Clear();
        }

        /// <summary>
        /// NPC 측에서 나타나는 대화. (serializable)
        /// </summary>
        /// <param name="contents">대화창에 보여지는 텍스트</param>
        public void ChatNPCSide(string contents, bool hasReturn = true)
        {
            // 호환성을 위해 return option을 추가.
            if (hasReturn)
                container.Add((npcChatItem, contents, new ChatItemOption[] {returnOption}));
            else
                container.Add((npcChatItem, contents, new ChatItemOption[] { null }));
        }

        /// <summary>
        /// NPC 측에서 나타나는 대화.
        /// </summary>
        /// <param name="text">대화창에 보여지는 텍스트</param>
        /// <param name="option"><see cref="UI.ChatItemOption"/>에 정의된 대화창 옵션. <see langword="null"/>일 경우, button이 추가되지 않는다.</param>
        public void ChatNPCSide(string text, ChatItemOption option)
        {
            container.Add((npcChatItem, text, new ChatItemOption[] {option}) );
        }

        /// <summary>
        /// NPC측에서 나타나는 대화, <see cref="ChatItemOption"/>을 직접 지정할 수 있다.
        /// </summary>
        /// <param name="text">채팅에 보여지는 텍스트</param>
        /// <param name="options">채팅에 정의된 대화창 옵션, 여러개의 옵션을 지정 가능하다.</param>
        public void ChatNPCSide(string text, ChatItemOption[] options)
        {
            container.Add((npcChatItem, text, options) );
        }

        /// <summary>
        /// NPC측에서 나타나는 채팅. 최초 채팅에서 나타난 아이템을 선택하여 보여준다.
        /// </summary>
        /// <param name="text">채팅에서 보여지는 텍스트</param>
        public void ChatNPCSideFirstItemOfSelected(string text)
        {
            var selected = npcManageUI.optionInfoArray[idx];
            ChatItemOption tmp = new ChatItemOption()
            {
                optionName = selected.optionName,
                accessibleRole = selected.accessibleRole,
                isInteractable = false,
                onClick = selected.onClick
            };
            container.Add((npcChatItem, text, new ChatItemOption[] {tmp}));
        }
        
        /// <summary>
        /// NPC측에서 나타나는 채팅. 채팅을 보여주고, 이전 채팅으로 돌아간다. 일반적으로 UnityAction List의 
        /// 마지막에 위치한다.
        /// </summary>
        /// <param name="contents">채팅에서 보여지는 텍스트</param>
        public void ChatNPCSideWithReturn(string contents)
        {
            container.Add((npcChatItem, contents, new ChatItemOption[] {returnOption}) );
            ShowContainer();
        }

        /// <summary>
        /// USER 측에서 나타나는 대화.
        /// </summary>
        /// <param name="text">대화창에 보여지는 텍스트</param>
        public void ChatUserSide(string text)
        {
            container.Add((userChatItem, text, new ChatItemOption[] {}));
        }

        /// <summary>
        /// 유저측에서 나타나는 채팅. 채팅을 보여주고, 이전 채팅으로 돌아간다. 일반적으로 UnityAction List의
        /// 마지막에 위치한다.
        /// </summary>
        /// <param name="text"></param>
        public void ChatUserSideWithReturn(string text)
        {
            container.Add((userChatItem, text, new ChatItemOption[] {returnOption}));
            ShowContainer();
        }

        /// <summary>
        /// content 사이즈(스크롤) 리로드 진행
        /// </summary>
        public IEnumerator SetChatPosition()
        {
            yield return null;

            LayoutRebuilder.ForceRebuildLayoutImmediate(chatItemParentTransform);
            chatScrollRect.normalizedPosition = new Vector2(0, 0);
        }
        

        /// <summary>
        /// TODO: insert this.
        /// </summary>
        /// <param name="chatOptions"></param>
        /// <param name="toAdds"></param>
        public void AddItemToChatOptions(ref ChatItemOption[] chatOptions, ChatItemOption[] toAdds, int index = 1)
        {
            var tmp = new List<ChatItemOption>();
            if (chatOptions.Length < 1)
            {
                tmp.AddRange(toAdds);
                chatOptions = tmp.ToArray();
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    tmp.Add(chatOptions[i]);
                }
                tmp.AddRange(toAdds);
                for (int i = index; i < chatOptions.Length; i++)
                {
                    tmp.Add(chatOptions[i]);
                }
                chatOptions = tmp.ToArray();
            }
        }

        public bool ReplaceItemOfChatOptions(ref ChatItemOption[] chatOptions, string oldValue, ChatItemOption newValue)
        {
            if (chatOptions == null || chatOptions.Length == 0)
                return false;

            int idx = -1;
            for (int i = 0; i < chatOptions.Length; i++)
            {
                if (chatOptions[i].optionName.Equals(oldValue))
                    idx = i;
            }

            if (idx == -1)
                return false;
            
            chatOptions.SetValue(newValue, idx);
            return true;
        }

        public void GetIndex(int idx)
        {
            this.idx = idx;
        }


    }
}
