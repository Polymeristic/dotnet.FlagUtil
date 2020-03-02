# dotnet.FlagUtil
dotnet.FlagUtil is a helper library guided towards assisting with binary flag operations. Allowing for an object-oriented approach to binary flags it enables easy method calls for modifying and checking flags.

dotnet.FlagUtil is open source under the MIT licence.

## Installation
The only dependancy for dotnet.FlagUtil is `dotnetcore2.0`. 
### Nuget
dotnet.FlagUtil is available as a NuGet package here:
https://www.nuget.org/packages/FlagUtil/1.0.0

### .dll
A pre-build .dll for dotnet.FlagUtil is available https://github.com/Polymeristic/dotnet.FlagUtil/tree/master/lib

### Source
Download the repo and build using Visual Studio.

## Usage
Using dotnet.FlagUtil is painlessly simple. It can be used with an `Enum` based approach or using `long`.  We will be using the below example enum throughout the documentation.
```csharp
public enum Status {
    ONLINE = 0b0001,
    OFFLINE = 0b0010,
    PROCESSING = 0b0100,
    FAILED = 0b1000
}
```


### Creating a Flag
To create a flag you need to provide the `Flag` with a base value, to do so you can use either a; `long`, `Flag` or `Enum` - all can be parsed into a `Flag`. Usefully, the library supports unlimited arguments on almost all it's functions. If multiple arguments are detected it will automatically merge those flags into a single flag, as shown below.

```csharp
public static void Main() {
    // Assignment using an Enum possible,
    // equiv. to 0b0001
    Flag f1 = new Flag(Status.ONLINE);
    
    // Equiv. to 0b0101
    Flag f2 = new Flag(Status.ONLINE,
                       Status.PROCESSING);
                       
    // You can also assign using any unsigned numerical type,
    // in this case f3 and f4 are equal
    Flag f3 = new Flag(0b0100);
    Flag f4 = new Flag(4);

    // And finally you can copy or compound flags using
    Flag f5 = new Flag(f1, f3);
    // Which would be equiv. to 0b0101
}
```
### Flag Modification Operations
#### Merging Indicators
Merging a flag essentially means adding a new indicator to the existing flag, in binary terms, it is like an 'OR' operator. It would look like such `0010 | 0100 = 0110`. Merging can be done either through methods or on initialization, as the initializer supports multiple parameters. An example  of initialization merging would be:
```csharp
// Has merged two flags into one
Flag status = new Flag(Status.ONLINE,
                       Status.PROCESSING);
```
Onced merged, you can use the `==` operator (or other equality operators) to check for the indicators contained within the overall flag. You can also merge additional indicators after the flag has been initialised, using our variable `status` we defined above, we can add a new indicator:
```csharp
status += Status.FAILED;
``` 
As you can see the `+` operator can be used to add new indicators to a flag. Likewise, the `-` operator will remove indicators from a flag. See more about this in the [Removing Indicators](#Removing-Indicators) section.

You can also use the `Flag.Merge(x, ..)`  method to merge more indicators, as such:
```csharp
status.Merge(Status.FAILED, Status.OFFLINE);
```

#### Removing Indicators
As merging adds indicators, removing indicators will take them away. This can be visualised as a little bit more complicated of a binary expression, to remove `0010` from `0110` we can represent this as `0110 & ~0010`.  The removal of indicators can be done either by using the method `Flag.Remove(x, ..)` or by using the `-` operator.

In the below demonstration we remove the `ONLINE` and `PROCESSING` flags and replace them with `OFFLINE` and `ERROR`. 
```csharp
// Has merged two flags into one
Flag status = new Flag(Status.ONLINE,
                       Status.PROCESSING);

status -= Status.ONLINE;
status -= Status.PROCESSING;
status += Status.OFFLINE;
status += Status.ERROR;
```

This can be done in a cleaner way however, using the `Flag.Remove(x, ..)` and `Flag.Merge(x, ..)` methods.
```csharp
// Has merged two flags into one
Flag status = new Flag(Status.ONLINE,
                       Status.PROCESSING);

status.Remove(Status.ONLINE, Status.PROCESSING);
status.Merge(Status.OFFLINE, Status.ERROR)
```

#### Bit-flip
In some, admittedly odd, circumstances you can flip all indicators in the flag by using either the `!` or `~` operators, or `Flag.Reverse()`. Doing so will bit-flip all indicators, meaning `0011` would turn into `1100`.

#### Other binary operators
Other binary operators such as bit-shifting have not been included due to fear of unnecessary bloat. However, if you do wish to edit the flag's underlying value you can access it via the `Value` property.
```csharp
Flag status = new Flag(Status.ONLINE);
status.Value = status.Value << 1;
```

### Flag Equality
Flag equality in dotnet.FlagUtil isn't *exactly* what it would seem at first glance. Some assumptions were made around its usage in order to be better suited to its job, as such, it may not seem obvious at first as to the meaning of some operators.

#### Checking for flag indicators
The `==` operator is perhaps the most confusing as in our case it doesn't explicitly mean 'is x equals to y?'. In dotnet.FlagUtils, the equals operator means 'is **y** is contained in **x**' - note the flip! Doing things this way means we can easily check for flag indicators.

Say we want to create a flag `currentStatus`, we would initialise it as such:
```csharp
Flag currentStatus = new Flag(Status.ONLINE, Status.PROCESSING);
```
At present the flag is equivalent to the binary expression `0b0101`, so, if we wish to check if this flag has the `Status.ONLINE` (equiv. to `0b0001`) indicator all we need to do is:
```csharp
if (currentStatus == Status.ONLINE) {
    // do something
}
```
As you can see the `==` operator does **not** mean 'x is *equals* to y' but instead means 'y is *contained* within x'. If we did need to check for an *exact* match we need to use the helper function `Flag.MatchExact(x, ..)` 

In addition to using the `==` operator, you can also use `Flag.Match(x, ..)` - these methods are functionally identical, however, `Flag.Match(x, ..)` supports multiple parameters. As such it will merge them into a single flag then check that against the subject, meaning that all of the parameter flags need to be present. An example of this in our scenario would be:
```csharp
if (currentStatus.Match(Status.ONLINE, Status.PROCESSING)) {
   // would return TRUE because currentStatus has the Status.PROCESSING flag AND the Status.ONLINE flag set.
}
```

#### Checking for exact flag matches
If you wish to check if two flags are *exactly* identical you will need to use the method `Flag.MatchExact(x, ..)`. Yet again this method supports multiple parameters, these will be merged in the same way as the `Flag.Match(x, ..)` method.

Unline the `Flag.Match(x, ..)` and `==` methods, the `Flag.MatchExact(x, ..)` method *does* mean 'is x equals to y'. Again using the scenario of:
```csharp
Flag currentStatus = new Flag(Status.ONLINE, Status.PROCESSING);
```
Here is an example of using the exact match method:
```csharp
if (currentStatus.MatchExact(Status.ONLINE, Status.PROCESSING, Status.FAILED)) {
    // this code will NOT run
} else 
if (currentStatus.MatchExact(Status.ONLINE, Status.PROCESSING)) {
    // this code will run
}
```

#### Checking for any flag matches
If you have multiple flags that you want to check for, but not as a compound flag, you can use the method `Flag.MatchAny(x, ..)` to achieve this. There are two definitions for `Flag.MatchAny(x, ..)` but we'll talk about the second later. 

This method will check for a match with any flag passed as a parameter, so it can be used as follows:
```csharp
Flag currentStatus = new Flag(Status.ONLINE, Status.PROCESSING);

if (currentStatus.MatchAny(Status.ONLINE, Status.FAILED)) {
    // this code will run
}
```

As even though the flag `currentStatus` does not have the indicator `Status.FAILED` set, it does have `Status.ONLINE` so the method will return true.

As for the second definition of the `Flag.MatchAny(x, ..)` method, we can also get the flag that was actually matched, it has the signature `Flag.MatchAny(out match, x, ..)`. Importantly, it will only retrieve the *first* match - this may be changed at a later date if necessary.

An example of it's use is:
```csharp
Flag currentStatus = new Flag(Status.ONLINE, Status.PROCESSING);
ulong match;

if (currentStatus.MatchAny(out match, Status.ONLINE, Status.FAILED)) {
    Console.WriteLine(match); // Will output "0001"
}
```

### String Representation
The `Flag` class implements `ToString()` in a way that it will convert it's flag value into the binary representation of the flag. This was done to ensure ease of identification. As such
```csharp
new Flag(Status.ONLINE, Status.PROCESSING).ToString()
```
Would output `"0101"`

### HashCode
Due to the nature of a flag, the hash code is simply equal to the flag's value. This means hash tables can be produced based on combined flag indicators. 

## Feedback
For any feedback feel free to add to the Issues panel in the Github repo, or message me directly.
