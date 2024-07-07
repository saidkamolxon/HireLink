=== DETAILS ===
🔸 Developed using .NET 8 with EF Core 8.0.6, requires .NET SDK 8 for execution.
🔸 Automatically migrates the database on app launch in Visual Studio, eliminating the need for manual migration.
🔸 Used xUnit for unit testing and Moq for mocking.
🔸 Developed tests for the validator and the service; controller testing was omitted as I believe integration tests are typically more suitable for controllers in real projects, offering better coverage.
🔸 Implemented caching within the cached repository, following the decorator pattern.

=== IMPROVEMENTS ===
🔸 I followed the requirement for a single API endpoint for creating/updating candidate info, but I considered that adding another endpoint for retrieving data from the database could have been beneficial for clients of the API and for testing the task.

=== ASSUMPTIONS ===
🔸 Assumed that a predictable result pattern indicating whether a candidate was created or updated would be beneficial, but I was unsure if this assumption was correct.

=== TOTAL TIME ===
🔸 Approximately 7 hours spent (~6.5 hours on development, ~0.5 hours on notes).
