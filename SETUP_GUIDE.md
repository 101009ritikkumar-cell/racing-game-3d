# 🏁 Racing Game 3D - Complete Setup Guide

## Prerequisites
- Unity 2022 LTS or newer
- Visual Studio Community (free)
- Android SDK 21+ (for mobile)
- Git

## Step 1: Clone Repository

```bash
git clone https://github.com/101009ritikkumar-cell/racing-game-3d.git
cd racing-game-3d
```

## Step 2: Create Unity Project

1. Open **Unity Hub**
2. Create new **3D Project**
3. Choose **URP** for better graphics
4. Select **Unity 2022 LTS**
5. Create project

## Step 3: Import Project Files

1. Copy all folders from cloned repo to your `Assets/` folder
2. Wait for Unity to finish importing
3. Check Console for errors

## Step 4: Configure Project Settings

### Player Settings
Go to **Edit → Project Settings → Player**

**Android:**
- Orientation: **Auto Rotate** (unchecked)
- Portrait: Unchecked
- Landscape Right: Checked
- Landscape Left: Checked
- Target API: 31+
- Minimum API: 21

**Windows:**
- Resolution: 1920x1080
- Fullscreen: Windowed

## Step 5: Setup First Track Scene

1. Create new scene: `Assets/Scenes/RaceTrack_Tutorial.unity`
2. Add to Build Settings
3. Create hierarchy:
   - Track (Plane with collider)
   - Car (with CarController)
   - Checkpoints (empty parent with 5 child objects)
   - FinishLine
   - CameraRig (with CameraFollow)
   - Managers (empty parent)
     - GameManager (with GameManager.cs)
     - RaceManager (with RaceManager.cs)
     - UIManager (with UIManager.cs)
     - AudioManager (with AudioManager.cs)

## Step 6: Setup Car

1. Import 3D car model or use cube
2. Add **Rigidbody**:
   - Mass: 1500
   - Drag: 0.1
3. Add 4x **WheelCollider** for wheels
4. Attach **CarController.cs**
5. Configure:
   - Max Speed: 200
   - Acceleration: 50
   - Assign wheel colliders

## Step 7: Create UI Canvas

1. Create Canvas (TextMesh Pro)
2. Add UI Elements:
   - Speed Text (top-left)
   - Lap Text (top-center)
   - Timer Text (top-right)
   - Countdown Panel
   - Pause Menu Panel
   - Finish Screen Panel

## Step 8: Test in Editor

1. Press **Play**
2. Use **WASD** to drive
3. **Shift** for nitro
4. **Space** for handbrake
5. **ESC** to pause

## Step 9: Build Instructions

**For Windows:**
```
File → Build Settings → Windows Standalone → Build
```

**For Android:**
```
File → Build Settings → Android → Build APK
```

## Common Issues

**Car won't move:**
- Check Rigidbody constraints
- Verify InputManager in scene
- Check wheel colliders assigned

**Slow on mobile:**
- Reduce draw calls
- Lower texture quality
- Disable shadows

---

**Happy Racing! 🏁**