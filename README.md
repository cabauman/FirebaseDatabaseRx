# FirebaseDatabaseRx
Reactive extension wrappers for Xamarin's Firebase Database package (iOS and Android).

Nuget Link: https://www.nuget.org/packages/GameCtor.Firebase.Database.Rx/1.0.0

## Based On
Xamarin.Firebase.Database (Android): https://www.nuget.org/packages/Xamarin.Firebase.Database/42.1021.1

Firebase Database for iOS: https://components.xamarin.com/view/firebaseiosdatabase


## Android Code Samples

#### Retrieve data once

```C#
var observer = _dbRef.AddListenerForSingleValueEventRx();
_subscriber = observer
    .Where(snap => snap.Exists())
    .Timeout(TimeSpan.FromSeconds(2))
    .Retry(3)
    .Subscribe(
        snap =>
        {
            Console.WriteLine(snap.ToString());
        },
        error =>
        {
            Console.WriteLine(error.Message);
        },
        () =>
        {
            Console.WriteLine("Complete");
        });
```

#### Retrieve data and then listen for modifications anywhere in the hierarchy

```C#
var observer = _dbRef.AddValueEventListenerRx(out _eventListener);
_subscriber = observer
    .Where(snap => snap.Exists())
    .Skip(1)
    .Subscribe(
        snap =>
        {
            Console.WriteLine(snap.ToString());
        },
        error =>
        {
            Console.WriteLine(error.Message);
        },
        () =>
        {
            Console.WriteLine("Complete");
        });
```

#### Retrieve child data and then listen for modifications

```C#
var observer = _dbRef.AddChildEventListenerRx(out _childEventListener);
_subscriber = observer
    .Where(result => result.EventType == ChildEventType.Added)
    .Select(result => result.Snapshot)
    .Where(snap => snap.Exists())
    .Subscribe(
        snap =>
        {
            Console.WriteLine(snap.ToString());
        },
        error =>
        {
            Console.WriteLine(error.Message);
        },
        () =>
        {
            Console.WriteLine("Complete");
        });
```

#### Retrieve data at specified locations. Returns once, only when all data has been retrieved. All references are fetched concurrently, with the help of SubscribeOn.

```C#
var observer = FirebaseDatabaseUtils.AddListenerForSingleZippedEvent(_dbRef, _dbRef2, _dbRef3);
_subscriber = observer
    .ObserveOn(Scheduler.Default)
    .Subscribe(
        map =>
        {
            _snapshotMap = map;
            Console.WriteLine(map.Values.ToString());
        },
        error =>
        {
            Console.WriteLine(error.Message);
        },
        () =>
        {
            Console.WriteLine("Complete");
        });
```

#### Not a reactive extension. Instead, "TransactionHandler" is a convenience class that allows the developer to use lambdas. (Normally, the developer needs to create a class that implements Transaction.IHandler).

```C#
_dbRef.RunTransaction(new TransactionHandler(
    currentData =>
    {
        Console.WriteLine(currentData.ToString());
        return Transaction.Success(currentData);
    },
    (error, committed, snapshot) =>
    {
        Console.WriteLine("Complete");
    }));
```

## License

MIT License

Copyright (c) 2017 Colt Alan Bauman

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
