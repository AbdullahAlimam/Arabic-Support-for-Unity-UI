using UnityEngine;
using System.Collections;
using ArabicSupport;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ArabicText : MonoBehaviour
{
    [Multiline]
    public string Text;
    public bool ShowTashkeel;
    public bool UseHinduNumbers;

    private UnityEngine.UI.Text txt;

    private string OldText; // For Refresh on TextChange
    private int OldFontSize; // For Refresh on Font Size Change
    private RectTransform rectTransform;  // For Refresh on resize
    private Vector2 OldDeltaSize; // For Refresh on resize
    private bool OldEnabled = false; // For Refresh on enabled change // when text ui is not active then arabic text will not trigered when the control get active
    private List<RectTransform> OldRectTransformParents = new List<RectTransform>(); // For Refresh on parent resizing
    private Vector2 OldScreenRect = new Vector2(Screen.width, Screen.height); // For Refresh on screen resizing
    //public int NumberOfRedraws = 0;
    public void Awake()
    {
        GetRectTransformParents(OldRectTransformParents);
    }

    public void Start()
    {
        txt = gameObject.GetComponent<UnityEngine.UI.Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void GetRectTransformParents(List<RectTransform> rectTransforms)
    {
        rectTransforms.Clear();
        for (Transform parent = transform.parent; parent != null; parent = parent.parent)
        {
            GameObject goP = parent.gameObject;
            RectTransform rect = goP.GetComponent<RectTransform>();
            if (rect) rectTransforms.Add(rect);
        }
    }

    private bool CheckRectTransformParentsIfChanged()
    {
        List<RectTransform> CurrentRectTransformParents = new List<RectTransform>();
        GetRectTransformParents(CurrentRectTransformParents);

        if (CurrentRectTransformParents.Count != OldRectTransformParents.Count)
            return true;

        for (int i = 0; i < CurrentRectTransformParents.Count; i++)
        {
            if (CurrentRectTransformParents[i] != OldRectTransformParents[i])
                return true;

            if (!RectTransform.Equals(CurrentRectTransformParents[i], OldRectTransformParents[i]))
                return true;
        }

        return false;
    }

    public void Update()
    {
        if (OldText == Text &&
            OldFontSize == txt.fontSize &&
            OldDeltaSize == rectTransform.sizeDelta &&
            OldEnabled == txt.enabled &&
            (OldScreenRect.x == Screen.width && OldScreenRect.y == Screen.height &&
            !CheckRectTransformParentsIfChanged()) ) // Not Need Refresh
            return;

        //NumberOfRedraws++;

        FixTextForUI();

        OldText = Text;
        OldFontSize = txt.fontSize;
        OldDeltaSize = rectTransform.sizeDelta;
        OldEnabled = txt.enabled;
        OldScreenRect.x = Screen.width;
        OldScreenRect.y = Screen.height;
        GetRectTransformParents(OldRectTransformParents);
    }

    public void FixTextForUI()
    {
        if (!string.IsNullOrEmpty(Text))
        {
            string rtlText = ArabicSupport.ArabicFixer.Fix(Text, ShowTashkeel, UseHinduNumbers);
            rtlText = rtlText.Replace("\r", ""); // the Arabix fixer Return \r\n for everyy \n .. need to be removed

            string finalText = "";
            string[] rtlParagraph = rtlText.Split('\n');

            txt.text = "";
            for (int lineIndex = 0; lineIndex < rtlParagraph.Length; lineIndex++)
            {
                string[] words = rtlParagraph[lineIndex].Split(' ');
                Array.Reverse(words);
                txt.text = string.Join(" ", words);

                Canvas.ForceUpdateCanvases();
                for (int i = 0; i < txt.cachedTextGenerator.lines.Count; i++)
                {
                    int startIndex = txt.cachedTextGenerator.lines[i].startCharIdx;
                    int endIndex = (i == txt.cachedTextGenerator.lines.Count - 1) ? txt.text.Length
                        : txt.cachedTextGenerator.lines[i + 1].startCharIdx;
                    int length = endIndex - startIndex;

                    string[] lineWords = txt.text.Substring(startIndex, length).Split(' ');
                    Array.Reverse(lineWords);

                    finalText = finalText + string.Join(" ", lineWords).Trim() + "\n";
                }
            }
            txt.text = finalText.TrimEnd('\n');
        }
        else
            txt.text = "";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArabicText))]
public class ArabicTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArabicText myScript = (ArabicText)target;
        if (GUILayout.Button("Refresh"))
        {
            myScript.Start(); // init Varables
            myScript.FixTextForUI();
        }
    }
}
#endif