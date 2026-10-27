using System;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001DC4 RID: 7620
	[PublicizedFrom(EAccessModifier.Internal)]
	public class ProceduralAnimation : MonoBehaviour
	{
		// Token: 0x0600E0F1 RID: 57585 RVA: 0x0050252B File Offset: 0x0050072B
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.m_RestPos = base.transform.position;
		}

		// Token: 0x0600E0F2 RID: 57586 RVA: 0x00502540 File Offset: 0x00500740
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

		// Token: 0x0600E0F3 RID: 57587 RVA: 0x00502674 File Offset: 0x00500874
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

		// Token: 0x0400AA7D RID: 43645
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_RestPos;

		// Token: 0x0400AA7E RID: 43646
		public bool MoveAlongX;

		// Token: 0x0400AA7F RID: 43647
		public bool ForwardAndBackward;

		// Token: 0x0400AA80 RID: 43648
		public bool UpAndDown;

		// Token: 0x0400AA81 RID: 43649
		public bool SideToSide;

		// Token: 0x0400AA82 RID: 43650
		public bool Bounce;

		// Token: 0x0400AA83 RID: 43651
		public float TranslationMultiplier = 1f;

		// Token: 0x0400AA84 RID: 43652
		public bool RotateX;

		// Token: 0x0400AA85 RID: 43653
		public bool RotateY;

		// Token: 0x0400AA86 RID: 43654
		public float RotationMultiplier = 1f;
	}
}
