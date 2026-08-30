using System;
using TMPro;
using UnityEngine;

// Token: 0x02000C43 RID: 3139
public class DamageText : MonoBehaviour
{
	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06005F93 RID: 24467 RVA: 0x0025DE05 File Offset: 0x0025C005
	// (set) Token: 0x06005F94 RID: 24468 RVA: 0x0025DE15 File Offset: 0x0025C015
	public static bool Enabled
	{
		get
		{
			return DamageText.enabled || DamageText.SandboxEnabled;
		}
		set
		{
			DamageText.enabled = value;
		}
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x0025DE20 File Offset: 0x0025C020
	public static void Create(string _text, Color _color, Vector3 _worldPos, Vector3 _velocity, float _scale = 1f)
	{
		GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/DamageText"));
		DamageText component = gameObject.GetComponent<DamageText>();
		TextMeshPro component2 = gameObject.GetComponent<TextMeshPro>();
		component.textMeshPro = component2;
		component2.text = _text;
		component2.color = _color;
		component2.rectTransform.localScale = new Vector3(_scale, _scale, _scale);
		component.worldPos = _worldPos;
		component.velocity = _velocity;
		component.cameraT = Camera.main.transform;
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x0025DE98 File Offset: 0x0025C098
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		float deltaTime = Time.deltaTime;
		this.TimeDuration -= deltaTime;
		if (this.TimeDuration <= 0f || !this.cameraT)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.textMeshPro.alpha = Utils.FastLerp(0f, 1f, this.TimeDuration * 2f);
		this.velocityDecayDelay -= deltaTime;
		if (this.velocityDecayDelay <= 0f)
		{
			this.velocityDecayDelay = 0.1f;
			this.velocity *= 0.8f;
		}
		this.worldPos += this.velocity * deltaTime;
		base.transform.SetPositionAndRotation(Vector3.MoveTowards(this.worldPos - Origin.position, this.cameraT.position + this.cameraT.forward * 0.18f, 0.25f), this.cameraT.rotation);
	}

	// Token: 0x04004AC5 RID: 19141
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public new static bool enabled;

	// Token: 0x04004AC6 RID: 19142
	public static bool SandboxEnabled;

	// Token: 0x04004AC7 RID: 19143
	public float TimeDuration = 1.5f;

	// Token: 0x04004AC8 RID: 19144
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMeshPro textMeshPro;

	// Token: 0x04004AC9 RID: 19145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 worldPos;

	// Token: 0x04004ACA RID: 19146
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 velocity;

	// Token: 0x04004ACB RID: 19147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float velocityDecayDelay = 0.2f;

	// Token: 0x04004ACC RID: 19148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform cameraT;
}
