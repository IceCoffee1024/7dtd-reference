using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class NavObjectManager
{
	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x00171892 File Offset: 0x0016FA92
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

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06003851 RID: 14417 RVA: 0x001718AA File Offset: 0x0016FAAA
	public static bool HasInstance
	{
		get
		{
			return NavObjectManager.instance != null;
		}
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x001718B4 File Offset: 0x0016FAB4
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObjectManager()
	{
		NavObjectManager.instance = this;
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x001718EE File Offset: 0x0016FAEE
	public void Cleanup()
	{
		NavObjectManager.instance = null;
		this.NavObjectList.Clear();
		this.removedNavObjectPool.Clear();
		this.tagList.Clear();
	}

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06003854 RID: 14420 RVA: 0x00171918 File Offset: 0x0016FB18
	// (remove) Token: 0x06003855 RID: 14421 RVA: 0x00171950 File Offset: 0x0016FB50
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectAdded;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x06003856 RID: 14422 RVA: 0x00171988 File Offset: 0x0016FB88
	// (remove) Token: 0x06003857 RID: 14423 RVA: 0x001719C0 File Offset: 0x0016FBC0
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRemoved;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06003858 RID: 14424 RVA: 0x001719F8 File Offset: 0x0016FBF8
	// (remove) Token: 0x06003859 RID: 14425 RVA: 0x00171A30 File Offset: 0x0016FC30
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRefreshed;

	// Token: 0x0600385A RID: 14426 RVA: 0x00171A68 File Offset: 0x0016FC68
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

	// Token: 0x0600385B RID: 14427 RVA: 0x00171B4C File Offset: 0x0016FD4C
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

	// Token: 0x0600385C RID: 14428 RVA: 0x00171CC4 File Offset: 0x0016FEC4
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

	// Token: 0x0600385D RID: 14429 RVA: 0x00171DEC File Offset: 0x0016FFEC
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

	// Token: 0x0600385E RID: 14430 RVA: 0x00171E54 File Offset: 0x00170054
	public void UnRegisterNavObjectByPosition(Vector3 position, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.IsTrackedPosition(position) && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("UnRegisterNavObjectByOwnerEntity {0}", position));
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x00171E9C File Offset: 0x0017009C
	public void UnRegisterNavObjectByOwnerEntity(Entity ownerEntity, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.OwnerEntity == ownerEntity && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("\"UnRegisterNavObjectByOwnerEntity {0}", ownerEntity));
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x00171EE0 File Offset: 0x001700E0
	public void UnRegisterNavObjectByEntityID(int entityId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.EntityID == entityId, string.Format("UnRegisterNavObjectByEntityID {0}", entityId));
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00171F24 File Offset: 0x00170124
	public void UnRegisterNavObjectByClass(string className)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == className, string.Format("UnRegisterNavObjectByClass {0}", className));
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00171F60 File Offset: 0x00170160
	public void UnRegisterNavObjectByClass(int classId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName.GetHashCode() == classId, string.Format("UnRegisterNavObjectByClass {0}", EntityClass.GetEntityClassName(classId)));
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x00171FA4 File Offset: 0x001701A4
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

	// Token: 0x06003864 RID: 14436 RVA: 0x0017208C File Offset: 0x0017028C
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

	// Token: 0x06003865 RID: 14437 RVA: 0x0017212C File Offset: 0x0017032C
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

	// Token: 0x06003866 RID: 14438 RVA: 0x001721CC File Offset: 0x001703CC
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

	// Token: 0x06003867 RID: 14439 RVA: 0x00172244 File Offset: 0x00170444
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

	// Token: 0x06003868 RID: 14440 RVA: 0x00172294 File Offset: 0x00170494
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

	// Token: 0x06003869 RID: 14441 RVA: 0x00172394 File Offset: 0x00170594
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

	// Token: 0x0600386A RID: 14442 RVA: 0x00172404 File Offset: 0x00170604
	[Conditional("DEBUG_NAV")]
	public static void LogNav(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Nav {1}", GameManager.frameCount, _format);
		Log.Out(_format, _args);
	}

	// Token: 0x04002E3F RID: 11839
	[PublicizedFrom(EAccessModifier.Private)]
	public static NavObjectManager instance;

	// Token: 0x04002E43 RID: 11843
	public List<NavObject> NavObjectList = new List<NavObject>();

	// Token: 0x04002E44 RID: 11844
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removedNavObjectPool = new List<NavObject>();

	// Token: 0x04002E45 RID: 11845
	public Dictionary<string, List<NavObject>> tagList = new Dictionary<string, List<NavObject>>();

	// Token: 0x04002E46 RID: 11846
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removeList = new List<NavObject>();

	// Token: 0x02000766 RID: 1894
	// (Invoke) Token: 0x0600386C RID: 14444
	public delegate void NavObjectChangedDelegate(NavObject newNavObject);

	// Token: 0x02000767 RID: 1895
	// (Invoke) Token: 0x06003870 RID: 14448
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool includeInList(NavObject navObject);
}
