using System;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D61 RID: 7521
	[PublicizedFrom(EAccessModifier.Internal)]
	public class ProceduralAnimation : MonoBehaviour
	{
		// Token: 0x0600DE64 RID: 56932 RVA: 0x004F9DD7 File Offset: 0x004F7FD7
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.m_RestPos = base.transform.position;
		}

		// Token: 0x0600DE65 RID: 56933 RVA: 0x004F9DEC File Offset: 0x004F7FEC
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

		// Token: 0x0600DE66 RID: 56934 RVA: 0x004F9F20 File Offset: 0x004F8120
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

		// Token: 0x0400A8BD RID: 43197
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_RestPos;

		// Token: 0x0400A8BE RID: 43198
		public bool MoveAlongX;

		// Token: 0x0400A8BF RID: 43199
		public bool ForwardAndBackward;

		// Token: 0x0400A8C0 RID: 43200
		public bool UpAndDown;

		// Token: 0x0400A8C1 RID: 43201
		public bool SideToSide;

		// Token: 0x0400A8C2 RID: 43202
		public bool Bounce;

		// Token: 0x0400A8C3 RID: 43203
		public float TranslationMultiplier = 1f;

		// Token: 0x0400A8C4 RID: 43204
		public bool RotateX;

		// Token: 0x0400A8C5 RID: 43205
		public bool RotateY;

		// Token: 0x0400A8C6 RID: 43206
		public float RotationMultiplier = 1f;
	}
}
