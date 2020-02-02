namespace UniGreenModules.UniRoutine.Runtime {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Interfaces;
	using UniCore.Runtime.Interfaces;
	using UniCore.Runtime.ObjectPool.Runtime;
	using UniCore.Runtime.ObjectPool.Runtime.Extensions;
	using UniCore.Runtime.ProfilerTools;
	using UniGame.Core.Runtime.DataStructure.LinkedList;
	using Unity.IL2CPP.CompilerServices;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[Serializable]
	public class UniRoutine : IUniRoutine, IResetable
	{
		private int idCounter = 1;
		private Dictionary<int,UniRoutineTask> activeRoutines = new Dictionary<int, UniRoutineTask>();
		private UniLinkedList<UniRoutineTask> routineTasks = new UniLinkedList<UniRoutineTask>();
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IUniRoutineTask AddRoutine(IEnumerator enumerator,bool moveNextImmediately = true) {

			if (enumerator == null) return null;
			
			var routine = ClassPool.Spawn<UniRoutineTask>();

#if UNITY_EDITOR
			if (routine.IsCompleted == false) {
				GameLog.LogError("ROUTINE: routine task is not completed");
			}
#endif
			var id = idCounter++;
			//get routine from pool
			routine.Initialize(id,enumerator, moveNextImmediately);
			routineTasks.Add(routine);
			
			activeRoutines[id] = routine;
			
			return routine;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CancelRoutine(int id)
		{
			if (!activeRoutines.TryGetValue(id, out var routineTask)) {
				return false;
			}

			routineTask.Dispose();
			return true;
		}
		
		/// <summary>
		/// update all registered routine tasks
		/// </summary>
		public void Update()
		{
			var current = routineTasks.root;
			
			while (current!=null) {

				var next = current.Next;
				var routine = current.Value;
				
				var isComplete = routine.lifeTime.IsTerminated || routine.MoveNext() == false;

				if (isComplete) {
					routineTasks.Remove(current);
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
