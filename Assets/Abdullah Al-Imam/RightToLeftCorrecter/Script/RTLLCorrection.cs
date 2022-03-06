﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity3D_Arabic;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RTLLCorrection : MonoBehaviour
{
    [Multiline]
    public string Text;
    public InputField RefrenceInput;
    //public bool ShowTashkeel;
    public NumberStyles NumberStyle = NumberStyles.Hindu;
    public bool FlipBracket = true;

    private NumberStyles oldNumberStyle = NumberStyles.Hindu;
    private bool oldFlipBracket = true;

    private Text txt;

    private string OldText; // For Refresh on TextChange
    private int OldFontSize; // For Refresh on Font Size Change
    private RectTransform rectTransform;  // For Refresh on resize
    private Vector2 OldDeltaSize; // For Refresh on resize
    private bool OldEnabled = false; // For Refresh on enabled change // when text ui is not active then arabic text will not trigered when the control get active
    private List<RectTransform> OldRectTransformParents = new List<RectTransform>(); // For Refresh on parent resizing
    private Vector2 OldScreenRect = new Vector2(Screen.width, Screen.height); // For Refresh on screen resizing
    
    public void Awake()
    {
        GetRectTransformParents(OldRectTransformParents);
    }

    public void Start()
    {
        txt = gameObject.GetComponent<UnityEngine.UI.Text>();
        rectTransform = GetComponent<RectTransform>();
        oldNumberStyle = NumberStyle;
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
        bool hasChanged = false;
        for (int i = 0; i < OldRectTransformParents.Count; i++)
        {
            hasChanged |= OldRectTransformParents[i].hasChanged;
            OldRectTransformParents[i].hasChanged = false;
        }

        return hasChanged;
    }

    public void Update()
    {
        if (!txt)
            return;

        if (RefrenceInput)
            Text = RefrenceInput.text;

        // if No Need to Refresh
        if (OldText == Text &&
            OldFontSize == txt.fontSize &&
            OldDeltaSize == rectTransform.sizeDelta &&
            OldEnabled == txt.enabled &&
            (OldScreenRect.x == Screen.width && OldScreenRect.y == Screen.height &&
            oldNumberStyle == NumberStyle &&
            oldFlipBracket == FlipBracket &&
            !CheckRectTransformParentsIfChanged()))
            return;


        Correct();

        OldText = Text;
        oldNumberStyle = NumberStyle;
        oldFlipBracket = FlipBracket;
        OldFontSize = txt.fontSize;
        OldDeltaSize = rectTransform.sizeDelta;
        OldEnabled = txt.enabled;
        OldScreenRect.x = Screen.width;
        OldScreenRect.y = Screen.height;
    }

    public void Correct()
    {
        if (!string.IsNullOrEmpty(Text))
        {
            string rtlText = RTLLCorrecter.Correct(Text, FlipBracket, NumberStyle);

            string finalText = "";
            string[] rtlParagraph = rtlText.Split(RTLLCorrecter.NewLine);

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

                    finalText = finalText + string.Join(" ", lineWords).Trim() + RTLLCorrecter.NewLine;
                }
            }
            txt.text = finalText.TrimEnd(RTLLCorrecter.NewLine);
        }
        else if(txt)
            txt.text = "";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RTLLCorrection))]
public class RTLLCorrectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RTLLCorrection myScript = (RTLLCorrection)target;
        if (GUILayout.Button("Refresh"))
        {
            myScript.Start(); // init Varables
            myScript.Update();
        }
    }
}
#endif