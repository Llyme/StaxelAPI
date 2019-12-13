# StaxelAPI
Some helpful stuff to make code-based mods for Staxel.


#### Requirements
* `0Harmony` (Included in the releases.)


#### Installment
* Extract the files in `[Staxel Directory]\bin`.
* Play.


### INI
A simple INI system with an automatic read-and-write when any value changes in both inside and outside the game.

Inheriting the `ModPlugin` automatically creates an `INI` object, as well as a file in `[Staxel Directory]\bin\PluginConfig\[Assembly Name].ini`.


### InputHelper
Check keyboard state, as well as a just-pressed key check.


### ModPlugin
A less bloated hook so you don't have to create all the functions that you won't be using.
