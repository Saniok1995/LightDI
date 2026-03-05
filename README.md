# LightDI
A lightweight DI container for Unity with a minimal API: bind/resolve, parent container fallback, and `[Inject]` field injection.

## Why LightDI

- Minimal code and dependencies.
- Good fit for small and medium Unity projects (including WebGL).
- Useful when full-featured DI frameworks feel too heavy.

## Installation (UPM via Git URL)

Add this dependency to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.saniok1995.lightdi": "https://github.com//Saniok1995/LightDI.git?path=Assets/LightDI#v1.0.0"
  }
}
```

## Quick Start (Global Container)

```csharp
using LightDI.Runtime;

public interface IApiClient {}
public class ApiClient : IApiClient {}

public class Bootstrap
{
    public void Init()
    {
        GlobalContainer.Instance.Bind<IApiClient>(new ApiClient());
    }
}

public class GameService
{
    [Inject] private IApiClient apiClient;

    public void Construct()
    {
        this.Inject();
    }
}
```

## Scene Container Example (Local Scope + Root Fallback)

You can create a container per scene and still resolve shared dependencies from the global/root container.

```csharp
using LightDI.Runtime;

public interface IGlobalConfig {}
public class GlobalConfig : IGlobalConfig {}

public interface ISceneHudService {}
public class SceneHudService : ISceneHudService {}

public class SceneBootstrap
{
    private IDependencyContainer sceneContainer;

    public void InitScene()
    {
        // Global/shared dependencies
        GlobalContainer.Instance.Bind<IGlobalConfig>(new GlobalConfig());

        // Scene-local container with fallback to global container
        sceneContainer = new DependencyContainer(GlobalContainer.Instance, serviceCount: 16, tag: "GameScene");
        sceneContainer.Bind<ISceneHudService>(new SceneHudService());

        var presenter = new ScenePresenter();
        presenter.Inject(sceneContainer); // Resolves both local and global dependencies
    }
}

public class ScenePresenter
{
    [Inject] private IGlobalConfig globalConfig;      // from global/root container
    [Inject] private ISceneHudService hudService;     // from scene container
}
```

## Included in the Package

- `DependencyContainer` for dependency registration and resolution.
- Parent fallback (a child container can resolve from its root container).
- `InjectExtensions` + `[Inject]` for reflection-based field injection with caching.
- `GlobalContainer` for simple global access.

## Limitations

- This is an intentionally lightweight solution, not a full-featured DI framework.
- The current focus is API simplicity and low runtime overhead.

## License

MIT.