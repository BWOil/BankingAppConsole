# 🙋‍♀️ Authors: s3959666-s3961136-a1
Group 11
- s3961136 Benjaporn Wongmayura
- s3959666 Xiaohan Qin
- [GitHub Repository for Assignment 1: https://github.com/rmit-wdt-summer-2024/s3959666-s3961136-a1](https://github.com/rmit-wdt-summer-2024/s3959666-s3961136-a1)

# 💸 MCBA (Most Common Bank of Australia)
<p align="center">
  <img src="trello-board/MCBA_logo.png" alt="Image" width="500"/>
</p>

## 🔗 Table of Contents
1. [About the Project](#about-the-project)
2. [Technology Stack](#technology-stack)
3. [Design Patterns](#design-patterns)
4. [Class Library Justification](#class-library-justification)
5. [Asynchronous Programming](#asynchronous-programming)
6. [Trello Board Screenshots](#trello-board-screenshots)

## 🔭 About the Project
The console banking application is designed to provide basic banking functionality through a text-based console interface. It allows customers to perform various banking operations such as depositing, withdrawing, transferring funds, checking their account statements, and logging out.

## 🛠 Technology Stack
- ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
- ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
- ![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
- ![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

## 📦 Design Patterns
### DTO Pattern in DTO Deserialisation
- **Summary**: DTO (Data Transfer Object) is a pattern used to transfer data between parts of a software application. It encapsulates data into a single object, reducing the number of calls and simplifying data transfer, especially in network environments. DTOs are simple, serializable objects used mainly for data transport without business logic.
- **Purpose**: DTO is used to efficiently transfer data between different parts of a software application, serving as a container for data
- **Advantages**:
  - Reduced Network Calls: DTOs reduce the number of calls between application layers by bundling multiple data fields into a single object.
  - Simplified Data Transfer: They simplify the process of transferring data, especially in distributed or networked environments.
  - Encapsulation: DTOs encapsulate data, separating it from business logic, promoting clean code architecture.
  - Serialization: DTOs are serializable, making them suitable for data exchange in various formats.
- **Implementation Details**:
<p align="left">
  <ul>
    <li>Define DTO Classes: Create DTO classes (Account, Customer, Login, and Transaction) which are structured for data transfer.
      <br><img src="design patterns/DTO_Account.png"/>
      <br><img src="design patterns/DTO_Customer.png"/>
      <br><img src="design patterns/DTO_Login.png"/>
      <br><img src="design patterns/DTO_Transaction.png"/>
    </li>
    <li>Use DTOs in Service Layer: In CustomerWebService, we deserialize JSON data into DTOs and use domain models.
      <br><img src="Async-await/img/Async-await-example.png"/>
    </li>
  </ul>
</p>

- **Additional Comments**:

### Facade Pattern in Managers
- **Summary**: The Facade pattern simplifies interactions with a complex system by providing a single unified interface. This pattern hides the system's complexity and makes it more accessible and easier for clients to use.
- **Purpose**: Simplify interactions with complex systems by offering a single, user-friendly interface.
- **Advantages**:
  - Complexity Concealment: Facade hides the intricate details of the system, shielding clients from its complexity.
  - User-Friendly: It offers a user-friendly interface that simplifies interactions.
  - Abstraction Layer: Acts as an abstraction layer, reducing the learning curve for clients.
  - Improved Maintainability: Changes to the underlying system can be isolated within the facade, minimizing the impact on clients.
- **Implementation Details**:
<p align="left">
  <ul>
    <li>Create <code>FacadeOperation</code> to provide a simple way for users to load data and run the program, hiding the complexity of individual managers.</li>
    <li>Apply Separation of Concerns: Managers like <code>AccountManager</code>, <code>CustomerManager</code>, <code>TransactionManager</code>, and <code>LoginManager</code> each handle specific tasks, aligning with the Single Responsibility Principle.</li>
    <li>Simplify Interface for Users: The <code>LoadingData</code> and <code>RunProgram</code> methods are clear entry points for client code, with <code>LoadingData</code> handling data initialization/loading before program execution.</li>
  </ul>
  <img src="design patterns/updated_facade.png" alt="Facade Pattern Implementation Image"/>
</p>

- **Additional Comments**:

### Dependency Injection in Managers
- **Summary**: Dependency Injection is a technique in programming where objects receive their dependencies from an external source rather than creating them themselves. This promotes loosely coupled and easily testable code, as it separates object creation from object use.
- **Purpose**: Enhance code maintainability, testability, and flexibility
- **Advantages**:
  - Loose Coupling: DI decouples managers from dependencies, making the code modular and maintainable.
  - Testability: Simplifies unit testing by enabling the use of mock objects or test implementations for dependencies.
  - Flexibility: Managers can be configured with different dependencies at runtime, offering application behavior flexibility.
  - Reusability: Components and services can be reused across multiple managers.
- **Implementation Details**:
<p align="left">
  The ⁠ LoginSystem ⁠ class achieves dependency injection through its constructor. The LoginSystem and Menu classes utilize constructor-based dependency injection for CustomerManager and other dependencies, enhancing flexibility and testability. This approach allows easy integration of different implementations and facilitates unit testing, aligning with the Dependency Inversion Principle for a more robust and modular design.
  <img src="design patterns/dependency-injection.png" alt="Image"/>
  <img src="design patterns/dependency-injection-1.png" alt="Image"/>
  <img src="design patterns/updated_facade.png" alt="Image"/>
</p>
- **Additional Comments**:

## 📓 Class Library Justification
A text library has been developed for this bank console application to facilitate the application of colors to text and enhance the printing format of content. This versatile library is designed to be seamlessly implemented in any C# console application, saving developers valuable time that would otherwise be spent on designing string formats for the terminal.

Additionally, the library includes functionality to construct tables for console applications in C#, streamlining the process of presenting tabular data in a visually appealing manner.

## ⏱️ Asynchronous Programming
- **Summary**: Async and await are used in programming to do tasks in the background. 'Async' tells the computer that a function can run while other things are happening. 'Await' is used inside these functions to wait for a task to finish without stopping everything else. 
- **Purpose**: Enhances application responsiveness and scalability. It facilitates non-blocking execution of tasks like I/O operations and network requests, ensuring user interactions remain smooth.
- **Advantages**:
  - Responsiveness: Asynchronous tasks prevent applications from freezing when executing time-consuming operations.
  - Efficiency: Concurrent execution of tasks maximizes CPU and resource utilization.
  - Scalability: Asynchronous code can handle multiple concurrent requests or operations efficiently.
  - Improved User Experience: Applications feel more responsive and smooth to users.
- **Implementation Details**:
<p align="left">
  In the program, the `async` keyword in `GetAndSaveCustomer` marks the method as asynchronous, making it return a `Task`. The `await` keyword is used with `client.GetString(Url)`, allowing the program to asynchronously wait for the HTTP request to complete before continuing, without blocking the execution thread.
  <img src="Async-await/img/Async-await-example.png" alt="Image"/>
  <img src="Async-await/img/calling GetAndSaveCustomer by using await1.png" alt="Image"/>
  <img src="Async-await/img/calling GetAndSaveCustomer by using await2.png" alt="Image"/>
</p>
- **Additional Comments**:

### 📋 Trello Board Screenshots
<p align="left">
  <img src="trello-board/Screenshot 2024-01-08 at 4.28.50 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-08 at 6.47.00 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-11 at 1.06.30 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-11 at 12.07.20 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-12 at 1.05.30 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-13 at 1.20.07 am.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-13 at 4.18.27 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-13 at 9.38.10 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-14 at 1.52.13 am.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-14 at 13.16.33 pm.png" alt="Image"/>
  <img src="trello-board/Screenshot 2024-01-14 at 22.51.03pm.png" alt="Image"/>
</p>


