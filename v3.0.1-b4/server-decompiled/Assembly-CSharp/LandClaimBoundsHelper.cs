using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public static class LandClaimBoundsHelper
{
	// Token: 0x06002C2E RID: 11310 RVA: 0x0011795C File Offset: 0x00115B5C
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

	// Token: 0x06002C2F RID: 11311 RVA: 0x001179A4 File Offset: 0x00115BA4
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

	// Token: 0x06002C30 RID: 11312 RVA: 0x00117A38 File Offset: 0x00115C38
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

	// Token: 0x06002C31 RID: 11313 RVA: 0x00117BEC File Offset: 0x00115DEC
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

	// Token: 0x06002C32 RID: 11314 RVA: 0x00117C5C File Offset: 0x00115E5C
	public static void CleanupHelpers()
	{
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			LandClaimBoundsHelper.list[i].Remove();
			global::UnityEngine.Object.Destroy(LandClaimBoundsHelper.list[i].Helper);
		}
		LandClaimBoundsHelper.list.Clear();
	}

	// Token: 0x040021E5 RID: 8677
	[PublicizedFrom(EAccessModifier.Private)]
	public const string landClaimBoundaryMaterialPath = "Materials/LandClaimBoundary";

	// Token: 0x040021E6 RID: 8678
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goRoot;

	// Token: 0x040021E7 RID: 8679
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goPool;

	// Token: 0x040021E8 RID: 8680
	public static List<LandClaimBoundsHelper.BoundsHelperEntry> list = new List<LandClaimBoundsHelper.BoundsHelperEntry>();

	// Token: 0x02000543 RID: 1347
	public class BoundsHelperEntry
	{
		// Token: 0x06002C34 RID: 11316 RVA: 0x00117CB9 File Offset: 0x00115EB9
		public BoundsHelperEntry(Vector3 _position, Transform _helper)
		{
			this.Position = _position;
			this.Helper = _helper;
			Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00117CEF File Offset: 0x00115EEF
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnOriginChanged(Vector3 _newOrigin)
		{
			this.Helper.localPosition = this.Position - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00117D25 File Offset: 0x00115F25
		public void Remove()
		{
			Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x040021E9 RID: 8681
		public Vector3 Position;

		// Token: 0x040021EA RID: 8682
		public Transform Helper;
	}
}
