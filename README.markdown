TypeSieve is a micro-framework providing two things:

1. .NET assembly scanning.
2. A fluent type-filtering API.

While the problem of assembly scanning and type filtering is simple and solution implementations are fairly common and similar, APIs differ in nearly every incarnation. Or you can just use unadorned LINQ, but it's easy for the noise of the reflection API to obscure your intentions. The goal of TypeSieve is simply to provide a fluent API for type scanning and filtering that supports the most common operations. TypeSieve also supports a convention system for specifying more complex filtering scenarios that can then be included in your scan configuration using the same fluent API.

API documentation and usage examples are forthcoming. In the meantime you can see how it works fairly clearly by looking at the unit tests in http://github.com/cammerman/TypeSieve/blob/master/TypeSieve.Tests/DetectTypesTests.cs

The recommended usage for open source projects or reusable assemblies is either to use ILMerge, or to include the source code as a whole, to prevent conflicts between multiple assemblies that may depend on different versions of TypeSieve.

This project is licensed under the BSD license.  The license file is included in the project: http://github.com/cammerman/TypeSieve/blob/master/license.txt
