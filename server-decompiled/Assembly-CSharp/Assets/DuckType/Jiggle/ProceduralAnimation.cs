using System;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D93 RID: 7571
	[PublicizedFrom(EAccessModifier.Internal)]
	public class ProceduralAnimation : MonoBehaviour
	{
		// Token: 0x0600DFDD RID: 57309 RVA: 0x004FE993 File Offset: 0x004FCB93
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			this.m_RestPos = base.transform.position;
		}

		// Token: 0x0600DFDE RID: 57310 RVA: 0x004FE9A8 File Offset: 0x004FCBA8
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

		// Token: 0x0600DFDF RID: 57311 RVA: 0x004FEADC File Offset: 0x004FCCDC
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

		// Token: 0x0400A994 RID: 43412
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Vector3 m_RestPos;

		// Token: 0x0400A995 RID: 43413
		public bool MoveAlongX;

		// Token: 0x0400A996 RID: 43414
		public bool ForwardAndBackward;

		// Token: 0x0400A997 RID: 43415
		public bool UpAndDown;

		// Token: 0x0400A998 RID: 43416
		public bool SideToSide;

		// Token: 0x0400A999 RID: 43417
		public bool Bounce;

		// Token: 0x0400A99A RID: 43418
		public float TranslationMultiplier = 1f;

		// Token: 0x0400A99B RID: 43419
		public bool RotateX;

		// Token: 0x0400A99C RID: 43420
		public bool RotateY;

		// Token: 0x0400A99D RID: 43421
		public float RotationMultiplier = 1f;
	}
}
