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
    [SerializeField] private Sprite[] stageThreeWalkingCycle;
    [SerializeField] private Sprite[] stageFourWalkingCycle;
    [SerializeField] private Sprite[] stageFiveWalkingCycle;
    [SerializeField] private Sprite[] stageSixWalkingCycle;
    [SerializeField] private Sprite[] stageSevenWalkingCycle;
    [SerializeField] private Sprite[] stageEightWalkingCycle;
    [SerializeField] private Sprite[] stageNineWalkingCycle;

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
            case 3:
                return stageThreeWalkingCycle;
            case 4:
                return stageThreeWalkingCycle; // Level 1 Platform
            case 5:
                return stageFourWalkingCycle;
            case 6:
                return stageFiveWalkingCycle;
            case 7:
                return stageSixWalkingCycle;
            case 8:
                return stageSevenWalkingCycle;
            case 9:
                return stageEightWalkingCycle;
            case 10:
                return stageNineWalkingCycle;
            case 11:
                return stageNineWalkingCycle; // Level 2 Platform
		}
		return stageOneWalkingCycle;
	}
}