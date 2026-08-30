using System;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class TimeRotateObject : MonoBehaviour
{
	// Token: 0x06002C81 RID: 11393 RVA: 0x00119A68 File Offset: 0x00117C68
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

	// Token: 0x0400223E RID: 8766
	public Transform hourTransform;

	// Token: 0x0400223F RID: 8767
	public Transform minuteTransform;
}
