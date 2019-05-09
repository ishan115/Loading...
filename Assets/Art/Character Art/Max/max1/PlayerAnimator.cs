using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	public static PlayerAnimator mainPlayer;
	[SerializeField] private bool isMainPlayer;

	[SerializeField] private float velocityCycleSpeedCoefficient;
	[SerializeField] private float baseCycleSpeed;

	[SerializeField] private Rigidbody2D linkedBody;
	[SerializeField] private SpriteRenderer playerRender;

	[SerializeField] private Sprite[] stageOneWalkingCycle;
	[SerializeField] private Sprite[] stageTwoWalkingCycle;

	private float currentFrameTime = 0;
	private int currentStage = 1;
	private int currentFrame = 0;

	void Awake()
	{
		if(isMainPlayer)
		{
			mainPlayer = this;
		}
	}

    // Update is called once per frame
    void Update() 
    {
		playerRender.flipX = linkedBody.velocity.x < 0;


		currentFrameTime += velocityCycleSpeedCoefficient * linkedBody.velocity.magnitude * Time.deltaTime;

		if(currentFrameTime > baseCycleSpeed)
		{
			currentFrame++;
			if(currentFrame > GetStage(currentStage).Length - 1)
			{
				currentFrame = 0;
			}
			currentFrameTime = 0;

			playerRender.sprite = GetStage(currentStage)[currentFrame];
		}
    }

	public void AdvanceSprite()
	{
		currentStage++;
		playerRender.sprite = GetStage(currentStage)[0];
	}

	private Sprite[] GetStage(int stage)
	{
		switch(stage)
		{
			case 1:
				return stageOneWalkingCycle;
			case 2:
				return stageTwoWalkingCycle;
		}
		return stageOneWalkingCycle;
	}
}