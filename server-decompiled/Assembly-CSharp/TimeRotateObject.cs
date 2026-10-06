using System;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class TimeRotateObject : MonoBehaviour
{
	// Token: 0x06002C77 RID: 11383 RVA: 0x0011A838 File Offset: 0x00118A38
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

	// Token: 0x04002246 RID: 8774
	public Transform hourTransform;

	// Token: 0x04002247 RID: 8775
	public Transform minuteTransform;
}
