**VR 360° Tour Walkthrough**

A VR app for exploring Kimironko Neighbourhood in 360°. Look around and click on hotspots to learn about locations or navigate to other areas.
Features

*360° panoramic environments*

-[] Gaze-based interaction (look to interact)
-[] Multiple campus locations
Info hotspots and navigation points
Background audio and effects
Smooth transitions between locations

Quick Start

Open project in Unity (2021 LTS or newer)
Open CampusWalkthrough scene
Add all 3 scenes to Build Settings:

WelcomeScene
CampusWalkthrough
OutroScene


Build for Meta Quest (Android platform)
Deploy to VR headset

Setup in 3 Steps
Step 1: Add Locations

Open LocationData ScriptableObject
Add campus locations with 360° skybox images
Add ambience audio for each location

Step 2: Create Hotspots

Add info hotspots (show information)
Add navigation hotspots (go to other locations)
Position them in 3D space using world coordinates

Step 3: Build & Deploy

Switch platform to Android
Build and install on Meta Quest device
Put on headset and explore!
