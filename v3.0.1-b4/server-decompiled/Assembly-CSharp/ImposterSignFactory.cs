using System;
using UnityEngine;

// Token: 0x02000A5B RID: 2651
public static class ImposterSignFactory
{
	// Token: 0x06005051 RID: 20561 RVA: 0x001EB608 File Offset: 0x001E9808
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform GetPrefab(bool isDecal)
	{
		if (isDecal)
		{
			if (ImposterSignFactory.decalPrefab == null)
			{
				ImposterSignFactory.decalPrefab = DataLoader.LoadAsset<Transform>("@:Entities/Crafting/sign_decal_imposter_Prefab.prefab", false);
			}
			return ImposterSignFactory.decalPrefab;
		}
		if (ImposterSignFactory.canvasPrefab == null)
		{
			ImposterSignFactory.canvasPrefab = DataLoader.LoadAsset<Transform>("@:Entities/Crafting/sign_canvas_imposter_Prefab.prefab", false);
		}
		return ImposterSignFactory.canvasPrefab;
	}

	// Token: 0x06005052 RID: 20562 RVA: 0x001EB660 File Offset: 0x001E9860
	public static SignCanvas Create(GameObject _parent, ImposterCanvas _data)
	{
		ImposterSignFactory.<>c__DisplayClass5_0 CS$<>8__locals1 = new ImposterSignFactory.<>c__DisplayClass5_0();
		CS$<>8__locals1._data = _data;
		if (!CS$<>8__locals1._data.State.SignId.IsValid)
		{
			return null;
		}
		string text = (CS$<>8__locals1._data.IsDecal ? "@:Entities/Crafting/sign_decal_imposter_Prefab.prefab" : "@:Entities/Crafting/sign_canvas_imposter_Prefab.prefab");
		Transform prefab = ImposterSignFactory.GetPrefab(CS$<>8__locals1._data.IsDecal);
		if (prefab == null)
		{
			Log.Error("[ImposterSignFactory] Failed to load prefab: " + text);
			return null;
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(prefab.gameObject, _parent.transform);
		gameObject.name = string.Format("sign_{0:F0}_{1:F0}_{2:F0}", CS$<>8__locals1._data.WorldPosition.x, CS$<>8__locals1._data.WorldPosition.y, CS$<>8__locals1._data.WorldPosition.z);
		gameObject.transform.localPosition = CS$<>8__locals1._data.WorldPosition;
		gameObject.transform.localRotation = CS$<>8__locals1._data.WorldRotation;
		gameObject.transform.localScale = new Vector3(CS$<>8__locals1._data.Size.x, CS$<>8__locals1._data.Size.y, 1f);
		CS$<>8__locals1.canvas = gameObject.GetComponent<SignCanvas>();
		CS$<>8__locals1.canvas.State = CS$<>8__locals1._data.State.Clone();
		CS$<>8__locals1.canvas.CanvasAspect = CS$<>8__locals1._data.CanvasAspect;
		CS$<>8__locals1.canvas.Initialize(new SignDataManager.RenderingDataPatcher(CS$<>8__locals1.<Create>g__PatchRenderingData|0));
		CS$<>8__locals1._data.Canvas = CS$<>8__locals1.canvas;
		return CS$<>8__locals1.canvas;
	}

	// Token: 0x06005053 RID: 20563 RVA: 0x001EB806 File Offset: 0x001E9A06
	public static void Destroy(ImposterCanvas _data)
	{
		if (((_data != null) ? _data.Canvas : null) == null)
		{
			return;
		}
		_data.Canvas.Cleanup();
		_data.Canvas = null;
	}

	// Token: 0x06005054 RID: 20564 RVA: 0x001EB830 File Offset: 0x001E9A30
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector4 ComputeAspectST(float canvasAspect)
	{
		float num = ((canvasAspect >= 1f) ? 2f : (2f * canvasAspect));
		float num2 = ((canvasAspect >= 1f) ? (2f / canvasAspect) : 2f);
		return new Vector4(num, num2, -num * 0.5f, -num2 * 0.5f);
	}

	// Token: 0x04003D98 RID: 15768
	[PublicizedFrom(EAccessModifier.Private)]
	public const string CanvasPrefabPath = "@:Entities/Crafting/sign_canvas_imposter_Prefab.prefab";

	// Token: 0x04003D99 RID: 15769
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DecalPrefabPath = "@:Entities/Crafting/sign_decal_imposter_Prefab.prefab";

	// Token: 0x04003D9A RID: 15770
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform canvasPrefab;

	// Token: 0x04003D9B RID: 15771
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform decalPrefab;
}
