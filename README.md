# 🏁 Racing Game 3D - Cross Platform

A high-performance 3D racing game for Android and Windows built with Unity. Features realistic car physics, multiple tracks, beautiful graphics, and optimized mobile performance.

## 🎮 Features

- ✅ **Cross-Platform**: Android & Windows
- ✅ **Advanced Physics**: Realistic car handling & tire friction
- ✅ **Beautiful Graphics**: PBR materials, dynamic shadows, particles
- ✅ **Mobile Optimized**: 30-60 FPS on mid-range devices
- ✅ **Responsive Controls**: Touch (mobile) & keyboard (PC)
- ✅ **Lap Racing**: Multiple tracks with checkpoint system
- ✅ **Sound Design**: Engine audio, music, SFX
- ✅ **Car Customization**: Upgrades and personalization

## 📋 Quick Start

1. Clone: `git clone https://github.com/101009ritikkumar-cell/racing-game-3d.git`
2. Open in **Unity 2022 LTS+**
3. See **SETUP_GUIDE.md** for detailed setup instructions

## 🕹️ Controls

### Windows
- **WASD** - Drive
- **Space** - Handbrake
- **Shift** - Nitro
- **ESC** - Pause

### Android
- **Tilt** - Steer
- **Tap Top** - Accelerate
- **Tap Bottom** - Brake
- **Swipe Down (2 fingers)** - Nitro

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Car/
│   │   ├── CarController.cs      (Physics & movement)
│   │   └── InputManager.cs       (Cross-platform input)
│   ├── Gameplay/
│   │   ├── GameManager.cs        (Game flow)
│   │   ├── RaceManager.cs        (Race logic)
│   │   └── UIManager.cs          (UI system)
│   ├── Camera/
│   │   └── CameraFollow.cs       (Chase camera)
│   └── Utilities/
│       └── AudioManager.cs       (Sound system)
├── Scenes/
├── Prefabs/
├── Models/
├── Materials/
├── Audio/
└── UI/
```

## 🚀 Build Instructions

**Windows:**
```
File → Build Settings → Windows Standalone → Build
```

**Android:**
```
File → Build Settings → Android → Build APK
```

## 📊 Performance

| Platform | FPS | Resolution |
|----------|-----|-------------|
| Windows | 60+ | 1920x1080 |
| Android | 30-60 | 1080x2340 |

## 📄 License

MIT - Free for commercial & personal use

---

**Ready to race? See SETUP_GUIDE.md to begin! 🏁**