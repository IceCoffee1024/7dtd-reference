using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200164B RID: 5707
public class vp_PainHUD : MonoBehaviour
{
	// Token: 0x0600B2DD RID: 45789 RVA: 0x0042BFB9 File Offset: 0x0042A1B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B2DE RID: 45790 RVA: 0x0042BFCC File Offset: 0x0042A1CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B2DF RID: 45791 RVA: 0x0042BFE8 File Offset: 0x0042A1E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B2E0 RID: 45792 RVA: 0x0042C004 File Offset: 0x0042A204
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGUI()
	{
		this.UpdatePainFlash();
		this.UpdateInflictorArrows();
		this.UpdateDeathTexture();
	}

	// Token: 0x0600B2E1 RID: 45793 RVA: 0x0042C018 File Offset: 0x0042A218
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdatePainFlash()
	{
		if (this.m_PainColor.a < 0.01f)
		{
			this.m_PainColor.a = 0f;
			return;
		}
		this.m_PainColor = Color.Lerp(this.m_PainColor, this.m_FlashInvisibleColor, Time.deltaTime * 0.4f);
		GUI.color = this.m_PainColor;
		if (this.PainTexture != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.PainTexture);
		}
		GUI.color = Color.white;
	}

	// Token: 0x0600B2E2 RID: 45794 RVA: 0x0042C0B4 File Offset: 0x0042A2B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInflictorArrows()
	{
		if (this.ArrowTexture == null)
		{
			return;
		}
		for (int i = this.m_Inflictors.Count - 1; i > -1; i--)
		{
			if (this.m_Inflictors[i] == null || this.m_Inflictors[i].Transform == null || !vp_Utility.IsActive(this.m_Inflictors[i].Transform.gameObject))
			{
				this.m_Inflictors.Remove(this.m_Inflictors[i]);
			}
			else
			{
				this.m_ArrowColor.a = (this.ArrowVisibleDuration - (Time.time - this.m_Inflictors[i].DamageTime)) / this.ArrowVisibleDuration;
				if (this.m_ArrowColor.a >= 0f)
				{
					Vector2 vector = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
					float num = vp_3DUtility.LookAtAngleHorizontal(base.transform.position, base.transform.forward, this.m_Inflictors[i].Transform.position) + this.ArrowAngleOffset;
					float num2 = (float)Screen.width * this.ArrowScale;
					float num3 = (this.ArrowShakeDuration - (Time.time - this.m_LastInflictorTime)) / this.ArrowShakeDuration;
					num3 = Mathf.Lerp(0f, 1f, num3);
					num2 += (float)(Screen.width / 100) * num3;
					Matrix4x4 matrix = GUI.matrix;
					GUIUtility.RotateAroundPivot(num, vector);
					GUI.color = this.m_ArrowColor;
					GUI.DrawTexture(new Rect(vector.x, vector.y, num2, num2), this.ArrowTexture);
					GUI.matrix = matrix;
				}
			}
		}
	}

	// Token: 0x0600B2E3 RID: 45795 RVA: 0x0042C272 File Offset: 0x0042A472
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateDeathTexture()
	{
		if (this.DeathTexture == null)
		{
			return;
		}
		if (!this.m_Player.Dead.Active)
		{
			return;
		}
		GUI.color = this.m_SplatColor;
		GUI.DrawTexture(this.m_SplatRect, this.DeathTexture);
	}

	// Token: 0x0600B2E4 RID: 45796 RVA: 0x0042C2B4 File Offset: 0x0042A4B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_HUDDamageFlash(vp_DamageInfo damageInfo)
	{
		if (damageInfo == null || damageInfo.Damage == 0f)
		{
			this.m_PainColor.a = 0f;
			return;
		}
		this.m_PainColor.a = this.m_PainColor.a + damageInfo.Damage * this.PainIntensity;
		if (damageInfo.Source != null)
		{
			this.m_LastInflictorTime = Time.time;
			bool flag = true;
			foreach (vp_PainHUD.Inflictor inflictor in this.m_Inflictors)
			{
				if (inflictor.Transform == damageInfo.Source.transform)
				{
					inflictor.DamageTime = Time.time;
					flag = false;
				}
			}
			if (flag)
			{
				this.m_Inflictors.Add(new vp_PainHUD.Inflictor(damageInfo.Source, Time.time));
			}
		}
	}

	// Token: 0x0600B2E5 RID: 45797 RVA: 0x0042C39C File Offset: 0x0042A59C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStart_Dead()
	{
		float num = global::UnityEngine.Random.value * 0.6f + 0.4f;
		this.m_SplatColor = new Color(num, num, num, 1f);
		float num2 = (float)((global::UnityEngine.Random.value < 0.5f) ? (Screen.width / global::UnityEngine.Random.Range(5, 10)) : (Screen.width / global::UnityEngine.Random.Range(4, 7)));
		this.m_SplatRect = new Rect(global::UnityEngine.Random.Range(-num2, 0f), global::UnityEngine.Random.Range(-num2, 0f), (float)Screen.width + num2, (float)Screen.height + num2);
		if (global::UnityEngine.Random.value < 0.5f)
		{
			this.m_SplatRect.x = (float)Screen.width - this.m_SplatRect.x;
			this.m_SplatRect.width = -this.m_SplatRect.width;
		}
		if (global::UnityEngine.Random.value < 0.125f)
		{
			num *= 0.5f;
			this.m_SplatColor = new Color(num, num, num, 1f);
			this.m_SplatRect.y = (float)Screen.height - this.m_SplatRect.y;
			this.m_SplatRect.height = -this.m_SplatRect.height;
		}
	}

	// Token: 0x0600B2E6 RID: 45798 RVA: 0x0042C4C8 File Offset: 0x0042A6C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_PainColor.a = 0f;
		for (int i = this.m_Inflictors.Count - 1; i > -1; i--)
		{
			this.m_Inflictors[i].DamageTime = 0f;
		}
	}

	// Token: 0x04008645 RID: 34373
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_PainHUD.Inflictor> m_Inflictors = new List<vp_PainHUD.Inflictor>();

	// Token: 0x04008646 RID: 34374
	public Texture PainTexture;

	// Token: 0x04008647 RID: 34375
	public Texture DeathTexture;

	// Token: 0x04008648 RID: 34376
	public Texture ArrowTexture;

	// Token: 0x04008649 RID: 34377
	public float PainIntensity = 0.2f;

	// Token: 0x0400864A RID: 34378
	[Range(0.01f, 0.5f)]
	public float ArrowScale = 0.083f;

	// Token: 0x0400864B RID: 34379
	public float ArrowAngleOffset = -135f;

	// Token: 0x0400864C RID: 34380
	public float ArrowVisibleDuration = 1.5f;

	// Token: 0x0400864D RID: 34381
	public float ArrowShakeDuration = 0.125f;

	// Token: 0x0400864E RID: 34382
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastInflictorTime;

	// Token: 0x0400864F RID: 34383
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_PainColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x04008650 RID: 34384
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_ArrowColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x04008651 RID: 34385
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_FlashInvisibleColor = new Color(1f, 0f, 0f, 0f);

	// Token: 0x04008652 RID: 34386
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SplatColor = Color.white;

	// Token: 0x04008653 RID: 34387
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_SplatRect;

	// Token: 0x04008654 RID: 34388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0200164C RID: 5708
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Inflictor
	{
		// Token: 0x0600B2E8 RID: 45800 RVA: 0x0042C5D1 File Offset: 0x0042A7D1
		public Inflictor(Transform transform, float damageTime)
		{
			this.Transform = transform;
			this.DamageTime = damageTime;
		}

		// Token: 0x04008655 RID: 34389
		public Transform Transform;

		// Token: 0x04008656 RID: 34390
		public float DamageTime;
	}
}
