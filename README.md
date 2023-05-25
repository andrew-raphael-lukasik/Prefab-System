

IT IS NOT READY TO USE YET :D
will fix it soon :I


# ECS Prefab Lookup System
Package that implements a ease way to lookup entity prefabs in a burst-compiled systems.

# Example
```csharp
var prefabs = SystemAPI.GetSingleton<PrefabSystem.Lookup>().Prefabs;
Entity prefab = prefabs["an unique prefab id"];
```

# Installation Unity 2022.2
Add this line in `manifest.json` under `dependencies`:
```
"com.andrewraphaellukasik.basicscience": "https://github.com/andrew-raphael-lukasik/ECS-Prefab-Lookup-System.git#upm"
```
Or via **Package Manager** / "Add package from git URL":
```
https://github.com/andrew-raphael-lukasik/ECS-Prefab-Lookup-System.git#upm
```
