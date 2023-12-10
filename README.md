<p>
  <a href="https://safekeeper.aarthificial.com">
    <picture>
      <source media="(prefers-color-scheme: dark)" srcset="/.docfx/images/logotype-dark.svg">
      <img height="48" alt="Safekeeper logo" src="/.docfx/images/logotype.svg">
    </picture>
  </a>
</p>

<a href="https://unity3d.com"><img src="https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity" alt="made with Unity"></a>
<a href="https://github.com/semantic-release/semantic-release"><img src="https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg" alt="semantic release"></a>
<a href="https://www.patreon.com/aarthificial"><img src="https://img.shields.io/endpoint.svg?url=https%3A%2F%2Fshieldsio-patreon.vercel.app%2Fapi%3Fusername%3Daarthificial%26type%3Dpatrons&style=flat" alt="Patreon"></a>

---

> NOTE: Typewriter has been released for educational purposes. It probably won't
> be developed further.

Safekeeper is a simple save system for [Unity].

It's an opinionated solution designed mostly for non-procedural, adventure games.

An individual playthrough is stored in its own slot. Every time the game is
saved the previous state is overwritten. The game can have multiple slots
to store simultaneous playthroughs.

Currently, the number of available slots needs to be known in advance. The
player can't declare new slots. They can only select one of the existing ones and either:

- Start a new playthrough.
- Continue an existing one.
- Clear the slot.

This limitation comes from the fact that you'd need a way to store the information
about the slots themselves. Safekeeper doesn't have an out-of-the-box solution for that.
You could, however, try to implement this functionality yourself.

[Unity]: https://unity.com/
[Patreon]: https://www.patreon.com/aarthificial
[YouTube]: https://www.youtube.com/@aarthificial/join
