# ToDo List
WPF ToDo list application created with Ado.net

# Table of Contents
* [Design patterns](#design-patterns)
* [Data model](#data-model)
* [Unit tests](#unit-tests)
* [Application demo](#application-demo)
* [About](#about)


<hr>



# Design Patterns

Design pattern implemented here is dependency injection
We have here `ToDoListService` class, which implements all functionality associated with database.

`ToDoListSerice` implements `IToDoList` interface.

`Service` class connects UI to Data, with the help of IToDoList service layer.

<hr/>

# Data Model

Data model is very simple, there is only one small table for basic information of ToDo task

![Model](https://user-images.githubusercontent.com/28567416/56579442-36073600-65e1-11e9-99a3-994be9d4c727.jpg)

<hr/>

# Unit Tests

There are not any tests currently, but soon it is planed to add tests and refactor/optimize

<hr/>

 # Application Demo
 
 Some screenshots of a demo application
 
 <img src="https://user-images.githubusercontent.com/28567416/56655893-0faace80-66a5-11e9-9ed6-4d117d877e9c.jpg" alt="drawing" width="400"/>
 
  <img src="https://user-images.githubusercontent.com/28567416/56655900-12a5bf00-66a5-11e9-970d-bde3bf08371f.jpg" alt="drawing" width="400"/>
  
   <img src="https://user-images.githubusercontent.com/28567416/56655903-15081900-66a5-11e9-8013-e48dc232aa24.jpg" alt="drawing" width="400"/>

<hr/>

# About
 
 This is an application for creating/deleting ToDo tasks, searching, and editing them, application is not optimized and has some memory leaks, which will be fixed in future, Hope you enjoy it.

If You found a bug, please add a topic in Issue section. 
