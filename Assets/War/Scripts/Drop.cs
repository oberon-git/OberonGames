using UnityEngine;
using UnityEngine.EventSystems;

namespace War
{
    public class Drop : MonoBehaviour, IDropHandler
    {
        #region Drop Handlers

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped.TryGetComponent<Drag>(out var drag))
            {
                drag.ParentAfterDrag = transform;
            }
        }

        #endregion
    }
}
