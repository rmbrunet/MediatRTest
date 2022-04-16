# MediatRTest

## Sample of using MediatR to organize REST API endpoints into features.

Using the MediatR library to implement an in-process Medidiator patter at the Application level. MediatR (60 M downloads from nuget.org) is from the 
same authors of AutoMapper (250 M downloads from nuget.org).

**Not a substitution for Clean Architecture. Just a pattern to better organize the Application-level code.**

## Advantages

* Cleaner code: each Feature totally contained in a single file with identical structure and components: 
  * Query/Command
  * Handler
  * Result
  * QueryValidator
* Easy to work in a multi-developer environment with each developer working in his feature file.
* Easy unit testing, only the Handlers and the Query/Command Validators must be tested (the Mappings are tested as part of the Handler). 
