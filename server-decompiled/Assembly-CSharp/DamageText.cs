using System;
using TMPro;
using UnityEngine;

// Token: 0x02000C67 RID: 3175
public class DamageText : MonoBehaviour
{
	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06006036 RID: 24630 RVA: 0x00263FA1 File Offset: 0x002621A1
	// (set) Token: 0x06006037 RID: 24631 RVA: 0x00263FB1 File Offset: 0x002621B1
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

	// Token: 0x06006038 RID: 24632 RVA: 0x00263FBC File Offset: 0x002621BC
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

	// Token: 0x06006039 RID: 24633 RVA: 0x00264034 File Offset: 0x00262234
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

	// Token: 0x04004BA1 RID: 19361
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public new static bool enabled;

	// Token: 0x04004BA2 RID: 19362
	public static bool SandboxEnabled;

	// Token: 0x04004BA3 RID: 19363
	public float TimeDuration = 1.5f;

	// Token: 0x04004BA4 RID: 19364
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMeshPro textMeshPro;

	// Token: 0x04004BA5 RID: 19365
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 worldPos;

	// Token: 0x04004BA6 RID: 19366
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 velocity;

	// Token: 0x04004BA7 RID: 19367
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float velocityDecayDelay = 0.2f;

	// Token: 0x04004BA8 RID: 19368
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform cameraT;
}
