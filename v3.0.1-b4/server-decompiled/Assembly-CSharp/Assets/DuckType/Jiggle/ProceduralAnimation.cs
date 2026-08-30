using System;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D39 RID: 7481
	[PublicizedFrom(EAccessModifier.Internal)]
	public class ProceduralAnimation : MonoBehaviour
	{
		// Token: 0x0600DCC3 RID: 56515 RVA: 0x004F3363 File Offset: 0x004F1563
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.m_RestPos = base.transform.position;
		}

		// Token: 0x0600DCC4 RID: 56516 RVA: 0x004F3378 File Offset: 0x004F1578
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			float num = (this.MoveAlongX ? (Time.time * this.TranslationMultiplier) : 0f);
			if (this.ForwardAndBackward)
			{
				num += this.GetSineValue(this.Bounce, this.TranslationMultiplier);
			}
			base.transform.position = this.m_RestPos + new Vector3(num, this.UpAndDown ? this.GetSineValue(this.Bounce, this.TranslationMultiplier) : 0f, this.SideToSide ? this.GetSineValue(this.Bounce, this.TranslationMultiplier) : 0f);
			base.transform.rotation = Quaternion.Euler(this.RotateX ? (Mathf.Sin(Time.time * 6f) * 30f * this.RotationMultiplier) : base.transform.eulerAngles.x, this.RotateY ? (Mathf.Sin(Time.time * 6f) * 30f * this.RotationMultiplier) : base.transform.eulerAngles.y, base.transform.eulerAngles.z);
		}

		// Token: 0x0600DCC5 RID: 56517 RVA: 0x004F34AC File Offset: 0x004F16AC
		[PublicizedFrom(EAccessModifier.Private)]
		public float GetSineValue(bool bounce, float mult)
		{
			float num = Mathf.Sin(Time.time * 6f) * 3f * mult;
			if (!bounce)
			{
				return num;
			}
			return Mathf.Abs(num);
		}

		// Token: 0x0400A788 RID: 42888
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_RestPos;

		// Token: 0x0400A789 RID: 42889
		public bool MoveAlongX;

		// Token: 0x0400A78A RID: 42890
		public bool ForwardAndBackward;

		// Token: 0x0400A78B RID: 42891
		public bool UpAndDown;

		// Token: 0x0400A78C RID: 42892
		public bool SideToSide;

		// Token: 0x0400A78D RID: 42893
		public bool Bounce;

		// Token: 0x0400A78E RID: 42894
		public float TranslationMultiplier = 1f;

		// Token: 0x0400A78F RID: 42895
		public bool RotateX;

		// Token: 0x0400A790 RID: 42896
		public bool RotateY;

		// Token: 0x0400A791 RID: 42897
		public float RotationMultiplier = 1f;
	}
}
