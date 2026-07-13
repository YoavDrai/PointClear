using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015: minimal greybox uGUI construction helpers. The front-end is built
    /// in code (not hand-authored in the scene) so the PrototypeScene footprint
    /// stays a single GameObject and the whole UI is reviewable in C#. This is
    /// still retained-mode uGUI (Canvas + EventSystem + Button/Text/Image) — NOT
    /// OnGUI. Uses the built-in LegacyRuntime font so no TMP Essentials import is
    /// needed. Styling is deliberately plain and prototype-looking.
    /// </summary>
    public static class UIFactory
    {
        public static readonly Color ScreenBg = new Color(0.09f, 0.10f, 0.13f, 1f);
        public static readonly Color PanelBg = new Color(0.14f, 0.16f, 0.20f, 1f);
        public static readonly Color ButtonBg = new Color(0.22f, 0.25f, 0.32f, 1f);
        public static readonly Color AccentBg = new Color(0.20f, 0.45f, 0.72f, 1f);
        public static readonly Color TextPrimary = new Color(0.93f, 0.95f, 0.98f, 1f);
        public static readonly Color TextMuted = new Color(0.62f, 0.67f, 0.75f, 1f);
        public static readonly Color WarnText = new Color(0.95f, 0.55f, 0.35f, 1f);

        private static Font cachedFont;

        public static Font Font
        {
            get
            {
                if (cachedFont == null)
                {
                    cachedFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    if (cachedFont == null)
                    {
                        cachedFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    }
                }
                return cachedFont;
            }
        }

        /// <summary>Creates (or returns) an EventSystem driven by the Input System UI module.</summary>
        public static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
            {
                return;
            }

            var existing = UnityEngine.Object.FindAnyObjectByType<EventSystem>();
            if (existing != null)
            {
                return;
            }

            var go = new GameObject("EventSystem", typeof(EventSystem));
            var module = go.AddComponent<InputSystemUIInputModule>();
            // Runtime-added modules have no serialized actions; give it the default set.
            module.AssignDefaultActions();
        }

        /// <summary>A Screen-Space-Overlay canvas that scales with screen size.</summary>
        public static Canvas CreateCanvas(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // draw above any in-scene world UI

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        /// <summary>A full-screen opaque panel (one per screen). Returns its GameObject.</summary>
        public static GameObject CreatePanel(Transform parent, string name, Color bg)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            Stretch(go.GetComponent<RectTransform>());
            go.GetComponent<Image>().color = bg;
            return go;
        }

        /// <summary>A vertically-centered column with spacing, for stacking widgets.</summary>
        public static RectTransform CreateColumn(Transform parent, float width, float spacing, TextAnchor align)
        {
            var go = new GameObject("Column", typeof(RectTransform), typeof(VerticalLayoutGroup));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(width, 0f);

            var layout = go.GetComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = align;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return rt;
        }

        public static Text CreateLabel(Transform parent, string text, int fontSize, Color color, TextAnchor align, FontStyle style)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = Font;
            t.text = text;
            t.fontSize = fontSize;
            t.color = color;
            t.alignment = align;
            t.fontStyle = style;
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            FitLabelHeight(t);
            return t;
        }

        /// <summary>
        /// Sizes a label's LayoutElement height to its line count (explicit
        /// newlines), so multi-line labels reserve enough vertical space in a
        /// layout group instead of overlapping their neighbours. Call again after
        /// changing a label's text to more/fewer lines.
        /// </summary>
        public static void FitLabelHeight(Text label)
        {
            if (label == null)
            {
                return;
            }
            int lines = 1;
            string s = label.text;
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '\n')
                    {
                        lines++;
                    }
                }
            }
            SetPreferredHeight(label.gameObject, lines * (label.fontSize + 10f) + 6f);
        }

        public static Button CreateButton(Transform parent, string label, float height, Color bg, Action onClick)
        {
            var go = new GameObject("Button_" + label, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            var img = go.GetComponent<Image>();
            img.color = bg;

            var button = go.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1f, 1f, 1f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            colors.selectedColor = Color.white;
            colors.fadeDuration = 0.08f;
            button.colors = colors;
            button.targetGraphic = img;
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            SetPreferredHeight(go, height);

            var textGo = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textGo.transform.SetParent(go.transform, false);
            Stretch(textGo.GetComponent<RectTransform>());
            var t = textGo.GetComponent<Text>();
            t.font = Font;
            t.text = label;
            t.fontSize = 30;
            t.color = TextPrimary;
            t.alignment = TextAnchor.MiddleCenter;

            return button;
        }

        /// <summary>Stretch a RectTransform to fill its parent.</summary>
        public static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        /// <summary>A horizontal row with spacing, for laying widgets side by side.</summary>
        public static RectTransform CreateRow(Transform parent, float height, float spacing)
        {
            var go = new GameObject("Row", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            go.transform.SetParent(parent, false);
            var layout = go.GetComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            SetPreferredHeight(go, height);
            return go.GetComponent<RectTransform>();
        }

        /// <summary>A solid square color swatch (used for the character preset preview).</summary>
        public static Image CreateSwatch(Transform parent, float size)
        {
            var go = new GameObject("Swatch", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var le = go.AddComponent<LayoutElement>();
            le.preferredWidth = size;
            le.minWidth = size;
            le.preferredHeight = size;
            le.minHeight = size;
            return go.GetComponent<Image>();
        }

        /// <summary>
        /// A minimal greybox text field (new-Input-System driven; see NameInputField).
        /// Builds the background, value text, and placeholder, and wires them up.
        /// </summary>
        public static NameInputField CreateNameInput(Transform parent, string placeholder, float height, int limit)
        {
            var go = new GameObject("NameInput", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(NameInputField));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = new Color(0.20f, 0.22f, 0.27f, 1f);
            SetPreferredHeight(go, height);

            var valueGo = new GameObject("Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            valueGo.transform.SetParent(go.transform, false);
            var vrt = valueGo.GetComponent<RectTransform>();
            Stretch(vrt);
            vrt.offsetMin = new Vector2(16f, 4f);
            vrt.offsetMax = new Vector2(-16f, -4f);
            var vt = valueGo.GetComponent<Text>();
            vt.font = Font;
            vt.fontSize = 32;
            vt.color = TextPrimary;
            vt.alignment = TextAnchor.MiddleLeft;
            vt.supportRichText = false;

            var phGo = new GameObject("Placeholder", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            phGo.transform.SetParent(go.transform, false);
            var prt = phGo.GetComponent<RectTransform>();
            Stretch(prt);
            prt.offsetMin = new Vector2(16f, 4f);
            prt.offsetMax = new Vector2(-16f, -4f);
            var pt = phGo.GetComponent<Text>();
            pt.font = Font;
            pt.fontSize = 30;
            pt.color = TextMuted;
            pt.alignment = TextAnchor.MiddleLeft;
            pt.fontStyle = FontStyle.Italic;
            pt.text = placeholder;

            var field = go.GetComponent<NameInputField>();
            field.Configure(vt, pt, limit);
            field.SetValue(string.Empty);
            return field;
        }

        private static void SetPreferredHeight(GameObject go, float height)
        {
            var le = go.GetComponent<LayoutElement>();
            if (le == null)
            {
                le = go.AddComponent<LayoutElement>();
            }
            le.preferredHeight = height;
            le.minHeight = height;
        }
    }
}
