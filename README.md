# SpamFilterKata

This is a refactoring kata.  The idea is to go beyond the concepts learned in the Gilded Rose kata, and to explore problems you would likely face in 
the real world.

## Background
We have an EmailAddress spam filter to flag incoming email addresses from known spammers.

This code is reported as not scaling well.  Over time as the list of spam messages grew, we started seeing legitimate email addresses filtered.  
We've been asked to take a look at this code and figure out how to make it scale better.

TODO:

1) Run tests and verify they are all green.
2) Take a look at the tests.  Note the IsSpam() method is the one that doesn't seem to scale.
3) Create characterization tests around IsSpam() and make a plan for scaling the method.

TODO: provide a markdown file detailing a possible solution.

