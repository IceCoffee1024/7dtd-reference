using System;
using UnityEngine;

// Token: 0x0200055B RID: 1371
public class TimeRotateObject : MonoBehaviour
{
	// Token: 0x06002C7F RID: 11391 RVA: 0x0011ADC4 File Offset: 0x00118FC4
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

	// Token: 0x0400225D RID: 8797
	public Transform hourTransform;

	// Token: 0x0400225E RID: 8798
	public Transform minuteTransform;
}
