User Manual: Adding a New Place for 360-Virtual Tour in Unity
[Add a New 360 Sphere with Place Image]
Step 1: Import the 360 Image
1. Open Unity and navigate to the Assets folder.
2. Right-click in the Project window and select Import New Asset.
3. Select the 360-degree image you want to add and click Import.

Step 2: Create a New 360 Sphere
![image](https://github.com/user-attachments/assets/6e9dc9a0-10e8-4281-8d7c-5b96bfbfbc95)
1. In the Hierarchy panel, right-click and select 3D Object > Sphere.
2. Rename the sphere to reflect the place you are adding (e.g., Library).

Step 3: Apply the 360 Image to the Sphere
![image](https://github.com/user-attachments/assets/d73e6f46-39c0-47ca-b3e6-64d72f18578b)
1. Select the newly created sphere in the Hierarchy.
2. In the Inspector panel, click on the Mesh Renderer component.
3. Drag and drop the imported 360 image onto the Material slot under Mesh Renderer.
4. Adjust Scale: Set the scale of the sphere to (10, 10, 10) to match  previous configurations.

[Add the New Place in the TourManager]
![image](https://github.com/user-attachments/assets/77c22495-eea0-4d0a-95fe-a68fc0568819)
1. Select the Main Camera in the Hierarchy and locate the TourManager script component in the Inspector.
2. In the TourManager, find the Scenes or Places list/array and add a new entry by increasing the size. Drag the new 360 sphere to this new slot.

[Add a New 3D Renderer Button for Navigation]
Step 1: Create the Button Object
![image](https://github.com/user-attachments/assets/d38d0996-fcf5-451d-bf48-f1deb76a5e4e)
1. In the Hierarchy, right-click and select 3D Object > Cube (or any shape you prefer for the button).
2. Rename the object to reflect its function (e.g., Hotel Mock Room & Incubator btn).

Step 2: Position and Scale the Button
![image](https://github.com/user-attachments/assets/f314f8c6-40e6-4969-ab45-8c2599190676)
Move the button to the desired location in the scene (position it near the user’s view but not obstructing the scene).
Adjust the scale to make it clickable but unobtrusive (e.g., (0.015, 0.015, 0.015)).

Step 3: Add a Collider and Tag
![image](https://github.com/user-attachments/assets/635f7969-8058-4e87-983f-ac243b571f94)
1. With the button selected, go to the Inspector panel.
2. Ensure it has a Box Collider component for click detection.
![image](https://github.com/user-attachments/assets/d8340551-8bdf-499e-97b6-9dd940bbc2c9)
4. Add a Tag: Click on the Tag dropdown, select Add Tag, and create a new tag called NextSite.
5. Assign the NextSite tag to the button.

Step4: Assign the NextSite Script and Configure
1. Drag and drop the NextSite script onto the button object in the Inspector.
2. In the NextSite script, fill in the value enter at main camera's tour manager just now.

