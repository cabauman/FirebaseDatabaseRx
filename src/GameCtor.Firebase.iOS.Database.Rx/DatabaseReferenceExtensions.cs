using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Firebase.Database;
using Foundation;

namespace GameCtor.Firebase.Database.Rx
{
    public static class DatabaseReferenceExtensions
    {
        /// <summary>
        /// Equivalent to SetValue<typeparamref name="T"/>, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <typeparam name="T">The type of value you want to set.</typeparam>
        /// <param name="dbRef">The database reference.</param>
        /// <param name="value">The value you want to set.</param>
        /// <returns>Returns a reference to the database location.</returns>
        public static IObservable<DatabaseReference> SetValueRx<T>(this DatabaseReference dbRef, T value)
            where T : NSObject
        {
            return Observable.Create<DatabaseReference>(observer =>
            {
                dbRef.SetValue(
                    value,
                    (error, reference) =>
                    {
                        if(error != null)
                        {
                            observer.OnError(new NSErrorException(error));
                        }
                        else
                        {
                            observer.OnNext(reference);
                            observer.OnCompleted();
                        }
                    });

                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Equivalent to UpdateChildValues, except returns an IObservable for use with reactive extensions.
        /// </summary>
        /// <param name="dbRef">The database reference.</param>
        /// <param name="value">The values you want to set.</param>
        /// <returns>Returns a reference to the database location.</returns>
        public static IObservable<DatabaseReference> UpdateChildValuesRx(this DatabaseReference dbRef, NSDictionary values)
        {
            return Observable.Create<DatabaseReference>(observer =>
            {
                dbRef.UpdateChildValues(
                    values,
                    (error, reference) =>
                    {
                        if(error != null)
                        {
                            observer.OnError(new NSErrorException(error));
                        }
                        else
                        {
                            observer.OnNext(reference);
                            observer.OnCompleted();
                        }
                    });

                return Disposable.Empty;
            });
        }
    }
}