---
title: Safekeeper
_appTitle: null
_description: A simple save system for Unity
---

# Safekeeper

Safekeeper is a simple save system for [Unity].

It's an opinionated solution designed mostly for non-procedural, adventure
games.

An individual playthrough is stored in its own slot. Every time the game is
saved the previous state is overwritten. The game can have multiple slots to
store simultaneous playthroughs.

Currently, the number of available slots needs to be known in advance. The
player can't declare new slots. They can only select one of the existing ones
and either:

- Start a new playthrough.
- Continue an existing one.
- Clear the slot.

This limitation comes from the fact that you'd need a way to store the
information about the slots themselves. Safekeeper doesn't have an
out-of-the-box solution for that. You could, however, try to implement this
functionality yourself.

> [!NOTE]
>
> Typewriter has been released for educational purposes. It probably won't be
> developed further.

> [!Video https://www.youtube.com/embed/wTsrGkMlMN0]

[Unity]: https://unity.com/
[Patreon]: https://www.patreon.com/aarthificial
[YouTube]: https://www.youtube.com/@aarthificial/join
[instructions]: https://www.patreon.com/posts/typewriter-early-86967465
