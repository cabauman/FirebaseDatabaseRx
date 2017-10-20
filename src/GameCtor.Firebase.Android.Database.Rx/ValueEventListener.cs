using System;
using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    public static partial class DatabaseQueryExtensions
    {
        private sealed class ValueEventListener : Java.Lang.Object, IValueEventListener
        {
            private IObserver<DataSnapshot> _eventObserver;
            
            public ValueEventListener(IObserver<DataSnapshot> eventObserver)
            {
                _eventObserver = eventObserver;
            }

            public void OnCancelled(DatabaseError error)
            {
                _eventObserver.OnError(error.ToException());
            }

            public void OnDataChange(DataSnapshot snapshot)
            {
                _eventObserver.OnNext(snapshot);
            }
        }
    }
}