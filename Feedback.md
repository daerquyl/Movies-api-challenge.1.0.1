### Feedback

*Please add below any feedback you want to send to the team*
Very nice challenge, I truly enjoyed working on it. It seemed trivial at the beginning, but I faced some interesting challenges here. Thanks!

Most of these challenging aspects were not purely technical. It was about understanding the features. 
While I grasped the general purpose of each question, some required additional details to uncover the hidden rules.

For instance, what does 'contiguous' mean? Does it refer to all seats adjacent in the same row, or should we consider even seats with the same numbers on adjacent rows?
Similarly, when the same seat cannot be reserved twice within 10 minutes, what happens if someone tries to reserve a seat that's already reserved? Should we cancel the other reservation?
Moreover, what should we do when users attempt to reserve some seats, but some are already sold or reserved? Should we cancel the entire reservation? 
For this last scenario, I proposed some variations. Users can choose to perform a partial reservation if not all seats are available. 
Additionally, users can decide to reserve seats by providing only the number of seats along with the showtime ID.

Regarding caching, I implemented a caching service with Redis, but I'm not entirely sure if my usage aligns with expectations.
The cache intervenes only when retrieving movie data from the provided API during showtime creation.
The provided API offers two endpoints, and I opted for the one with movie ID as query parameter as it seemed more suitable.

As for queries, I was unsure whether to implement them, as the test instructions weren't explicit.
Nevertheless, I implemented some queries to facilitate my own testing and, hopefully, yours, given that data is stored in memory.

[Important] In this part, I deviated slightly from the code structure and employed my approach to handling requests,
incorporating some elements of CQRS and DDD-Lite. I preferred this approach because it allowed me to manage domain rules more efficiently.
However, it's possible that the challenge was not oriented this way, and I should have adhered to the basic CRUD approach, which the base seemed to follow.
In an existing project, I would have no reservations about following the established architecture but might suggest and discuss improvements.
Given that this is a test challenge, I felt like I had a certain degree of creative freedom.

In any case, I genuinely enjoyed this test. See you soon, perhaps. Have a great day!