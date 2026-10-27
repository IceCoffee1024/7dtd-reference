using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x020017E1 RID: 6113
	public static class UAIBase
	{
		// Token: 0x0600BCEE RID: 48366 RVA: 0x0045EC8A File Offset: 0x0045CE8A
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

		// Token: 0x0600BCEF RID: 48367 RVA: 0x0045ECC4 File Offset: 0x0045CEC4
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

		// Token: 0x0600BCF0 RID: 48368 RVA: 0x0045ED8C File Offset: 0x0045CF8C
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

		// Token: 0x0600BCF1 RID: 48369 RVA: 0x0045EECC File Offset: 0x0045D0CC
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

		// Token: 0x0600BCF2 RID: 48370 RVA: 0x0045EF24 File Offset: 0x0045D124
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

		// Token: 0x0600BCF3 RID: 48371 RVA: 0x0045F001 File Offset: 0x0045D201
		public static void Cleanup()
		{
			if (UAIBase.AIPackages != null)
			{
				UAIBase.AIPackages.Clear();
			}
		}

		// Token: 0x0600BCF4 RID: 48372 RVA: 0x0045F014 File Offset: 0x0045D214
		public static void Reload()
		{
			UAIBase.AIPackages.Clear();
			WorldStaticData.Reset("utilityai");
		}

		// Token: 0x04008D76 RID: 36214
		public static Dictionary<string, UAIPackage> AIPackages = new Dictionary<string, UAIPackage>();

		// Token: 0x04008D77 RID: 36215
		public static int MaxEntitiesToConsider = 5;

		// Token: 0x04008D78 RID: 36216
		public static int MaxWaypointsToConsider = 5;

		// Token: 0x04008D79 RID: 36217
		public static float ActionChoiceDelay = 0.2f;
	}
}
