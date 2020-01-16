﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Behavior for wallmount sprites.
///
/// Looks at the way a wallmount is facing by using the parent gameobject's Directional.CurrentDirection.
///
/// Wallmount sprites are occluded like all the other objects, however they should only be visible on the
/// side of the wall that they are on. The occlusion logic keeps the nearest wall visible so we must
/// check which side of the wall the mount is on so we can hide it if it's on the wrong side.
/// This behavior makes the wallmount invisible if it is not facing towards the player.
/// </summary>
public class WallmountSpriteBehavior : MonoBehaviour {
	// This sprite's renderer
	private SpriteRenderer spriteRenderer;
	//parent wallmount behavior
	private WallmountBehavior wallmountBehavior;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		wallmountBehavior = GetComponentInParent<WallmountBehavior>();
		//some of the mapped ones have duplicates of this, this ensures they're removed
		foreach (var wsb in GetComponents<WallmountSpriteBehavior>())
		{
			if (wsb != this) Destroy(wsb);
		}
	}

	// Handles rendering logic, only runs when this sprite is on camera
	private void OnWillRenderObject()
	{
		//don't run check until player is created
		if (PlayerManager.LocalPlayer == null || wallmountBehavior == null)
		{
			return;
		}

		//recalculate if it is facing the player
		bool visible = wallmountBehavior.IsFacingPosition(Camera2DFollow.followControl.target.position);
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, visible ? 1 : 0);
	}
}
