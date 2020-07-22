namespace UniGreenModules.UniRoutine.Runtime
{
    public struct RoutineHandle
    {
        public readonly int Id;
        public readonly RoutineType Type;
        public readonly RoutineScope Scope;

        public RoutineHandle(int id, RoutineType routineType,RoutineScope scope)
        {
            Id = id;
            Type = routineType;
            Scope = scope;
        }

        public override int GetHashCode() => Id;

        public bool Equals(RoutineHandle obj) => Id == obj.Id;

        public override bool Equals(object obj) 
        {
            if (obj is RoutineHandle value)
                return Id == value.Id;

            // compare elements here
            return false;
        }
    }
}