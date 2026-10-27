using System;
using TMPro;
using UnityEngine;

// Token: 0x02000C72 RID: 3186
public class DamageText : MonoBehaviour
{
	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x0600605B RID: 24667 RVA: 0x002654C9 File Offset: 0x002636C9
	// (set) Token: 0x0600605C RID: 24668 RVA: 0x002654D9 File Offset: 0x002636D9
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

	// Token: 0x0600605D RID: 24669 RVA: 0x002654E4 File Offset: 0x002636E4
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

	// Token: 0x0600605E RID: 24670 RVA: 0x0026555C File Offset: 0x0026375C
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

	// Token: 0x04004BC6 RID: 19398
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public new static bool enabled;

	// Token: 0x04004BC7 RID: 19399
	public static bool SandboxEnabled;

	// Token: 0x04004BC8 RID: 19400
	public float TimeDuration = 1.5f;

	// Token: 0x04004BC9 RID: 19401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public TextMeshPro textMeshPro;

	// Token: 0x04004BCA RID: 19402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 worldPos;

	// Token: 0x04004BCB RID: 19403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 velocity;

	// Token: 0x04004BCC RID: 19404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float velocityDecayDelay = 0.2f;

	// Token: 0x04004BCD RID: 19405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform cameraT;
}
