# reMarkable.fs

This repo provides various software useful in creating custom applications for the [reMarkable](https://remarkable.com) e-ink tablets.

- `reMarkable.fs` provides bindings to various inputs and outputs on the device:
  - buttons
  - digitizers (styluses)
  - USB keyboards (not done)
  - wireless connectivity (are we connected? how's the signal?)
  - power supply (is it powering via USB? what % is the battery at?)
  - checking performance metrics (e.g. CPU usage)
  - touch screen, incl. multi-touch tracking
- `reMarkable.fs.UI` provides an OOP-style UI layer
- `reMarkable.fs.MVU` (TODO)
- `reMarkable.fs.Demo` is a small demo showcasing the options available

This is all a WIP and changing often.

---

The [LICENSE](./License.md) is MIT.

This project is wholly unaffiliated with reMarkable, legally - this just a hobby project :)

The first commits of this code were a _direct_ port of another fantastic repo, [ReMarkable.NET](https://github.com/parzivail/ReMarkable.NET).

I "ported" the code for a few reasons:
- I wanted to understand how everything works
- I have intentions of building an MVU framework for some custom development, and ReMarkable.NET didn't quite fit my needs
