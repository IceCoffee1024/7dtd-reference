using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x02001784 RID: 6020
	public static class UAIBase
	{
		// Token: 0x0600BA83 RID: 47747 RVA: 0x00456DBE File Offset: 0x00454FBE
		public static void Update(Context _context)
		{
			if (_context.updateTimer <= 0f)
			{
				_context.updateTimer = UAIBase.ActionChoiceDelay;
				UAIBase.chooseAction(_context);
			}
			UAIBase.updateAction(_context);
			_context.updateTimer -= Time.deltaTime;
		}

		// Token: 0x0600BA84 RID: 47748 RVA: 0x00456DF8 File Offset: 0x00454FF8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void updateAction(Context _context)
		{
			if (_context.ActionData.CurrentTask == null)
			{
				return;
			}
			if (!_context.ActionData.Initialized)
			{
				_context.ActionData.CurrentTask.Init(_context);
			}
			if (!_context.ActionData.Started)
			{
				_context.ActionData.CurrentTask.Start(_context);
			}
			if (_context.ActionData.Executing)
			{
				_context.ActionData.CurrentTask.Update(_context);
				return;
			}
			_context.ActionData.CurrentTask.Reset(_context);
			if (_context.ActionData.TaskIndex + 1 < _context.ActionData.Action.GetTasks().Count)
			{
				_context.ActionData.TaskIndex = _context.ActionData.TaskIndex + 1;
				return;
			}
			_context.ActionData.Action = null;
		}

		// Token: 0x0600BA85 RID: 47749 RVA: 0x00456EC0 File Offset: 0x004550C0
		[PublicizedFrom(EAccessModifier.Private)]
		public static void chooseAction(Context _context)
		{
			float num = 0f;
			_context.ConsiderationData.EntityTargets.Clear();
			_context.ConsiderationData.WaypointTargets.Clear();
			UAIBase.addEntityTargetsToConsider(_context);
			UAIBase.addWaypointTargetsToConsider(_context);
			for (int i = 0; i < _context.AIPackages.Count; i++)
			{
				UAIAction uaiaction;
				object obj;
				if (UAIBase.AIPackages.ContainsKey(_context.AIPackages[i]) && UAIBase.AIPackages[_context.AIPackages[i]].DecideAction(_context, out uaiaction, out obj) * UAIBase.AIPackages[_context.AIPackages[i]].Weight > num && _context.ActionData.Action != uaiaction)
				{
					if (_context.ActionData.Action != null && _context.ActionData.CurrentTask != null)
					{
						if (_context.ActionData.Started)
						{
							_context.ActionData.CurrentTask.Stop(_context);
						}
						if (_context.ActionData.Initialized)
						{
							_context.ActionData.CurrentTask.Reset(_context);
						}
					}
					_context.ActionData.Action = uaiaction;
					_context.ActionData.Target = obj;
					_context.ActionData.TaskIndex = 0;
				}
			}
		}

		// Token: 0x0600BA86 RID: 47750 RVA: 0x00457000 File Offset: 0x00455200
		[PublicizedFrom(EAccessModifier.Private)]
		public static void addWaypointTargetsToConsider(Context _context)
		{
			if (_context.ConsiderationData.WaypointTargets == null)
			{
				_context.ConsiderationData.WaypointTargets = new List<Vector3>();
			}
			if (_context.ConsiderationData.WaypointTargets.Count > 1)
			{
				_context.ConsiderationData.WaypointTargets.Sort(new UAIUtils.NearestWaypointSorter(_context.Self));
			}
		}

		// Token: 0x0600BA87 RID: 47751 RVA: 0x00457058 File Offset: 0x00455258
		[PublicizedFrom(EAccessModifier.Private)]
		public static void addEntityTargetsToConsider(Context _context)
		{
			if (_context.ConsiderationData.EntityTargets == null)
			{
				_context.ConsiderationData.EntityTargets = new List<Entity>();
			}
			if (_context.Self.GetRevengeTarget() != null)
			{
				_context.ConsiderationData.EntityTargets.Add(_context.Self.GetRevengeTarget());
			}
			_context.ConsiderationData.EntityTargets.AddRange(_context.Self.world.GetEntitiesInBounds(_context.Self, BoundsUtils.ExpandBounds(_context.Self.boundingBox, _context.Self.GetSeeDistance(), _context.Self.GetSeeDistance(), _context.Self.GetSeeDistance())));
			if (_context.ConsiderationData.EntityTargets.Count > 1)
			{
				_context.ConsiderationData.EntityTargets.Sort(new UAIUtils.NearestEntitySorter(_context.Self));
			}
		}

		// Token: 0x0600BA88 RID: 47752 RVA: 0x00457135 File Offset: 0x00455335
		public static void Cleanup()
		{
			if (UAIBase.AIPackages != null)
			{
				UAIBase.AIPackages.Clear();
			}
		}

		// Token: 0x0600BA89 RID: 47753 RVA: 0x00457148 File Offset: 0x00455348
		public static void Reload()
		{
			UAIBase.AIPackages.Clear();
			WorldStaticData.Reset("utilityai");
		}

		// Token: 0x04008BC8 RID: 35784
		public static Dictionary<string, UAIPackage> AIPackages = new Dictionary<string, UAIPackage>();

		// Token: 0x04008BC9 RID: 35785
		public static int MaxEntitiesToConsider = 5;

		// Token: 0x04008BCA RID: 35786
		public static int MaxWaypointsToConsider = 5;

		// Token: 0x04008BCB RID: 35787
		public static float ActionChoiceDelay = 0.2f;
	}
}
