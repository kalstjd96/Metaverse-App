using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SCM.Platform.Counsel;

namespace SCM.Platform.Common.UI
{
    public class NPCManageUI : MonoBehaviour
    {
        public int id;
        public string npcName;
        public RectTransform npcNameArea;
        public TextMeshProUGUI npcNameText;
        public Renderer[] renderers;
        public Sprite thumbnailImage;
        public ChatItemOption[] optionInfoArray;



        protected virtual void Start()
        {
            npcNameText.text = npcName;

            if (renderers == null || renderers.Length < 1)
                return;

            Bounds bounds = renderers[0].bounds;

            // 렌더러의 수가 2개 이상인 경우에는 InCapsulate를 호출해야한다.
            foreach (var r in renderers)
            {
                bounds.Encapsulate(r.bounds);
            }

            npcNameArea.localPosition = new Vector3(0, 
                bounds.size.y + 0.3f, 0);
        }
    }
}
