using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200075D RID: 1885
public class NavObjectManager
{
	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06003839 RID: 14393 RVA: 0x001708FA File Offset: 0x0016EAFA
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

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x0600383A RID: 14394 RVA: 0x00170912 File Offset: 0x0016EB12
	public static bool HasInstance
	{
		get
		{
			return NavObjectManager.instance != null;
		}
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x0017091C File Offset: 0x0016EB1C
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObjectManager()
	{
		NavObjectManager.instance = this;
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00170956 File Offset: 0x0016EB56
	public void Cleanup()
	{
		NavObjectManager.instance = null;
		this.NavObjectList.Clear();
		this.removedNavObjectPool.Clear();
		this.tagList.Clear();
	}

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x0600383D RID: 14397 RVA: 0x00170980 File Offset: 0x0016EB80
	// (remove) Token: 0x0600383E RID: 14398 RVA: 0x001709B8 File Offset: 0x0016EBB8
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectAdded;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x0600383F RID: 14399 RVA: 0x001709F0 File Offset: 0x0016EBF0
	// (remove) Token: 0x06003840 RID: 14400 RVA: 0x00170A28 File Offset: 0x0016EC28
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRemoved;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06003841 RID: 14401 RVA: 0x00170A60 File Offset: 0x0016EC60
	// (remove) Token: 0x06003842 RID: 14402 RVA: 0x00170A98 File Offset: 0x0016EC98
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRefreshed;

	// Token: 0x06003843 RID: 14403 RVA: 0x00170AD0 File Offset: 0x0016ECD0
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

	// Token: 0x06003844 RID: 14404 RVA: 0x00170BB4 File Offset: 0x0016EDB4
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

	// Token: 0x06003845 RID: 14405 RVA: 0x00170D2C File Offset: 0x0016EF2C
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

	// Token: 0x06003846 RID: 14406 RVA: 0x00170E54 File Offset: 0x0016F054
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

	// Token: 0x06003847 RID: 14407 RVA: 0x00170EBC File Offset: 0x0016F0BC
	public void UnRegisterNavObjectByPosition(Vector3 position, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.IsTrackedPosition(position) && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("UnRegisterNavObjectByOwnerEntity {0}", position));
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x00170F04 File Offset: 0x0016F104
	public void UnRegisterNavObjectByOwnerEntity(Entity ownerEntity, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.OwnerEntity == ownerEntity && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("\"UnRegisterNavObjectByOwnerEntity {0}", ownerEntity));
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x00170F48 File Offset: 0x0016F148
	public void UnRegisterNavObjectByEntityID(int entityId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.EntityID == entityId, string.Format("UnRegisterNavObjectByEntityID {0}", entityId));
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x00170F8C File Offset: 0x0016F18C
	public void UnRegisterNavObjectByClass(string className)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == className, string.Format("UnRegisterNavObjectByClass {0}", className));
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x00170FC8 File Offset: 0x0016F1C8
	public void UnRegisterNavObjectByClass(int classId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName.GetHashCode() == classId, string.Format("UnRegisterNavObjectByClass {0}", EntityClass.GetEntityClassName(classId)));
	}

	// Token: 0x0600384C RID: 14412 RVA: 0x0017100C File Offset: 0x0016F20C
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

	// Token: 0x0600384D RID: 14413 RVA: 0x001710F4 File Offset: 0x0016F2F4
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

	// Token: 0x0600384E RID: 14414 RVA: 0x00171194 File Offset: 0x0016F394
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

	// Token: 0x0600384F RID: 14415 RVA: 0x00171234 File Offset: 0x0016F434
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

	// Token: 0x06003850 RID: 14416 RVA: 0x001712AC File Offset: 0x0016F4AC
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

	// Token: 0x06003851 RID: 14417 RVA: 0x001712FC File Offset: 0x0016F4FC
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

	// Token: 0x06003852 RID: 14418 RVA: 0x001713FC File Offset: 0x0016F5FC
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

	// Token: 0x06003853 RID: 14419 RVA: 0x0017146C File Offset: 0x0016F66C
	[Conditional("DEBUG_NAV")]
	public static void LogNav(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Nav {1}", GameManager.frameCount, _format);
		Log.Out(_format, _args);
	}

	// Token: 0x04002E23 RID: 11811
	[PublicizedFrom(EAccessModifier.Private)]
	public static NavObjectManager instance;

	// Token: 0x04002E27 RID: 11815
	public List<NavObject> NavObjectList = new List<NavObject>();

	// Token: 0x04002E28 RID: 11816
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removedNavObjectPool = new List<NavObject>();

	// Token: 0x04002E29 RID: 11817
	public Dictionary<string, List<NavObject>> tagList = new Dictionary<string, List<NavObject>>();

	// Token: 0x04002E2A RID: 11818
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removeList = new List<NavObject>();

	// Token: 0x0200075E RID: 1886
	// (Invoke) Token: 0x06003855 RID: 14421
	public delegate void NavObjectChangedDelegate(NavObject newNavObject);

	// Token: 0x0200075F RID: 1887
	// (Invoke) Token: 0x06003859 RID: 14425
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool includeInList(NavObject navObject);
}
