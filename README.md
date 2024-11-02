
# This is an interpreter for ZLang.

## How to use it?
1. Clone this repo.
2. Run the project and enter the code you want to execute in the terminal.
3. If you want to execute a file, enter the path to the file in the terminal.

## Language Syntax

### Variables 

Variable Declaration
`` let x = 5; ``

Variable Assignment
`` x = 5;``

### Types
1. number 
    ``let x = 5;``
2. string ``let x = "hello";``
3. boolean ``let x = true;``
4. null ``let x = null;``
5. array ``let x = [1, 2, 3];``
6. map ``let x = {a: 1, b: 2};``

### Functions

Function Declaration
``let add = fn(x, y) {return x + y; } ``

Function Call
``add(2, 3);``

### Control Flow

If else Statement
`` if (x > 5) { return x; } else { return y; } ``

### Notes
1. Arrays are zero-indexed.
2. Maps are key-value pairs.
3. Functions are first-class objects.
4. This language is not typed.
5. This language relies totally on host language (C#) for garbage collection.
6. Maybe I will build our own garbage collector.