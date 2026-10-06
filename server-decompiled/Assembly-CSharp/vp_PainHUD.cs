using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001639 RID: 5689
public class vp_PainHUD : MonoBehaviour
{
	// Token: 0x0600B285 RID: 45701 RVA: 0x00429BB5 File Offset: 0x00427DB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B286 RID: 45702 RVA: 0x00429BC8 File Offset: 0x00427DC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B287 RID: 45703 RVA: 0x00429BE4 File Offset: 0x00427DE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B288 RID: 45704 RVA: 0x00429C00 File Offset: 0x00427E00
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGUI()
	{
		this.UpdatePainFlash();
		this.UpdateInflictorArrows();
		this.UpdateDeathTexture();
	}

	// Token: 0x0600B289 RID: 45705 RVA: 0x00429C14 File Offset: 0x00427E14
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

	// Token: 0x0600B28A RID: 45706 RVA: 0x00429CB0 File Offset: 0x00427EB0
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

	// Token: 0x0600B28B RID: 45707 RVA: 0x00429E6E File Offset: 0x0042806E
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

	// Token: 0x0600B28C RID: 45708 RVA: 0x00429EB0 File Offset: 0x004280B0
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

	// Token: 0x0600B28D RID: 45709 RVA: 0x00429F98 File Offset: 0x00428198
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

	// Token: 0x0600B28E RID: 45710 RVA: 0x0042A0C4 File Offset: 0x004282C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_PainColor.a = 0f;
		for (int i = this.m_Inflictors.Count - 1; i > -1; i--)
		{
			this.m_Inflictors[i].DamageTime = 0f;
		}
	}

	// Token: 0x04008604 RID: 34308
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_PainHUD.Inflictor> m_Inflictors = new List<vp_PainHUD.Inflictor>();

	// Token: 0x04008605 RID: 34309
	public Texture PainTexture;

	// Token: 0x04008606 RID: 34310
	public Texture DeathTexture;

	// Token: 0x04008607 RID: 34311
	public Texture ArrowTexture;

	// Token: 0x04008608 RID: 34312
	public float PainIntensity = 0.2f;

	// Token: 0x04008609 RID: 34313
	[Range(0.01f, 0.5f)]
	public float ArrowScale = 0.083f;

	// Token: 0x0400860A RID: 34314
	public float ArrowAngleOffset = -135f;

	// Token: 0x0400860B RID: 34315
	public float ArrowVisibleDuration = 1.5f;

	// Token: 0x0400860C RID: 34316
	public float ArrowShakeDuration = 0.125f;

	// Token: 0x0400860D RID: 34317
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastInflictorTime;

	// Token: 0x0400860E RID: 34318
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_PainColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x0400860F RID: 34319
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_ArrowColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x04008610 RID: 34320
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_FlashInvisibleColor = new Color(1f, 0f, 0f, 0f);

	// Token: 0x04008611 RID: 34321
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SplatColor = Color.white;

	// Token: 0x04008612 RID: 34322
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_SplatRect;

	// Token: 0x04008613 RID: 34323
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0200163A RID: 5690
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Inflictor
	{
		// Token: 0x0600B290 RID: 45712 RVA: 0x0042A1CD File Offset: 0x004283CD
		public Inflictor(Transform transform, float damageTime)
		{
			this.Transform = transform;
			this.DamageTime = damageTime;
		}

		// Token: 0x04008614 RID: 34324
		public Transform Transform;

		// Token: 0x04008615 RID: 34325
		public float DamageTime;
	}
}
