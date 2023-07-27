using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace sexee.DevConsole
{
    public class ConsoleTitleBar : MonoBehaviour, IDragHandler
    {
        public Canvas canvas;

        private RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

}