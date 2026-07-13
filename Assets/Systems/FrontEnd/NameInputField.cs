using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015: a minimal greybox text field driven by the new Input System. The
    /// project's Active Input Handling is "Input System" only, so the legacy
    /// uGUI InputField — which reads the old Input.inputString — cannot receive
    /// typed characters. This is intentionally NOT a full input field (no
    /// selection, clipboard, or mouse-positioned caret); it is just enough to
    /// name a prototype character: click to focus, type via Keyboard.current,
    /// Backspace to delete, Enter to submit.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class NameInputField : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text valueText;
        [SerializeField] private Text placeholderText;
        [SerializeField] private int characterLimit = 24;

        public string Value { get; private set; } = string.Empty;
        public bool IsFocused { get; private set; }

        /// <summary>Raised whenever the text changes (typing, backspace, or SetValue).</summary>
        public event Action<string> ValueChanged;
        /// <summary>Raised when the user presses Enter.</summary>
        public event Action Submitted;

        private float caretBlinkTimer;
        private bool caretVisible;

        public void Configure(Text value, Text placeholder, int limit)
        {
            valueText = value;
            placeholderText = placeholder;
            characterLimit = Mathf.Max(1, limit);
        }

        public void SetValue(string value)
        {
            Value = value ?? string.Empty;
            if (Value.Length > characterLimit)
            {
                Value = Value.Substring(0, characterLimit);
            }
            Render();
            ValueChanged?.Invoke(Value);
        }

        public void SetFocused(bool focused)
        {
            if (IsFocused == focused)
            {
                return;
            }

            IsFocused = focused;
            Keyboard kb = Keyboard.current;
            if (kb != null)
            {
                if (focused)
                {
                    kb.onTextInput += HandleTextInput;
                }
                else
                {
                    kb.onTextInput -= HandleTextInput;
                }
            }

            caretVisible = focused;
            caretBlinkTimer = 0f;
            Render();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SetFocused(true);
        }

        private void OnDisable()
        {
            if (IsFocused)
            {
                Keyboard kb = Keyboard.current;
                if (kb != null)
                {
                    kb.onTextInput -= HandleTextInput;
                }
                IsFocused = false;
                caretVisible = false;
            }
        }

        // Printable characters only. Backspace ('\b') and Enter arrive here as
        // control chars on some platforms — they are filtered out here and
        // handled deterministically in Update instead, so there is no risk of a
        // double delete.
        private void HandleTextInput(char c)
        {
            if (c < ' ' || c == (char)127)
            {
                return;
            }
            if (Value.Length >= characterLimit)
            {
                return;
            }
            Value += c;
            Render();
            ValueChanged?.Invoke(Value);
        }

        private void Update()
        {
            if (!IsFocused)
            {
                return;
            }

            Keyboard kb = Keyboard.current;
            if (kb == null)
            {
                return;
            }

            if (kb.backspaceKey.wasPressedThisFrame && Value.Length > 0)
            {
                Value = Value.Substring(0, Value.Length - 1);
                Render();
                ValueChanged?.Invoke(Value);
            }

            if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame)
            {
                SetFocused(false);
                Submitted?.Invoke();
                return;
            }

            caretBlinkTimer += Time.unscaledDeltaTime;
            if (caretBlinkTimer >= 0.5f)
            {
                caretBlinkTimer = 0f;
                caretVisible = !caretVisible;
                Render();
            }
        }

        private void Render()
        {
            bool empty = string.IsNullOrEmpty(Value);
            if (placeholderText != null)
            {
                placeholderText.enabled = empty && !IsFocused;
            }
            if (valueText != null)
            {
                valueText.text = Value + (IsFocused && caretVisible ? "|" : string.Empty);
            }
        }
    }
}
