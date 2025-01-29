- [x] A pricing table has at least one tier
- [x] A pricing table has a set of tiers
- [x] Pricing Table covers 24h
- [x] pricing tiers ordered by hour limit
- [x] Max Price eq to sum of tiers if not defined
- [x] Max Price eq to the provided value
- [x] Max Price < full day through table

- [x] Improve exception-based tests
- [x] Hour limit should be between 1 and 24
- [x] The price can't be negative


- [ ] Price per hour

----

- [x] Fail if request is null
- [x] Return bool as true if succeed
- [x] Invoke storage only once
- [x] Request is mapped correctly to storage
- [x] External dependency is invoked to store pricing and that it stored the input


----


- [x] Throw exception if missing connection
- [x] Insert if not exists
- [x] Replace if exists
- [x] Replace keeps the new values
- [x] Return true if succeed


----

- [x] 500 if unknown error
- [x] 200 if success
- [x] 400 if returns false

----

- [x] ticket for 30min when 1h has the cost of 2
- [x] Entry and Exit time
- [x] Get Price Table from storage
- [x] Price service needs to return calculator result
- [x] Calculate ticket duration
- [x] Total Cost = Calculate partial hours
- [x] Total Cost = calculate with Ticket and Pricing Table
- [x] Total Cost = Ensure doesn't go over max daily price


----

- [ ] Refactoring the Acceptance Tests
- [ ] Test-driving Price calculator using max daily price