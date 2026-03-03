# ⚠️ DEPRECATED — AnkleBreaker-Utils

> **This package is deprecated.** It has been split into smaller, independent packages listed below.

## Replacement Packages

| Package | Repository |
|---------|------------|
| **Utils Inspector** | [AnkleBreaker-Utils-Inspector](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Inspector) |
| **Utils Extensions** | [AnkleBreaker-Utils-Extensions](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Extensions) |
| **Utils Editor** | [AnkleBreaker-Utils-Editor](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Editor) |
| **Utils Features** | [AnkleBreaker-Utils-Features](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Features) |
| **Integration GPU Instancer** | [AnkleBreaker-Integration-GPUInstancer](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-GPUInstancer) |
| **Integration Wwise** | [AnkleBreaker-Integration-Wwise](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-Wwise) |
| **Integration RSG Promise** | [AnkleBreaker-Integration-RSGPromise](https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-RSGPromise) |

## Migration

Replace the old `AnkleBreaker-Utils` Git URL in your `manifest.json` with only the packages you need:

```json
"com.anklebreaker-studio.utils.inspector": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Inspector.git#Release",
"com.anklebreaker-studio.utils.extensions": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Extensions.git#Release",
"com.anklebreaker-studio.utils.editor": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Editor.git#Release",
"com.anklebreaker-studio.utils.features": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Features.git#Release",
"com.anklebreaker-studio.integration.gpuinstancer": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-GPUInstancer.git#Release",
"com.anklebreaker-studio.integration.wwise": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-Wwise.git#Release",
"com.anklebreaker-studio.integration.rsg-promise": "https://github.com/AnkleBreaker-Studio/AnkleBreaker-Integration-RSGPromise.git#Release"
```

## Why?

The monolithic Utils package included too many unrelated features. Splitting lets each project pull in only what it needs, reducing compile times and dependency complexity.
