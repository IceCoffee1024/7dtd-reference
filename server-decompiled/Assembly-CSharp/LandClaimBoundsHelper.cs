using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public static class LandClaimBoundsHelper
{
	// Token: 0x06002C58 RID: 11352 RVA: 0x001189D0 File Offset: 0x00116BD0
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

	// Token: 0x06002C59 RID: 11353 RVA: 0x00118A18 File Offset: 0x00116C18
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

	// Token: 0x06002C5A RID: 11354 RVA: 0x00118AAC File Offset: 0x00116CAC
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

	// Token: 0x06002C5B RID: 11355 RVA: 0x00118C60 File Offset: 0x00116E60
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

	// Token: 0x06002C5C RID: 11356 RVA: 0x00118CD0 File Offset: 0x00116ED0
	public static void CleanupHelpers()
	{
		for (int i = 0; i < LandClaimBoundsHelper.list.Count; i++)
		{
			LandClaimBoundsHelper.list[i].Remove();
			global::UnityEngine.Object.Destroy(LandClaimBoundsHelper.list[i].Helper);
		}
		LandClaimBoundsHelper.list.Clear();
	}

	// Token: 0x04002217 RID: 8727
	[PublicizedFrom(EAccessModifier.Private)]
	public const string landClaimBoundaryMaterialPath = "Materials/LandClaimBoundary";

	// Token: 0x04002218 RID: 8728
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goRoot;

	// Token: 0x04002219 RID: 8729
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform goPool;

	// Token: 0x0400221A RID: 8730
	public static List<LandClaimBoundsHelper.BoundsHelperEntry> list = new List<LandClaimBoundsHelper.BoundsHelperEntry>();

	// Token: 0x02000548 RID: 1352
	public class BoundsHelperEntry
	{
		// Token: 0x06002C5E RID: 11358 RVA: 0x00118D2D File Offset: 0x00116F2D
		public BoundsHelperEntry(Vector3 _position, Transform _helper)
		{
			this.Position = _position;
			this.Helper = _helper;
			Origin.OriginChanged = (Action<Vector3>)Delegate.Combine(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x00118D63 File Offset: 0x00116F63
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnOriginChanged(Vector3 _newOrigin)
		{
			this.Helper.localPosition = this.Position - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x00118D99 File Offset: 0x00116F99
		public void Remove()
		{
			Origin.OriginChanged = (Action<Vector3>)Delegate.Remove(Origin.OriginChanged, new Action<Vector3>(this.OnOriginChanged));
		}

		// Token: 0x0400221B RID: 8731
		public Vector3 Position;

		// Token: 0x0400221C RID: 8732
		public Transform Helper;
	}
}
