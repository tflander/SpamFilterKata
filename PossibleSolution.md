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

1) Comment out the line of code:  `if (!_hashBits[Math.Abs(h1)]) return false;`
2) Run tests
3) Verify tests are still green.  This means we need a new test.
4) Un-comment the line of code.
5) Comment out the line of code: `return _hashBits[Math.Abs((int)h2)] && _hashBits[Math.Abs((int)h3)];`
6) Run tests
7) Verify tests are still green.  Because there are two parts of the predicate, we need two new tests.
8) Revert all changes to the production code.

## Crossroads:  Evolution or Revolution?
At this point we have a choice.  We can:
1) Bulldoze the code and rebuild from scratch
2) Follow the kata as intended by characterizing the code and test-driving changes

We take approach #1 too much in our industry.  We take on long rewrites that solve some problems, while introducing new bugs and dropping existing features.
At some point in the re-write, we will go through a long process of beta testing to discover dropped features.  If the response to these features is 
"We need them all", then a re-write was not the best choice.

Some useful questions to ask:

1) How much will customers be disrupted when we introduce new bugs?
2) How likely does the legacy code contain hidden features that customers rely on?

In this case, spam filters against email addresses are a known computer science problem.  There are off-the-shelf solutions and algorithms that are easy 
to plug in.  There is little possibility of introducing new bugs, because the scope of this problem is very narrow.  There are no hidden features.

The best answer in this case is to bulldoze and rebuild, but let's do a code rescue anyway for the purpose of exploring and learning.

## Characterization
1) Create a new test suite `IsSpamTests.cs` in the same folder as `SpamFilterTests.cs`
2) Write the simplest possible test:
```c#
        [Fact]
        public void ByDefaultNothingIsSpam()
        {
            var sf = new EmailAddressSpamFilter.SpamFilter();
            sf.IsSpam("anything@anything.com").Should().BeFalse();
        }
```
This is not a very useful test, but it's a start.  Let's write a more useful test.

3) Verify the `Contains()` method checks the state of the `_hashBits` field, which is inaccessible from the tests.
4) Promote the `_hashBits field to a get/set property`. 
5) Consider adding a TODO comment that this promotion was done for testing.  This might serve as a hint for later refactoring.
6) Write a test to characterize behavior when all bits are set:

```c#
        [Fact]
        public void WhenAllBitsSetThenEverythingIsSpam()
        {
            var sf = new EmailAddressSpamFilter.SpamFilter();
            sf._hashBits.SetAll(true);
            sf.IsSpam("anything@anything.com").Should().BeTrue();
        }
```

7) Write the opposite test where all bits are clear:

Now we understand and have proven that the hash bits are the key to identifying spam.  Let's write the opposite test where all bits are clear.

```c#
        [Fact]
        public void WhenNoBitsSetThenNothingIsSpam()
        {
            var sf = new EmailAddressSpamFilter.SpamFilter();
            sf._hashBits.SetAll(false);
            sf.IsSpam("anything@anything.com").Should().BeFalse();
        }
```
