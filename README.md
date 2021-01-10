# Rust.ModLoader
A simple server-side script loader for [Rust](https://store.steampowered.com/app/252490/Rust/).

## Goals
* Support modding for [Rust](https://store.steampowered.com/app/252490/Rust/) servers
* Allow hotloading of mods (adding, removing, or updating them at runtime)
* Extensible hooks system so you can Bring Your Own Hooks if you need something else
* Minimal impact on server performance (CPU use, memory use, GCs, ...)
* ...

**Note**: This is very early in development and isn't very useful yet. [Contributions](/CONTRIBUTING.md) are welcome.

## Features
* Hooks into the game server dynamically using [Harmony](https://github.com/pardeike/Harmony)
* Scripts can implement hook methods which the mod loader calls
* Compiles and loads C# scripts automatically, without server restarts
