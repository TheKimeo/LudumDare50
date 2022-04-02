using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Notification", menuName = "Notification", order = 0)]
public class Notification : ScriptableObject
{
	[SerializeField] public string m_text;
	[SerializeField] public Sprite m_UI_Icon;


}
