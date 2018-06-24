using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Firebase.Database;

namespace GameCtor.Firebase.Database.Rx
{
    public class FirebaseDatabaseUtils
    {
        /// <summary>
        /// Calls ObserveSingleZippedEvent on each reference and returns
        /// a dictionary of DataSnapshots that can be accessed via the reference url.
        /// Only returns once, after all snapshots are received.
        /// </summary>
        /// <param name="dbQueries"></param>
        /// <returns>Returns a dictionary of DataSnapshots that can be accessed via the reference url.</returns>
        public static IObservable<IDictionary<string, DataSnapshot>> ObserveSingleZippedEvent(params DatabaseQuery[] dbQueries)
        {
            IObservable<DatabaseQuery> dbQueryObservable = dbQueries
                .ToObservable();

            IObservable<DataSnapshot> snapshotObservable = dbQueryObservable
                .SelectMany(x => x.ObserveSingleEventRx(DataEventType.Value));

            return Observable
                .Zip(
                    dbQueryObservable,
                    snapshotObservable,
                    (query, snap) => new { Key = query.Reference.Url, Value = snap })
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}