
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using System;

namespace SCM.Platform.InteractableObject
{
    /// <summary>
    /// 오브젝트의 공통 부분을 가지고있는 부모 컴포넌트
    /// 상호작용 가능한 오브젝트들은 이 클래스를 상속하여 사용한다.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        public Renderer[] renderers;

        [Foldout("Icon Setting", beforeSpace: 15)]
        // Icon과 Blur를 담은 GameObject
        public GameObject iconGroup;
        public Transform iconTransform;
        public SpriteRenderer iconRenderer;
        public Sprite interactableIconImage;

        [Foldout("Model Setting", false)]
        public GameObject objectPhysics;

        protected Outlinable outline;
        // 인터랙션 중에는 아웃라인을 막기 위한 변수(ex.의자에 앉을 경우 등)
        protected bool isOutlinable = true;

        [HideInInspector] public bool isActive = true;

        public Action<bool> changeStateEvent;

        /// <summary>
        /// 특정 상호작용을 실행하는 함수. 상호작용 가능한 컴포넌트마다 개별적으로 구현할 수 있다.
        /// </summary>
        public abstract void Interaction();

        // (22.08.11) 노도현 - 아웃라인 오브젝트 세팅 
        private void Awake()
        {
            outline = gameObject.GetComponent<Outlinable>();           
            if(outline != null)
            {
                outline.enabled = false;
                //outline.OutlineParameters.Color = Color.green;

                outline.RenderStyle = RenderStyle.FrontBack;
                outline.BackParameters.Enabled = false;
                outline.FrontParameters.Color = Color.green;
            }
        }

        // (22.08.11) 노도현 - 에디터 전용 함수(자식 렌더러들을 모두 가져옴)
        [ContextMenu("FindRenderer")]
        public void FindRenderer()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        protected virtual void Start()
        {
            if(iconRenderer != null)
                iconRenderer.sprite = interactableIconImage;


            if (renderers == null || renderers.Length == 0)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD                
                Logging.Log($"renderer is null in {gameObject.name} <color=red>(only shows in editor or development build)</color>.");
#endif                
                return;
            }

            var bounds = renderers[0].bounds;

            // 렌더러의 수가 2개 이상인 경우에는 InCapsulate를 호출해야한다.
            foreach (var r in renderers)
            {
                bounds.Encapsulate(r.bounds);             
            }

            // (22.08.10 추가) iconTransform이 없을 경우 에만 위치 조정 
            if(iconTransform == null)
            {
                if(iconGroup != null)
                {
                    iconGroup.transform.position = new Vector3(objectPhysics.transform.position.x,
                       objectPhysics.transform.position.y + bounds.size.y + 1F,
                       objectPhysics.transform.position.z);
                }
       

            }


            // 아이콘 그룹의 위치를 메시의 높이에 따라 재배치한다. sclae에도 영향을 받으므로 scale도 곱해준다.
            // x, z의 위치는 오브젝트의 위치. 높이는 메시의 높이


            // 기본 아이콘 모두 숨김 상태
            if(iconGroup != null)
                iconGroup.gameObject.SetActive(false);
        }

        public virtual void SetActive(bool isActive)
        {
            if (!isActive)
                InteractableManager.Instance.RemoveInteractableObject(this);
            this.isActive = isActive;
        }

        /// <summary>
        /// 인터랙션 상태 변경 => 활성/비활성
        /// <summary>
        public virtual void ChangeInteractionState(bool flag)
        {
            flag &= isActive;
            if(iconGroup != null)
                iconGroup.gameObject.SetActive(flag);
            if(isOutlinable)
                SetOutline(flag);
            changeStateEvent?.Invoke(flag);
        }

        /// <summary>
        /// 아웃라인 on/off
        /// </summary>
        /// <param name="flag"></param>
        private void SetOutline(bool flag)
        {
            if (outline != null)
                outline.enabled = flag;
        }

        /// <summary>
        /// 아웃라인 + 아웃라인 가능 여부 on/off
        /// </summary>
        /// <param name="flag"></param>
        public void SetOutlineMode(bool flag)
        {
            isOutlinable = flag;
            SetOutline(flag);
        }
      
    }
}
