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
        public static IObservable<Dictionary<string, DataSnapshot>> ObserveSingleZippedEvent(params DatabaseQuery[] dbQueries)
        {
            var observables = new List<IObservable<DataSnapshot>>();

            foreach(var query in dbQueries)
            {
                var observable = query.ObserveSingleEventRx(DataEventType.Value)
                    .SubscribeOn(Scheduler.Default);
                observables.Add(observable);
            }

            return observables
                .Zip()
                .Select(snapshots =>
                {
                    return dbQueries.Zip(snapshots, (query, snap) => new { Key = query.Reference.Url, Value = snap })
                        .ToDictionary(x => x.Key, x => x.Value);
                });
        }
    }
}