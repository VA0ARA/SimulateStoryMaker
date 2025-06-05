Project Overview – Animation Builder with Unity Playback (WPF + Unity)

This project enables users to visually create 2D animations, export them as JSON, and play them back using a Unity-based player running on a server. The application is built using WPF following the MVVM architecture and uses the Unity Container for dependency injection to manage object creation cleanly.

Technologies Used

WPF (Windows Presentation Foundation)

MVVM (Model-View-ViewModel)

Unity Container (Dependency Injection)

JSON (Animation data serialization)

HttpClient (REST API communication)

Reflection (Dynamic property inspection and UI generation)

Unity Engine (Animation playback on the server)

Core Modules

1. Animation Editor (Mechanics)
Users can drag and drop visual elements into a layout space, configure their properties, and assign animation behaviors through a timeline-based interface.

2. Animation Model
The core data structure includes classes like AnimationClip, Bounds, and Element which define the logical structure, size, and timing of each animation asset.

3. API Integration
ApiHelper handles communication with a remote server, managing secure HTTP requests using headers like App, User-Token, and Device-Id.

4. JSON Export and Unity Playback
Animations are saved as structured JSON files and uploaded to a server where they are played back in real-time by a Unity-based engine.

5. Reflection and the Inspector System
The InspectorViewModel uses Reflection to read all public properties of the selected object dynamically at runtime. It filters out properties marked with a [HideInInspector] attribute and creates corresponding UI controls. This allows developers to inspect and modify objects without hardcoding each UI binding, greatly improving scalability and maintainability.

UI Description

Editor Canvas: Main area where users visually compose animations.

Inspector Panel: Displays the editable properties of the selected object using Reflection. This is dynamic and adapts based on the type of object selected.

Timeline Control: Manages frame sequencing and keyframes for animation.

Export/Upload Panel: Buttons to convert animations to JSON and send them to the Unity server.

Visual Placeholder

 UI screenshot:
[DatabStructure Diagram](DataStructure/6.jpg)

##Film-link:
[https://youtube.com/playlist?list=PLHKqhcgMIjlzsXpVFFV5wWazAfdbNrbtF&si=XFUialVfspwNSfL6]



