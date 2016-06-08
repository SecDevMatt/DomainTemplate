using System.Data.Entity;
using DomainTemplate.Interfaces;

namespace DomainTemplate.Helpers
{
    public static class StateHelper
    {
        public static EntityState ConvertState(State state)
        {
            switch (state)
            {
                case State.Added:
                    return EntityState.Added;
                case State.Modified:
                    return EntityState.Modified;
                case State.Deleted:
                    return EntityState.Deleted;
                case State.Unchanged:
                    return EntityState.Unchanged;
                default:
                    return EntityState.Unchanged;
            }
        }

        public static void ApplyStateChanges(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IObjectWithState>())
            {
                var stateInfo = entry.Entity;
                entry.State = ConvertState(stateInfo.State);
            }
        }
    }
}
