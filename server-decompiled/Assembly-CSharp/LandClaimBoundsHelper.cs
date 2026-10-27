using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public static class LandClaimBoundsHelper
{
	// Token: 0x06002C56 RID: 11350 RVA: 0x00119D2C File Offset: 0x00117F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static LandClaimBoundsHelper.BoundsHelperEntry GetEntryFromList(Vector3 _worldPos)
	{
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			if (LandClaimBoundsHelper.list[i].Position == _worldPos)
			{
				return LandClaimBoundsHelper.list[i];
			}
		}
		return null;
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x00119D74 File Offset: 0x00117F74
	public static void RemoveBoundsHelper(Vector3 _worldPos)
	{
		Transform transform = null;
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			if (LandClaimBoundsHelper.list[i].Position == _worldPos)
			{
				LandClaimBoundsHelper.list[i].Remove();
				transform = LandClaimBoundsHelper.list[i].Helper;
				LandClaimBoundsHelper.list.RemoveAt(i);
			}
		}
		if (transform != null)
		{
			transform.parent = LandClaimBoundsHelper.goPool;
			transform.localPosition = Vector3.zero;
			transform.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x00119E08 File Offset: 0x00118008
	public static Transform GetBoundsHelper(Vector3 _worldPos)
	{
		if (LandClaimBoundsHelper.goRoot == null)
		{
			LandClaimBoundsHelper.InitHelpers();
		}
		LandClaimBoundsHelper.BoundsHelperEntry entryFromList = LandClaimBoundsHelper.GetEntryFromList(_worldPos);
		Transform transform;
		if (entryFromList != null)
		{
			transform = entryFromList.Helper;
		}
		else
		{
			if (LandClaimBoundsHelper.goPool.childCount > 0)
			{
				transform = LandClaimBoundsHelper.goPool.GetChild(0);
				transform.parent = LandClaimBoundsHelper.goRoot;
			}
			else
			{
				List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
				if (localPlayers == null || localPlayers.Count <= 0)
				{
					return null;
				}
				NGuiWdwInGameHUD inGameHUD = LocalPlayerUI.GetUIForPlayer(localPlayers[0]).nguiWindowManager.InGameHUD;
				GameObject gameObject = new GameObject("LandClaimBoundary");
				GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				global::UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				Renderer component = gameObject2.GetComponent<Renderer>();
				Material material = Resources.Load("Materials/LandClaimBoundary", typeof(Material)) as Material;
				component.material = material;
				transform = gameObject.transform;
				transform.transform.parent = LandClaimBoundsHelper.goRoot;
			}
			Vector3 one = Vector3.one;
			transform.localPosition = new Vector3(0.5f, 0.01f, 0.5f);
			float num = (float)GameStats.GetInt(EnumGameStats.LandClaimSize);
			transform.localScale = new Vector3(num, num * 10000f, num);
			transform.localPosition = _worldPos - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
			LandClaimBoundsHelper.list.Add(new LandClaimBoundsHelper.BoundsHelperEntry(_worldPos, transform));
		}
		return transform;
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x00119FBC File Offset: 0x001181BC
	public static void InitHelpers()
	{
		if (LandClaimBoundsHelper.goRoot == null)
		{
			LandClaimBoundsHelper.goRoot = new GameObject("LandClaimHelpers").transform;
			LandClaimBoundsHelper.goPool = new GameObject("Pool").transform;
			LandClaimBoundsHelper.goPool.parent = LandClaimBoundsHelper.goRoot;
			LandClaimBoundsHelper.goPool.localPosition = new Vector3(9999f, 9999f, 9999f);
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x0011A02C File Offset: 0x0011822C
	public static void CleanupHelpers()
	{
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			LandClaimBoundsHelper.list[i].Remove();
			global::UnityEngine.Object.Destroy(LandClaimBoundsHelper.list[i].Helper);
		}
		LandClaimBoundsHelper.list.Clear();
	}

	// Token: 0x04002236 RID: 8758
	[PublicizedFrom(EAccessModifier.Private)]
	public const string landClaimBoundaryMaterialPath = "Materials/LandClaimBoundary";

	// Token: 0x04002237 RID: 8759
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goRoot;

	// Token: 0x04002238 RID: 8760
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goPool;

	// Token: 0x04002239 RID: 8761
	public static List<LandClaimBoundsHelper.BoundsHelperEntry> list = new List<LandClaimBoundsHelper.BoundsHelperEntry>();

	// Token: 0x02000552 RID: 1362
	public class BoundsHelperEntry
	{
		// Token: 0x06002C5C RID: 11356 RVA: 0x0011A089 File Offset: 0x00118289
		public BoundsHelperEntry(Vector3 _position, Transform _helper)
		{
			this.Position = _position;
			this.Helper = _helper;
			Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x0011A0BF File Offset: 0x001182BF
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnOriginChanged(Vector3 _newOrigin)
		{
			this.Helper.localPosition = this.Position - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x0011A0F5 File Offset: 0x001182F5
		public void Remove()
		{
			Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x0400223A RID: 8762
		public Vector3 Position;

		// Token: 0x0400223B RID: 8763
		public Transform Helper;
	}
}
