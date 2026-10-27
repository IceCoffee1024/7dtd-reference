using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200139D RID: 5021
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class SetShadowMapAsGlobalTexture : MonoBehaviour
{
	// Token: 0x06009D92 RID: 40338 RVA: 0x003B8BD8 File Offset: 0x003B6DD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		this.SetupCommandBuffer();
	}

	// Token: 0x06009D93 RID: 40339 RVA: 0x003B8BEC File Offset: 0x003B6DEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.lightComponent.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
		this.ReleaseCommandBuffer();
	}

	// Token: 0x06009D94 RID: 40340 RVA: 0x003B8C08 File Offset: 0x003B6E08
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupCommandBuffer()
	{
		this.commandBuffer = new CommandBuffer();
		this.commandBuffer.name = "SetShadowMapAsGlobalTexture";
		RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CurrentActive;
		this.commandBuffer.SetGlobalTexture(this.textureSemanticName, renderTargetIdentifier);
		this.lightComponent.AddCommandBuffer(LightEvent.AfterShadowMap, this.commandBuffer);
	}

	// Token: 0x06009D95 RID: 40341 RVA: 0x003B8C5B File Offset: 0x003B6E5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReleaseCommandBuffer()
	{
		this.commandBuffer.Clear();
	}

	// Token: 0x04007711 RID: 30481
	public string textureSemanticName = "_SunCascadedShadowMap";

	// Token: 0x04007712 RID: 30482
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public RenderTexture shadowMapRenderTexture;

	// Token: 0x04007713 RID: 30483
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer commandBuffer;

	// Token: 0x04007714 RID: 30484
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light lightComponent;
}
