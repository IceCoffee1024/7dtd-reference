using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001351 RID: 4945
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class SetShadowMapAsGlobalTexture : MonoBehaviour
{
	// Token: 0x06009B93 RID: 39827 RVA: 0x003AC9B0 File Offset: 0x003AABB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		this.SetupCommandBuffer();
	}

	// Token: 0x06009B94 RID: 39828 RVA: 0x003AC9C4 File Offset: 0x003AABC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.lightComponent.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
		this.ReleaseCommandBuffer();
	}

	// Token: 0x06009B95 RID: 39829 RVA: 0x003AC9E0 File Offset: 0x003AABE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupCommandBuffer()
	{
		this.commandBuffer = new CommandBuffer();
		this.commandBuffer.name = "SetShadowMapAsGlobalTexture";
		RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CurrentActive;
		this.commandBuffer.SetGlobalTexture(this.textureSemanticName, renderTargetIdentifier);
		this.lightComponent.AddCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
	}

	// Token: 0x06009B96 RID: 39830 RVA: 0x003ACA33 File Offset: 0x003AAC33
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReleaseCommandBuffer()
	{
		this.commandBuffer.Clear();
	}

	// Token: 0x0400757D RID: 30077
	public string textureSemanticName = "_SunCascadedShadowMap";

	// Token: 0x0400757E RID: 30078
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture shadowMapRenderTexture;

	// Token: 0x0400757F RID: 30079
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer commandBuffer;

	// Token: 0x04007580 RID: 30080
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light lightComponent;
}
