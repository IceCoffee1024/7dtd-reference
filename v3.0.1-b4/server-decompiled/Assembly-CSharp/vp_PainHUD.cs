using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015FF RID: 5631
public class vp_PainHUD : MonoBehaviour
{
	// Token: 0x0600B0F6 RID: 45302 RVA: 0x00420B99 File Offset: 0x0041ED99
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B0F7 RID: 45303 RVA: 0x00420BAC File Offset: 0x0041EDAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B0F8 RID: 45304 RVA: 0x00420BC8 File Offset: 0x0041EDC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B0F9 RID: 45305 RVA: 0x00420BE4 File Offset: 0x0041EDE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGUI()
	{
		this.UpdatePainFlash();
		this.UpdateInflictorArrows();
		this.UpdateDeathTexture();
	}

	// Token: 0x0600B0FA RID: 45306 RVA: 0x00420BF8 File Offset: 0x0041EDF8
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

	// Token: 0x0600B0FB RID: 45307 RVA: 0x00420C94 File Offset: 0x0041EE94
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

	// Token: 0x0600B0FC RID: 45308 RVA: 0x00420E52 File Offset: 0x0041F052
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

	// Token: 0x0600B0FD RID: 45309 RVA: 0x00420E94 File Offset: 0x0041F094
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

	// Token: 0x0600B0FE RID: 45310 RVA: 0x00420F7C File Offset: 0x0041F17C
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

	// Token: 0x0600B0FF RID: 45311 RVA: 0x004210A8 File Offset: 0x0041F2A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_PainColor.a = 0f;
		for (int i = this.m_Inflictors.Count - 1; i > -1; i--)
		{
			this.m_Inflictors[i].DamageTime = 0f;
		}
	}

	// Token: 0x040084D5 RID: 34005
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_PainHUD.Inflictor> m_Inflictors = new List<vp_PainHUD.Inflictor>();

	// Token: 0x040084D6 RID: 34006
	public Texture PainTexture;

	// Token: 0x040084D7 RID: 34007
	public Texture DeathTexture;

	// Token: 0x040084D8 RID: 34008
	public Texture ArrowTexture;

	// Token: 0x040084D9 RID: 34009
	public float PainIntensity = 0.2f;

	// Token: 0x040084DA RID: 34010
	[Range(0.01f, 0.5f)]
	public float ArrowScale = 0.083f;

	// Token: 0x040084DB RID: 34011
	public float ArrowAngleOffset = -135f;

	// Token: 0x040084DC RID: 34012
	public float ArrowVisibleDuration = 1.5f;

	// Token: 0x040084DD RID: 34013
	public float ArrowShakeDuration = 0.125f;

	// Token: 0x040084DE RID: 34014
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastInflictorTime;

	// Token: 0x040084DF RID: 34015
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_PainColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x040084E0 RID: 34016
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_ArrowColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x040084E1 RID: 34017
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_FlashInvisibleColor = new Color(1f, 0f, 0f, 0f);

	// Token: 0x040084E2 RID: 34018
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SplatColor = Color.white;

	// Token: 0x040084E3 RID: 34019
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_SplatRect;

	// Token: 0x040084E4 RID: 34020
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x02001600 RID: 5632
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Inflictor
	{
		// Token: 0x0600B101 RID: 45313 RVA: 0x004211B1 File Offset: 0x0041F3B1
		public Inflictor(Transform transform, float damageTime)
		{
			this.Transform = transform;
			this.DamageTime = damageTime;
		}

		// Token: 0x040084E5 RID: 34021
		public Transform Transform;

		// Token: 0x040084E6 RID: 34022
		public float DamageTime;
	}
}
