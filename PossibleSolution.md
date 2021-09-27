# Possible Solution for the Spam Filter Kata

## Getting Started
1) Run tests to make sure they are green
2) Note that IsSpam() is the method that needs to scale
3) Notice that IsSpam() is a thin facade over the Contains() method
4) Review the Contains method
5) Verify the code is short, but cryptic

## Mutation Testing
Mutation testing is a technique to see how well logic is covered in the tests.  Mutation testing goes a step beyond code coverage.
Where code coverage shows all the paths through the code that are exercised, mutation testing verifies that there are assertions 
against the functionality expressed in each path through the code.

Any mutation of the code must result in a test failure.  If a mutation survives all tests, then you have to write a new test that would
flag this mutation with a failed test.

There are automated mutation testing tools, but we will do manual mutation testing

