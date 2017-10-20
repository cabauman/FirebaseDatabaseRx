using System.Reactive.Subjects;
using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    public static partial class DatabaseQueryExtensions
    {
        private sealed class ChildEventListener : Java.Lang.Object, IChildEventListener
        {
            private Subject<ChildEventResult> _eventSubject = new Subject<ChildEventResult>();

            public ChildEventListener(Subject<ChildEventResult> eventSubject)
            {
                _eventSubject = eventSubject;
            }

            public void OnCancelled(DatabaseError error)
            {
                _eventSubject.OnError(error.ToException());
            }

            public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
            {
                _eventSubject.OnNext(new ChildEventResult(ChildEventType.Added, snapshot, previousChildName));
            }

            public void OnChildChanged(DataSnapshot snapshot, string previousChildName)
            {
                _eventSubject.OnNext(new ChildEventResult(ChildEventType.Changed, snapshot, previousChildName));
            }

            public void OnChildMoved(DataSnapshot snapshot, string previousChildName)
            {
                _eventSubject.OnNext(new ChildEventResult(ChildEventType.Moved, snapshot, previousChildName));
            }

            public void OnChildRemoved(DataSnapshot snapshot)
            {
                _eventSubject.OnNext(new ChildEventResult(ChildEventType.Removed, snapshot, null));
            }
        }
    }
}