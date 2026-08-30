using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class TurnTable : MonoBehaviour
{
	// Token: 0x06000108 RID: 264 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this._pingPong)
		{
			float num = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time * this._rotationSpeed, 1f));
			float num2 = Mathf.Lerp(this._pingPongDegreeOffset - this._pingPongDegreeSpan / 2f, this._pingPongDegreeOffset + this._pingPongDegreeSpan / 2f, num);
			base.transform.localRotation = Quaternion.Euler(0f, num2, 0f);
			return;
		}
		base.transform.localRotation *= Quaternion.Euler(0f, this._rotationSpeed * 180f * Time.deltaTime, 0f);
	}

	// Token: 0x04000124 RID: 292
	public float _rotationSpeed = 1f;

	// Token: 0x04000125 RID: 293
	public bool _pingPong;

	// Token: 0x04000126 RID: 294
	public float _pingPongDegreeSpan = 90f;

	// Token: 0x04000127 RID: 295
	public float _pingPongDegreeOffset;
}
