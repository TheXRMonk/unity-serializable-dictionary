﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SerializableDictionaryPropertyDrawer<TKey, TValue> : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
		label = EditorGUI.BeginProperty(position, label, property);

		// EditorGUI.PropertyField(position, property, label, false);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
		if (property.isExpanded)
		{
			var keysProperty = property.FindPropertyRelative("m_keys");
			var valuesProperty = property.FindPropertyRelative("m_values");

        	EditorGUI.indentLevel++;
			var linePosition = EditorGUI.IndentedRect(position);
			linePosition.y += EditorGUIUtility.singleLineHeight;

			int n = keysProperty.arraySize;
			for(int i = 0; i < n; ++i)
			{
				var keyProperty = keysProperty.GetArrayElementAtIndex(i);
				var valueProperty = valuesProperty.GetArrayElementAtIndex(i);
				float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
				float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);

				float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
				linePosition.height = lineHeight;

				var keyPosition = linePosition;
				keyPosition.xMax = EditorGUIUtility.labelWidth;
				EditorGUI.PropertyField(keyPosition, keyProperty, GUIContent.none, false);

				var valuePosition = linePosition;
				valuePosition.xMin = EditorGUIUtility.labelWidth;
				EditorGUI.PropertyField(valuePosition, valueProperty, GUIContent.none, false);

				linePosition.y += lineHeight;
			}

	        EditorGUI.indentLevel--;
		}

        EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (property.isExpanded)
        	return EditorGUIUtility.singleLineHeight * (1 + property.FindPropertyRelative("m_keys").arraySize);
		else
        	return EditorGUIUtility.singleLineHeight;
    }
}

[CustomPropertyDrawer(typeof(DictionaryTest.StringDictionary))]
public class StringDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer<string, string>
{
}