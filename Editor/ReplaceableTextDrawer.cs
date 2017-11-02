using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PDYXS.TextReplacement
{
    [CustomPropertyDrawer(typeof(ReplaceableTextAttribute))]
    public class ReplaceableTextDrawer : PropertyDrawer
    {
        private const int TEXTAREA_HEIGHT = 80;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2 + TEXTAREA_HEIGHT;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            var baseHeight = base.GetPropertyHeight(property, label);
            EditorGUI.LabelField(new Rect(pos.x, pos.y, pos.width, baseHeight), label, EditorStyles.boldLabel);
            pos = new Rect(pos.x, pos.y + baseHeight, pos.width, pos.height - baseHeight);

            property.stringValue = EditorGUI.TextArea(new Rect(pos.x, pos.y, pos.width, TEXTAREA_HEIGHT),
                                                      property.stringValue, EditorStyles.textArea);
            pos = new Rect(pos.x, pos.y + TEXTAREA_HEIGHT, pos.width, pos.height - TEXTAREA_HEIGHT);
        }
    }
}