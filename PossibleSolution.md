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
Now we want to write characterization tests to validate the bits required to flag an email address as spam.

**Tip: Now might be a good time to refactor tests to create the spam filter in the test setup constructor**

4)  Characterize when `if (!_hashBits[Math.Abs(h1)])` evaluates to true

looking at the first four lines of `Contains()`:

```c#
     var primaryHash = item.GetHashCode();
     var secondaryHash = _getHashSecondary(item);
     var h1 = primaryHash % _hashBits.Count;
     if (!_hashBits[Math.Abs(h1)]) return false;
```
...we want to make the 4th line true.  We don't seem to need the secondary hash (whatever that is).

Now we have this test
```c#
        [Fact]
        public void IsNotSpamWhenOnlyThePrimaryBitIsSet()
        {
            const string testEmailAddress = "anything@anything.com"; 
            var primaryHash = testEmailAddress.GetHashCode();
            var h1 = primaryHash % _spamFilter._hashBits.Count;
            _spamFilter._hashBits[Math.Abs(h1)] = true;
            
            _spamFilter.IsSpam(testEmailAddress).Should().BeFalse();
        }
```
This is a pretty bad test by itself.  But now we get the understanding that in order to mark an address as spam, we need to set the bit for the 
primary hash, plus bits for the secondary hash.  Let's try to improve this test.

5) Characterize when all three bits are set to flag an email address as spam.

Now we have to look at the secondary hash.  We will need access to the field `_getHashSecondary`, so let's promote it to a get-only property for testing.
This requires that I make `delegate int HashFunction(string input);` public as well.  That's OK -- we may be able to clean up this access in a future refactor.

**Tip: Now would be a good time to make `const string testEmailAddress = "anything@anything.com";` a class-level constant.**



