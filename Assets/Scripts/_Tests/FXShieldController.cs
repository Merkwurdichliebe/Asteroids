using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXShieldController : MonoBehaviour {

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.speed = Random.Range(0.2f, 2.0f);
		animator.Play("ShieldAnimationClip", 0, Random.Range(0, 1f));
	}
}
