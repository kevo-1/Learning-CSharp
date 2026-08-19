## SOLID Analysis:
---
### SRP:

Instead of `ParkingGarage` being responsible for Processing cars, Generating and saving Receipts, Printing them, and Sending notifications; I have separated them into separate classes responsible for each task, and `ParkingGarage` responsible for the abstract functionality.

---
### OCP:

Keeping `ParkingGarage` adaptable if new `Vehicle` type or notification channel is added, open for more types, closed for modification on the class itself.

---
### LSP:

Each `Vehicle` subclass (`Car`, `Bus`, etc) should behave the same if a `Vehicle` is substitued by them, We can't change the number of wheels for example so adding a `DoubleWheels` method to the main `Vehicle` class would break the `Car` logic for example as a car is generally only 4 wheels. 

---
### ISP:

Each class implementing an interface shouldn't be forced to implement every function, that was why the notifier is responsible for deciding which Notification Channel to use, without making each channel class implement all methods.

---
### DIP:

`ParkingGarage` depends only on `INotifier`, `IFeesCalculatorFactory`, etc., never on a concrete class like `EmailNotifier` directly. The concrete instances are constructed once, outside `ParkingGarage` (in Main), and handed in through the constructor. This is what makes it possible to add `WhatsAppNotifier` later without touching `ParkingGarage`, but that extensibility is a consequence of DIP, not DIP itself.