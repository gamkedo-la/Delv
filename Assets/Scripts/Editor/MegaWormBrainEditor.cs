
using UnityEditor;

[CustomEditor(typeof(MegaWormBrain))]
public class MegaWormBrainEditor : Editor
{
	public override void OnInspectorGUI()
	{
		SerializedProperty personalityProperty = serializedObject.FindProperty("personality");
		SerializedProperty mainProperty = serializedObject.FindProperty("main");

		EditorGUILayout.PropertyField(personalityProperty);

		if (mainProperty.boolValue == true)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("hitPoints"));
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("initialDistance"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("startDelay"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("attackDelay"));

		if (personalityProperty.enumValueIndex == 1)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("targetFollow"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lineShowDelay"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lineMinSize"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidthFactor"));
		}
		else if (personalityProperty.enumValueIndex == 2)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("playfulMaxGap"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("playfulMinGap"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("playfulGapTransition"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("playfulAngleChange"));
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("playerToAttackIndex"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("alternate"));
		EditorGUILayout.PropertyField(mainProperty);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("DamagedParticle"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("HeadAni"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("HealthBar"));

		serializedObject.ApplyModifiedProperties();
	}
}
