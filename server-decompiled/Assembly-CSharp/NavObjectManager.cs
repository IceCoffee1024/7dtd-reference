using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200075B RID: 1883
public class NavObjectManager
{
	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x060037F6 RID: 14326 RVA: 0x0016FF96 File Offset: 0x0016E196
	public static NavObjectManager Instance
	{
		get
		{
			if (NavObjectManager.instance == null)
			{
				NavObjectManager.instance = new NavObjectManager();
			}
			return NavObjectManager.instance;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x060037F7 RID: 14327 RVA: 0x0016FFAE File Offset: 0x0016E1AE
	public static bool HasInstance
	{
		get
		{
			return NavObjectManager.instance != null;
		}
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x0016FFB8 File Offset: 0x0016E1B8
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObjectManager()
	{
		NavObjectManager.instance = this;
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x0016FFF2 File Offset: 0x0016E1F2
	public void Cleanup()
	{
		NavObjectManager.instance = null;
		this.NavObjectList.Clear();
		this.removedNavObjectPool.Clear();
		this.tagList.Clear();
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060037FA RID: 14330 RVA: 0x0017001C File Offset: 0x0016E21C
	// (remove) Token: 0x060037FB RID: 14331 RVA: 0x00170054 File Offset: 0x0016E254
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectAdded;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x060037FC RID: 14332 RVA: 0x0017008C File Offset: 0x0016E28C
	// (remove) Token: 0x060037FD RID: 14333 RVA: 0x001700C4 File Offset: 0x0016E2C4
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRemoved;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060037FE RID: 14334 RVA: 0x001700FC File Offset: 0x0016E2FC
	// (remove) Token: 0x060037FF RID: 14335 RVA: 0x00170134 File Offset: 0x0016E334
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRefreshed;

	// Token: 0x06003800 RID: 14336 RVA: 0x0017016C File Offset: 0x0016E36C
	public NavObject RegisterNavObject(string className, Transform trackedTransform, string overrideSprite = "", bool hiddenOnCompass = false)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].IsTrackedTransform(trackedTransform))
			{
				return this.NavObjectList[i];
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.hiddenOnCompass = hiddenOnCompass;
		navObject.TrackedTransform = trackedTransform;
		navObject.OverrideSpriteName = overrideSprite;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x00170250 File Offset: 0x0016E450
	public NavObject RegisterNavObject(string className, Vector3 trackedPosition, string overrideSprite = "", bool hiddenOnCompass = false, int entityId = -1, Entity ownerEntity = null)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].NavObjectClass != null && !(className != this.NavObjectList[i].NavObjectClass.NavObjectClassName) && (!(ownerEntity != null) || !(ownerEntity != this.NavObjectList[i].OwnerEntity)))
			{
				if (entityId != -1 && this.NavObjectList[i].EntityID == entityId)
				{
					return this.NavObjectList[i];
				}
				if (this.NavObjectList[i].IsTrackedPosition(trackedPosition))
				{
					return this.NavObjectList[i];
				}
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.EntityID = entityId;
		navObject.OwnerEntity = ownerEntity;
		navObject.hiddenOnCompass = hiddenOnCompass;
		navObject.TrackedPosition = trackedPosition;
		navObject.OverrideSpriteName = overrideSprite;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x001703C8 File Offset: 0x0016E5C8
	public NavObject RegisterNavObject(string className, Entity trackedEntity, string overrideSprite = "", bool hiddenOnCompass = false)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].IsTrackedEntity(trackedEntity) && (this.NavObjectList[i].NavObjectClass == null || this.NavObjectList[i].NavObjectClass.NavObjectClassName == className))
			{
				this.NavObjectList[i].TrackedEntity = trackedEntity;
				return this.NavObjectList[i];
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.OverrideSpriteName = overrideSprite;
		navObject.TrackedEntity = trackedEntity;
		navObject.hiddenOnCompass = hiddenOnCompass;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x001704F0 File Offset: 0x0016E6F0
	public void UnRegisterNavObject(NavObject navObject)
	{
		if (navObject == null)
		{
			return;
		}
		bool flag = false;
		if (this.NavObjectList.Contains(navObject))
		{
			flag = true;
			this.NavObjectList.Remove(navObject);
		}
		if (!this.removedNavObjectPool.Contains(navObject))
		{
			this.removedNavObjectPool.Add(navObject);
		}
		if (flag)
		{
			this.RemoveNavObjectTag(navObject);
			if (this.OnNavObjectRemoved != null)
			{
				this.OnNavObjectRemoved(navObject);
			}
		}
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x00170558 File Offset: 0x0016E758
	public void UnRegisterNavObjectByPosition(Vector3 position, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.IsTrackedPosition(position) && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("UnRegisterNavObjectByOwnerEntity {0}", position));
	}

	// Token: 0x06003805 RID: 14341 RVA: 0x001705A0 File Offset: 0x0016E7A0
	public void UnRegisterNavObjectByOwnerEntity(Entity ownerEntity, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.OwnerEntity == ownerEntity && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("\"UnRegisterNavObjectByOwnerEntity {0}", ownerEntity));
	}

	// Token: 0x06003806 RID: 14342 RVA: 0x001705E4 File Offset: 0x0016E7E4
	public void UnRegisterNavObjectByEntityID(int entityId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.EntityID == entityId, string.Format("UnRegisterNavObjectByEntityID {0}", entityId));
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x00170628 File Offset: 0x0016E828
	public void UnRegisterNavObjectByClass(string className)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == className, string.Format("UnRegisterNavObjectByClass {0}", className));
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x00170664 File Offset: 0x0016E864
	public void UnRegisterNavObjectByClass(int classId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName.GetHashCode() == classId, string.Format("UnRegisterNavObjectByClass {0}", EntityClass.GetEntityClassName(classId)));
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x001706A8 File Offset: 0x0016E8A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void unRegisterNavObjects(NavObjectManager.includeInList includeTest, string logStringPrefix)
	{
		foreach (NavObject navObject in this.NavObjectList)
		{
			if (includeTest(navObject))
			{
				this.removeList.Add(navObject);
			}
		}
		foreach (NavObject navObject2 in this.removeList)
		{
			this.NavObjectList.Remove(navObject2);
			this.RemoveNavObjectTag(navObject2);
			if (!this.removedNavObjectPool.Contains(navObject2))
			{
				this.removedNavObjectPool.Add(navObject2);
			}
			NavObjectManager.NavObjectChangedDelegate onNavObjectRemoved = this.OnNavObjectRemoved;
			if (onNavObjectRemoved != null)
			{
				onNavObjectRemoved(navObject2);
			}
		}
		this.removeList.Clear();
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x00170790 File Offset: 0x0016E990
	public void AddNavObjectTag(NavObject navObject)
	{
		for (int i = 0; i < navObject.NavObjectClassList.Count; i++)
		{
			if (!string.IsNullOrEmpty(navObject.NavObjectClassList[i].Tag))
			{
				if (!this.tagList.ContainsKey(navObject.NavObjectClassList[i].Tag))
				{
					this.tagList.Add(navObject.NavObjectClassList[i].Tag, new List<NavObject>());
				}
				this.tagList[navObject.NavObjectClassList[i].Tag].Add(navObject);
			}
		}
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x00170830 File Offset: 0x0016EA30
	public void RemoveNavObjectTag(NavObject navObject)
	{
		for (int i = 0; i < navObject.NavObjectClassList.Count; i++)
		{
			if (!string.IsNullOrEmpty(navObject.NavObjectClassList[i].Tag))
			{
				if (!this.tagList.ContainsKey(navObject.NavObjectClassList[i].Tag))
				{
					this.tagList.Add(navObject.NavObjectClassList[i].Tag, new List<NavObject>());
				}
				this.tagList[navObject.NavObjectClassList[i].Tag].Remove(navObject);
			}
		}
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x001708D0 File Offset: 0x0016EAD0
	public bool HasNavObjectTag(string tag)
	{
		if (this.tagList.ContainsKey(tag))
		{
			for (int i = 0; i < this.tagList[tag].Count; i++)
			{
				if (this.tagList[tag][i].NavObjectClass != null && this.tagList[tag][i].NavObjectClass.Tag == tag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x00170948 File Offset: 0x0016EB48
	public NavObject GetNavObjectByEntityID(int entityId)
	{
		for (int i = this.NavObjectList.Count - 1; i >= 0; i--)
		{
			NavObject navObject = this.NavObjectList[i];
			if (navObject != null && navObject.EntityID == entityId)
			{
				return this.NavObjectList[i];
			}
		}
		return null;
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x00170998 File Offset: 0x0016EB98
	public void RefreshNavObjects()
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			bool flag = false;
			if (this.NavObjectList[i].NavObjectClass != null)
			{
				this.NavObjectList[i].NavObjectClass = NavObjectClass.GetNavObjectClass(this.NavObjectList[i].NavObjectClass.NavObjectClassName);
				flag = true;
			}
			if (this.NavObjectList[i].NavObjectClassList != null)
			{
				for (int j = 0; j < this.NavObjectList[i].NavObjectClassList.Count; j++)
				{
					this.NavObjectList[i].NavObjectClassList[j] = NavObjectClass.GetNavObjectClass(this.NavObjectList[i].NavObjectClassList[j].NavObjectClassName);
					flag = true;
				}
			}
			if (flag && this.OnNavObjectRefreshed != null)
			{
				this.OnNavObjectRefreshed(this.NavObjectList[i]);
			}
		}
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x00170A98 File Offset: 0x0016EC98
	public void Update()
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		for (int i = this.NavObjectList.Count - 1; i >= 0; i--)
		{
			NavObject navObject = this.NavObjectList[i];
			if (!navObject.IsValid())
			{
				this.UnRegisterNavObject(navObject);
			}
			else if (primaryPlayer != null)
			{
				navObject.HandleActiveNavClass(primaryPlayer);
			}
		}
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x00170B08 File Offset: 0x0016ED08
	[Conditional("DEBUG_NAV")]
	public static void LogNav(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Nav {1}", GameManager.frameCount, _format);
		Log.Out(_format, _args);
	}

	// Token: 0x04002E2A RID: 11818
	[PublicizedFrom(EAccessModifier.Private)]
	public static NavObjectManager instance;

	// Token: 0x04002E2E RID: 11822
	public List<NavObject> NavObjectList = new List<NavObject>();

	// Token: 0x04002E2F RID: 11823
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removedNavObjectPool = new List<NavObject>();

	// Token: 0x04002E30 RID: 11824
	public Dictionary<string, List<NavObject>> tagList = new Dictionary<string, List<NavObject>>();

	// Token: 0x04002E31 RID: 11825
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removeList = new List<NavObject>();

	// Token: 0x0200075C RID: 1884
	// (Invoke) Token: 0x06003812 RID: 14354
	public delegate void NavObjectChangedDelegate(NavObject newNavObject);

	// Token: 0x0200075D RID: 1885
	// (Invoke) Token: 0x06003816 RID: 14358
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool includeInList(NavObject navObject);
}
