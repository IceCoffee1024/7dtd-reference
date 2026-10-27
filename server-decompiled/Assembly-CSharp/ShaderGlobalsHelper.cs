using System;
using UnityEngine;

// Token: 0x0200139E RID: 5022
[ExecuteInEditMode]
public class ShaderGlobalsHelper : MonoBehaviour
{
	// Token: 0x06009D97 RID: 40343 RVA: 0x003B8C7C File Offset: 0x003B6E7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		Shader.SetGlobalFloat("_ElectricShockIntensity", this.electricShockIntensity);
		Shader.SetGlobalColor("_ElectricShockColor", this.electricShockColor);
		Shader.SetGlobalFloat("_ElectricShockSpeed", this.electricShockSpeed);
		Shader.SetGlobalFloat("_ElectricShockScale", this.electricShockScale);
		Shader.SetGlobalTexture("_ElectricShockTexture", this.electricShockTexture);
		Shader.SetGlobalVector("_ElectricShockTexture_ST", this.electricShockTexture_ST);
		Shader.SetGlobalFloat("_ElectricShockTexturePanSpeed", this.electricShockTexturePanSpeed);
	}

	// Token: 0x06009D98 RID: 40344 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x04007715 RID: 30485
	[Header("Electric Shock Shader Properties")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockIntensity = 1f;

	// Token: 0x04007716 RID: 30486
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color electricShockColor = new Color(0.5f, 0.8f, 1f, 1f);

	// Token: 0x04007717 RID: 30487
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockSpeed = 5f;

	// Token: 0x04007718 RID: 30488
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockScale = 2.5f;

	// Token: 0x04007719 RID: 30489
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture electricShockTexture;

	// Token: 0x0400771A RID: 30490
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 electricShockTexture_ST;

	// Token: 0x0400771B RID: 30491
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockTexturePanSpeed = 10f;

	// Token: 0x0400771C RID: 30492
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateEveryFrame;
}
