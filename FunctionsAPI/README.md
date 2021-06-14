Functions API
A plugin that allows the in-game scripting to be extended with new functions.


## Changelog:
### v1.0.1:
- Added dependency to **BepInEx**.
### v1.0.2:
- Each custom function can now be turned on or off in a single configuration file.
### v1.0.2001:
- Added correct dependecy to BepInEx (Version 5.4.1101)
### v1.0.3:
- Functions can now be blacklisted using `FunctionsSystem.SetBlacklisted(String functionName, bool isBlacklisted)`. *(This applies to both original and custom functions)*
- Added extended versions of the Wait and Interval functions.
- **Wait (Extended)** functions just like a normal Wait but the wait time can be reset with a boolean *(true/false)* value.
- **Interval (Extended)** functions just like normal Interval but can be paused with a boolean *(true/false)* value.