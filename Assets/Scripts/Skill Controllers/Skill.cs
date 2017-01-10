using UnityEngine;

using System.Collections;



public abstract class Skill : MonoBehaviour
{
	public string skillName;
	public string description;
	public float coolDown;
	public float channelTime;
	public Figure caster;
	public bool canCast;
	
	public abstract void Cast();
	public abstract void EndCasting();
}