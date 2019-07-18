using UnityEditor;
using UnityEngine;

namespace UME {
	
	[CustomEditor(typeof(AbilityTrigger))]
	[CanEditMultipleObjects]
	public class UIAbilityEditor : Editor
	{

		SerializedProperty type;
		SerializedProperty activate;
		SerializedProperty value;
		SerializedProperty duration;

		void onEnable()
		{
		}

		public override void OnInspectorGUI()
		{

			AbilityTrigger trigger = (AbilityTrigger)target;

			type = serializedObject.FindProperty("type");
			duration = serializedObject.FindProperty("duration");


			EditorGUILayout.PropertyField(type, new GUIContent("Type"));
			EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));


			serializedObject.ApplyModifiedProperties();
		}
	}

}