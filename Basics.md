# C# Basics

## TODOs:
- [X] Loops
- [X] conditional statements
- [X] data types
- [X] reference type vs value types
- [X] access modifiers
- [X] list & array
- [X] interfaces
- [X] enums

---

## Loops:

```cs
// For each 
for (var X in Y) {
    // body
}

// rest of the loops are similar to C++
```

---

## Conditional Statements:

```cs
// if and else are the default too

//ternary operator
string access = (age >= 18) ? "Allow" : "Deny";
```

---

## Data types:

| Reference | Value |
| - | - |
| string, object, class, array, list | int (4B), long(8B), short(2B), byte(1B), unsigned variants, float(4B), double(8B), decimal(16B), bool(1B), char(2B)| 
| allocate on managed heap| allocate on stack|
| memory pointer to the data object | actual binary data|
| defaults to `null`| numerical defaults to `0`, bool defaults to `false`|

> `Chars` take **2 bytes** that is because they are used to represent `UTF-16` which would require $2^{16}$ bits (65536)

---

## Access Modifiers:

|Access modifier| Level of access |
| - | - |
| public | Anywhere in the assembly |
| private | Only within the class/struct|
| protected | Any class or child class |
| internal | only within the current project/assembly |
| protected internal | current assembly or child classes in other assemblies |
| private protected | Current assembly and only by derived classes |

> The compiler defaults to: `internal` if (enum, struct, etc.) are declared directly in namespace, `private` if fields or methods are within a class/struct, `public in other cases`

---

## List & Array:

| Array | List |
| - | - |
| Fixed at initialization| Dynamic (grows automatically)|
| Faster, lower memory overhead | Marginally slower due to resizing |
| Built-in | `System.Collection.Generic` |
| Supports dimensionality `[,]` | Single dimension and can be nested using `lists `|


---

## Interfaces:

```cs
public interface Notification {
    void Send(string message); // No method body nor fields are allowed here
}

public class Email: Notification {
    public void Send(string message) {
        Console.WriteLine($"Email Sent: {message}");
    }
}
```

> They help provide loose coupling, isolate code

> Each class can implement multiple interfaces

---

## Enums:

```cs
public enum Status {
    Pending, // 0
    Shipped = 2,
    Arrived, // 3
    Cancelled // 4
}
```

> You can also change the underlying datatype 

```cs
public enum GameDifficulty : byte {
    Easy = 1,
    Medium = 2,
    Hard = 3
}
```

> And also cast types