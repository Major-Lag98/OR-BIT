using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{
	public enum StateTypeEnum { Win, Lose }

	public StateTypeEnum StateType = StateTypeEnum.Win; 
	public float Delay = 0f;
	public float TimeToTween = 1f;

	float counter = 0f;

    // Start is called before the first frame update
    void Start()
    {
		counter -= Delay; // We subtract the delay so we don't need a second timer
		transform.localScale = new Vector3(0,0,0); // Start the scale at 0
		gameObject.SetActive(false); // Disable the object until we win

		if(StateType == StateTypeEnum.Win)
			// Set active when we win (new delegates in LevelStateMachine)
			LevelStateMachine.Instance.AddOnWinDelegate(() => gameObject.SetActive(true));
		else
			LevelStateMachine.Instance.AddOnLoseDelegate(() => gameObject.SetActive(true));
	}

	// Update is called once per frame
	void Update()
    {
		counter += Time.deltaTime; // Count up

		// We make sure this is between 0 and TimeToTween because we have the delay added in Start() which causes negative counter
		if(counter >= 0 && counter <= TimeToTween)
        {
			var ratio = counter / TimeToTween; // We get the ratio of current to our time to tween
			// Use that ratio in the bounce function. This returns a value between 0 and 1 scaled by the tween
			// In this case, bounce causes a 'bounce' on the end (Bounce.Out) of the animation
			var value = Bounce.Out(ratio); 
			transform.localScale = new Vector3(value, value, 1); // We want to tween to 1, so we just use 'value' since we dont need to scale it
        }
    }

	// I only needed a bounce animation so I just snuck this code in here. Pretty messy here...
	public class Bounce
	{
		// A function for bounce on the start (in) of the tween
		public static float In(float k)
		{
			return 1f - Out(1f - k);
		}

		// Bounces on the end (out) of the tween
		public static float Out(float k)
		{
			if (k < (1f / 2.75f))
			{
				return 7.5625f * k * k;
			}
			else if (k < (2f / 2.75f))
			{
				return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
			}
			else if (k < (2.5f / 2.75f))
			{
				return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
			}
			else
			{
				return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
			}
		}

		// Bounces on the start and end of the tween
		public static float InOut(float k)
		{
			if (k < 0.5f) return In(k * 2f) * 0.5f;
			return Out(k * 2f - 1f) * 0.5f + 0.5f;
		}
	};
}
