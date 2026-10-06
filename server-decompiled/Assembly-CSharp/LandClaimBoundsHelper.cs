using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public static class LandClaimBoundsHelper
{
	// Token: 0x06002C4E RID: 11342 RVA: 0x001197A0 File Offset: 0x001179A0
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

	// Token: 0x06002C4F RID: 11343 RVA: 0x001197E8 File Offset: 0x001179E8
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

	// Token: 0x06002C50 RID: 11344 RVA: 0x0011987C File Offset: 0x00117A7C
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

	// Token: 0x06002C51 RID: 11345 RVA: 0x00119A30 File Offset: 0x00117C30
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

	// Token: 0x06002C52 RID: 11346 RVA: 0x00119AA0 File Offset: 0x00117CA0
	public static void CleanupHelpers()
	{
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			LandClaimBoundsHelper.list[i].Remove();
			global::UnityEngine.Object.Destroy(LandClaimBoundsHelper.list[i].Helper);
		}
		LandClaimBoundsHelper.list.Clear();
	}

	// Token: 0x0400221F RID: 8735
	[PublicizedFrom(EAccessModifier.Private)]
	public const string landClaimBoundaryMaterialPath = "Materials/LandClaimBoundary";

	// Token: 0x04002220 RID: 8736
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goRoot;

	// Token: 0x04002221 RID: 8737
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goPool;

	// Token: 0x04002222 RID: 8738
	public static List<LandClaimBoundsHelper.BoundsHelperEntry> list = new List<LandClaimBoundsHelper.BoundsHelperEntry>();

	// Token: 0x0200054E RID: 1358
	public class BoundsHelperEntry
	{
		// Token: 0x06002C54 RID: 11348 RVA: 0x00119AFD File Offset: 0x00117CFD
		public BoundsHelperEntry(Vector3 _position, Transform _helper)
		{
			this.Position = _position;
			this.Helper = _helper;
			Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00119B33 File Offset: 0x00117D33
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnOriginChanged(Vector3 _newOrigin)
		{
			this.Helper.localPosition = this.Position - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00119B69 File Offset: 0x00117D69
		public void Remove()
		{
			Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x04002223 RID: 8739
		public Vector3 Position;

		// Token: 0x04002224 RID: 8740
		public Transform Helper;
	}
}
