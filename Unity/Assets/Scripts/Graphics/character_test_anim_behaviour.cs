﻿using UnityEngine;
using System.Collections;

public class character_test_anim_behaviour : MonoBehaviour 
{
	public AudioClip footstepSound;
	public AudioClip stumbleSound;

	CustomCharacterController cc;
    Rigidbody rb;
		
	Animator anim;
	public float animSpeedFactorWalking = 0.8f;
	public float animSpeedFactorRunning = 0.3f;
	public float speedThresholdRun = 2.5f;
	public float framesBetweenStepsWalking = 10;
	public float framesBetweenStepsRunning = 8;
	FootTargetBehaviour lf;
	FootTargetBehaviour rf;

	void Start () 
	{
		anim = GetComponent<Animator> ();
		cc = GetComponentInParent<CustomCharacterController> ();
        rb = GetComponentInParent<Rigidbody> ();
		lf = cc.leftFoot.GetComponent<FootTargetBehaviour> ();
		rf = cc.rightFoot.GetComponent<FootTargetBehaviour> ();
		lf.stepUpTolerance = cc.stepUpTolerance;
		lf.stepDownTolerance = cc.stepDownTolerance;
		rf.stepUpTolerance = cc.stepUpTolerance;
		rf.stepDownTolerance = cc.stepDownTolerance;
	}

	float PlayerSpeed()
	{
		return rb.velocity.magnitude;
	}

	void Update () 
	{
		if (PlayerSpeed() > 0.1f)	//rather than != 0 (just to prevent noise triggering animations)
		{
			//Play walk animation when player speed is slow
			if (PlayerSpeed() < speedThresholdRun) 
			{
				AnimSetWalking();
			} 
			//Play run animation when player speed is faster
			else 
			{
				AnimSetRunning();
			} 
		}
		//Play idle animation when not moving
		else 
		{
			AnimSetIdle();
		}

		//Prevent run animations while falling (maybe even play falling animation??)
		if (!cc.IsGrounded ()) 
		{
			//AnimSetIdle();
		}
	
		//if (jump != 0) 
		//{
		//	anim.SetBool("jump", true);
		//}
		//else
		//{
		//	anim.SetBool("jump", false);
		//}


	}

	void AnimSetIdle()
	{
		anim.SetBool ("walking", false);
		anim.SetBool ("running", false);
		anim.speed = 1.0f;
		lf.currentPlayingAnimation = "idle";
		rf.currentPlayingAnimation = "idle";
	}

	void AnimSetWalking()
	{
		anim.SetBool ("walking", true);
		anim.SetBool ("running", false);
		anim.speed = Mathf.Min(PlayerSpeed (), 5F) * animSpeedFactorWalking / (rb.velocity.y != 0 ? (0.001F * Mathf.Pow(cc.RelativeSlopeAngleDeg(), 2) + 1) : 1);
		lf.currentPlayingAnimation = "walking";
		rf.currentPlayingAnimation = "walking";
	}

	void AnimSetRunning()
	{
		anim.SetBool ("walking", false);
		anim.SetBool ("running", true);
		anim.speed = Mathf.Min(PlayerSpeed(), 5F) * animSpeedFactorRunning / (rb.velocity.y != 0 ? (0.0005F * Mathf.Pow(cc.RelativeSlopeAngleDeg(), 2) + 1) : 1);
		lf.currentPlayingAnimation = "running";
		rf.currentPlayingAnimation = "running";
	}

	public void Event_LeftFootLift() {
		lf.DetermineTarget ();
	}

	public void Event_RightFootLift() {
		rf.DetermineTarget ();
	}

	public void Event_LeftFootLand() {
		lf.HandleStep ();
	}
	
	public void Event_RightFootLand() {
		rf.HandleStep ();
	}

	public void Event_LeftFootBack() {
		lf.MoveToBack ();
	}

	public void Event_RightFootBack() {
		rf.MoveToBack ();
	}


	public void PlayFootstep() {}

	public void NewEvent() {}
}