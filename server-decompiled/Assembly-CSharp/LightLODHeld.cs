using System;
using UnityEngine;

// Token: 0x02001269 RID: 4713
public class LightLODHeld : LightLOD
{
	// Token: 0x060095C9 RID: 38345 RVA: 0x003895F9 File Offset: 0x003877F9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.startPos = this.selfT.localPosition;
	}

	// Token: 0x060095CA RID: 38346 RVA: 0x00389614 File Offset: 0x00387814
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

	// Token: 0x04007064 RID: 28772
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cHeightCheckY = 0.5f;

	// Token: 0x04007065 RID: 28773
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCastOffset = 0.02f;

	// Token: 0x04007066 RID: 28774
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCastRadius = 0.15f;

	// Token: 0x04007067 RID: 28775
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cClosestDist = 0.15f;

	// Token: 0x04007068 RID: 28776
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMask = -554734606;

	// Token: 0x04007069 RID: 28777
	public float flickerRadius;

	// Token: 0x0400706A RID: 28778
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 flickerOffset;

	// Token: 0x0400706B RID: 28779
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 flickerTargetOffset;

	// Token: 0x0400706C RID: 28780
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 startPos;

	// Token: 0x0400706D RID: 28781
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform rootT;

	// Token: 0x0400706E RID: 28782
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 parentStartPos;

	// Token: 0x0400706F RID: 28783
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal player;
}
