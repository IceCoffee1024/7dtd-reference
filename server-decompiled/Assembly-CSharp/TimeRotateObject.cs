using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class TimeRotateObject : MonoBehaviour
{
	// Token: 0x06002C57 RID: 11351 RVA: 0x001189F4 File Offset: 0x00116BF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (GameManager.Instance)
		{
			World world = GameManager.Instance.World;
			if (world != null)
			{
				uint num = GameUtils.WorldTimeToTotalMinutes(world.worldTime) % 60U;
				if (this.hourTransform)
				{
					int num2 = GameUtils.WorldTimeToTotalHours(world.worldTime);
					this.hourTransform.localEulerAngles = new Vector3(0f, 0f, (float)(num2 % 12) * 30f + num * 0.5f);
				}
				if (this.minuteTransform)
				{
					this.minuteTransform.localEulerAngles = new Vector3(0f, 0f, num * 6U);
				}
			}
		}
	}

	// Token: 0x0400220C RID: 8716
	public Transform hourTransform;

	// Token: 0x0400220D RID: 8717
	public Transform minuteTransform;
}
