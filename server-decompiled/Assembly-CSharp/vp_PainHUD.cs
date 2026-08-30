using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200161A RID: 5658
public class vp_PainHUD : MonoBehaviour
{
	// Token: 0x0600B1F3 RID: 45555 RVA: 0x00425ED9 File Offset: 0x004240D9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B1F4 RID: 45556 RVA: 0x00425EEC File Offset: 0x004240EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B1F5 RID: 45557 RVA: 0x00425F08 File Offset: 0x00424108
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B1F6 RID: 45558 RVA: 0x00425F24 File Offset: 0x00424124
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGUI()
	{
		this.UpdatePainFlash();
		this.UpdateInflictorArrows();
		this.UpdateDeathTexture();
	}

	// Token: 0x0600B1F7 RID: 45559 RVA: 0x00425F38 File Offset: 0x00424138
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

	// Token: 0x0600B1F8 RID: 45560 RVA: 0x00425FD4 File Offset: 0x004241D4
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

	// Token: 0x0600B1F9 RID: 45561 RVA: 0x00426192 File Offset: 0x00424392
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

	// Token: 0x0600B1FA RID: 45562 RVA: 0x004261D4 File Offset: 0x004243D4
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

	// Token: 0x0600B1FB RID: 45563 RVA: 0x004262BC File Offset: 0x004244BC
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

	// Token: 0x0600B1FC RID: 45564 RVA: 0x004263E8 File Offset: 0x004245E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnStop_Dead()
	{
		this.m_PainColor.a = 0f;
		for (int i = this.m_Inflictors.Count - 1; i > -1; i--)
		{
			this.m_Inflictors[i].DamageTime = 0f;
		}
	}

	// Token: 0x040085AE RID: 34222
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_PainHUD.Inflictor> m_Inflictors = new List<vp_PainHUD.Inflictor>();

	// Token: 0x040085AF RID: 34223
	public Texture PainTexture;

	// Token: 0x040085B0 RID: 34224
	public Texture DeathTexture;

	// Token: 0x040085B1 RID: 34225
	public Texture ArrowTexture;

	// Token: 0x040085B2 RID: 34226
	public float PainIntensity = 0.2f;

	// Token: 0x040085B3 RID: 34227
	[Range(0.01f, 0.5f)]
	public float ArrowScale = 0.083f;

	// Token: 0x040085B4 RID: 34228
	public float ArrowAngleOffset = -135f;

	// Token: 0x040085B5 RID: 34229
	public float ArrowVisibleDuration = 1.5f;

	// Token: 0x040085B6 RID: 34230
	public float ArrowShakeDuration = 0.125f;

	// Token: 0x040085B7 RID: 34231
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_LastInflictorTime;

	// Token: 0x040085B8 RID: 34232
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_PainColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x040085B9 RID: 34233
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_ArrowColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x040085BA RID: 34234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_FlashInvisibleColor = new Color(1f, 0f, 0f, 0f);

	// Token: 0x040085BB RID: 34235
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Color m_SplatColor = Color.white;

	// Token: 0x040085BC RID: 34236
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rect m_SplatRect;

	// Token: 0x040085BD RID: 34237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0200161B RID: 5659
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Inflictor
	{
		// Token: 0x0600B1FE RID: 45566 RVA: 0x004264F1 File Offset: 0x004246F1
		public Inflictor(Transform transform, float damageTime)
		{
			this.Transform = transform;
			this.DamageTime = damageTime;
		}

		// Token: 0x040085BE RID: 34238
		public Transform Transform;

		// Token: 0x040085BF RID: 34239
		public float DamageTime;
	}
}
