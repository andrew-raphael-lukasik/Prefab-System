# Prefab System

### Why:

`com.unity.entities 1.0` introduced a baking workflow. This created a need for a new way to identify baked entity prefabs without costly search schemes and dozens of one-off tags.

### What:

Package that implements a simple way to lookup entity prefabs. Lookup is burst-compiled.

# Example
```csharp
var singleton = SystemAPI.GetSingleton<Prefabs>();
var prefabs = singleton.Registry;
Entity prefab = prefabs["an unique prefab id"];

// singleton.Dependency contains JobHandle to queue read/write access to singleton.Registry
```

# Installation Unity 2022.2
Add this line in `manifest.json` under `dependencies`:
```
"com.andrewraphaellukasik.Prefab-System": "https://github.com/andrew-raphael-lukasik/Prefab-System.git#upm"
```
Or via **Package Manager** / "Add package from git URL":
```
https://github.com/andrew-raphael-lukasik/Prefab-System.git#upm
```
