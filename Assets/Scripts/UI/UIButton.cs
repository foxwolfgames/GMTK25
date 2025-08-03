using System;
using Chronomance.Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chronomance.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public enum UIButtonAction
        {
            Play,
            Options,
            Quit
        }

        [SerializeField] private UIButtonAction action;
        [SerializeField] private TMP_Text label;

        private Button _button;
        private bool _isMouseOver;
        private float _labelStartingSize;
        private float _labelCurrentSize;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        private void Start()
        {
            // Record starting size of the label
            _labelStartingSize = label.fontSize;
            _labelCurrentSize = label.fontSize;
        }

        private void Update()
        {
            float labelDesiredSize = _labelStartingSize * (_isMouseOver ? 1.25f : 1f);
            if (Math.Abs(label.fontSize - labelDesiredSize) > 0.01f)
            {
                _labelCurrentSize = Mathf.Lerp(_labelCurrentSize, labelDesiredSize, Time.deltaTime * 10);
                label.fontSize = _labelCurrentSize;
            }
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            _button.onClick.AddListener(() => Debug.Log("Button clicked: " + label.text));
        }

        private void OnButtonClick()
        {
            new UIButtonPressEvent(action).Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseOver = false;
        }
    }
}