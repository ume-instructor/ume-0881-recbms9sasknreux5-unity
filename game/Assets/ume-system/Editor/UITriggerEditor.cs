using UnityEditor;
using UnityEngine;
namespace UME
{

    [CustomEditor(typeof(UITrigger))]
    [CanEditMultipleObjects]
    public class UITriggerEditor : Editor
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

            UITrigger uiTrigger = (UITrigger)target;

            type = serializedObject.FindProperty("type");
            activate = serializedObject.FindProperty("activate");
            value = serializedObject.FindProperty("value");
            duration = serializedObject.FindProperty("duration");


            EditorGUILayout.PropertyField(type, new GUIContent("Type"));

            switch(uiTrigger.type)
            {
                case UITriggerType.message:
					EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                    EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                    EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));
                    break;

			    case UITriggerType.health:
					    EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                        EditorGUILayout.PropertyField(value, new GUIContent("Health"));
                        break;

			    case UITriggerType.score:
					    EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                        EditorGUILayout.PropertyField(value, new GUIContent("Score"));
                        break;

			    case UITriggerType.time:
					    EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                        EditorGUILayout.PropertyField(value, new GUIContent("Time"));
                        break;

			    case UITriggerType.win:
					    EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                        EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                        break;

			    case UITriggerType.lose:
					    EditorGUILayout.PropertyField(activate, new GUIContent("Activate"));
                        EditorGUILayout.PropertyField(value, new GUIContent("Message"));
                        break;

            default:
                    Debug.Log("Invalid UI Trigger Type");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}