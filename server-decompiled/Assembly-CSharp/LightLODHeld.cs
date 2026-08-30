using System;
using UnityEngine;

// Token: 0x02001251 RID: 4689
public class LightLODHeld : LightLOD
{
	// Token: 0x0600953B RID: 38203 RVA: 0x00384DB1 File Offset: 0x00382FB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.startPos = this.selfT.localPosition;
	}

	// Token: 0x0600953C RID: 38204 RVA: 0x00384DCC File Offset: 0x00382FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		Transform selfT = this.selfT;
		if (!this.rootT)
		{
			this.rootT = RootTransformRefEntity.FindEntityUpwards(selfT);
			if (this.rootT)
			{
				this.player = this.rootT.GetComponent<EntityPlayerLocal>();
				this.parentStartPos = this.parentT.localPosition;
			}
			return;
		}
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		bool flag = this.player && this.player.bFirstPersonView;
		Vector3 vector;
		if (this.flickerRadius > 0f)
		{
			this.flickerTargetOffset = world.GetGameRandom().RandomOnUnitSphere;
			this.flickerOffset = Vector3.Lerp(this.flickerOffset, this.flickerTargetOffset, 0.2f);
			selfT.localPosition = this.startPos + this.flickerOffset * this.flickerRadius;
			vector = selfT.position;
			if (!flag)
			{
				vector.y += 0.2f;
			}
		}
		else if (flag)
		{
			selfT.localPosition = this.startPos;
			vector = selfT.position;
		}
		else
		{
			this.parentT.localPosition = Vector3.Lerp(this.parentStartPos, this.parentT.localPosition, 0.1f);
			selfT.localPosition = this.startPos;
			vector = selfT.position;
			vector.y += 0.2f;
		}
		Vector3 vector2 = this.rootT.position;
		vector2.y += 0.5f;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector2, Vector3.up, out raycastHit, 1.4f, -554734606))
		{
			vector2.y = raycastHit.point.y - 0.22f - 0.15f;
		}
		else
		{
			vector2.y += 0.95000005f;
		}
		if (vector2.y > vector.y + 0.25f)
		{
			vector2.y = vector.y + 0.25f;
		}
		Vector3 vector3 = vector - vector2;
		float num = vector3.magnitude;
		vector3 *= 1f / num;
		if (flag)
		{
			num *= 0.72f;
		}
		else
		{
			num *= 1.5f;
		}
		vector = vector2 + vector3 * num;
		vector2 += vector3 * 0.02f;
		if (Physics.SphereCast(vector2, 0.15f, vector3, out raycastHit, num - 0.15f - 0.02f + 0.15f, -554734606))
		{
			vector2 = raycastHit.point;
			vector2 += raycastHit.normal * 0.15f;
			selfT.position = vector2;
			return;
		}
		selfT.position = vector;
	}

	// Token: 0x04006FFA RID: 28666
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cHeightCheckY = 0.5f;

	// Token: 0x04006FFB RID: 28667
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCastOffset = 0.02f;

	// Token: 0x04006FFC RID: 28668
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCastRadius = 0.15f;

	// Token: 0x04006FFD RID: 28669
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cClosestDist = 0.15f;

	// Token: 0x04006FFE RID: 28670
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMask = -554734606;

	// Token: 0x04006FFF RID: 28671
	public float flickerRadius;

	// Token: 0x04007000 RID: 28672
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 flickerOffset;

	// Token: 0x04007001 RID: 28673
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 flickerTargetOffset;

	// Token: 0x04007002 RID: 28674
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPos;

	// Token: 0x04007003 RID: 28675
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform rootT;

	// Token: 0x04007004 RID: 28676
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 parentStartPos;

	// Token: 0x04007005 RID: 28677
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;
}
