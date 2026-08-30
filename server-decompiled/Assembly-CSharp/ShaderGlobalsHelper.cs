using System;
using UnityEngine;

// Token: 0x0200136C RID: 4972
[ExecuteInEditMode]
public class ShaderGlobalsHelper : MonoBehaviour
{
	// Token: 0x06009C93 RID: 40083 RVA: 0x003B1CFC File Offset: 0x003AFEFC
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

	// Token: 0x06009C94 RID: 40084 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
	}

	// Token: 0x04007659 RID: 30297
	[Header("Electric Shock Shader Properties")]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockIntensity = 1f;

	// Token: 0x0400765A RID: 30298
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Color electricShockColor = new Color(0.5f, 0.8f, 1f, 1f);

	// Token: 0x0400765B RID: 30299
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockSpeed = 5f;

	// Token: 0x0400765C RID: 30300
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockScale = 2.5f;

	// Token: 0x0400765D RID: 30301
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture electricShockTexture;

	// Token: 0x0400765E RID: 30302
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 electricShockTexture_ST;

	// Token: 0x0400765F RID: 30303
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public float electricShockTexturePanSpeed = 10f;

	// Token: 0x04007660 RID: 30304
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateEveryFrame;
}
