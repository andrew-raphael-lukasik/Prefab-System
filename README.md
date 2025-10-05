# Prefab System

## What:

Package that implements a simple way to lookup entity prefabs. Lookup is burst-compiled.

## Why:

`com.unity.entities 1.0` introduced a baking workflow. This created a need for a new way to identify baked entity prefabs without costly search schemes and dozens of one-off tags.

I expect this repo to become obsolete in 3-18 months once `com.unity.entities` introduces more refined and tested workflow + API to do this.

UPDATE: 2 years later; 😂

## Example
- Create in your sub-scene a `GameObject` with `Prefab System/Prefab Pool Authoring` component and fill it's `Prefabs` field with prefabs you want to register.
- Alternatively use `Prefab System/Prefab Authoring` component to bake and turn any `GameObject` in your sub-scene into a registered prefab.
- Write your game code and refer to these prefabs like this:
```csharp
var prefabsRef = SystemAPI.GetSingletonRW<PrefabSystem.Prefabs>();

// `Dependency` is a read/write access handle for `Lookup`
prefabsRef.ValueRW.Dependency.Complete();// call before accessing prefabs or add to a job dependencies (when instantiating in a job)

var prefabsRO = prefabsRef.ValueRO.Lookup;
Entity prefab = prefabsRO["prefab_name"];
Entity instance = entityManager.Instantiate(prefab);
```
> NOTE: GameObject prefab name will become it's unique prefab id

## Installation Unity 2022.2
Add this line in `manifest.json` under `dependencies`:
```
"com.andrewraphaellukasik.Prefab-System": "https://github.com/andrew-raphael-lukasik/Prefab-System.git#upm"
```
Or via **Package Manager** / "Add package from git URL":
```
https://github.com/andrew-raphael-lukasik/Prefab-System.git#upm
```
