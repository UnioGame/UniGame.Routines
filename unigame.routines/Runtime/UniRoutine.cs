using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]

namespace UniModules.UniRoutine.Runtime {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using global::UniCore.Runtime.ProfilerTools;
	using Interfaces;
	using UniCore.Runtime.ObjectPool.Runtime;
	using UniCore.Runtime.ObjectPool.Runtime.Extensions;
	using UniGame.Core.Runtime.DataStructure;
	using UniGame.Core.Runtime.Interfaces;
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[Serializable]
	public class UniRoutine : IUniRoutine, IResetable
	{
		private int idCounter = 1;
		private Dictionary<int,UniRoutineTask> activeRoutines = new Dictionary<int, UniRoutineTask>(256);
		private UniLinkedList<UniRoutineTask> routineTasks = new UniLinkedList<UniRoutineTask>();
		
		public IUniRoutineTask AddRoutine(IEnumerator enumerator,bool moveNextImmediately = true, Action finalAction = null) {

			if (enumerator == null) return null;
			
			var routine = ClassPool.Spawn<UniRoutineTask>();
#if UNITY_EDITOR
			if (routine.IsCompleted == false) {
				GameLog.LogError("ROUTINE: routine task is not completed");
			}
#endif
			var id = idCounter++;
			routine.Initialize(id,enumerator, moveNextImmediately);
			if (finalAction != null)
			{
				routine.LifeTime.AddCleanUpAction(finalAction);
			}
			
			routineTasks.Add(routine);
			
			activeRoutines[id] = routine;
			
			return routine;
		}

		public bool AddFinally(int id, Action action)
		{
			
			if (!activeRoutines.TryGetValue(id, out var routineTask)) 
				return false;
			
			routineTask.LifeTime.AddCleanUpAction(action);
			return true;
		}
		
		public bool IsActive(int id)
		{
			return activeRoutines.ContainsKey(id);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CancelRoutine(int id)
		{
			if (!activeRoutines.TryGetValue(id, out var routineTask)) {
				return false;
			}
			routineTask.Dispose();
            activeRoutines.Remove(id);
            return true;
		}
		
		/// <summary>
		/// update all registered routine tasks
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Update()
		{
			var current = routineTasks.root;
			
			while (current!=null) {

				var next = current.Next;
				var routine = current.Value;
				
				var isComplete = routine.isComplete || routine.MoveNext() == false;

				if (isComplete) {
					routineTasks.Remove(current);
                    activeRoutines.Remove(current.Value.Id);
					current.Dispose();
					
					CancelRoutine(routine.IdValue);
					
					routine.Despawn();
				}

				current = next;
			}
		}

		public void Reset()
		{
			var current = routineTasks.root;
			while (current!=null) {

				var next    = current.Next;
				var routine = current.Value;
				routine.Complete();
				routine.Despawn();

				current = next;	
			}

			activeRoutines.Clear();
			routineTasks.Release();
		}
	}
}
